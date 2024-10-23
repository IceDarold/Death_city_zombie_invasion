using DataCenter;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ui;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zombie3D;

public class InGamePage : GamePage, IGameUIControl
{
    [CompilerGenerated]
    private sealed class _003CCloseNPCTip_003Ec__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal InGamePage _0024this;

        internal object _0024current;

        internal bool _0024disposing;

        internal int _0024PC;

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this._0024current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this._0024current;
            }
        }

        [DebuggerHidden]
        public _003CCloseNPCTip_003Ec__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._0024PC;
            this._0024PC = -1;
            switch (num)
            {
                case 0u:
                    this._0024current = new WaitForSeconds(3f);
                    if (!this._0024disposing)
                    {
                        this._0024PC = 1;
                    }
                    return true;
                case 1u:
                    this._0024this.NpcHurtGo.SetActive(false);
                    this._0024PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Dispose()
        {
            this._0024disposing = true;
            this._0024PC = -1;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }
    }

    public Text gameCoinTxt;

    public Text scoreNameTxt;

    public Text scoreTxt;

    public Text distanceTxt;

    public Text propNumTxt;

    public Text prop2NumTxt;

    public Text prop3NumTxt;

    public Text curbulletNum;

    public Text bulletCount;

    public Text taskCotentTxt;

    public Text taskCotent2Txt;

    public Text killNumTxt;

    public Text currentNumTxt;

    public Text bossTimeTxt;

    public Image propImg;

    public Image prop2Img;

    public Image prop3Img;

    public Image weaponImg;

    public GameObject NpcHurtGo;

    public DOTweenAnimation NpcHurtAni;

    public Text NpcTipTxt;

    public Image weaponcdImg;

    public Image curTaskIcon;

    public Image[] propcdImg;

    public Image QteImg;

    public Image NpcIconImg;

    public Button[] propBtn;

    public Button changeBtn;

    public Sprite[] spriteCollection;

    public Slider bulletSlider;

    public Slider taskProcess;

    public Slider playerSlider;

    public Slider npcSlider;

    public Slider BossTimeSlider;

    public GameObject changeWeapon;

    public GameObject tipEndLessPop;

    public GameObject curTaskGo;

    public GameObject CanShoot;

    public GameObject HeadGo;

    public GameObject KillsGo;

    public GameObject SpecialKillGo;

    public GameObject DangerGo;

    public GameObject HurtGo;

    public GameObject HeadParticlGo;

    public GameObject ProcessGo;

    public GameObject ParticlDrug;

    public GameObject LimitLevel;

    public GameObject BossLevel;

    public GameObject NomalLevel;

    public GameObject BatteryLevel;

    public GameObject BatteryBtn;

    public GameObject QteGO;

    public GameObject PauseBtn;

    public Button PauseBtnInRole;

    public Button QteButton;

    public GameObject curProp01Go;

    public GameObject curProp02Go;

    public GameObject curPropsGo;

    public GameObject curTaskDes;

    public GameObject curTaskTarget;

    public GameObject EffectObjMedic;

    public DOTweenAnimation taskTipAni;

    public DOTweenAnimation processAni;

    public GameObject WeaponInfo;

    public GameObject circleProgress;

    public GameObject bulletProgress;

    public Image circlePercent;

    public RectTransform[] bulletsBg;

    public RectTransform[] bulletFg;

    protected const float BULLET_IMAGE_WIDTH = 27f;

    public GameObject ProcessAddBullet;

    public Text AddBulletTxt;

    public Slider AddBulletSlider;

    public GameObject DebrisGo;

    public Text NumDebris;

    public Image IconDebrisWeapon;

    public Slider DebrisSlider;

    public GameObject bossSkillEffect;

    public float PaoTaiPressTime;

    private bool startPaotai;

    private bool startEffect;

    private int mainWeaponBullet;

    private int secondWeaponBullet;

    private PropData curProp;

    private PropData curProp2;

    private PropData curProp3;

    private WeaponData curMainWeapon;

    private WeaponData curSecondWeapon;

    private WeaponData curWeapon;

    private int weaponIndex;

    private IPlayerControl player;

    private IGameSceneControl gameScene;

    private TimeScheduler pathFindScheduler;

    private Mission curMission;

    private Quaternion missionTargetDir;

    public Transform missionGuideObj;

    public Transform PosMissionGuide;

    public Transform PickUpParent;

    public GameObject radar;

    public GameObject NoArmor;

    private float boosTimeNum;

    private float perTime;

    private bool openVip;

    private float timeSlider;

    private float timeChange;

    private float timePick = 0.25f;

    private float timeQteCd;

    private float timeProp1Cd;

    private float timeProp2Cd;

    private float timeProp3Cd;

    private float maxTime = 3f;

    private float maxPropTime = 5f;

    private float maxQteTime = 40f;

    private Vector3 vectorTaskGuild;

    private Vector3 vectorTaskDes;

    private Vector3 vectorTaskTarget;

    private CheckpointData curData;

    private List<GameObject> listPickGo = new List<GameObject>();

    private List<PickUpData> listPickUpDataGO = new List<PickUpData>();

    public MissionStateUI mState;

    private int curDNA;

    private int curGold;

    private int curMaxProp01;

    private int curMaxProp02;

    private List<DebrisData> listDebris = new List<DebrisData>();

    private TeachPage curTeachPage;

    private Dictionary<int, int> tempProp = new Dictionary<int, int>();

    private TreasureBox triggeredTreasureBox;

    public List<RectTransform> pointTags;

    public PlayerShootBtn playerShootBtn;

    public SnipeUI snipeUI;

    public GameObject normalUI;

    public GameObject btnTreasureBox;

    private int BLOOD = 5;

    private int WeaponId;

    private int Bullets;

    private int goldNum;

    private int num;

    public Mission CurMission
    {
        get
        {
            return this.curMission;
        }
    }

    public new void Awake()
    {
        GameLogManager.SendPageLog(base.Name.ToString(), 8.ToString() + this.SendCheckPointLog());
        GameLogManager.StartGameLog();
        this.listPickGo.Clear();
        this.listPickUpDataGO.Clear();
        this.curTeachPage = null;
        Singleton<GlobalData>.Instance.ListDropDebris.Clear();
        this.vectorTaskGuild = this.PosMissionGuide.position;
        this.vectorTaskDes = this.curTaskDes.transform.position;
        this.vectorTaskTarget = this.curTaskTarget.transform.position;
        this.curData = CheckpointDataManager.SelectCheckpoint;
        if (this.curData.Chapters == ChapterEnum.CHAPTERNAME_01 && this.curData.Index <= 4 && Singleton<GlobalData>.Instance.FirstShouLei == 2)
        {
            this.curPropsGo.SetActive(false);
            this.PauseBtnInRole.enabled = false;
            this.PauseBtn.SetActive(false);
        }
        else
        {
            this.curPropsGo.SetActive(true);
            this.curProp01Go.SetActive(true);
            this.curProp02Go.SetActive(true);
            this.PauseBtnInRole.enabled = true;
            this.PauseBtn.SetActive(true);
        }
        RoleData roleData = RoleDataManager.GetRoleData(PlayerDataManager.Player.Role);
        if (RoleDataManager.GetAttribute(roleData, true).Count == 0)
        {
            this.QteGO.SetActive(false);
        }
        else
        {
            this.QteGO.SetActive(true);
            ((Component)this.QteButton).GetComponent<Image>().sprite = Singleton<UiManager>.Instance.GetSprite(RoleDataManager.GetAttribute(roleData, true)[0].Icon);
        }
        switch (CheckpointDataManager.SelectCheckpoint.Type)
        {
            case CheckpointType.BATTERY:
                this.QteGO.SetActive(false);
                this.NomalLevel.SetActive(false);
                this.WeaponInfo.SetActive(false);
                this.BossLevel.SetActive(false);
                this.BatteryLevel.SetActive(true);
                this.changeBtn.gameObject.SetActive(false);
                this.radar.gameObject.SetActive(false);
                break;
            case CheckpointType.SNIPE:
            case CheckpointType.CARRY:
            case CheckpointType.COLLECT:
            case CheckpointType.ARRIVE:
            case CheckpointType.CONVOY:
            case CheckpointType.REPAIR:
                this.NomalLevel.SetActive(true);
                this.BatteryLevel.SetActive(false);
                this.WeaponInfo.SetActive(true);
                this.BossLevel.SetActive(false);
                this.changeBtn.gameObject.SetActive(true);
                this.radar.gameObject.SetActive(true);
                break;
        }
        this.InitData();
        this.RefreshPage();
        this.pathFindScheduler = new TimeScheduler(0.2f, 0f, new Action(this.ResetMissionDir), new Func<bool>(this.CheckMission));
        Singleton<GlobalData>.Instance.GameTimes = 0f;
        this.curDNA = 0;
        this.curGold = 0;
        this.listDebris.Clear();
    }

    public void OnDestroy()
    {
        if ((UnityEngine.Object)Singleton<UiManager>.Instance.CurrentPage != (UnityEngine.Object)null && Singleton<UiManager>.Instance.CurrentPage.Name == PageName.InGamePage)
        {
            base.Close();
        }
    }

    public string SendCheckPointLog()
    {
        CheckpointData selectCheckpoint = CheckpointDataManager.SelectCheckpoint;
        CheckpointType type = selectCheckpoint.Type;
        if (type == CheckpointType.MAINLINE)
        {
            return 1.ToString() + "_ID:" + selectCheckpoint.ID;
        }
        return selectCheckpoint.Type.ToString() + "_ID:" + selectCheckpoint.ID;
    }

    public new void OnEnable()
    {
        Singleton<GameAudioManager>.Instance.PlaySoundInGame(Singleton<GameAudioManager>.Instance.GameSoundClip, true);
        this.NpcHurtGo.SetActive(false);
        if ((UnityEngine.Object)this.curTeachPage != (UnityEngine.Object)null)
        {
            this.curTeachPage.gameObject.SetActive(true);
        }
        if (this.curMission != null)
        {
            this.SetTaskDecscription();
        }
    }

    public void OnDisable()
    {
        if ((UnityEngine.Object)this.curTeachPage != (UnityEngine.Object)null)
        {
            this.curTeachPage.gameObject.SetActive(false);
        }
        Singleton<GlobalData>.Instance.GameDNA = this.curDNA;
        Singleton<GlobalData>.Instance.GameGoldCoins = this.curGold;
        Singleton<GlobalData>.Instance.ListGameDebris = this.listDebris;
    }

    public override void OnBack()
    {
        if (this.PauseBtn.activeInHierarchy && GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.Changing && GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.WaitForEnd)
        {
            base.Close();
            this.SetGameState(PlayingState.GamePause);
            GameApp.GetInstance().GetGameScene().GetPlayer()
                .SetPlayerIdle(false);
            Singleton<UiManager>.Instance.ShowPage(PageName.PausePage, null);
        }
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void InitData()
    {
        this.weaponIndex = Singleton<GlobalData>.Instance.InGameWeaponIndex;
        this.curMainWeapon = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon1);
        this.curSecondWeapon = WeaponDataManager.GetWeaponData(PlayerDataManager.Player.Weapon2);
        if (PlayerDataManager.Player.Prop2 != 0)
        {
            this.curProp2 = PropDataManager.GetPropData(PlayerDataManager.Player.Prop2);
            if (this.curProp2.Count > this.curProp2.MaxCarryCount)
            {
                this.curMaxProp02 = this.curProp2.MaxCarryCount;
            }
            else
            {
                this.curMaxProp02 = this.curProp2.Count;
            }
        }
        if (PlayerDataManager.Player.Prop1 != 0)
        {
            this.curProp = PropDataManager.GetPropData(PlayerDataManager.Player.Prop1);
            if (this.curProp.Count > this.curProp.MaxCarryCount)
            {
                this.curMaxProp01 = this.curProp.MaxCarryCount;
            }
            else
            {
                this.curMaxProp01 = this.curProp.Count;
            }
        }
        if (PlayerDataManager.Player.Weapon2 == -1)
        {
            goto IL_0116;
        }
        goto IL_0116;
    IL_0116:
        if (this.weaponIndex == 0)
        {
            this.curWeapon = this.curMainWeapon;
        }
        else
        {
            this.curWeapon = this.curSecondWeapon;
        }
    }

    public bool CheckListPick()
    {
        for (int i = 0; i < this.listPickGo.Count; i++)
        {
            if (!this.listPickGo[i].activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    public void ShowPickup(DropItemType dropItemType, int num = 1)
    {
        if (dropItemType == DropItemType.DEBRIS)
        {
            DebrisData debrisData = DebrisDataManager.CollectDropDebris();
            ItemData itemData = ItemDataManager.GetItemData(debrisData.ItemID);
            this.listDebris.Add(debrisData);
            if (itemData.ItemTag == DataCenter.ItemType.Prop)
            {
                PropData propData = PropDataManager.GetPropData(itemData.ID);
                this.NumDebris.text = debrisData.Count + "/" + propData.RequiredDebris;
                this.DebrisSlider.value = (float)debrisData.Count;
                this.DebrisSlider.maxValue = (float)propData.RequiredDebris;
                this.IconDebrisWeapon.sprite = Singleton<UiManager>.Instance.GetSprite(propData.Icon);
            }
            else if (itemData.ItemTag == DataCenter.ItemType.Weapon)
            {
                WeaponData weaponData = WeaponDataManager.GetWeaponData(itemData.ID);
                this.NumDebris.text = debrisData.Count + "/" + weaponData.RequiredDebris;
                this.DebrisSlider.value = (float)debrisData.Count;
                this.DebrisSlider.maxValue = (float)weaponData.RequiredDebris;
                this.IconDebrisWeapon.sprite = Singleton<UiManager>.Instance.GetSprite(weaponData.Icon);
                if (weaponData.State != 0)
                {
                    Singleton<GlobalData>.Instance.UnlockWd = weaponData;
                }
                else
                {
                    Singleton<GlobalData>.Instance.UnlockWd = null;
                }
            }
            Singleton<GlobalData>.Instance.ListDropDebris.Add(debrisData);
            Singleton<GlobalData>.Instance.InGameCurItem = debrisData;
            Singleton<GlobalData>.Instance.InGameState = (int)GameApp.GetInstance().GetGameScene().PlayingState;
            Singleton<UiManager>.Instance.ShowPage(PageName.PickInGamePage, null);
        }
        else
        {
            PickUpData pickUpData = new PickUpData();
            pickUpData.dropItemType = dropItemType;
            switch (dropItemType)
            {
                case DropItemType.BLOOD:
                    pickUpData.num = num * this.BLOOD;
                    break;
                case DropItemType.BULLET:
                    pickUpData.num = num * this.Bullets;
                    break;
                case DropItemType.GOLD:
                    if (CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.GOLD)
                    {
                        this.curGold += num * CheckpointDataManager.SelectCheckpoint.AwardCount[0] / 80;
                        pickUpData.num = num * CheckpointDataManager.SelectCheckpoint.AwardCount[0] / 80;
                    }
                    else
                    {
                        this.curGold += num * CheckpointDataManager.GetCurrentCheckpoint().AwardCount[0] / 80;
                        pickUpData.num = num * CheckpointDataManager.GetCurrentCheckpoint().AwardCount[0] / 80;
                    }
                    break;
                case DropItemType.DNA:
                    this.curDNA += num * 10;
                    pickUpData.num = num * 10;
                    break;
            }
            this.listPickUpDataGO.Add(pickUpData);
        }
    }

    public void ShowPickItem(DropItemType dropItemType = DropItemType.GOLD, int num = 0)
    {
        dropItemType = this.listPickUpDataGO[0].dropItemType;
        num = this.listPickUpDataGO[0].num;
        if (this.listPickGo.Count == 0 || !this.CheckListPick())
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/PickUpItem")) as GameObject;
            PickUpItem component = gameObject.GetComponent<PickUpItem>();
            gameObject.transform.SetParent(this.PickUpParent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            switch (dropItemType)
            {
                case DropItemType.BLOOD:
                    component.Num.text = "+" + num;
                    component.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[8];
                    break;
                case DropItemType.BULLET:
                    component.Num.text = "+" + num;
                    component.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[9];
                    break;
                case DropItemType.GOLD:
                    UnityEngine.Debug.Log(CheckpointDataManager.SelectCheckpoint.AwardCount[0]);
                    component.Num.text = "+" + num;
                    component.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[0];
                    break;
                case DropItemType.DNA:
                    component.Num.text = "＋" + num;
                    component.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[5];
                    break;
            }
            this.listPickGo.Add(gameObject);
            this.listPickUpDataGO.Remove(this.listPickUpDataGO[0]);
        }
        else
        {
            int num2 = 0;
            while (true)
            {
                if (num2 < this.listPickGo.Count)
                {
                    if (this.listPickGo[num2].activeInHierarchy)
                    {
                        num2++;
                        continue;
                    }
                    break;
                }
                return;
            }
            PickUpItem component2 = this.listPickGo[num2].GetComponent<PickUpItem>();
            switch (dropItemType)
            {
                case DropItemType.BLOOD:
                    component2.Num.text = "+" + num;
                    component2.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[8];
                    break;
                case DropItemType.BULLET:
                    component2.Num.text = "+" + num;
                    component2.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[9];
                    break;
                case DropItemType.GOLD:
                    component2.Num.text = "+" + num;
                    component2.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[0];
                    break;
                case DropItemType.DNA:
                    component2.Num.text = "＋" + num;
                    component2.Icon.sprite = Singleton<UiManager>.Instance.CommonSprites[5];
                    break;
            }
            component2.gameObject.SetActive(true);
            this.listPickUpDataGO.Remove(this.listPickUpDataGO[0]);
        }
    }

    public void Update()
    {
        if (this.startPaotai)
        {
            this.PaoTaiPressTime += Time.deltaTime;
        }
        this.pathFindScheduler.DoUpdate(Time.deltaTime);
        this.missionGuideObj.localRotation = Quaternion.Lerp(this.missionGuideObj.localRotation, this.missionTargetDir, Time.deltaTime * 5f);
        if (this.timeQteCd > 0f)
        {
            this.QteButton.enabled = false;
            this.timeQteCd -= Time.deltaTime;
            this.QteImg.fillAmount = this.timeQteCd / this.maxQteTime;
        }
        else
        {
            this.QteButton.enabled = true;
            this.QteImg.fillAmount = 0f;
            this.timeQteCd = 0f;
        }
        if (this.timeProp1Cd > 0f)
        {
            this.propBtn[0].enabled = false;
            this.timeProp1Cd -= Time.deltaTime;
            this.propcdImg[0].fillAmount = this.timeProp1Cd / this.maxPropTime;
        }
        else
        {
            this.propBtn[0].enabled = true;
            this.propcdImg[0].fillAmount = 0f;
            this.timeProp1Cd = 0f;
        }
        if (this.timeProp2Cd > 0f)
        {
            this.propBtn[1].enabled = false;
            this.timeProp2Cd -= Time.deltaTime;
            this.propcdImg[1].fillAmount = this.timeProp2Cd / this.maxPropTime;
        }
        else
        {
            this.propBtn[1].enabled = true;
            this.propcdImg[1].fillAmount = 0f;
            this.timeProp2Cd = 0f;
        }
        if (GameApp.GetInstance().GetGameScene() != null)
        {
            if (GameApp.GetInstance().GetGameScene().PlayingState != 0 && GameApp.GetInstance().GetGameScene().PlayingState != PlayingState.WaitForEnd)
            {
                return;
            }
            Singleton<GlobalData>.Instance.GameTimes += Time.deltaTime;
            if (this.listPickUpDataGO.Count > 0 && this.timePick <= 0f)
            {
                this.timePick = 0.25f;
                this.ShowPickItem(DropItemType.GOLD, 0);
            }
            else if (this.listPickUpDataGO.Count > 0)
            {
                this.timePick -= Time.deltaTime;
            }
        }
    }

    private bool CheckMission()
    {
        return this.curMission != null && this.curMission.mType != 0 && this.curMission.mType != EMission.KILL_WAVE && this.curMission.mType != EMission.NONE;
    }

    private void ResetMissionDir()
    {
        Vector3[] playerMissionPath = this.gameScene.GetPlayerMissionPath();
        if (playerMissionPath != null)
        {
            if (playerMissionPath.Length > 1)
            {
                Vector3 vector = this.player.GetTransform().InverseTransformPoint(playerMissionPath[1]);
                this.missionTargetDir = Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z));
            }
            this.gameScene.GetMissionPath().Reset(playerMissionPath);
        }
    }

    public void CheckNpcHp(float cur, float max)
    {
        this.npcSlider.value = cur / max;
    }

    public void ShowNpcHurtTip()
    {
        this.NpcTipTxt.text = Singleton<GlobalData>.Instance.GetText("NPC_TIP");
        Singleton<FontChanger>.Instance.SetFont(NpcTipTxt);
        this.NpcHurtGo.SetActive(true);
        this.NpcHurtAni.DORestart(false);
        base.StartCoroutine(this.CloseNPCTip());
    }

    [DebuggerHidden]
    private IEnumerator CloseNPCTip()
    {
        _003CCloseNPCTip_003Ec__Iterator0 _003CCloseNPCTip_003Ec__Iterator = new _003CCloseNPCTip_003Ec__Iterator0();
        _003CCloseNPCTip_003Ec__Iterator._0024this = this;
        return _003CCloseNPCTip_003Ec__Iterator;
    }

    public void SetBossHp(float cur, float max)
    {
        this.BossTimeSlider.value = cur / max;
    }

    public void CheckTaskValue(float cur, float max)
    {
        if (max == 0f)
        {
            this.ProcessGo.SetActive(false);
            this.SetTaskDecscription();
        }
        else
        {
            if (this.curMission.mType != 0)
            {
                this.curTaskTarget.gameObject.SetActive(true);
                this.ProcessGo.SetActive(true);
            }
            else
            {
                this.currentNumTxt.text = (max - cur).ToString();
            }
            this.taskProcess.value = cur / max;
        }
    }

    public void SetPlayerHpInUI(float cur, float max)
    {
        this.playerSlider.value = cur / max;
        if (cur < 0.35f * max)
        {
            if (this.curProp2 != null)
            {
                if (this.curProp2.ID == 4001)
                {
                    this.EffectObjMedic.transform.position = this.prop2Img.transform.position;
                }
                else if (this.curProp.ID == 4001)
                {
                    this.EffectObjMedic.transform.position = this.propImg.transform.position;
                }
            }
            else if (this.curProp.ID == 4001)
            {
                this.EffectObjMedic.transform.position = this.propImg.transform.position;
            }
            this.EffectObjMedic.SetActive(true);
        }
        else
        {
            this.EffectObjMedic.SetActive(false);
        }
        if (cur < 0.3f * max)
        {
            this.ShowDanger();
        }
        else
        {
            this.CloseDanger();
        }
    }

    public void RefreshCoin()
    {
        this.goldNum = 0;
        List<DropItemType> dropList = Singleton<DropItemManager>.Instance.GetDropList();
        for (int i = 0; i < dropList.Count; i++)
        {
            if (dropList[i] == DropItemType.GOLD)
            {
                this.goldNum += this.num;
            }
        }
        this.gameCoinTxt.text = this.goldNum.ToString();
    }

    public void RefreshPage()
    {
        this.scoreNameTxt.text = Singleton<GlobalData>.Instance.GetText("SCORE");
        this.taskCotentTxt.text = Singleton<GlobalData>.Instance.GetText("SCORE");
        Singleton<FontChanger>.Instance.SetFont(scoreNameTxt);
        Singleton<FontChanger>.Instance.SetFont(taskCotentTxt);
        this.curTaskGo.SetActive(false);
        this.RefreshProp();
        this.RefreshProp2();
        this.RefreshProp3();
        this.RefreshWeaponUI();
    }

    public void RefreshProp()
    {
        if (PlayerDataManager.Player.Prop1 != 0)
        {
            this.propImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.curProp.IconInGame);
            this.propNumTxt.text = this.curMaxProp01.ToString();
        }
        else
        {
            this.propNumTxt.text = "0";
        }
    }

    public void RefreshProp2()
    {
        if (PlayerDataManager.Player.Prop2 != 0)
        {
            this.prop2Img.sprite = Singleton<UiManager>.Instance.GetSprite(this.curProp2.IconInGame);
            this.prop2NumTxt.text = this.curMaxProp02.ToString();
        }
        else
        {
            this.prop2NumTxt.text = "0";
        }
    }

    public void RefreshProp3()
    {
    }

    public void RefreshCoinAndScore()
    {
        this.gameCoinTxt.text = this.PlayerGameCoin().ToString();
        this.scoreTxt.text = this.PlayerGameScore().ToString();
    }

    public void RefreshDistance()
    {
    }

    public void RefreshWeaponUI()
    {
        if (this.weaponIndex == 0)
        {
            this.weaponImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.curMainWeapon.Icon);
        }
        else if (this.weaponIndex == 1)
        {
            this.weaponImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.curSecondWeapon.Icon);
        }
    }

    public void OnclickProp()
    {
        if (this.curProp != null && this.curMaxProp01 > 0 && this.player.IsFullBodyActionOver)
        {
            this.timeProp1Cd = this.maxPropTime;
            if (this.tempProp.ContainsKey(this.curProp.ID) && this.tempProp[this.curProp.ID] > 0)
            {
                Dictionary<int, int> dictionary;
                int iD;
                dictionary = this.tempProp; iD = this.curProp.ID; (dictionary)[iD] = dictionary[iD] - 1;
            }
            else
            {
                PropDataManager.UseProp(this.curProp.ID);
            }
            this.curMaxProp01--;
            this.player.DoSkill(this.curProp.ID);
            this.RefreshProp();
            GameLogManager.SendPageLog(this.curProp.Name, "USE");
            if ((UnityEngine.Object)null != (UnityEngine.Object)Singleton<UiManager>.Instance.GetPage(PageName.TeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.TeachPage).gameObject.activeInHierarchy)
            {
                ((Component)Singleton<UiManager>.Instance.GetPage(PageName.TeachPage)).GetComponent<TeachPage>().OnclickShouLei();
            }
        }
    }

    public void OnclickProp2()
    {
        if (this.curProp2 != null && this.curMaxProp02 > 0 && this.player.IsFullBodyActionOver)
        {
            this.timeProp2Cd = this.maxPropTime;
            if (this.tempProp.ContainsKey(this.curProp2.ID) && this.tempProp[this.curProp2.ID] > 0)
            {
                Dictionary<int, int> dictionary;
                int iD;
                dictionary = this.tempProp; iD = this.curProp2.ID; (dictionary)[iD] = dictionary[iD] - 1;
            }
            else
            {
                PropDataManager.UseProp(this.curProp2.ID);
            }
            this.curMaxProp02--;
            this.player.DoSkill(this.curProp2.ID);
            GameLogManager.SendPageLog(this.curProp.Name, "USE");
            this.RefreshProp2();
            if ((UnityEngine.Object)null != (UnityEngine.Object)Singleton<UiManager>.Instance.GetPage(PageName.TeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.TeachPage).gameObject.activeInHierarchy)
            {
                ((Component)Singleton<UiManager>.Instance.GetPage(PageName.TeachPage)).GetComponent<TeachPage>().OnclickMedic();
            }
        }
    }

    public void OnclickProp3()
    {
        if (this.curProp3 != null && this.curProp3.Count > 0 && this.player.IsFullBodyActionOver)
        {
            this.timeProp3Cd = this.maxPropTime;
            this.curProp3.Count--;
            PropDataManager.UseProp(this.curProp3.ID);
            this.player.DoSkill(this.curProp3.ID);
            this.RefreshProp3();
        }
    }

    public void OnclickChangeWeapon()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        if (this.curSecondWeapon != null && this.player.IsFullBodyActionOver && this.player.ChangeWeapon())
        {
            if (this.weaponIndex == 0)
            {
                this.weaponIndex = 1;
            }
            else
            {
                this.weaponIndex = 0;
            }
            if ((UnityEngine.Object)null != (UnityEngine.Object)Singleton<UiManager>.Instance.GetPage(PageName.TeachPage) && Singleton<UiManager>.Instance.GetPage(PageName.TeachPage).gameObject.activeInHierarchy)
            {
                this.SetGameState(PlayingState.GamePlaying);
                ((Component)Singleton<UiManager>.Instance.GetPage(PageName.TeachPage)).GetComponent<TeachPage>().OnclickWeapon();
            }
            Singleton<GlobalData>.Instance.InGameWeaponIndex = this.weaponIndex;
        }
    }

    public void OnclickExit()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        Singleton<UiManager>.Instance.ClosePage(PageType.Normal, null);
        Singleton<UiManager>.Instance.ShowLoadingPage("UI", PageName.MainPage);
        this.Close();
        Singleton<UiManager>.Instance.ShowTopBar(true);
    }

    public void OnclickPause()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        this.SetGameState(PlayingState.GamePause);
        GameApp.GetInstance().GetGameScene().GetPlayer()
            .SetPlayerIdle(false);
        if (GameApp.GetInstance().GetGameScene().PlayingMode != GamePlayingMode.SnipeMode)
        {
            this.Close();
        }
        Singleton<UiManager>.Instance.ShowPage(PageName.PausePage, null);
        GameLogManager.SendPageLog(base.Name.ToString(), 12.ToString());
    }

    public void OnclickFailed()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        this.GameFalied();
    }

    public void OnclickSuccess()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        this.SetGameState(PlayingState.GameWin);
        GameApp.GetInstance().GetGameScene().GetPlayer()
            .SetPlayerIdle(false);
        Singleton<UiManager>.Instance.SetLevelFinish(true);
    }

    public void OnclickTeach()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        Singleton<UiManager>.Instance.ShowPage(PageName.TeachPage, null);
    }

    public void OnclickQTE()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        if (this.player.IsFullBodyActionOver)
        {
            this.timeQteCd = this.maxQteTime;
            this.player.DoQTE();
        }
    }

    public void SetTaskDecscription()
    {
        LanguageEnum currentLanguage = Singleton<GlobalData>.Instance.GetCurrentLanguage();
        string text = (currentLanguage != LanguageEnum.Russian) ? this.curMission.description_en : this.curMission.description_ru;
        string text2 = string.Format(text, this.curMission.curAmount, this.curMission.targetAmount);
        this.taskCotentTxt.text = text;
        this.taskCotent2Txt.text = text2;
        this.currentNumTxt.text = (this.curMission.targetAmount - this.curMission.curAmount).ToString();
        base.Invoke("HideCenterTask", 3f);
        this.snipeUI.SetMission(this.curMission.description_ru, this.curMission.description_en, this.curMission.curAmount, this.curMission.targetAmount);
    }

    public void SetTaskDescriptionEnable(bool _enable)
    {
        this.curTaskGo.SetActive(_enable);
    }

    public void MissionComplete()
    {
        this.missionGuideObj.gameObject.SetActive(false);
        this.mState = MissionStateUI.NONE;
        this.SetTaskDescriptionEnable(false);
        this.gameScene.GetMissionPath().Hide();
    }

    public void HideCenterTask()
    {
    }

    public void CheckTeach()
    {
        if (this.curMission != null)
        {
            if (this.curData.Index == 1 && this.curData.Chapters == ChapterEnum.CHAPTERNAME_01 && !Singleton<GlobalData>.Instance.isDebug)
            {
                if (Singleton<GlobalData>.Instance.FirstMove > 0 && this.curMission.mType == EMission.REACH_POSITION)
                {
                    this.ShowTeachPage(TeachType.MOVE, false);
                }
                else if (Singleton<GlobalData>.Instance.FirstShoot > 0 && this.curMission.mType == EMission.KILL_WAVE)
                {
                    this.ShowTeachPage(TeachType.SHOOT, false);
                }
            }
            else if (this.curData.Index == 3 && this.curData.Chapters == ChapterEnum.CHAPTERNAME_01 && !Singleton<GlobalData>.Instance.isDebug)
            {
                if (Singleton<GlobalData>.Instance.FirstShouLei == 1 && this.curMission.mType == EMission.PROTECT_NPC)
                {
                    this.ShowTeachPage(TeachType.SHOULEI, true);
                }
            }
            else if (this.curData.Index == 4 && this.curData.Chapters == ChapterEnum.CHAPTERNAME_01 && !Singleton<GlobalData>.Instance.isDebug && Singleton<GlobalData>.Instance.FirstWeapon == 1)
            {
                this.ShowTeachPage(TeachType.WEAPON, true);
            }
        }
    }

    public void CheckTeach(TeachType teachType)
    {
        if (!((UnityEngine.Object)this.curTeachPage != (UnityEngine.Object)null))
        {
            bool flag = false;
            bool pauseGame = false;
            switch (teachType)
            {
                case TeachType.MOVE:
                    flag = (Singleton<GlobalData>.Instance.FirstMove > 0);
                    break;
                case TeachType.SHOOT:
                    flag = (Singleton<GlobalData>.Instance.FirstShoot > 0);
                    break;
                case TeachType.SHOULEI:
                    pauseGame = true;
                    flag = (this.curProp.ID == 4003 && Singleton<GlobalData>.Instance.FirstShouLei == 1);
                    if (!flag)
                    {
                        Singleton<GlobalData>.Instance.FirstShouLei = 0;
                    }
                    break;
                case TeachType.WEAPON:
                    pauseGame = true;
                    flag = (Singleton<GlobalData>.Instance.FirstWeapon == 1);
                    break;
                case TeachType.MEDIC:
                    pauseGame = true;
                    flag = (this.curProp2.ID == 4001 && Singleton<GlobalData>.Instance.FirstMedic > 0);
                    if (!flag)
                    {
                        Singleton<GlobalData>.Instance.FirstMedic = 0;
                    }
                    break;
                case TeachType.PAOTAI:
                    flag = (Singleton<GlobalData>.Instance.FirstPaoTai > 0);
                    break;
            }
            switch (flag)
            {
                case false:
                    break;
                default:
                    if (this.curProp != null && this.curProp.ID == 4003)
                    {
                        PropDataManager.CollectProp(4003, 1);
                        this.curMaxProp01++;
                        this.timeProp1Cd = 0f;
                        this.propBtn[0].enabled = true;
                        this.propcdImg[0].fillAmount = 0f;
                    }
                    this.ShowTeachPage(teachType, pauseGame);
                    break;
            }
        }
    }

    private void ShowTeachPage(TeachType _type, bool _pauseGame = true)
    {
        if (_pauseGame)
        {
            this.SetGameState(PlayingState.GamePause);
        }
        TeachPage teachPage = Singleton<UiManager>.Instance.ShowPage<TeachPage>(PageName.TeachPage, (UnityAction)null);
        ((Component)Singleton<UiManager>.Instance.GetPage(PageName.TeachPage)).GetComponent<TeachPage>().teach = _type;
        Singleton<UiManager>.Instance.GetPage(PageName.TeachPage).Refresh();
        this.curTeachPage = teachPage;
        teachPage.gameObject.SetActive(base.gameObject.activeSelf);
        this.curTeachPage.SetOnCloseCallback(delegate
        {
            this.curTeachPage = null;
        });
        if (_type == TeachType.SHOOT)
        {
            this.SetInputMovePlayer(false);
        }
    }

    public void MissionStart(Mission _mission)
    {
        this.SetTaskDescriptionEnable(true);
        this.ProcessGo.SetActive(false);
        this.curMission = _mission;
        this.npcSlider.gameObject.SetActive(this.curMission.mType == EMission.PROTECT_NPC);
        if ((UnityEngine.Object)null != (UnityEngine.Object)this.curMission.npcCreater && (UnityEngine.Object)null != (UnityEngine.Object)this.curMission.npcCreater.GetNpc())
        {
            this.NpcIconImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.curMission.npcCreater.GetNpc().name);
        }
        bool flag = this.CheckMission();
        this.missionGuideObj.gameObject.SetActive(flag);
        if (flag && GameApp.GetInstance().GetGameScene().PlayingMode != GamePlayingMode.Cannon)
        {
            this.gameScene.GetMissionPath().Show(this.curMission);
        }
        if (flag)
        {
            this.ResetMissionDir();
        }
        switch (this.curMission.targetIcon)
        {
            case EMissionIcon.KILL_WAVE:
                this.curTaskDes.transform.position = this.vectorTaskGuild;
                this.curTaskTarget.gameObject.SetActive(false);
                this.curTaskDes.gameObject.SetActive(true);
                break;
            case EMissionIcon.KILL_ENEMY:
                this.curTaskDes.transform.position = this.vectorTaskGuild;
                this.curTaskTarget.transform.position = this.vectorTaskDes;
                this.curTaskIcon.sprite = this.spriteCollection[0];
                this.curTaskTarget.gameObject.SetActive(true);
                this.currentNumTxt.gameObject.SetActive(true);
                this.ProcessGo.gameObject.SetActive(false);
                break;
            case EMissionIcon.PROTECT_NPC:
                this.PosMissionGuide.position = this.vectorTaskGuild;
                this.curTaskDes.transform.position = this.vectorTaskDes;
                this.curTaskTarget.transform.position = this.vectorTaskTarget;
                this.curTaskIcon.sprite = this.spriteCollection[5];
                this.curTaskTarget.gameObject.SetActive(true);
                this.currentNumTxt.gameObject.SetActive(false);
                break;
            case EMissionIcon.REACH_POSITION:
                this.PosMissionGuide.position = this.vectorTaskGuild;
                this.curTaskDes.transform.position = this.vectorTaskDes;
                this.curTaskTarget.transform.position = this.vectorTaskTarget;
                this.curTaskIcon.sprite = this.spriteCollection[1];
                this.curTaskTarget.gameObject.SetActive(false);
                this.currentNumTxt.gameObject.SetActive(false);
                break;
            case EMissionIcon.PLACE_ITEM:
                this.PosMissionGuide.position = this.vectorTaskGuild;
                this.curTaskDes.transform.position = this.vectorTaskDes;
                this.curTaskTarget.transform.position = this.vectorTaskTarget;
                this.curTaskIcon.sprite = this.spriteCollection[1];
                this.curTaskTarget.gameObject.SetActive(true);
                this.currentNumTxt.gameObject.SetActive(false);
                break;
            case EMissionIcon.TRANSPORT_ITEM:
                this.PosMissionGuide.position = this.vectorTaskGuild;
                this.curTaskDes.transform.position = this.vectorTaskDes;
                this.curTaskTarget.transform.position = this.vectorTaskTarget;
                this.curTaskIcon.sprite = this.spriteCollection[2];
                this.curTaskTarget.gameObject.SetActive(true);
                this.currentNumTxt.gameObject.SetActive(false);
                break;
            case EMissionIcon.TIME_ACTION:
                this.PosMissionGuide.position = this.vectorTaskGuild;
                this.curTaskDes.transform.position = this.vectorTaskDes;
                this.curTaskTarget.transform.position = this.vectorTaskTarget;
                this.curTaskIcon.sprite = this.spriteCollection[4];
                this.curTaskTarget.gameObject.SetActive(true);
                this.currentNumTxt.gameObject.SetActive(false);
                break;
            case EMissionIcon.NONE:
                this.PosMissionGuide.position = this.vectorTaskGuild;
                this.curTaskDes.transform.position = this.vectorTaskDes;
                this.curTaskTarget.transform.position = this.vectorTaskTarget;
                this.curTaskTarget.gameObject.SetActive(false);
                this.currentNumTxt.gameObject.SetActive(false);
                break;
            default:
                this.PosMissionGuide.position = this.vectorTaskGuild;
                this.curTaskIcon.sprite = this.spriteCollection[2];
                this.curTaskDes.transform.position = this.vectorTaskDes;
                this.curTaskTarget.gameObject.SetActive(false);
                this.curTaskTarget.transform.position = this.vectorTaskTarget;
                break;
        }
        this.SetTaskDecscription();
        this.mState = MissionStateUI.STARTED;
    }

    public void SetInputMovePlayer(bool isTeach)
    {
        this.gameScene.GetPlayer().InputController.EnableMoveInput = isTeach;
    }

    public void SetMissionGuidePosition(Vector2 _pos)
    {
    }

    public void SetInstantiatedInterface(IGameSceneControl _gameScene, IPlayerControl _player)
    {
        this.gameScene = _gameScene;
        this.player = _player;
    }

    public void DoCannonShoot(bool isShooting)
    {
        if (Singleton<GlobalData>.Instance.FirstPaoTai > 0)
        {
            this.startPaotai = isShooting;
        }
        this.gameScene.DoControlCannonShoot(isShooting);
    }

    public void SetScore(int score)
    {
    }

    public void ResetGameUI()
    {
        if ((UnityEngine.Object)null != (UnityEngine.Object)this.HeadGo)
        {
            this.HeadGo.SetActive(false);
        }
        if ((UnityEngine.Object)null != (UnityEngine.Object)this.KillsGo)
        {
            this.KillsGo.SetActive(false);
        }
    }

    public void GameFalied()
    {
        Singleton<UiManager>.Instance.ShowPage(PageName.FailedNoticePage, null);
        GameLogManager.SendPageLog(base.Name.ToString(), 14.ToString() + this.SendCheckPointLog());
        this.SetGameState(PlayingState.GameLose);
    }

    public void Recevie()
    {
        if (CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.RACING)
        {
            if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.BOSS)
            {
                this.boosTimeNum = 120f;
            }
            this.gameScene.Revive();
        }
    }

    public void GameWin()
    {
        Singleton<UiManager>.Instance.SetLevelFinish(true);
        this.SetGameState(PlayingState.GameWin);
    }

    public int PlayerGameCoin()
    {
        return 0;
    }

    public int PlayerGameScore()
    {
        return 0;
    }

    public void SetGameState(PlayingState gameState)
    {
        GameApp.GetInstance().GetGameScene().PlayingState = gameState;
    }

    public PlayingState GetGameState()
    {
        return GameApp.GetInstance().GetGameScene().PlayingState;
    }

    public void ShowOrHideCanShoot(bool isShow)
    {
        this.CanShoot.SetActive(isShow);
    }

    public void ShowHead()
    {
        this.curGold += 3;
        PickUpData pickUpData = new PickUpData();
        pickUpData.dropItemType = DropItemType.GOLD;
        pickUpData.num = 3;
        this.listPickUpDataGO.Add(pickUpData);
    }

    public void CloseHead()
    {
        this.HeadParticlGo.SetActive(false);
        this.HeadGo.SetActive(false);
    }

    public void ShowKills(int num)
    {
        PickUpData pickUpData = new PickUpData();
        pickUpData.dropItemType = DropItemType.GOLD;
        if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.GOLD)
        {
            if (num > 3)
            {
                num = 3;
            }
            this.curGold += num * CheckpointDataManager.GetCurrentCheckpoint().AwardCount[0] / 160;
            pickUpData.num = num * CheckpointDataManager.GetCurrentCheckpoint().AwardCount[0] / 160;
        }
        else
        {
            if (num > 3)
            {
                num = 3;
            }
            this.curGold += num * CheckpointDataManager.SelectCheckpoint.AwardCount[0] / 160;
            pickUpData.num = num * CheckpointDataManager.SelectCheckpoint.AwardCount[0] / 160;
        }
        this.listPickUpDataGO.Add(pickUpData);
    }

    public void CloseKills()
    {
        this.KillsGo.SetActive(false);
    }

    public void ShowSpecial()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        this.SpecialKillGo.SetActive(true);
        this.SpecialKillGo.GetComponent<DOTweenAnimation>().DORestart(false);
    }

    public void CloseSpecial()
    {
        this.SpecialKillGo.SetActive(false);
    }

    public void ShowDanger()
    {
        this.DangerGo.SetActive(true);
    }

    public void CloseDanger()
    {
        this.DangerGo.SetActive(false);
    }

    public void ShowHurt()
    {
        this.HurtGo.SetActive(true);
        UnityEngine.Debug.Log("受伤" + this.gameScene.GetGamePlotState());
        if (!this.gameScene.GetGamePlotState() && this.curProp2.ID == 4001)
        {
            this.ShowMedicTeach();
        }
        base.Invoke("CloseHurt", 1f);
    }

    public void ShowMedicTeach()
    {
        if (this.curData.Chapters == ChapterEnum.CHAPTERNAME_01 && this.curData.Index < 10 && this.curData.Index > 5)
        {
            this.CheckTeach(TeachType.MEDIC);
        }
    }

    public void CloseHurt()
    {
        this.HurtGo.SetActive(false);
    }

    public void SetUIDisplayMode(GamePlayingMode _mode)
    {
        switch (_mode)
        {
            case GamePlayingMode.Cannon:
                this.QteGO.SetActive(false);
                this.NomalLevel.SetActive(false);
                this.WeaponInfo.SetActive(false);
                this.BossLevel.SetActive(false);
                this.BatteryLevel.SetActive(true);
                this.changeBtn.gameObject.SetActive(false);
                this.radar.gameObject.SetActive(false);
                this.playerShootBtn.gameObject.SetActive(false);
                this.CheckTeach(TeachType.PAOTAI);
                this.snipeUI.gameObject.SetActive(false);
                this.normalUI.SetActive(true);
                break;
            case GamePlayingMode.Normal:
                this.NomalLevel.SetActive(true);
                this.BatteryLevel.SetActive(false);
                this.BossLevel.SetActive(false);
                this.playerShootBtn.gameObject.SetActive(Singleton<GlobalData>.Instance.ShootingMode == 1 && Singleton<UiControllers>.Instance.IsMobile);
                this.snipeUI.gameObject.SetActive(false);
                this.normalUI.SetActive(true);
                break;
            case GamePlayingMode.SnipeMode:
                this.radar.SetActive(false);
                this.normalUI.SetActive(false);
                this.snipeUI.gameObject.SetActive(true);
                break;
        }
    }

    public void SetUIDisplayEvnt(UIDisplayEvnt evnt, params float[] param)
    {
        switch (evnt)
        {
            case UIDisplayEvnt.BULLET_ACROSS:
                break;
            case UIDisplayEvnt.FLASH_RELOAD:
                break;
            case UIDisplayEvnt.DOUBLE_DNA:
                break;
            case UIDisplayEvnt.DOUBLE_COIN:
                break;
            case UIDisplayEvnt.CRITICAL:
                break;
            case UIDisplayEvnt.PLAYER_DODDGE:
                break;
            case UIDisplayEvnt.CONTINOUS_KILL:
                if (param[0] > 1f)
                {
                    this.ResetGameUI();
                    this.ShowKills((int)param[0]);
                }
                break;
            case UIDisplayEvnt.HEADSHOT:
                this.ShowHead();
                break;
            case UIDisplayEvnt.PLAYER_ONHIT:
                this.ShowHurt();
                break;
            case UIDisplayEvnt.BULLET_COUNT:
                this.SetBulletCount((int)param[0], (int)param[1], (int)param[2]);
                break;
            case UIDisplayEvnt.PLAYER_HP:
                this.SetPlayerHpInUI(param[0], param[1]);
                break;
            case UIDisplayEvnt.FOCUS_NPC_HP:
                this.CheckNpcHp(param[0], param[1]);
                break;
            case UIDisplayEvnt.MISSION_ITEM_PERCENT:
                {
                    LanguageEnum currentLanguage = Singleton<GlobalData>.Instance.GetCurrentLanguage();
                    string format = (currentLanguage != LanguageEnum.Russian) ? this.curMission.description_en : this.curMission.description_ru;
                    this.CheckTaskValue(param[0], param[1]);
                    if (this.curMission.mType == EMission.KILL_ENEMY)
                    {
                        this.taskCotent2Txt.text = string.Format(format, param[0], this.curMission.targetAmount);
                    }
                    this.snipeUI.RefreshMission(this.curMission.curAmount, this.curMission.targetAmount);
                    break;
                }
            case UIDisplayEvnt.ON_MISSIONITEM:
                this.missionGuideObj.gameObject.SetActive(false);
                break;
            case UIDisplayEvnt.EXIT_MISSIONITEM:
                this.missionGuideObj.gameObject.SetActive(true);
                break;
            case UIDisplayEvnt.TRIGGER_MISSIONITEM:
                if (this.curMission.curAmount < this.curMission.targetAmount)
                {
                    this.missionGuideObj.gameObject.SetActive(true);
                }
                else
                {
                    this.missionGuideObj.gameObject.SetActive(false);
                }
                this.SetTaskDecscription();
                break;
            case UIDisplayEvnt.FOCUS_ON_ARMORBOX:
                this.ProcessAddBullet.SetActive(param[0] != 0f);
                break;
            case UIDisplayEvnt.FILL_BULLET_PERCENT:
                this.AddBulletSlider.value = param[0];
                if (param[0] == 1f)
                {
                    this.AddBulletTxt.text = Singleton<GlobalData>.Instance.GetText("ADD_BULLET_FULL") + (int)(100f * param[0]) + "%";
                }
                else
                {
                    this.AddBulletTxt.text = Singleton<GlobalData>.Instance.GetText("ADD_BULLET");
                }
                Singleton<FontChanger>.Instance.SetFont(AddBulletTxt);
                break;
            case UIDisplayEvnt.NOARMOR:
                this.NoArmor.SetActive(param[0] != 0f);
                var noArmorText = NoArmor.GetComponent<Text>();
                noArmorText.text = Singleton<GlobalData>.Instance.GetText("NOARMOR");
                Singleton<FontChanger>.Instance.SetFont(noArmorText);
                break;
            case UIDisplayEvnt.PLAYER_GET_BULLET:
                this.WeaponId = (int)param[0];
                this.Bullets = (int)param[1];
                break;
            case UIDisplayEvnt.NPC_ONHIT:
                if (!this.NpcHurtGo.activeInHierarchy)
                {
                    this.ShowNpcHurtTip();
                }
                break;
            case UIDisplayEvnt.BOSS_APPEAR:
                this.BossLevel.SetActive(true);
                this.boosTimeNum = 120f;
                this.BatteryLevel.SetActive(false);
                this.NomalLevel.SetActive(true);
                this.WeaponInfo.SetActive(true);
                this.changeBtn.gameObject.SetActive(true);
                this.radar.gameObject.SetActive(true);
                break;
            case UIDisplayEvnt.BOSS_DEAD:
                this.BossTimeSlider.gameObject.SetActive(false);
                break;
            case UIDisplayEvnt.BOSS_HP_PERCENT:
                this.BossTimeSlider.value = param[0] / param[1];
                break;
            case UIDisplayEvnt.COUNTDOWN_PERCENT:
                this.LimitLevel.SetActive(param[0] > 0f);
                if (param[0] != 0f)
                {
                    this.curTaskTarget.gameObject.SetActive(false);
                }
                if (param[0] > 0f && param[0] < 10f && this.LimitLevel.activeSelf)
                {
                    if (!this.startEffect)
                    {
                        this.startEffect = true;
                        this.bossTimeTxt.DOColor(Color.red, 0.2f).SetLoops(-1, LoopType.Yoyo);
                    }
                }
                else
                {
                    this.startEffect = false;
                    this.bossTimeTxt.DOPause();
                    this.bossTimeTxt.color = Color.white;
                }
                this.bossTimeTxt.text = UITick.secToStr((int)param[0]);
                this.snipeUI.SetMissionTime((int)param[0]);
                break;
            case UIDisplayEvnt.MISSION_TIME_LIMITED:
                this.LimitLevel.SetActive(param[0] != 0f);
                this.snipeUI.SetMissionTimeLimited(param[0] != 0f);
                break;
            case UIDisplayEvnt.SHOW_BOSS_SKILL_EFFECT:
                this.bossSkillEffect.SetActive(param[0] == 1f);
                break;
        }
    }

    public void SetBulletCount(int cur, int limit, int max)
    {
        this.bulletSlider.maxValue = (float)limit;
        this.bulletSlider.value = (float)cur;
        this.curbulletNum.text = cur.ToString();
        this.bulletCount.text = max.ToString();
        this.snipeUI.SetBulletCount(cur, max);
    }

    public void ChangeBullet()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        if (this.player.IsFullBodyActionOver)
        {
            this.timeChange = this.maxTime;
            this.player.DoReload();
        }
    }

    public void SetProgressEnable(bool enable, bool isCircle)
    {
        if (isCircle)
        {
            this.circleProgress.SetActive(enable);
        }
        else
        {
            this.bulletProgress.SetActive(enable);
        }
    }

    public void SetReloadProgressPercent(float percent)
    {
        this.circlePercent.fillAmount = percent;
    }

    public void SetReloadProgressPercent(int cur, int max)
    {
        float num = 27f * ((0f - (float)max) / 2f + 0.5f);
        for (int i = 0; i < this.bulletsBg.Length; i++)
        {
            this.bulletsBg[i].gameObject.SetActive(i < max);
            this.bulletFg[i].gameObject.SetActive(i < cur);
            this.bulletsBg[i].anchoredPosition = new Vector2(num + 27f * (float)i, 0f);
        }
    }

    public void SetUIPlayerWeapon(string weapon)
    {
        this.curWeapon = WeaponDataManager.GetWeaponData(weapon);
        this.weaponImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.curWeapon.Icon);
    }

    public RectTransform GetPointTagRectTransform()
    {
        RectTransform rectTransform = null;
        int num = 0;
        while (num < this.pointTags.Count)
        {
            if (this.pointTags[num].gameObject.activeSelf)
            {
                num++;
                continue;
            }
            rectTransform = this.pointTags[num];
            break;
        }
        if ((UnityEngine.Object)rectTransform == (UnityEngine.Object)null)
        {
            rectTransform = UnityEngine.Object.Instantiate(this.pointTags[0].gameObject, this.pointTags[0].parent).GetComponent<RectTransform>();
            this.pointTags.Add(rectTransform);
        }
        rectTransform.gameObject.SetActive(true);
        return rectTransform;
    }

    public bool IsTouchInShootThumb(Vector2 touchPos)
    {
        return this.playerShootBtn.IsInShootRadius(touchPos);
    }

    public void SetTouchShootThumb(bool isShoot)
    {
        if (isShoot)
        {
            this.playerShootBtn.OnTouchEnter();
        }
        else
        {
            this.playerShootBtn.OnTouchRelease();
        }
    }

    public void SetGameControlMode(GameControlMode mode)
    {
        this.playerShootBtn.gameObject.SetActive(mode == GameControlMode.MANUALFIRE && 
                                                 GameApp.GetInstance().GetGameScene().PlayingMode == GamePlayingMode.Normal && 
                                                 Singleton<UiControllers>.Instance.IsMobile);
    }

    public void AddGrenade(int id, int num)
    {
        if (this.tempProp.ContainsKey(id))
        {
            Dictionary<int, int> dictionary;
            int key;
            dictionary = this.tempProp; key = id; (dictionary)[key] = dictionary[key] + num;
        }
        else
        {
            this.tempProp.Add(id, num);
        }
        if (this.curProp != null && this.curProp.ID == 4003)
        {
            this.curMaxProp01 += num;
            this.propNumTxt.text = this.curMaxProp01.ToString();
        }
        else if (this.curProp2 != null && this.curProp2.ID == 4003)
        {
            this.curMaxProp02 += num;
            this.prop2NumTxt.text = this.curMaxProp02.ToString();
        }
        else
        {
            this.curProp = PropDataManager.GetPropData(4003);
            this.curMaxProp01 = Mathf.Min(this.curProp.Count, this.curProp.MaxCarryCount);
            this.curMaxProp01 += num;
            this.propNumTxt.text = this.curMaxProp01.ToString();
            this.propImg.sprite = Singleton<UiManager>.Instance.GetSprite(this.curProp.IconInGame);
        }
    }

    public void OnSnipeFirePressed()
    {
        Weapon weapon = GameApp.GetInstance().GetGameScene().GetPlayer()
            .GetWeapon();
        if (weapon.BulletCount > 0)
        {
            weapon.Fire(0f);
            this.snipeUI.DoShakeScope();
        }
    }

    public void SetPlayerEquipedWeaponData(Weapon wp)
    {
        if (wp.GetWeaponType() == Zombie3D.WeaponType.Sniper)
        {
            this.snipeUI.SetWeaponData(wp);
        }
    }

    public void SetLevelDescription(string intro_key, string descriptio_key)
    {
        this.snipeUI.SetLevelDescription(Singleton<GlobalData>.Instance.GetText(intro_key), Singleton<GlobalData>.Instance.GetText(descriptio_key));
        Time.timeScale = 0f;
    }

    public void HideSnipeUIImmediatly()
    {
        this.snipeUI.HideSnipeUI();
    }

    public void OnTreasureBoxBtnPressed()
    {
        this.btnTreasureBox.SetActive(false);
        this.triggeredTreasureBox.DoOpen();
        this.triggeredTreasureBox = null;
    }

    public void DoShowTreasureBoxBtn(TreasureBox box)
    {
        this.triggeredTreasureBox = box;
        this.btnTreasureBox.SetActive((UnityEngine.Object)box != (UnityEngine.Object)null);
    }
}