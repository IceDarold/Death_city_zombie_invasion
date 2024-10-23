using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class NpcScenceControl : MonoBehaviour
{
	private void Awake()
	{
		NpcScenceControl.instance = this;
		this.eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
		this.graphicRaycaster = UnityEngine.Object.FindObjectsOfType<GraphicRaycaster>();
		this.npcCam.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		this.checkScreen();
	}

	public void checkScreen()
	{
		float num = 1280f;
		float num2 = 720f;
		float num3 = (float)Screen.width;
		float num4 = (float)Screen.height;
		float num5 = num / num2;
		float num6 = num3 / num4;
		this.adjustor = num5 / num6;
	}

	public void ChangeView(bool reset = false)
	{
	}

	public void ResetPos()
	{
		this.curBlur.blurIterations = 2;
		this.curBlur.downsample = 1;
		this.ChangeView(false);
		base.transform.DORotateQuaternion(this.initPos.rotation, this.speed);
		base.transform.DOMove(this.initPos.position, this.speed, false).OnComplete(delegate
		{
			this.curBlur.enabled = false;
			this.openBlur = false;
			this.curBlur.blurIterations = 0;
			this.curBlur.downsample = 0;
			this.SetLayer(this.curMedicGO, 0);
			this.LookNpc(this.curMedicGO, false, false);
			this.SetLayer(this.curPropGO, 0);
			this.LookNpc(this.curPropGO, false, false);
			this.SetLayer(this.curTechGO, 0);
			this.LookNpc(this.curTechGO, false, false);
			this.SetLayer(this.curWeaponGO, 0);
			this.LookNpc(this.curWeaponGO, false, false);
			this.eventSystem.enabled = true;
		});
		this.openBlur = false;
	}

	public void LookNpc(GameObject go, bool can, bool Back = true)
	{
		go.GetComponent<NpcRoleControll>().canLook = can;
		go.GetComponent<NpcRoleControll>().canBack = Back;
	}

	public void SetLayer(GameObject go, int num)
	{
		go.layer = num;
		IEnumerator enumerator = go.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.layer = num;
				this.SetLayer(transform.gameObject, num);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	private bool CheckGuiRaycastObjects()
	{
		PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
		pointerEventData.pressPosition = UnityEngine.Input.mousePosition;
		pointerEventData.position = UnityEngine.Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		if (list != null && this.graphicRaycaster != null)
		{
			this.graphicRaycaster[0].Raycast(pointerEventData, list);
			this.graphicRaycaster[1].Raycast(pointerEventData, list);
			return list.Count > 0;
		}
		return false;
	}

	public void OnclickWeapon()
	{
	}

	public void OnclickEngineer()
	{
	}

	public void OnclickMedic()
	{
	}

	public void OnclickTech()
	{
	}

	public void Update()
	{
		if (this.openBlur)
		{
			this.curBlur.blurIterations = 1;
			this.curBlur.downsample = 1;
			this.curBlur.blurSize = 3f;
			this.curBlur.blurSize = Mathf.SmoothDamp(this.curBlur.blurSize, 3f, ref this.smooth, this.speed);
			this.curCamer.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(this.curCamer.GetComponent<Camera>().fieldOfView, this.initView * this.adjustor, ref this.smoothMain, this.moveSpeed * this.speed);
			this.npcCam.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(this.npcCam.GetComponent<Camera>().fieldOfView, this.initView * this.adjustor, ref this.smoothNpc, this.moveSpeed * this.speed);
			Mathf.Lerp(this.curBlur.blurSize, 3f, 0.5f);
		}
		else if ((double)this.curBlur.blurSize > 0.01)
		{
			Mathf.Lerp(this.curBlur.blurSize, 0f, 0.5f);
			this.curBlur.blurSize = 0f;
			this.curBlur.blurSize = Mathf.SmoothDamp(this.curBlur.blurSize, 0f, ref this.smooth, this.speed);
			this.curCamer.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(this.curCamer.GetComponent<Camera>().fieldOfView, this.initView, ref this.smoothMain, this.moveSpeed * this.speed);
			this.npcCam.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(this.curCamer.GetComponent<Camera>().fieldOfView, this.initView, ref this.smoothNpc, this.moveSpeed * this.speed);
		}
		else
		{
			this.curBlur.enabled = false;
			this.openBlur = false;
		}
		if (!this.CanShow && null != MainPage.instance)
		{
			MainPage.instance.InitBtnPos(this.techTrans, this.techEnterTrans, 0);
			MainPage.instance.InitBtnPos(this.medicTrans, this.medicEnterTrans, 1);
			MainPage.instance.InitBtnPos(this.weaponTrans, this.weaponEnterTrans, 2);
			MainPage.instance.InitBtnPos(this.propTrans, this.propEnterTrans, 3);
		}
		if (UnityEngine.Input.touchCount == 1 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began)
		{
			if (!this.CheckGuiRaycastObjects())
			{
				this.isPressUI = true;
				Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit) && null != MainPage.instance)
				{
					string tag = raycastHit.collider.gameObject.tag;
					if (tag != null)
					{
						if (!(tag == "Tech"))
						{
							if (!(tag == "Drug"))
							{
								if (!(tag == "Weapon"))
								{
									if (tag == "Prop")
									{
										if (this.isPressUI)
										{
											this.CanShow = true;
											MainPage.instance.InitBtnPos(this.propTrans, this.propEnterTrans, 3);
										}
									}
								}
								else if (this.isPressUI)
								{
									this.CanShow = true;
									MainPage.instance.InitBtnPos(this.weaponTrans, this.weaponEnterTrans, 2);
								}
							}
							else if (this.isPressUI)
							{
								this.CanShow = true;
								MainPage.instance.InitBtnPos(this.medicTrans, this.medicEnterTrans, 1);
							}
						}
						else if (this.isPressUI)
						{
							this.CanShow = true;
							MainPage.instance.InitBtnPos(this.techTrans, this.techEnterTrans, 0);
						}
					}
				}
			}
			else
			{
				this.isPressUI = false;
			}
		}
	}

	public GameObject RoomGo;

	public static NpcScenceControl instance;

	public bool CanMedic = true;

	public bool CanTech = true;

	public bool CanWeapon = true;

	public bool CanPop = true;

	public bool CanShow;

	public EventSystem eventSystem;

	public GraphicRaycaster[] graphicRaycaster;

	public Transform medicTrans;

	public Transform techTrans;

	public Transform weaponTrans;

	public Transform propTrans;

	public Transform medicEnterTrans;

	public Transform techEnterTrans;

	public Transform weaponEnterTrans;

	public Transform propEnterTrans;

	public Transform medicCamPos;

	public Transform techCamPos;

	public Transform weaponCamPos;

	public Transform propCamPos;

	public Transform initPos;

	public GameObject curMedicGO;

	public GameObject curPropGO;

	public GameObject curTechGO;

	public GameObject curWeaponGO;

	public GameObject[] RolesGo;

	public Transform[] RolesCamPos;

	public GameObject curCamer;

	public BlurOptimized curBlur;

	public bool openBlur;

	public Transform npcCam;

	[CNName("渐变速度")]
	public float speed = 0.5f;

	private float smooth;

	private float smoothMain;

	private float smoothNpc;

	private float adjustor;

	[CNName("初始视距")]
	public float initView = 50f;

	public Transform lookPos;

	public Transform backPos;

	public float moveSpeed;

	public GameObject Weapon01;

	public GameObject Weapon02;

	private bool isPressUI;
}
