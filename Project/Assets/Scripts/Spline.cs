using System;
using System.Collections.Generic;
using SplineUtilities;
using UnityEngine;

[AddComponentMenu("SuperSplines/Spline")]
public class Spline : MonoBehaviour
{
	public float Length
	{
		get
		{
			return (float)this.lengthData.length;
		}
	}

	public bool AutoClose
	{
		get
		{
			return this.autoClose && !this.IsBezier;
		}
	}

	public int Step
	{
		get
		{
			return (!this.IsBezier) ? 1 : 3;
		}
	}

	public int SegmentCount
	{
		get
		{
			return (this.ControlNodeCount - 1) / this.Step;
		}
	}

	public int ControlNodeCount
	{
		get
		{
			return (!this.AutoClose) ? this.splineNodesInternal.Count : (this.splineNodesInternal.Count + 1);
		}
	}

	private double InvertedAccuracy
	{
		get
		{
			return 1.0 / (double)this.interpolationAccuracy;
		}
	}

	private bool IsBezier
	{
		get
		{
			return this.interpolationMode == Spline.InterpolationMode.Bezier;
		}
	}

	private bool HasNodes
	{
		get
		{
			return this.splineNodesInternal != null && this.splineNodesInternal.Count > 0;
		}
	}

	public SplineNode[] SplineNodes
	{
		get
		{
			if (this.splineNodesInternal == null)
			{
				this.splineNodesInternal = new List<SplineNode>();
			}
			return this.splineNodesInternal.ToArray();
		}
	}

	public SplineSegment[] SplineSegments
	{
		get
		{
			SplineSegment[] array = new SplineSegment[this.SegmentCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new SplineSegment(this, this.GetNode(i * this.Step, 0), this.GetNode(i * this.Step, 1));
			}
			return array;
		}
	}

	public void OnEnable()
	{
		this.UpdateSpline();
	}

	public void LateUpdate()
	{
		if (this.autoUpdate.Update())
		{
			this.UpdateSpline();
		}
	}

	public void UpdateSpline()
	{
		this.splineNodesArray.Remove(null);
		for (int i = 0; i < this.splineNodesArray.Count; i++)
		{
			this.splineNodesArray[i].spline = this;
		}
		int relevantNodeCount = this.GetRelevantNodeCount(this.splineNodesArray.Count);
		if (this.splineNodesInternal == null)
		{
			this.splineNodesInternal = new List<SplineNode>();
		}
		this.splineNodesInternal.Clear();
		if (!this.EnoughNodes(relevantNodeCount))
		{
			return;
		}
		this.splineNodesInternal.AddRange(this.splineNodesArray.GetRange(0, relevantNodeCount));
		this.ReparameterizeCurve();
	}

	public Vector3 GetPositionOnSpline(float param)
	{
		if (!this.HasNodes)
		{
			return Vector3.zero;
		}
		return this.GetPositionInternal(this.RecalculateParameter((double)param));
	}

	public Vector3 GetTangentToSpline(float param)
	{
		if (!this.HasNodes)
		{
			return Vector3.zero;
		}
		return this.GetTangentInternal(this.RecalculateParameter((double)param));
	}

