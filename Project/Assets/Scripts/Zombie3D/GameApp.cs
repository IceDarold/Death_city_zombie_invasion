using System;
using UnityEngine;

namespace Zombie3D
{
    public class GameApp
    {
        protected static GameApp instance;

        protected ResourceConfigScript resourceConfig;

        protected GameScene scene;

        protected GameScript script;

        public Action<float> setRoleHp;

        protected bool isInit;

        public DeviceOrientation PreviousOrientation
        {
            get;
            set;
        }

        protected GameApp()
        {
        }

        public static GameApp GetInstance()
        {
            if (GameApp.instance == null)
            {
                GameApp.instance = new GameApp();
                GameApp.instance.PreviousOrientation = DeviceOrientation.Portrait;
            }
            return GameApp.instance;
        }

        public GameObject ShowUI(string name, int zorder)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(name)) as GameObject;
            gameObject.transform.parent = GameObject.Find("Canvas").transform;
            gameObject.transform.localPosition = new Vector3(0f, 0f, (float)zorder);
            gameObject.transform.localScale = Vector3.one;
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            gameObject.name = name;
            return gameObject;
        }

        public void ShowTip(string content)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FlagTextPanel")) as GameObject;
            gameObject.transform.parent = GameObject.Find("Canvas").transform;
            gameObject.transform.localPosition = new Vector3(0f, 0f, -1000f);
            gameObject.transform.localScale = Vector3.one;
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            gameObject.GetComponent<FlagText>().SetString(content);
        }

        public void InitConfigDataAndGameRecorder()
        {
        }

        public void Save()
        {
        }

        public bool Load()
        {
            return false;
        }

        public void Init()
        {
            if (!this.isInit)
            {
                this.isInit = true;
                this.LoadResource();
            }
        }

        public void LoadResource()
        {
            this.resourceConfig = Singleton<ResourceConfigScript>.Instance;
        }

        public void LoadConfig()
        {
        }

        public void CreateScene(string sceneName)
        {
            this.script = UnityEngine.Object.FindObjectOfType<GameScript>();
            this.scene = new GameScene();
            this.scene.Init();
        }

        public void InitScene()
        {
            this.scene.GetPlayer().SetPlayerPosition();
        }

        public void AddMultiplayerComponents()
        {
            this.scene.AddNetworkComponents();
        }

        public void ClearScene()
        {
            this.scene = null;
        }

        public void Loop(float deltaTime)
        {
            this.scene.DoLogic(deltaTime);
        }

        public ResourceConfigScript GetResourceConfig()
        {
            return this.resourceConfig;
        }

        public GameScene GetGameScene()
        {
            return this.scene;
        }

        public void ReleaseGameScene()
        {
            this.scene.Dispose();
            this.scene = null;
        }

        public void DoSetRoleHp(float percent)
        {
            if (this.setRoleHp != null)
            {
                this.setRoleHp(percent);
            }
        }

        public static void SendStartGameLog()
        {
        }

        public static void SendGameOverLog(bool isWin, int reviveCount, float distances, float score, int gameTimeSeconds, int coinCollection)
        {
        }

        public static void SendPageLog(bool isPage, string from, string to, string mark)
        {
        }

        public static void DoVideo(int page, Action callback, Action failCallBack = null)
        {
        }

        public static void ShowAdAndVideo(int position, Action<bool, PushType, AdPlayResultCode, int> resultCB = null)
        {
        }

        public static void DoCharge(int cId, bool isBuyItem, int itemIdFor, int consumePath, Action<bool> _callback)
        {
        }
    }
}


