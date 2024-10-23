using System;
using UnityEngine;
using Zombie3D;

public class ItemScript : MonoBehaviour
{
	private void Start()
	{
		Ray ray = new Ray(base.transform.position + Vector3.up * 1f, Vector3.down);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, 32768))
		{
			this.floorY = raycastHit.point.y;
		}
	}

	private void Update()
	{
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime < 0.03f)
		{
			return;
		}
		base.transform.Rotate(this.rotationSpeed * this.deltaTime);
		if (this.enableUpandDown)
		{
			if (!this.moveUp)
			{
				float num = Mathf.MoveTowards(base.transform.position.y, this.floorY + this.LowPos, this.moveSpeed * this.deltaTime);
				base.transform.position = new Vector3(base.transform.position.x, num, base.transform.position.z);
				if (num <= this.floorY + this.LowPos)
				{
					this.moveUp = true;
				}
			}
			else
			{
				float num2 = Mathf.MoveTowards(base.transform.position.y, this.floorY + this.HighPos, this.moveSpeed * this.deltaTime);
				base.transform.position = new Vector3(base.transform.position.x, num2, base.transform.position.z);
				if (num2 >= this.floorY + this.HighPos)
				{
					this.moveUp = false;
				}
			}
		}
		this.deltaTime = 0f;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer == 8)
		{
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			player.OnPickUp(this.itemType);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public ItemType itemType;

	private bool moveUp;

	public Vector3 rotationSpeed = new Vector3(0f, 45f, 0f);

	public bool enableUpandDown = true;

	protected float deltaTime;

	public float moveSpeed = 0.2f;

	public float HighPos = 1.2f;

	public float LowPos = 1f;

	protected float floorY;
}