	public Quaternion GetOrientationOnSpline(float param)
	{
		if (!this.HasNodes)
		{
			return Quaternion.identity;
		}
		Spline.RotationMode rotationMode = this.rotationMode;
		if (rotationMode != Spline.RotationMode.Tangent)
		{
			if (rotationMode != Spline.RotationMode.Node)
			{
				return Quaternion.identity;
			}
			return this.GetRotationInternal(this.RecalculateParameter((double)param));
		}
		else
		{
			Vector3 tangentToSpline = this.GetTangentToSpline(param);
			if (tangentToSpline.sqrMagnitude == 0f || this.tanUpVector.sqrMagnitude == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(tangentToSpline.normalized, this.tanUpVector.normalized);
		}
	}

	public float GetCustomValueOnSpline(float param)
	{
		if (!this.HasNodes)
		{
			return 0f;
		}
		return (float)this.GetValueInternal(this.RecalculateParameter((double)param));
	}

	public SplineSegment GetSplineSegment(float param)
	{
		param = Mathf.Clamp(param, 0f, 1f);
		for (int i = 0; i < this.ControlNodeCount - 1; i += this.Step)
		{
			if ((double)param - this.splineNodesInternal[i].posInSpline < (double)this.splineNodesInternal[i].Length)
			{
				return new SplineSegment(this, this.GetNode(i, 0), this.GetNode(i, this.Step));
			}
		}
		return new SplineSegment(this, this.GetNode(this.MaxNodeIndex(), 0), this.GetNode(this.MaxNodeIndex(), this.Step));
	}

	public float ConvertNormalizedParameterToDistance(float param)
	{
		return this.Length * param;
	}

	public float ConvertDistanceToNormalizedParameter(float param)
	{
		return (this.Length > 0f) ? (param / this.Length) : 0f;
	}

	private Spline.SegmentParameter RecalculateParameter(double param)
	{
		if (param <= 0.0)
		{
			return new Spline.SegmentParameter(0, 0.0);
		}
		if (param > 1.0)
		{
			return new Spline.SegmentParameter(this.MaxNodeIndex(), 1.0);
		}
		double invertedAccuracy = this.InvertedAccuracy;
		int i = this.lengthData.subSegmentPosition.Length - 1;
		while (i >= 0)
		{
			if (this.lengthData.subSegmentPosition[i] < param)
			{
				int num = i - i % this.interpolationAccuracy;
				int num2 = num * this.Step / this.interpolationAccuracy;
				double param2 = invertedAccuracy * ((double)(i - num) + (param - this.lengthData.subSegmentPosition[i]) / this.lengthData.subSegmentLength[i]);
				if (num2 >= this.ControlNodeCount - 1)
				{
					return new Spline.SegmentParameter(this.MaxNodeIndex(), 1.0);
				}
				return new Spline.SegmentParameter(num2, param2);
			}
			else
			{
				i--;
			}
		}
		return new Spline.SegmentParameter(this.MaxNodeIndex(), 1.0);
	}

	private SplineNode GetNode(int idxNode, int idxOffset)
	{
		idxNode += idxOffset;
		if (this.AutoClose)
		{
			return this.splineNodesInternal[(idxNode % this.splineNodesInternal.Count + this.splineNodesInternal.Count) % this.splineNodesInternal.Count];
		}
		return this.splineNodesInternal[Mathf.Clamp(idxNode, 0, this.splineNodesInternal.Count - 1)];
	}

	private void ReparameterizeCurve()
	{
		if (this.lengthData == null)
		{
			this.lengthData = new Spline.LengthData();
		}
		this.lengthData.Calculate(this);
	}

	private int MaxNodeIndex()
	{
		return this.ControlNodeCount - this.Step - 1;
	}

	private int GetRelevantNodeCount(int nodeCount)
	{
		int num = nodeCount;
		if (this.IsBezier)
		{
			if (nodeCount < 7)
			{
				num -= nodeCount % 4;
			}
			else
			{
				num -= (nodeCount - 4) % 3;
			}
		}
		return num;
	}

	private bool EnoughNodes(int nodeCount)
	{
		if (this.IsBezier)
		{
			return nodeCount >= 4;
		}
		return nodeCount >= 2;
	}

	public float GetClosestPointParam(Vector3 point, int iterations, float start = 0f, float end = 1f, float step = 0.01f)
	{
		return this.GetClosestPointParamIntern((Vector3 splinePos) => (point - splinePos).sqrMagnitude, iterations, start, end, step);
	}

	public float GetClosestPointParamToRay(Ray ray, int iterations, float start = 0f, float end = 1f, float step = 0.01f)
	{
		return this.GetClosestPointParamIntern((Vector3 splinePos) => Vector3.Cross(ray.direction, splinePos - ray.origin).sqrMagnitude, iterations, start, end, step);
	}

	public float GetClosestPointParamToPlane(Plane plane, int iterations, float start = 0f, float end = 1f, float step = 0.01f)
	{
		return this.GetClosestPointParamIntern((Vector3 splinePos) => plane.GetDistanceToPoint(splinePos), iterations, start, end, step);
	}

	private float GetClosestPointParamIntern(Spline.DistanceFunction distFnc, int iterations, float start, float end, float step)
	{
		iterations = Mathf.Clamp(iterations, 0, 5);
		float closestPointParamOnSegmentIntern = this.GetClosestPointParamOnSegmentIntern(distFnc, start, end, step);
		for (int i = 0; i < iterations; i++)
		{
			float num = Mathf.Pow(10f, -((float)i + 2f));
			start = Mathf.Clamp01(closestPointParamOnSegmentIntern - num);
			end = Mathf.Clamp01(closestPointParamOnSegmentIntern + num);
			step = num * 0.1f;
			closestPointParamOnSegmentIntern = this.GetClosestPointParamOnSegmentIntern(distFnc, start, end, step);
		}
		return closestPointParamOnSegmentIntern;
	}

	private float GetClosestPointParamOnSegmentIntern(Spline.DistanceFunction distFnc, float start, float end, float step)
	{
		float num = float.PositiveInfinity;
		float result = 0f;
		for (float num2 = start; num2 <= end; num2 += step)
		{
			float num3 = distFnc(this.GetPositionOnSpline(num2));
			if (num > num3)
			{
				num = num3;
				result = num2;
			}
		}
		return result;
	}

	private void OnDrawGizmos()
	{
		this.UpdateSpline();
		if (!this.HasNodes)
		{
			return;
		}
		this.DrawSplineGizmo(new Color(0.5f, 0.5f, 0.5f, 0.5f));
		Plane plane = default(Plane);
		Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
		plane.SetNormalAndPosition(Camera.current.transform.forward, Camera.current.transform.position);
		foreach (SplineNode splineNode in this.splineNodesInternal)
		{
			Gizmos.DrawSphere(splineNode.Position, this.GetSizeMultiplier(splineNode) * 2f);
		}
	}

	private void OnDrawGizmosSelected()
	{
		this.UpdateSpline();
		if (!this.HasNodes)
		{
			return;
		}
		this.DrawSplineGizmo(new Color(1f, 0.5f, 0f, 1f));
		Gizmos.color = new Color(1f, 0.5f, 0f, 0.75f);
		int num = -1;
		foreach (SplineNode splineNode in this.splineNodesInternal)
		{
			num++;
			if (this.IsBezier && num % 3 != 0)
			{
				Gizmos.color = new Color(0.8f, 1f, 0.1f, 0.7f);
			}
			else
			{
				Gizmos.color = new Color(1f, 0.5f, 0f, 0.75f);
			}
			Gizmos.DrawSphere(splineNode.Position, this.GetSizeMultiplier(splineNode) * 1.5f);
		}
	}

	private void DrawSplineGizmo(Color curveColor)
	{
		switch (this.interpolationMode)
		{
		case Spline.InterpolationMode.Bezier:
		case Spline.InterpolationMode.BSpline:
			Gizmos.color = new Color(curveColor.r, curveColor.g, curveColor.b, curveColor.a * 0.25f);
			for (int i = 0; i < this.ControlNodeCount - 1; i++)
			{
				Gizmos.DrawLine(this.GetNode(i, 0).Position, this.GetNode(i, 1).Position);
				if (i % 3 == 0 && this.IsBezier)
				{
					i++;
				}
			}
			break;
		}
		Gizmos.color = curveColor;
		for (int j = 0; j < this.ControlNodeCount - 1; j += this.Step)
		{
			Vector3 from = this.GetPositionInternal(new Spline.SegmentParameter(j, 0.0));
			for (float num = 0.1f; num < 1.1f; num += 0.1f)
			{
				Vector3 positionInternal = this.GetPositionInternal(new Spline.SegmentParameter(j, (double)num));
				Gizmos.DrawLine(from, positionInternal);
				from = positionInternal;
			}
		}
	}

	private float GetSizeMultiplier(SplineNode node)
	{
		if (!Camera.current.orthographic)
		{
			Plane plane = default(Plane);
			float num = 0f;
			plane.SetNormalAndPosition(Camera.current.transform.forward, Camera.current.transform.position);
			plane.Raycast(new Ray(node.Position, Camera.current.transform.forward), out num);
			return num * 0.0075f;
		}
		return Camera.current.orthographicSize * 0.01875f;
	}

	private Vector3 GetPositionInternal(Spline.SegmentParameter sParam)
	{
		SplineNode splineNode;
		SplineNode splineNode2;
		SplineNode splineNode3;
		SplineNode splineNode4;
		this.GetAdjacentNodes(sParam, out splineNode, out splineNode2, out splineNode3, out splineNode4);
		Vector3 position = splineNode.Position;
		Vector3 position2 = splineNode2.Position;
		Vector3 position3 = splineNode3.Position;
		Vector3 position4 = splineNode4.Position;
		this.RecalcVectors(splineNode, splineNode2, ref position3, ref position4);
		return this.InterpolatePosition(sParam.normalizedParam, position, position2, position3, position4);
	}

	private Vector3 GetTangentInternal(Spline.SegmentParameter sParam)
	{
		SplineNode splineNode;
		SplineNode splineNode2;
		SplineNode splineNode3;
		SplineNode splineNode4;
		this.GetAdjacentNodes(sParam, out splineNode, out splineNode2, out splineNode3, out splineNode4);
		Vector3 position = splineNode.Position;
		Vector3 position2 = splineNode2.Position;
		Vector3 position3 = splineNode3.Position;
		Vector3 position4 = splineNode4.Position;
		this.RecalcVectors(splineNode, splineNode2, ref position3, ref position4);
		return this.InterpolateTangent(sParam.normalizedParam, position, position2, position3, position4);
	}

	private double GetValueInternal(Spline.SegmentParameter sParam)
	{
		SplineNode splineNode;
		SplineNode splineNode2;
		SplineNode splineNode3;
		SplineNode splineNode4;
		this.GetAdjacentNodes(sParam, out splineNode, out splineNode2, out splineNode3, out splineNode4);
		double v = (double)splineNode.customValue;
		double v2 = (double)splineNode2.customValue;
		double v3 = (double)splineNode3.customValue;
		double v4 = (double)splineNode4.customValue;
		this.RecalcScalars(splineNode, splineNode2, ref v3, ref v4);
		return this.InterpolateValue(sParam.normalizedParam, v, v2, v3, v4);
	}

	private Quaternion GetRotationInternal(Spline.SegmentParameter sParam)
	{
		Quaternion rotation = this.GetNode(sParam.normalizedIndex, -1).Transform.rotation;
		Quaternion rotation2 = this.GetNode(sParam.normalizedIndex, 0).Transform.rotation;
		Quaternion rotation3 = this.GetNode(sParam.normalizedIndex, 1).Transform.rotation;
		Quaternion rotation4 = this.GetNode(sParam.normalizedIndex, 2).Transform.rotation;
		Quaternion squadIntermediate = Spline.GetSquadIntermediate(rotation, rotation2, rotation3);
		Quaternion squadIntermediate2 = Spline.GetSquadIntermediate(rotation2, rotation3, rotation4);
		return Spline.GetQuatSquad(sParam.normalizedParam, rotation2, rotation3, squadIntermediate, squadIntermediate2);
	}

	private Vector3 InterpolatePosition(double t, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		double num = t * t;
		double num2 = num * t;
		double[] matrix = this.GetMatrix(this.interpolationMode);
		double num3 = matrix[0] * num2 + matrix[1] * num + matrix[2] * t + matrix[3];
		double num4 = matrix[4] * num2 + matrix[5] * num + matrix[6] * t + matrix[7];
		double num5 = matrix[8] * num2 + matrix[9] * num + matrix[10] * t + matrix[11];
		double num6 = matrix[12] * num2 + matrix[13] * num + matrix[14] * t + matrix[15];
		return new Vector3((float)(num3 * (double)v0.x + num4 * (double)v1.x + num5 * (double)v2.x + num6 * (double)v3.x), (float)(num3 * (double)v0.y + num4 * (double)v1.y + num5 * (double)v2.y + num6 * (double)v3.y), (float)(num3 * (double)v0.z + num4 * (double)v1.z + num5 * (double)v2.z + num6 * (double)v3.z));
	}

	private Vector3 InterpolateTangent(double t, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		double num = t * t;
		double[] matrix = this.GetMatrix(this.interpolationMode);
		t *= 2.0;
		num *= 3.0;
		double num2 = matrix[0] * num + matrix[1] * t + matrix[2];
		double num3 = matrix[4] * num + matrix[5] * t + matrix[6];
		double num4 = matrix[8] * num + matrix[9] * t + matrix[10];
		double num5 = matrix[12] * num + matrix[13] * t + matrix[14];
		return new Vector3((float)(num2 * (double)v0.x + num3 * (double)v1.x + num4 * (double)v2.x + num5 * (double)v3.x), (float)(num2 * (double)v0.y + num3 * (double)v1.y + num4 * (double)v2.y + num5 * (double)v3.y), (float)(num2 * (double)v0.z + num3 * (double)v1.z + num4 * (double)v2.z + num5 * (double)v3.z));
	}

	private double InterpolateValue(double t, double v0, double v1, double v2, double v3)
	{
		double num = t * t;
		double num2 = num * t;
		double[] matrix = this.GetMatrix(this.interpolationMode);
		double num3 = matrix[0] * num2 + matrix[1] * num + matrix[2] * t + matrix[3];
		double num4 = matrix[4] * num2 + matrix[5] * num + matrix[6] * t + matrix[7];
		double num5 = matrix[8] * num2 + matrix[9] * num + matrix[10] * t + matrix[11];
		double num6 = matrix[12] * num2 + matrix[13] * num + matrix[14] * t + matrix[15];
		return num3 * v0 + num4 * v1 + num5 * v2 + num6 * v3;
	}

	private void GetAdjacentNodes(Spline.SegmentParameter sParam, out SplineNode node0, out SplineNode node1, out SplineNode node2, out SplineNode node3)
	{
		switch (this.interpolationMode)
		{
		case Spline.InterpolationMode.Hermite:
			node0 = this.GetNode(sParam.normalizedIndex, 0);
			node1 = this.GetNode(sParam.normalizedIndex, 1);
			node2 = this.GetNode(sParam.normalizedIndex, -1);
			node3 = this.GetNode(sParam.normalizedIndex, 2);
			return;
		case Spline.InterpolationMode.BSpline:
			node0 = this.GetNode(sParam.normalizedIndex, -1);
			node1 = this.GetNode(sParam.normalizedIndex, 0);
			node2 = this.GetNode(sParam.normalizedIndex, 1);
			node3 = this.GetNode(sParam.normalizedIndex, 2);
			return;
		}
		node0 = this.GetNode(sParam.normalizedIndex, 0);
		node1 = this.GetNode(sParam.normalizedIndex, 1);
		node2 = this.GetNode(sParam.normalizedIndex, 2);
		node3 = this.GetNode(sParam.normalizedIndex, 3);
	}

	private void RecalcVectors(SplineNode node0, SplineNode node1, ref Vector3 P2, ref Vector3 P3)
	{
		if (this.interpolationMode != Spline.InterpolationMode.Hermite)
		{
			return;
		}
		if (this.tangentMode == Spline.TangentMode.UseNodeForwardVector)
		{
			P2 = node0.Transform.forward * this.tension;
			P3 = node1.Transform.forward * this.tension;
		}
		else
		{
			P2 = node1.Position - P2;
			P3 -= node0.Position;
			if (this.tangentMode != Spline.TangentMode.UseTangents)
			{
				P2.Normalize();
				P3.Normalize();
			}
			P2 *= this.tension;
			P3 *= this.tension;
		}
	}

	private void RecalcScalars(SplineNode node0, SplineNode node1, ref double P2, ref double P3)
	{
		if (this.interpolationMode != Spline.InterpolationMode.Hermite)
		{
			return;
		}
		P2 = (double)node1.customValue - P2;
		P3 -= (double)node0.customValue;
		P2 *= (double)this.tension;
		P3 *= (double)this.tension;
	}

	private double GetSegmentLengthInternal(int idxFirstPoint, double startValue, double endValue, double step)
	{
		double num = 0.0;
		Vector3 positionInternal = this.GetPositionInternal(new Spline.SegmentParameter(idxFirstPoint, startValue));
		double num2 = (double)positionInternal.x;
		double num3 = (double)positionInternal.y;
		double num4 = (double)positionInternal.z;
		for (double num5 = startValue + step; num5 < endValue + step * 0.5; num5 += step)
		{
			positionInternal = this.GetPositionInternal(new Spline.SegmentParameter(idxFirstPoint, num5));
			double num6 = num2 - (double)positionInternal.x;
			double num7 = num3 - (double)positionInternal.y;
			double num8 = num4 - (double)positionInternal.z;
			num += Math.Sqrt(num6 * num6 + num7 * num7 + num8 * num8);
			num2 = (double)positionInternal.x;
			num3 = (double)positionInternal.y;
			num4 = (double)positionInternal.z;
		}
		return num;
	}

	private double[] GetMatrix(Spline.InterpolationMode iMode)
	{
		switch (iMode)
		{
		case Spline.InterpolationMode.Hermite:
			return Spline.HermiteMatrix;
		case Spline.InterpolationMode.Bezier:
			return Spline.BezierMatrix;
		case Spline.InterpolationMode.BSpline:
			return Spline.BSplineMatrix;
		case Spline.InterpolationMode.Linear:
			return Spline.LinearMatrix;
		default:
			return Spline.LinearMatrix;
		}
	}

	private static Quaternion GetQuatSquad(double t, Quaternion q0, Quaternion q1, Quaternion a0, Quaternion a1)
	{
		float t2 = 2f * (float)t * (1f - (float)t);
		Quaternion p = Spline.QuatSlerp(q0, q1, (float)t);
		Quaternion q2 = Spline.QuatSlerp(a0, a1, (float)t);
		return Spline.QuatSlerp(p, q2, t2);
	}

	private static Quaternion GetSquadIntermediate(Quaternion q0, Quaternion q1, Quaternion q2)
	{
		Quaternion quatConjugate = Spline.GetQuatConjugate(q1);
		Quaternion quatLog = Spline.GetQuatLog(quatConjugate * q0);
		Quaternion quatLog2 = Spline.GetQuatLog(quatConjugate * q2);
		Quaternion q3 = new Quaternion(-0.25f * (quatLog.x + quatLog2.x), -0.25f * (quatLog.y + quatLog2.y), -0.25f * (quatLog.z + quatLog2.z), -0.25f * (quatLog.w + quatLog2.w));
		return q1 * Spline.GetQuatExp(q3);
	}

	private static Quaternion QuatSlerp(Quaternion p, Quaternion q, float t)
	{
		float num = Quaternion.Dot(p, q);
		Quaternion result;
		if ((double)(1f + num) > 1E-05)
		{
			float num4;
			float num5;
			if ((double)(1f - num) > 1E-05)
			{
				float num2 = Mathf.Acos(num);
				float num3 = 1f / Mathf.Sin(num2);
				num4 = Mathf.Sin((1f - t) * num2) * num3;
				num5 = Mathf.Sin(t * num2) * num3;
			}
			else
			{
				num4 = 1f - t;
				num5 = t;
			}
			result.x = num4 * p.x + num5 * q.x;
			result.y = num4 * p.y + num5 * q.y;
			result.z = num4 * p.z + num5 * q.z;
			result.w = num4 * p.w + num5 * q.w;
		}
		else
		{
			float num6 = Mathf.Sin((1f - t) * 3.14159274f * 0.5f);
			float num7 = Mathf.Sin(t * 3.14159274f * 0.5f);
			result.x = num6 * p.x - num7 * p.y;
			result.y = num6 * p.y + num7 * p.x;
			result.z = num6 * p.z - num7 * p.w;
			result.w = p.z;
		}
		return result;
	}

	private static Quaternion GetQuatLog(Quaternion q)
	{
		Quaternion result = q;
		result.w = 0f;
		if (Mathf.Abs(q.w) < 1f)
		{
			float num = Mathf.Acos(q.w);
			float num2 = Mathf.Sin(num);
			if (Mathf.Abs(num2) > 0.0001f)
			{
				float num3 = num / num2;
				result.x = q.x * num3;
				result.y = q.y * num3;
				result.z = q.z * num3;
			}
		}
		return result;
	}

	private static Quaternion GetQuatExp(Quaternion q)
	{
		Quaternion result = q;
		float num = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
		float num2 = Mathf.Sin(num);
		result.w = Mathf.Cos(num);
		if (Mathf.Abs(num2) > 0.0001f)
		{
			float num3 = num2 / num;
			result.x = num3 * q.x;
			result.y = num3 * q.y;
			result.z = num3 * q.z;
		}
		return result;
	}

	private static Quaternion GetQuatConjugate(Quaternion q)
	{
		return new Quaternion(-q.x, -q.y, -q.z, q.w);
	}

	public List<SplineNode> splineNodesArray = new List<SplineNode>();

	public Spline.InterpolationMode interpolationMode;

	public Spline.RotationMode rotationMode = Spline.RotationMode.Tangent;

	public Spline.TangentMode tangentMode = Spline.TangentMode.UseTangents;

	public AutomaticUpdater autoUpdate = new AutomaticUpdater();

	public Vector3 tanUpVector = Vector3.up;

	public float tension = 0.5f;

	public bool autoClose;

	public int interpolationAccuracy = 5;

	private List<SplineNode> splineNodesInternal = new List<SplineNode>();

	private Spline.LengthData lengthData = new Spline.LengthData();

	public static readonly double[] HermiteMatrix = new double[]
	{
		2.0,
		-3.0,
		0.0,
		1.0,
		-2.0,
		3.0,
		0.0,
		0.0,
		1.0,
		-2.0,
		1.0,
		0.0,
		1.0,
		-1.0,
		0.0,
		0.0
	};

	public static readonly double[] BezierMatrix = new double[]
	{
		-1.0,
		3.0,
		-3.0,
		1.0,
		3.0,
		-6.0,
		3.0,
		0.0,
		-3.0,
		3.0,
		0.0,
		0.0,
		1.0,
		0.0,
		0.0,
		0.0
	};

	public static readonly double[] BSplineMatrix = new double[]
	{
		-0.16666666666666666,
		0.5,
		-0.5,
		0.16666666666666666,
		0.5,
		-1.0,
		0.0,
		0.66666666666666663,
		-0.5,
		0.5,
		0.5,
		0.16666666666666666,
		0.16666666666666666,
		0.0,
		0.0,
		0.0
	};

	public static readonly double[] LinearMatrix = new double[]
	{
		0.0,
		0.0,
		-1.0,
		1.0,
		0.0,
		0.0,
		1.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0,
		0.0
	};

	private sealed class SegmentParameter
	{
		public SegmentParameter()
		{
			this.normalizedParam = 0.0;
			this.normalizedIndex = 0;
		}

		public SegmentParameter(int index, double param)
		{
			this.normalizedParam = param;
			this.normalizedIndex = index;
		}

		public double normalizedParam;

		public int normalizedIndex;
	}

	private sealed class LengthData
	{
		public void Calculate(Spline spline)
		{
			int num = spline.SegmentCount * spline.interpolationAccuracy;
			double num2 = 1.0 / (double)spline.interpolationAccuracy;
			this.subSegmentLength = new double[num];
			this.subSegmentPosition = new double[num];
			this.length = 0.0;
			for (int i = 0; i < num; i++)
			{
				this.subSegmentLength[i] = 0.0;
				this.subSegmentPosition[i] = 0.0;
			}
			for (int j = 0; j < spline.SegmentCount; j++)
			{
				for (int k = 0; k < spline.interpolationAccuracy; k++)
				{
					int num3 = j * spline.interpolationAccuracy + k;
					this.subSegmentLength[num3] = spline.GetSegmentLengthInternal(j * spline.Step, (double)k * num2, (double)(k + 1) * num2, 0.2 * num2);
					this.length += this.subSegmentLength[num3];
				}
			}
			for (int l = 0; l < spline.SegmentCount; l++)
			{
				for (int m = 0; m < spline.interpolationAccuracy; m++)
				{
					int num4 = l * spline.interpolationAccuracy + m;
					this.subSegmentLength[num4] /= this.length;
					if (num4 < this.subSegmentPosition.Length - 1)
					{
						this.subSegmentPosition[num4 + 1] = this.subSegmentPosition[num4] + this.subSegmentLength[num4];
					}
				}
			}
			this.SetupSplinePositions(spline);
		}

		private void SetupSplinePositions(Spline spline)
		{
			foreach (SplineNode splineNode in spline.splineNodesInternal)
			{
				splineNode.Reset();
			}
			for (int i = 0; i < this.subSegmentLength.Length; i++)
			{
				spline.splineNodesInternal[(i - i % spline.interpolationAccuracy) / spline.interpolationAccuracy * spline.Step].length += this.subSegmentLength[i];
			}
			for (int j = 0; j < spline.splineNodesInternal.Count - spline.Step; j += spline.Step)
			{
				spline.splineNodesInternal[j + spline.Step].posInSpline = spline.splineNodesInternal[j].posInSpline + (double)spline.splineNodesInternal[j].Length;
			}
			if (spline.IsBezier)
			{
				for (int k = 0; k < spline.splineNodesInternal.Count - spline.Step; k += spline.Step)
				{
					spline.splineNodesInternal[k + 1].posInSpline = spline.splineNodesInternal[k].posInSpline;
					spline.splineNodesInternal[k + 2].posInSpline = spline.splineNodesInternal[k].posInSpline;
					spline.splineNodesInternal[k + 1].length = 0.0;
					spline.splineNodesInternal[k + 2].length = 0.0;
				}
			}
			if (!spline.AutoClose)
			{
				spline.splineNodesInternal[spline.splineNodesInternal.Count - 1].posInSpline = 1.0;
			}
		}

		public double[] subSegmentLength;

		public double[] subSegmentPosition;

		public double length;
	}

	public enum TangentMode
	{
		UseNormalizedTangents,
		UseTangents,
		UseNodeForwardVector
	}

	public enum RotationMode
	{
		None,
		Node,
		Tangent
	}

	public enum InterpolationMode
	{
		Hermite,
		Bezier,
		BSpline,
		Linear
	}

	private delegate float DistanceFunction(Vector3 splinePos);
}
