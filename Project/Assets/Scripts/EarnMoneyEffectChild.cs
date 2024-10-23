using System;
using UnityEngine;

public class EarnMoneyEffectChild : MonoBehaviour
{
	private void OnEnable()
	{
		base.transform.localPosition = Vector3.zero;
		this.delay = UnityEngine.Random.Range(0.2f, 0.8f);
		Vector3 p = new Vector3(base.transform.localPosition.x + (float)UnityEngine.Random.Range(-200, 200), base.transform.localPosition.y + (float)UnityEngine.Random.Range(-200, 200), 0f);
		this.bezier = new ThreePointBezier(base.transform.localPosition, p, this.Target.localPosition);
		this.dt = 0f;
	}

	private void Update()
	{
		if (this.dt < this.delay)
		{
			this.dt += Time.deltaTime;
		}
		else if (this.dt < this.delay + 1f)
		{
			this.dt += Time.deltaTime * 1.5f;
			base.transform.localPosition = this.bezier.GetPointAtTime(this.dt - this.delay);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	[HideInInspector]
	public Transform Current;

	[HideInInspector]
	public Transform Target;

	private float delay;

	private float dt;

	private ThreePointBezier bezier = new ThreePointBezier(Vector3.zero, Vector3.zero, Vector3.zero);
}
