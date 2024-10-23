using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class GraphPathFinding : IPathFinding
	{
		public Stack<Transform> FindPath(Vector3 enemyPos, Vector3 playerPos)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("WayPoint");
			float num = 99999f;
			WayPointScript wayPointScript = null;
			WayPointScript wayPointScript2 = null;
			foreach (GameObject gameObject in array)
			{
				WayPointScript component = gameObject.GetComponent<WayPointScript>();
				component.parent = null;
				float magnitude = (component.transform.position - enemyPos).magnitude;
				if (magnitude < num)
				{
					Ray ray = new Ray(enemyPos + new Vector3(0f, 0.5f, 0f), component.transform.position - enemyPos);
					RaycastHit raycastHit;
					if (!Physics.Raycast(ray, out raycastHit, magnitude, 100352))
					{
						wayPointScript = component;
						num = magnitude;
					}
				}
				wayPointScript2 = GameApp.GetInstance().GetGameScene().GetPlayer().NearestWayPoint;
			}
			if (wayPointScript != null && wayPointScript2 != null)
			{
				this.path = this.SearchPath(wayPointScript, wayPointScript2);
			}
			if (wayPointScript2 == null)
			{
				UnityEngine.Debug.Log("to null");
			}
			return this.path;
		}

		public Transform GetNextWayPoint(Vector3 enemyPos, Vector3 playerPos)
		{
			if (this.path != null && this.path.Count > 0)
			{
				return this.path.Peek();
			}
			this.path = this.FindPath(enemyPos, playerPos);
			if (this.path != null && this.path.Count > 0)
			{
				return this.path.Peek();
			}
			return null;
		}

		public void PopNode()
		{
			if (this.path != null && this.path.Count > 0)
			{
				this.path.Pop();
			}
		}

		public bool HavePath()
		{
			return this.path != null && this.path.Count > 0;
		}

		public void ClearPath()
		{
			if (this.path != null)
			{
				this.path.Clear();
			}
		}

		public Stack<Transform> SearchPath(WayPointScript from, WayPointScript to)
		{
			Stack<Transform> stack = new Stack<Transform>();
			if (from == to)
			{
				stack.Push(to.transform);
				return stack;
			}
			this.openStack.Push(from);
			while (this.openStack.Count > 0)
			{
				if (this.openStack.Count > 100)
				{
					UnityEngine.Debug.Log("Memeroy Explode! To many nodes in open stack..");
					UnityEngine.Debug.Break();
					break;
				}
				WayPointScript wayPointScript = this.openStack.Pop();
				this.closeStack.Push(wayPointScript);
				WayPointScript[] nodes = wayPointScript.nodes;
				foreach (WayPointScript wayPointScript2 in nodes)
				{
					if (wayPointScript2 == to)
					{
						wayPointScript2.parent = wayPointScript;
						break;
					}
					if (!this.openStack.Contains(wayPointScript2) && !this.closeStack.Contains(wayPointScript2))
					{
						wayPointScript2.parent = wayPointScript;
						this.openStack.Push(wayPointScript2);
					}
				}
			}
			this.openStack.Clear();
			this.closeStack.Clear();
			WayPointScript wayPointScript3 = to;
			stack.Push(to.transform);
			while (wayPointScript3.parent != null)
			{
				wayPointScript3 = wayPointScript3.parent;
				if (stack.Count > 30)
				{
					UnityEngine.Debug.Log("Memeroy Explode! Parent Forever..");
					UnityEngine.Debug.Break();
					break;
				}
				stack.Push(wayPointScript3.transform);
			}
			return stack;
		}

		protected WayPointScript currentWayPoint;

		protected Stack<WayPointScript> openStack = new Stack<WayPointScript>();

		protected Stack<WayPointScript> closeStack = new Stack<WayPointScript>();

		protected Stack<Transform> path;
	}
}
