using System;
using UnityEngine;

[RequireComponent(typeof(Spline))]
public class SplineAutoAlign : MonoBehaviour
{
	public void AutoAlign()
	{
		if (this.raycastDirection.x == 0f && this.raycastDirection.y == 0f && this.raycastDirection.z == 0f)
		{
			UnityEngine.Debug.LogWarning(base.gameObject.name + ": The raycast direction is zero!", base.gameObject);
			return;
		}
		Spline component = base.GetComponent<Spline>();
		foreach (SplineNode splineNode in component.SplineNodes)
		{
			RaycastHit[] array = Physics.RaycastAll(splineNode.Position, this.raycastDirection, float.PositiveInfinity, this.raycastLayers);
			RaycastHit raycastHit = default(RaycastHit);
			raycastHit.distance = float.PositiveInfinity;
			foreach (RaycastHit raycastHit2 in array)
			{
				bool flag = false;
				foreach (string b in this.ignoreTags)
				{
					if (raycastHit2.transform.tag == b)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (raycastHit.distance > raycastHit2.distance)
					{
						raycastHit = raycastHit2;
					}
				}
			}
			if (raycastHit.distance != float.PositiveInfinity)
			{
				splineNode.Transform.position = raycastHit.point - this.raycastDirection * this.offset;
			}
		}
	}

	public LayerMask raycastLayers = -1;

	public float offset = 0.1f;

	public string[] ignoreTags;

	public Vector3 raycastDirection = Vector3.down;
}
