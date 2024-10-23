using System;
using System.Collections;
using DataCenter;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPage : GamePage
{
	private new void Awake()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != "loading" && scene.name != "MainScene")
		{
			this.Hide();
			Singleton<UiManager>.Instance.ShowPage(this.NextPage, null);
		}
		if (scene.name != "MainScene" && scene.name != "UI" && scene.name != "loading" && scene.name != "RacingScene")
		{
			GameScript gameScript = UnityEngine.Object.FindObjectOfType<GameScript>();
			gameScript.DoLoadScene(this.NextScene);
		}
	}

	private void OnDisable()
	{
		Singleton<UiManager>.Instance.CanBack = true;
		if (!this.NextSceneIsGame())
		{
			Singleton<AssetBundleManager>.Instance.UnloadRetainedAB();
			Resources.UnloadUnusedAssets();
		}
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	public new void OnEnable()
	{
		Singleton<UiManager>.Instance.CanBack = false;
		if (UnityEngine.Random.Range(0, 100) < 25)
		{
			LogonAward recommendAward = LogonAwardSystem.GetRecommendAward();
			if (recommendAward == null)
			{
				this.RecommendPart.SetActive(false);
				int num = UnityEngine.Random.Range(2, 7);
				this.Tips.text = Singleton<GlobalData>.Instance.GetText("TIP_" + (num - 1).ToString("D2"));
				this.Background.sprite = this.LodingBackground[num - 1];
			}
			else
			{
				ItemData itemData = ItemDataManager.GetItemData(recommendAward.AwardItem);
				this.RecommendIcon.sprite = Singleton<UiManager>.Instance.GetSprite(itemData.Icon);
				string text = Singleton<GlobalData>.Instance.GetText("LOGON_RECOMMEND");
				text = text.Replace("#day#", recommendAward.ID.ToString());
				text = text.Replace("#award#", Singleton<GlobalData>.Instance.GetText(itemData.Name));
				this.RecommendTip.text = text;
				Singleton<FontChanger>.Instance.SetFont(RecommendTip);
				this.RecommendPart.SetActive(true);
				this.Background.sprite = this.LodingBackground[0];
				this.Tips.text = Singleton<GlobalData>.Instance.GetText("TIP_01");
			}
		}
		else
		{
			this.RecommendPart.SetActive(false);
			int num2 = UnityEngine.Random.Range(2, 7);
			this.Tips.text = Singleton<GlobalData>.Instance.GetText("TIP_" + (num2 - 1).ToString("D2"));
			this.Background.sprite = this.LodingBackground[num2 - 1];
		}
		Singleton<FontChanger>.Instance.SetFont(Tips);
		this.SetLoadProgress(0f);
	}

	public override void Show()
	{
		if (this.startLoad)
		{
			base.Show();
			string text = AssetBundleManager.SceneName2BundleName(this.NextScene);
			if (this.NextSceneIsGame() && !string.IsNullOrEmpty(text))
			{
				Singleton<AssetBundleManager>.Instance.CurSceneLoadFromAB = true;
				this.LoadAB(text, delegate
				{
					base.StartCoroutine(this.LoadScene());
				});
			}
			else
			{
				Singleton<AssetBundleManager>.Instance.CurSceneLoadFromAB = false;
				base.StartCoroutine(this.LoadScene());
			}
		}
		else
		{
			this.SetLoadProgress(0f);
			base.gameObject.SetActive(true);
		}
	}

	private bool NextSceneIsGame()
	{
		return !this.NextScene.Equals("MainScene") && !this.NextScene.Equals("UI") && !this.NextScene.Equals("loading");
	}

	public override void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public override void Close()
	{
	}

	public override void OnBack()
	{
	}

	public void Update()
	{
		if (this.op != null && this.startLoad && !this.op.isDone)
		{
			this.SetLoadProgress((float)((int)(100f * this.op.progress)));
		}
	}

	private IEnumerator LoadScene()
	{
		yield return new WaitForSeconds(0.2f);
		LoadSceneMode mode = LoadSceneMode.Single;
		this.op = SceneManager.LoadSceneAsync(this.NextScene, mode);
		yield return this.op;
		yield break;
	}

	private void LoadAB(string bundleName, Action cb)
	{
		Singleton<AssetBundleManager>.Instance.LoadABFromCatch("common", delegate
		{
			Singleton<AssetBundleManager>.Instance.LoadABFromCatch(bundleName, delegate
			{
				if (cb != null)
				{
					cb();
				}
			});
		});
	}

	private void SetLoadProgress(float dt)
	{
		this.LoadingSlider.maxValue = 100f;
		this.LoadingSlider.value = dt;
		this.LoadingValue.text = dt + "%";
	}

	public GameObject RecommendPart;

	public Image RecommendIcon;

	public Text RecommendTip;

	public Image Background;

	public Text Tips;

	public Text LoadingValue;

	public Slider LoadingSlider;

	public Sprite[] LodingBackground;

	[HideInInspector]
	public string NextScene;

	[HideInInspector]
	public PageName NextPage;

	public bool startLoad;

	private AsyncOperation op;
}
