using System;
using DG.Tweening;
using ui;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeachPage : GamePage, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	private bool _closed;
	
	private event Action OnWASD;
	private event Action OnMouseMove;
	private event Action OnQ;
	private event Action OnE;
	private event Action OnR;
	private event Action OnF;
	private event Action OnG;
	public void OnPointerDown(PointerEventData eventData)
	{
		this.TouchPosition = eventData.position;
		if (Singleton<GlobalData>.Instance.FirstMove == 1)
		{
			this.startMove = true;
		}
		else if (Singleton<GlobalData>.Instance.FirstShoot == 1)
		{
			this.startShoot = true;
			this.timeShoot -= Time.deltaTime;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		this.startMove = false;
		this.startShoot = false;
		if (Mathf.Abs(eventData.position.x - this.TouchPosition.x) > (float)(Screen.width / 8))
		{
			if (this.btnLeft.activeInHierarchy)
			{
				this.OnclickLeft();
				return;
			}
			if (this.btnRight.activeInHierarchy)
			{
				this.OnclickRight();
			}
		}
	}

	public override void Refresh()
	{
		base.Refresh();
		this.RefreshPage();
	}

	public new void OnEnable()
	{
		_closed = false;
		//this.RefreshPage();
	}

	public void RefreshPage()
	{
		if (teach == TeachType.NONE) return;
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, 0.2f);
		UnityEngine.Object.Destroy(this.Button);
		var isMobile = Singleton<UiControllers>.Instance.IsMobile;
		var keywordPlatform = isMobile ? "" : "_DESKTOP";
		Singleton<FontChanger>.Instance.SetFont(DescTxt);
		switch (this.teach)
		{
		case TeachType.MOVE:
			this.FirstObj.SetActive(true);
			this.PropObj.SetActive(false);
			this.btnLeft.SetActive(isMobile);
			this.btnRight.SetActive(false);
			this.WeaponObj.SetActive(false);
			this.DescTxt.text = Singleton<GlobalData>.Instance.GetText("MOVEGUIDE" + keywordPlatform);
			Singleton<UiControllers>.Instance.LeftJoystick.onStartInput += OnclickLeft;
			OnWASD += OnclickLeft;
			break;
		case TeachType.SHOOT:
			this.FirstObj.SetActive(true);
			this.PropObj.SetActive(false);
			this.btnLeft.SetActive(false);
			this.btnRight.SetActive(isMobile);
			this.WeaponObj.SetActive(false);
			this.DescTxt.text = Singleton<GlobalData>.Instance.GetText("SHOOTGUIDE" + keywordPlatform);
			Singleton<UiControllers>.Instance.RightJoystick.onStartInput += OnclickRight;
			OnMouseMove += OnclickRight;
			break;
		case TeachType.SHOULEI:
			this.FirstObj.SetActive(false);
			this.PropObj.SetActive(true);
			this.MedicObj.SetActive(false);
			this.ShouleiObj.SetActive(true);
			this.WeaponObj.SetActive(false);
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().curProp01Go, this.ShouleiObj.transform);
			this.DescTxt.text = Singleton<GlobalData>.Instance.GetText("SHOULEIGUIDE" + keywordPlatform);
			break;
		case TeachType.MEDIC:
			this.FirstObj.SetActive(false);
			this.PropObj.SetActive(true);
			this.MedicObj.SetActive(true);
			this.ShouleiObj.SetActive(false);
			this.WeaponObj.SetActive(false);
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().curProp02Go, this.MedicObj.transform);
			this.DescTxt.text = Singleton<GlobalData>.Instance.GetText("MEDICGUIDE" + keywordPlatform);
			break;
		case TeachType.PAOTAI:
			this.FirstObj.SetActive(false);
			this.PropObj.SetActive(false);
			this.btnLeft.SetActive(false);
			this.btnRight.SetActive(false);
			this.PaotaiObj.SetActive(true);
			this.WeaponObj.SetActive(false);
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().BatteryBtn.gameObject, this.PaotaiObj.transform);
			this.DescTxt.text = Singleton<GlobalData>.Instance.GetText("BATTERYGUIDE" + keywordPlatform);
			break;
		case TeachType.WEAPON:
			this.FirstObj.SetActive(false);
			this.PropObj.SetActive(false);
			this.MedicObj.SetActive(false);
			this.ShouleiObj.SetActive(false);
			this.WeaponObj.SetActive(true);
			this.Button = this.ProduceGo(Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().changeBtn.gameObject, this.WeaponObj.transform);
			this.DescTxt.text = Singleton<GlobalData>.Instance.GetText("WEAPONGUIDE" + keywordPlatform);
			break;
		}
	}

	public void Update()
	{
		if (Singleton<GlobalData>.Instance.FirstPaoTai == 1 && this.timeNum < Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().PaoTaiPressTime)
		{
			Singleton<GlobalData>.Instance.FirstPaoTai = 0;
			this.Close();
		}
		if (Singleton<GlobalData>.Instance.FirstMove == 1 && this.timeMove <= 0f)
		{
			this.OnclickLeft();
		}
		else if (this.timeMove > 0f && this.startMove)
		{
			this.timeMove -= Time.deltaTime;
		}
		else if (Singleton<GlobalData>.Instance.FirstShoot == 1 && this.timeShoot <= 0f)
		{
			this.OnclickRight();
		}
		else if (this.timeShoot > 0f && this.startShoot)
		{
			this.timeShoot -= Time.deltaTime;
		}
		CheckKeyToTutorial();
	}

	public void OnclickWeapon()
	{
		Singleton<GlobalData>.Instance.FirstWeapon = 0;
		this.Close();
	}

	public void OnclickMedic()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().SetGameState(PlayingState.GamePlaying);
		Singleton<GlobalData>.Instance.FirstMedic = 0;
		this.Close();
	}

	public void OnclickShouLei()
	{
		Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
		Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().SetGameState(PlayingState.GamePlaying);
		Singleton<GlobalData>.Instance.FirstShouLei = 0;
		this.Close();
	}

	public void OnclickLeft()
	{
		OnWASD = null;
		Singleton<UiControllers>.Instance.LeftJoystick.onStartInput -= OnclickLeft;
		Singleton<GlobalData>.Instance.FirstMove = 0;
		Debug.Log("OnClickLeft");
		this.Close();
	}

	public void OnclickRight()
	{
		OnMouseMove = null;
		Singleton<UiControllers>.Instance.RightJoystick.onStartInput -= OnclickRight;
		Singleton<GlobalData>.Instance.FirstShoot = 0;
		Singleton<UiManager>.Instance.GetPage(PageName.InGamePage).GetComponent<InGamePage>().SetInputMovePlayer(true);
		Debug.Log("OnClickRight");
		this.Close();
	}

	public override void OnBack()
	{
	}

	public GameObject ProduceGo(GameObject go, Transform Pos01)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
		gameObject.transform.SetParent(Pos01);
		gameObject.transform.localScale = go.transform.localScale;
		gameObject.transform.position = go.transform.position;
		return gameObject;
	}

	public void SetOnCloseCallback(Action callback)
	{
		this.onCloseCallback = callback;
	}

	public override void Close()
	{
		if (_closed) return;
		_closed = true;
		teach = TeachType.NONE;
		DOVirtual.DelayedCall(0.3f, base.Close);
		//base.Close();
		if (this.onCloseCallback != null)
		{
			this.onCloseCallback();
			this.onCloseCallback = null;
		}
	}
	
	private void CheckKeyToTutorial()
	{
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		{
			OnWASD?.Invoke();
		}

		if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
		{
			OnMouseMove?.Invoke();
		}
	}

	public GameObject FirstObj;

	public GameObject PropObj;

	public GameObject ShouleiObj;

	public GameObject MedicObj;

	public GameObject btnLeft;

	public GameObject btnRight;

	public GameObject PaotaiObj;

	public GameObject PaotaiEffect;

	public GameObject WeaponObj;

	public Transform oldPos;

	public Text DescTxt;

	private bool isMove;

	private Vector3 TouchPosition;

	public TeachType teach = TeachType.NONE;

	public GameObject Button;

	public CanvasGroup canvasGroup;

	private float timeNum = 3f;

	private float timeMove = 2f;

	private float timeShoot = 2f;

	private bool startMove;

	private bool startShoot;

	protected Action onCloseCallback;
}
