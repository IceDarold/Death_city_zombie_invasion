using System;
using System.Collections.Generic;
using DataCenter;
using DG.Tweening;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RolePanel : MonoBehaviour
{
	private bool CheckGuiRaycastObjects()
	{
		PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
		pointerEventData.pressPosition = UnityEngine.Input.mousePosition;
		pointerEventData.position = UnityEngine.Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		this.graphicRaycaster.Raycast(pointerEventData, list);
		return list.Count > 0;
	}

	private void Awake()
	{
		this.startModelRotate = this.CameraFocus.transform.localRotation;
		this.eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
		this.graphicRaycaster = UnityEngine.Object.FindObjectOfType<GraphicRaycaster>();
		this.RoleDataList = RoleDataManager.Roles;
		this.RoleRoot.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(180 * this.RoleDataList.Count), 144f);
		for (int i = 0; i < this.RoleDataList.Count; i++)
		{
			int index = i;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/RoleChild")) as GameObject;
			gameObject.transform.SetParent(this.RoleRoot);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			RoleInfoChild component = gameObject.GetComponent<RoleInfoChild>();
			this.Roles.Add(component);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.SelectRole(index);
			});
		}
	}

	private void OnEnable()
	{
		this.CurrentRole = RoleDataManager.GetRoleData(PlayerDataManager.Player.Role);
		this.CameraFocus.transform.localRotation = this.startModelRotate;
		this.selectIndex = RoleDataManager.Roles.IndexOf(this.CurrentRole);
		this.SelectImage.position = this.Roles[this.selectIndex].transform.position;
		this.SelectImage.gameObject.SetActive(false);
		this.RefreshRoles();
		this.RefreshInfo();
		this.InfoLayer.localPosition = new Vector3(850f, 60f, 0f);
		this.InfoLayer.DOLocalMove(this.RightAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
		this.RoleLayer.localPosition = new Vector3(0f, -500f, 0f);
		this.RoleLayer.DOLocalMove(this.DownAnchor.localPosition, 0.5f, false).SetEase(Ease.OutExpo);
		this.ShowRoleModel();
	}

	public void ShowRoleModel()
	{
		for (int i = 0; i < this.RoleModel.Length; i++)
		{
			this.RoleModel[i].SetActive(i == this.selectIndex);
			this.RoleModel[i].GetComponent<Animator>().SetBool("Show", true);
		}
	}

	public void SetRoleAnimator(bool isShow)
	{
		this.RoleModel[this.selectIndex].GetComponent<Animator>().SetBool("Show", isShow);
	}

	private void RefreshRoles()
	{
		this.count = 0;
		for (int i = 0; i < this.Roles.Count; i++)
		{
			this.Roles[i].Name.text = Singleton<GlobalData>.Instance.GetText(this.RoleDataList[i].Name);
			Singleton<FontChanger>.Instance.SetFont(Roles[i].Name);
			if (i == this.selectIndex)
			{
				this.Roles[i].SelectImage.gameObject.SetActive(true);
			}
			else
			{
				this.Roles[i].SelectImage.gameObject.SetActive(false);
			}
			if (this.RoleDataList[i].ID == PlayerDataManager.Player.Role)
			{
				this.Roles[i].UsingImage.gameObject.SetActive(true);
			}
			else
			{
				this.Roles[i].UsingImage.gameObject.SetActive(false);
			}
			if (this.RoleDataList[i].Enable)
			{
				this.Roles[i].LockImage.gameObject.SetActive(false);
				if (this.RoleDataList[i].isNew)
				{
					this.count++;
					this.Roles[i].NewImage.gameObject.SetActive(true);
				}
				else
				{
					this.Roles[i].NewImage.gameObject.SetActive(false);
				}
			}
			else
			{
				this.Roles[i].NewImage.gameObject.SetActive(false);
				this.Roles[i].LockImage.gameObject.SetActive(true);
			}
		}
		if (this.count > 0)
		{
			this.RoleRedPoint.SetActive(true);
		}
		else
		{
			this.RoleRedPoint.SetActive(false);
		}
	}

	private void RefreshInfo()
	{
		RoleData roleData = RoleDataManager.Roles[this.selectIndex];
		this.RoleName.text = Singleton<GlobalData>.Instance.GetText(roleData.Name);
		this.RoleDescribe.text = Singleton<GlobalData>.Instance.GetText(roleData.Describe);
		List<RoleAttribute> attribute = RoleDataManager.GetAttribute(roleData, true);
		Singleton<FontChanger>.Instance.SetFont(RoleName);
		Singleton<FontChanger>.Instance.SetFont(RoleDescribe);
		if (attribute.Count == 0)
		{
			this.RoleQte.gameObject.SetActive(false);
		}
		else
		{
			this.RoleQte.name = Singleton<GlobalData>.Instance.GetText(attribute[0].Name);
			this.RoleQte.Icon.sprite = Singleton<UiManager>.Instance.GetSprite(attribute[0].Icon);
			this.RoleQte.gameObject.SetActive(true);
			
		}
		List<RoleAttribute> attribute2 = RoleDataManager.GetAttribute(roleData, false);
		for (int i = 0; i < attribute2.Count; i++)
		{
			if (i < roleData.Attributes.Length)
			{
				this.RoleSkills[i].Icon.sprite = Singleton<UiManager>.Instance.GetSprite(attribute2[i].Icon);
				this.RoleSkills[i].Describe.text = Singleton<GlobalData>.Instance.GetText(attribute2[i].Describe);
				this.RoleSkills[i].gameObject.SetActive(true);
				Singleton<FontChanger>.Instance.SetFont(RoleSkills[i].Describe);
			}
			else
			{
				this.RoleSkills[i].gameObject.SetActive(false);
			}
		}
		if (roleData.Enable)
		{
			this.FuncitonButton.gameObject.SetActive(true);
			this.FuncitonTxt.gameObject.SetActive(false);
			if (this.CurrentRole.ID == PlayerDataManager.Player.Role)
			{
				this.FunctionButtonName.text = Singleton<GlobalData>.Instance.GetText("USING");
				this.FuncitonButton.interactable = false;
			}
			else
			{
				this.FunctionButtonName.text = Singleton<GlobalData>.Instance.GetText("CHOOSE");
				this.FuncitonButton.interactable = true;
			}
		}
		else if (roleData.ID == 1002)
		{
			this.FuncitonTxt.text = Singleton<GlobalData>.Instance.GetText("ROLE02_UNLOCK");
			this.FuncitonTxt.gameObject.SetActive(true);
			this.FuncitonButton.gameObject.SetActive(false);
		}
		else
		{
			this.FunctionButtonName.text = Singleton<GlobalData>.Instance.GetText("GOTOBUY");
			this.FuncitonButton.gameObject.SetActive(true);
			this.FuncitonTxt.gameObject.SetActive(false);
			this.FuncitonButton.interactable = true;
		}
		Singleton<FontChanger>.Instance.SetFont(FunctionButtonName);
		Singleton<FontChanger>.Instance.SetFont(FuncitonTxt);
		this.ShowRoleModel();
	}

	private void SelectRole(int index)
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		if (this.selectIndex == index)
		{
			return;
		}
		this.CameraFocus.transform.localRotation = this.startModelRotate;
		this.SelectImage.position = this.Roles[this.selectIndex].transform.position;
		this.SelectImage.gameObject.SetActive(true);
		this.Roles[this.selectIndex].SelectImage.gameObject.SetActive(false);
		this.selectIndex = index;
		this.CurrentRole = this.RoleDataList[this.selectIndex];
		if (this.CurrentRole.isNew)
		{
			RoleDataManager.RemoveNewTag(this.CurrentRole.ID);
			this.count--;
			if (this.count > 0)
			{
				this.RoleRedPoint.SetActive(true);
			}
			else
			{
				this.RoleRedPoint.SetActive(false);
			}
		}
		this.SelectImage.DOMoveX(this.Roles[index].transform.position.x, 0.2f, false).OnComplete(delegate
		{
			for (int i = 0; i < this.Roles.Count; i++)
			{
				if (i == this.selectIndex)
				{
					this.Roles[i].SelectImage.gameObject.SetActive(true);
				}
				else
				{
					this.Roles[i].SelectImage.gameObject.SetActive(false);
				}
			}
			this.SelectImage.gameObject.SetActive(false);
		});
		this.RefreshInfo();
		this.RefreshRoles();
	}

	public void ClickOnFunctionButton()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		RoleData roleData = RoleDataManager.Roles[this.selectIndex];
		if (roleData.Enable)
		{
			if (roleData.ID != PlayerDataManager.Player.Role)
			{
				PlayerDataManager.SelectRole(roleData.ID);
			}
		}
		else if (roleData.ID != 1002)
		{
			Singleton<UiManager>.Instance.ShowStorePage(0);
		}
		this.RefreshInfo();
		this.RefreshRoles();
	}

	private void Update()
	{
		if (this.autoRotate)
		{
			this.CameraFocus.transform.Rotate(0f, 10f * Time.deltaTime, 0f, Space.World);
		}
		if (UnityEngine.Input.touchCount > 0)
		{
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began)
			{
				if (!this.CheckGuiRaycastObjects())
				{
					this.isPressUI = true;
					Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.GetTouch(0).position);
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit))
					{
						if (raycastHit.collider.gameObject.tag == "Player")
						{
							this.canRotate = true;
						}
						else
						{
							this.canRotate = false;
						}
					}
					if (this.canRotate)
					{
						this.pos_1 = UnityEngine.Input.GetTouch(0).position;
						this.dis_x = 0f;
						this.dis_y = 0f;
						this.startpoint = UnityEngine.Input.GetTouch(0).position;
						this.speed = Vector3.zero;
						this.timeMove = 0.01f;
					}
					this.autoRotate = false;
				}
				else
				{
					this.isPressUI = false;
				}
			}
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved && this.isPressUI && this.canRotate)
			{
				this.CameraFocus.transform.Rotate(0f, -(UnityEngine.Input.GetTouch(0).position.x - this.pos_1.x) * 0.05f, 0f, Space.World);
				this.pos_1 = UnityEngine.Input.GetTouch(0).position;
				this.timeMove += Time.deltaTime;
			}
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended && this.isPressUI && this.canRotate)
			{
				this.dis_x = UnityEngine.Input.GetTouch(0).position.x - this.pos_1.x;
				this.curpoint = UnityEngine.Input.GetTouch(0).position;
				this.lerppoint = this.curpoint - this.startpoint;
				this.speed = this.lerppoint / this.timeMove;
				this.speed.x = Mathf.Clamp(this.speed.x, -2000f, 2000f);
			}
		}
		if (UnityEngine.Input.touchCount > 1)
		{
			if (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved || UnityEngine.Input.GetTouch(1).phase == TouchPhase.Moved)
			{
				Vector2 position = UnityEngine.Input.GetTouch(0).position;
				Vector2 position2 = UnityEngine.Input.GetTouch(1).position;
			}
		}
		if (this.canRotate)
		{
			if (this.speed.x > 10f)
			{
				this.speed.x = this.speed.x - 500f * Time.deltaTime;
			}
			else if (this.speed.x < -10f)
			{
				this.speed.x = this.speed.x + 500f * Time.deltaTime;
			}
			if (this.speed.x > 10f || this.speed.x < -10f)
			{
				this.CameraFocus.transform.Rotate(0f, -this.speed.x * 0.1f * Time.deltaTime, 0f, Space.World);
			}
		}
	}

	public const string AnimatorParmeter = "Show";

	public Transform InfoLayer;

	public Transform RoleLayer;

	public Text RoleName;

	public Text RoleDescribe;

	public Transform RoleRoot;

	public Transform SelectImage;

	public Button FuncitonButton;

	public Text FuncitonTxt;

	public Text FunctionButtonName;

	public RoleQteChild RoleQte;

	public List<RoleSkillChild> RoleSkills = new List<RoleSkillChild>();

	private List<RoleInfoChild> Roles = new List<RoleInfoChild>();

	public Transform RightAnchor;

	public Transform DownAnchor;

	public GameObject RoleRedPoint;

	public GameObject[] RoleModel;

	private int count;

	private List<RoleData> RoleDataList = new List<RoleData>();

	private RoleData CurrentRole = new RoleData();

	private int selectIndex;

	private Quaternion startModelRotate;

	public EventSystem eventSystem;

	public GraphicRaycaster graphicRaycaster;

	public GameObject CameraFocus;

	public bool autoRotate;

	private Vector3 startpoint;

	private Vector3 curpoint;

	private Vector3 lerppoint;

	private Vector3 pos_1;

	private Vector3 pos_2;

	private float dis_x;

	private float dis_y;

	private bool canRotate;

	private bool isPressUI;

	private Vector3 speed;

	private float timeMove;
}
