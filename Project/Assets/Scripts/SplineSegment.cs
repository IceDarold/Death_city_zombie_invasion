using System;

public class SplineSegment
{
	public SplineSegment(Spline pSpline, SplineNode sNode, SplineNode eNode)
	{
		if (pSpline != null)
		{
			this.parentSpline = pSpline;
			this.startNode = sNode;
			this.endNode = eNode;
			return;
		}
		throw new ArgumentNullException("pSpline");
	}

	public Spline ParentSpline
	{
		get
		{
			return this.parentSpline;
		}
	}

	public SplineNode StartNode
	{
		get
		{
			return this.startNode;
		}
	}

	public SplineNode EndNode
	{
		get
		{
			return this.endNode;
		}
	}

	public float Length
	{
		get
		{
			return (float)(this.startNode.length * (double)this.parentSpline.Length);
		}
	}

	public float NormalizedLength
	{
		get
		{
			return (float)this.startNode.length;
		}
	}

	public float ConvertSegmentToSplineParamter(float param)
	{
		return (float)(this.startNode.posInSpline + (double)param * this.startNode.length);
	}

	public float ConvertSplineToSegmentParamter(float param)
	{
		if ((double)param < this.startNode.posInSpline)
		{
			return 0f;
		}
		if ((double)param >= this.endNode.posInSpline)
		{
			return 1f;
		}
		return (float)(((double)param - this.startNode.posInSpline) / this.startNode.length);
	}

	public float ClampParameterToSegment(float param)
	{
		if ((double)param < this.startNode.posInSpline)
		{
			return (float)this.startNode.posInSpline;
		}
		if ((double)param >= this.endNode.posInSpline)
		{
			return (float)this.endNode.posInSpline;
		}
		return param;
	}

	private readonly Spline parentSpline;

	private readonly SplineNode startNode;

	private readonly SplineNode endNode;
}
