//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
using DataCenter;
using DG.Tweening;
using RacingMode;
using System.Collections.Generic;
using ads;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class CommonFinishPage : GamePage
{
    public CanvasGroup Content;

    public Text TitleName;

    public Text[] rewardNameTxt;

    public Text[] rewardNumTxt;

    public Text returnNameTxt;

    public Text doubleNameTxt;

    public Button DoubleButton;

    public GameObject[] DebrisObjects;

    public Text[] DebrisNumber;

    public Image[] DebrisIcon;

    private int goldNum;

    private int dnaNum;

    private int num = 1;

    private int index;

    private int DebrisNum;

    private List<DebrisData> listDebris = new List<DebrisData>();

    private Dictionary<DebrisData, int> DicDebris = new Dictionary<DebrisData, int>();

    private float countDown;

    public new void OnEnable()
    {
        this.Content.alpha = 0f;
        this.Content.DOFade(1f, 1f);
        PlayerDataManager.SetStatisticsDatas(PlayerStatistics.GameDuration, (int)Singleton<GlobalData>.Instance.GameTimes);
        Singleton<GlobalData>.Instance.ShowAdvertisement(16 + Mathf.Clamp(CheckpointDataManager.SelectCheckpoint.ID, 0, 10), null, null);
        Singleton<GlobalData>.Instance.CheckpointFinishTimes++;
        this.goldNum = 0;
        this.dnaNum = 0;
        this.DebrisNum = 0;
        for (int i = 0; i < this.DebrisObjects.Length; i++)
        {
            this.DebrisObjects[i].SetActive(false);
        }
        if (Singleton<UiManager>.Instance.GameSuccess == 1)
        {
            PlayerDataManager.SetStatisticsDatas(PlayerStatistics.CheckpointPassedTimes, 1);
            if (CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.BOSS && CheckpointDataManager.SelectCheckpoint.ID > 5)
            {
                CheckpointDataManager.SuccessTimes++;
                if (CheckpointDataManager.SuccessTimes == 2)
                {
                    Singleton<UiManager>.Instance.ShowPage(PageName.AdvertisementPushPage, null);
                }
            }
            if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.BOSS)
            {
                DailyMissionSystem.SetDailyMission(DailyMissionType.FINISH_BOSS_CHECKPOINT, 1);
                int pageIndex = MapsPage.instance.PageIndex;
                int @int = PlayerPrefs.GetInt("BOSS_CHECKPOINT_" + pageIndex);
                PlayerPrefs.SetInt("BOSS_CHECKPOINT_" + pageIndex, @int + 1);
                BossExtraAward bossExtraAward = ItemDataManager.GetBossExtraAward(CheckpointDataManager.SelectCheckpoint.ID);
                if (bossExtraAward != null)
                {
                    int[] id = new int[1] {
                        bossExtraAward.AwardID
                    };
                    int[] count = new int[1] {
                        bossExtraAward.AwardCount
                    };
                    Singleton<UiManager>.Instance.ShowAward(id, count, null);
                    ItemDataManager.CollectItem(bossExtraAward.AwardID, bossExtraAward.AwardCount);
                }
            }
            else if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.WEAPON)
            {
                if (PlayerPrefs.GetString("FirstWeaponPush", "true").Equals("true"))
                {
                    PlayerPrefs.SetString("FirstWeaponPush", "false");
                }
                int int2 = PlayerPrefs.GetInt("WEAPON_TRY_CHECKPOINT");
                PlayerPrefs.SetInt("WEAPON_TRY_CHECKPOINT", int2 - 1);
            }
            else if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.GOLD)
            {
                DailyMissionSystem.SetDailyMission(DailyMissionType.FINISH_GOLD_CHECKPOINT, 1);
                int int3 = PlayerPrefs.GetInt("GOLD_CHECKPOINT");
                PlayerPrefs.SetInt("GOLD_CHECKPOINT", int3 - 1);
                GameTimeManager.RecordTime("GOLD_CHECKPOINT");
            }
            else if (CheckpointDataManager.SelectCheckpoint.Type > (CheckpointType)10)
            {
                DailyMissionSystem.SetDailyMission(DailyMissionType.FINISH_RANDOM_CHECKPOINT, 1);
            }
        }
        this.RefreshPage();
    }

    public override void OnBack()
    {
        this.Close();
        if (CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.RACING)
        {
            Singleton<UiManager>.Instance.RemovePage(PageName.InGamePage);
        }
        Singleton<UiManager>.Instance.ShowTopBar(true);
        Singleton<UiManager>.Instance.ShowLoadingPage("UI", PageName.MainPage);
        Singleton<DropItemManager>.Instance.Recycle();
        Singleton<GamePropManager>.Instance.Recycle();
        GameLogManager.SendPageLog(base.Name.ToString(), 2.ToString());
    }

    public void AddDic()
    {
        for (int i = 0; i < this.listDebris.Count; i++)
        {
            if (!this.DicDebris.ContainsKey(this.listDebris[i]))
            {
                this.DicDebris.Add(this.listDebris[i], 1);
            }
            else
            {
                Dictionary<DebrisData, int> dicDebris;
                DebrisData key = null;
                dicDebris = this.DicDebris; (dicDebris)[key = this.listDebris[i]] = dicDebris[key] + 1;
            }
        }
        this.ShowDebris();
    }

    public void ShowDebris()
    {
        int num = 0;
        foreach (DebrisData key in this.DicDebris.Keys)
        {
            ItemData itemData = ItemDataManager.GetItemData(key.ItemID);
            Singleton<GlobalData>.Instance.InGameCurItem = key;
            this.DebrisObjects[num].SetActive(true);
            if (itemData.ItemTag == DataCenter.ItemType.Prop)
            {
                PropData propData = PropDataManager.GetPropData(itemData.ID);
                this.DebrisNumber[num].text = "X" + this.DicDebris[key];
                this.DebrisIcon[num].sprite = Singleton<UiManager>.Instance.GetSprite(propData.Icon);
            }
            else if (itemData.ItemTag == DataCenter.ItemType.Weapon)
            {
                WeaponData weaponData = WeaponDataManager.GetWeaponData(itemData.ID);
                this.DebrisNumber[num].text = "X" + this.DicDebris[key];
                this.DebrisIcon[num].sprite = Singleton<UiManager>.Instance.GetSprite(weaponData.Icon);
            }
            num++;
        }
    }

    public void RefreshPage()
    {
        this.listDebris.Clear();
        this.DicDebris.Clear();
        List<DropItemType> dropList = Singleton<DropItemManager>.Instance.GetDropList();
        Singleton<FontChanger>.Instance.SetFont(TitleName);
        if (Singleton<UiManager>.Instance.GameSuccess == 1)
        {
            PlayerDataManager.AddExperience(10);
            this.TitleName.text = Singleton<GlobalData>.Instance.GetText("SUCCEED");
            if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.CONVOY)
            {
                AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.CONVOY_CHECKPOINT, 1);
            }
            else if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.BATTERY)
            {
                AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.DEFENSE_CHECKPOINT, 1);
            }
            else if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.CARRY)
            {
                AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.TRANSPORT_CHECKPOINT, 1);
            }
            else if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.WIPE_OUT)
            {
                AchievementDataManager.SetAchievementValue(DataCenter.AchievementType.WIPEOUT_CHECKPOINT, 1);
            }
            if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.WEAPON)
            {
                this.DoubleButton.gameObject.SetActive(false);
            }
            else
            {
                this.DoubleButton.gameObject.SetActive(Singleton<GlobalData>.Instance.FinishPageAdvertisement > 0);
            }
            for (int i = 0; i < dropList.Count; i++)
            {
                if (dropList[i] == DropItemType.DEBRIS)
                {
                    this.DebrisNum++;
                }
            }
            this.dnaNum += Singleton<GlobalData>.Instance.GameDNA;
            this.dnaNum += this.GetRewardCount(CommonDataType.DNA);
            this.goldNum += Singleton<GlobalData>.Instance.GameGoldCoins;
            this.goldNum += this.GetRewardCount(CommonDataType.GOLD);
            if (ItemDataManager.GetCommonItem(CommonDataType.DOUBLE).Count == 1)
            {
                this.dnaNum += this.dnaNum;
                this.goldNum += this.goldNum;
            }
            ItemDataManager.SetCurrency(CommonDataType.DNA, this.dnaNum);
            ItemDataManager.SetCurrency(CommonDataType.GOLD, this.goldNum);
            if (Singleton<GlobalData>.Instance.ListGameDebris != null)
            {
                if (this.DebrisNum <= Singleton<GlobalData>.Instance.ListGameDebris.Count)
                {
                    this.listDebris = Singleton<GlobalData>.Instance.ListGameDebris;
                }
                else
                {
                    this.listDebris = Singleton<GlobalData>.Instance.ListGameDebris;
                    for (int j = 0; j < this.DebrisNum - this.listDebris.Count; j++)
                    {
                        DebrisData item = DebrisDataManager.CollectDropDebris();
                        this.listDebris.Add(item);
                    }
                }
            }
            else if (Singleton<UiManager>.Instance.GameSuccess == -1)
            {
                for (int k = 0; k < this.DebrisNum; k++)
                {
                    DebrisData item2 = DebrisDataManager.CollectDropDebris();
                    this.listDebris.Add(item2);
                }
            }
            this.AddDic();
            Singleton<GlobalData>.Instance.GameDNA = this.dnaNum;
            Singleton<GlobalData>.Instance.GameGoldCoins = this.goldNum;
            if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.MAINLINE || CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING || CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.MAINLINE_SNIPE || CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.SNIPE)
            {
                if (CheckpointDataManager.SelectCheckpoint.Chapters == ChapterEnum.CHAPTERNAME_02 && CheckpointDataManager.SelectCheckpoint.Index == 1 && !CheckpointDataManager.SelectCheckpoint.Passed && !PlayerPrefs.GetString("FACEBOOK_NOTICE_RESULT").Equals("SUCCESS") && Singleton<GlobalData>.Instance.FacebookNoticeCounts > 0)
                {
                    Singleton<UiManager>.Instance.ShowFacebookGroupPage(15);
                }
                CheckpointDataManager.SetCheckpointPassed(CheckpointDataManager.SelectCheckpoint);
            }
        }
        else
        {
            this.DoubleButton.gameObject.SetActive(false);
            this.TitleName.text = Singleton<GlobalData>.Instance.GetText("FAILED");
        }
        this.rewardNameTxt[0].text = Singleton<GlobalData>.Instance.GetText("KILL_ZOMBIES");
        this.rewardNameTxt[1].text = Singleton<GlobalData>.Instance.GetText("HEADSHOT_RATE");
        this.rewardNameTxt[2].text = Singleton<GlobalData>.Instance.GetText("EARN_GOLDS");
        this.rewardNameTxt[3].text = Singleton<GlobalData>.Instance.GetText("EARN_DNA");
        this.rewardNumTxt[2].text = this.goldNum.ToString();
        this.rewardNumTxt[3].text = this.dnaNum.ToString();
        Singleton<FontChanger>.Instance.SetFont(rewardNameTxt[0]);
        Singleton<FontChanger>.Instance.SetFont(rewardNameTxt[1]);
        Singleton<FontChanger>.Instance.SetFont(rewardNameTxt[2]);
        Singleton<FontChanger>.Instance.SetFont(rewardNameTxt[3]);
        Singleton<FontChanger>.Instance.SetFont(rewardNumTxt[2]);
        Singleton<FontChanger>.Instance.SetFont(rewardNumTxt[3]);
        if (CheckpointDataManager.SelectCheckpoint.Type == CheckpointType.RACING)
        {
            this.rewardNumTxt[0].text = RacingSceneManager.Instance.KillZombies.ToString();
            this.rewardNumTxt[1].text = 0 + "%";
        }
        else
        {
            this.rewardNumTxt[0].text = GameApp.GetInstance().GetGameScene().PlayerKill.ToString();
            if (GameApp.GetInstance().GetGameScene().PlayerKill != 0)
            {
                this.rewardNumTxt[1].text = (float)(1000 * GameApp.GetInstance().GetGameScene().PlayerHeadShot / GameApp.GetInstance().GetGameScene().PlayerKill) / 10f + "%";
            }
            else
            {
                this.rewardNumTxt[1].text = 0 + "%";
            }
        }
        this.returnNameTxt.text = Singleton<GlobalData>.Instance.GetText("RETURN");
        this.doubleNameTxt.text = Singleton<GlobalData>.Instance.GetText("DOUBLE");
        Singleton<FontChanger>.Instance.SetFont(returnNameTxt);
        Singleton<FontChanger>.Instance.SetFont(doubleNameTxt);
        
    }

    public int GetRewardCount(CommonDataType type)
    {
        int num = 0;
        CheckpointData checkpointData = null;
        checkpointData = ((CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.MAINLINE && CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.MAINLINE_SNIPE && CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.RACING && CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.SNIPE && CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.BOSS) ? CheckpointDataManager.GetMaxPassedCheckpoint() : CheckpointDataManager.SelectCheckpoint);
        if (checkpointData != null)
        {
            for (int i = 0; i < checkpointData.AwardItem.Length; i++)
            {
                if (checkpointData.AwardItem[i] == (int)type)
                {
                    num = checkpointData.AwardCount[i];
                }
            }
            switch (CheckpointDataManager.SelectCheckpoint.Type)
            {
                case CheckpointType.MAINLINE:
                case CheckpointType.RACING:
                case CheckpointType.MAINLINE_SNIPE:
                    if (checkpointData.Passed)
                    {
                        num = (int)((float)num * 0.1f);
                    }
                    break;
                case CheckpointType.GOLD:
                    num *= 2;
                    break;
                case CheckpointType.WEAPON:
                    num *= 2;
                    break;
            }
        }
        return num;
    }

    public void DoubuleSucesss()
    {
        this.DoubleButton.gameObject.SetActive(false);
        Singleton<GlobalData>.Instance.FinishPageAdvertisement--;
        ItemDataManager.SetCurrency(CommonDataType.GOLD, this.goldNum);
        ItemDataManager.SetCurrency(CommonDataType.DNA, this.dnaNum);
        int[] id = new int[2] {
            1,
            3
        };
        int[] count = new int[2] {
            this.goldNum,
            this.dnaNum
        };
        Singleton<UiManager>.Instance.ShowAward(id, count, null);
        Singleton<GlobalData>.Instance.GameDNA += Singleton<GlobalData>.Instance.GameDNA;
        Singleton<GlobalData>.Instance.GameGoldCoins += Singleton<GlobalData>.Instance.GameGoldCoins;
        GameLogManager.SendPageLog(base.Name.ToString(), "DoubleReward");
    }
    private void OnFinish(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            this.DoubuleSucesss();
        }
        else
        {
            //no reward
        }
    }
    public void OnclickDouble()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        //Advertisements.Instance.ShowRewardedVideo(OnFinish);
        Ads.ShowReward(DoubuleSucesss);
        //Singleton<GlobalData>.Instance.ShowAdvertisement(-11 - Mathf.Clamp(CheckpointDataManager.SelectCheckpoint.ID - 1, 0, 10), delegate
        //{
        //    this.DoubuleSucesss();
        //}, null);
    }

    public void OnclickReturn()
    {
        Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
        Singleton<GameAudioManager>.Instance.ClearAudio();
        this.Close();
        if (CheckpointDataManager.SelectCheckpoint.Type != CheckpointType.RACING)
        {
            Singleton<UiManager>.Instance.RemovePage(PageName.InGamePage);
        }
        Singleton<UiManager>.Instance.ShowTopBar(true);
        Singleton<UiManager>.Instance.ShowLoadingPage("UI", PageName.MainPage);
        Singleton<DropItemManager>.Instance.Recycle();
        Singleton<GamePropManager>.Instance.Recycle();
        GameLogManager.SendPageLog(base.Name.ToString(), 2.ToString());
    }
}


