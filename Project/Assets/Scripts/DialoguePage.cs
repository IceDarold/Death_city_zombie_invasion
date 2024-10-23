using DataCenter;
using DG.Tweening;
using RacingMode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ui.imageTranslator;
using UnityEngine;
using UnityEngine.UI;
using Zombie3D;

public class DialoguePage : GamePage
{
    [CompilerGenerated]
    private sealed class _003CDelayClose_003Ec__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal DialoguePage _0024this;

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
        public _003CDelayClose_003Ec__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._0024PC;
            this._0024PC = -1;
            switch (num)
            {
                case 0u:
                    {
                        this._0024this.IsAction = true;
                        Transform leftRoleRoot = this._0024this.LeftRoleRoot;
                        Vector3 localPosition = this._0024this.LeftRoleRoot.localPosition;
                        leftRoleRoot.DOLocalMoveX(localPosition.x - 200f, 0.5f, false);
                        Transform rightRoleRoot = this._0024this.RightRoleRoot;
                        Vector3 localPosition2 = this._0024this.RightRoleRoot.localPosition;
                        rightRoleRoot.DOLocalMoveX(localPosition2.x - 200f, 0.5f, false);
                        this._0024current = new WaitForSeconds(0.5f);
                        if (!this._0024disposing)
                        {
                            this._0024PC = 1;
                        }
                        return true;
                    }
                case 1u:
                    this._0024this.IsAction = false;
                    this._0024this.Close();
                    this._0024this.Clear();
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

    public GameObject DialogueInGame;

    public GameObject DialogueInUi;

    public Image RoleIcon;

    public Text Content;

    public Transform LeftRoleTag;

    public Transform RightRoleTag;

    public Transform RoleModelPool;

    public Transform LeftRoleRoot;

    public Transform RightRoleRoot;

    public Transform ContentRoot;

    public DialogueContent LeftStencil;

    public DialogueContent RightStencil;

    public float Duration;

    [HideInInspector]
    public int index;

    [HideInInspector]
    public List<DialogueData> Dialogues = new List<DialogueData>();

    private bool autoPlay;

    private float duration;

    private Animator LeftRole;

    private Animator RightRole;

    private Dictionary<string, Transform> RoleModels = new Dictionary<string, Transform>();

    private List<DialogueContent> LeftContents = new List<DialogueContent>();

    private List<DialogueContent> RightContents = new List<DialogueContent>();

    public override void Close()
    {
        base.Hide();
        Singleton<UiManager>.Instance.SetCurrentPage();
    }

    public override void OnBack()
    {
        this.OnClick();
    }

    private new void OnEnable()
    {
        ((Component)this.ContentRoot).GetComponent<RectTransform>().sizeDelta = new Vector2(640f, (float)(this.Dialogues.Count * 250));
        if (this.Dialogues.Count > this.index)
        {
            if (this.Dialogues[this.index].Type == 1)
            {
                this.DialogueInUi.SetActive(true);
                this.DialogueInGame.SetActive(false);
                DialogueData dia = this.Dialogues[this.index];
                this.ShowDialogue(dia);
                this.index++;
            }
            else if (this.Dialogues[this.index].Type == 2)
            {
                this.DialogueInUi.SetActive(false);
                this.DialogueInGame.SetActive(false);
                this.autoPlay = true;
                this.duration = this.Duration;
            }
        }
        else
        {
            UnityEngine.Debug.LogError("你确定这里有对话吗？");
        }
    }

    private void ShowDialogue(DialogueData dia)
    {
        if (dia.Position == 1)
        {
            this.LeftRoleRoot.localPosition = this.LeftRoleTag.localPosition + new Vector3(0f, 0f, -400f);
            this.RightRoleRoot.localPosition = this.RightRoleTag.localPosition + new Vector3(0f, 0f, 400f);
            if ((UnityEngine.Object)this.LeftRole == (UnityEngine.Object)null)
            {
                this.LoadRoleModel(dia);
                Transform leftRoleRoot = this.LeftRoleRoot;
                Vector3 localPosition = this.LeftRoleRoot.localPosition;
                leftRoleRoot.DOLocalMoveX(localPosition.x - 200f, 0.5f, false).From();
            }
            this.LeftRole.Play(dia.RoleAnimation);
        }
        else if (dia.Position == 2)
        {
            this.LeftRoleRoot.localPosition = this.LeftRoleTag.localPosition + new Vector3(0f, 0f, 400f);
            this.RightRoleRoot.localPosition = this.RightRoleTag.localPosition + new Vector3(0f, 0f, -400f);
            if ((UnityEngine.Object)this.RightRole == (UnityEngine.Object)null)
            {
                this.LoadRoleModel(dia);
                Transform rightRoleRoot = this.RightRoleRoot;
                Vector3 localPosition2 = this.RightRoleRoot.localPosition;
                rightRoleRoot.DOLocalMoveX(localPosition2.x + 200f, 0.5f, false).From();
            }
            this.RightRole.Play(dia.RoleAnimation);
        }
        this.CreateDialogueContent(dia);
    }

    private void LoadRoleModel(DialogueData dia)
    {
        string roleModel = dia.RoleModel;
        if (!this.RoleModels.ContainsKey(dia.RoleModel))
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("DialogueModel/" + dia.RoleModel)) as GameObject;
            gameObject.name = roleModel;
            gameObject.transform.SetParent(this.RoleModelPool);
            this.RoleModels.Add(roleModel, gameObject.transform);
        }
        if (dia.Position == 1)
        {
            this.RoleModels[roleModel].SetParent(this.LeftRoleRoot);
            this.RoleModels[roleModel].localRotation = new Quaternion(0f, 0f, 0f, 0f);
            this.RoleModels[roleModel].localPosition = Vector3.zero;
            this.RoleModels[roleModel].localScale = new Vector3(450f, 450f, 450f);
            this.LeftRole = ((Component)this.RoleModels[roleModel]).GetComponent<Animator>();
        }
        else if (dia.Position == 2)
        {
            this.RoleModels[roleModel].SetParent(this.RightRoleRoot);
            this.RoleModels[roleModel].localRotation = new Quaternion(0f, 0f, 0f, 0f);
            this.RoleModels[roleModel].localPosition = Vector3.zero;
            this.RoleModels[roleModel].localScale = new Vector3(450f, 450f, 450f);
            this.RightRole = ((Component)this.RoleModels[roleModel]).GetComponent<Animator>();
        }
        this.RoleModels[roleModel].gameObject.SetActive(true);
    }

    private void CreateDialogueContent(DialogueData dia)
    {
        if (dia.Position == 1)
        {
            for (int i = 0; i < this.LeftContents.Count; i++)
            {
                if (!this.LeftContents[i].gameObject.activeSelf)
                {
                    this.LeftContents[i].Show(dia);
                    this.LeftContents[i].transform.SetAsLastSibling();
                    this.LeftContents[i].gameObject.SetActive(true);
                    return;
                }
            }
        }
        else
        {
            for (int j = 0; j < this.RightContents.Count; j++)
            {
                if (!this.RightContents[j].gameObject.activeSelf)
                {
                    this.RightContents[j].Show(dia);
                    this.RightContents[j].transform.SetAsLastSibling();
                    this.RightContents[j].gameObject.SetActive(true);
                    return;
                }
            }
        }
        DialogueContent dialogueContent = UnityEngine.Object.Instantiate((dia.Position != 1) ? this.RightStencil : this.LeftStencil);
        dialogueContent.transform.SetParent(this.ContentRoot);
        dialogueContent.transform.localPosition = Vector3.zero;
        dialogueContent.transform.localScale = Vector3.one;
        dialogueContent.Show(dia);
        dialogueContent.gameObject.SetActive(true);
        if (dia.Position == 1)
        {
            this.LeftContents.Add(dialogueContent);
        }
        else
        {
            this.RightContents.Add(dialogueContent);
        }
    }

    private void AutoPlay()
    {
        if (this.autoPlay)
        {
            this.DialogueInGame.SetActive(true);
            if (this.duration >= this.Duration)
            {
                this.duration = 0f;
                if (this.Dialogues.Count > this.index)
                {
                    this.DialogueInGame.transform.localScale = Vector3.zero;
                    this.DialogueInGame.transform.DOScale(Vector3.one, 0.2f);
                    this.RoleIcon.sprite = Singleton<UiManager>.Instance.GetSprite(this.Dialogues[this.index].RoleIcon);
                    this.Content.text = Singleton<GlobalData>.Instance.GetText(this.Dialogues[this.index].Content);
                    Singleton<FontChanger>.Instance.SetFont(Content);
                    this.index++;
                }
                else
                {
                    this.autoPlay = false;
                    this.Close();
                }
            }
            else
            {
                this.duration += Time.deltaTime;
            }
        }
    }

    public void OnClick()
    {
        if (this.Dialogues.Count > this.index)
        {
            if (this.Dialogues[this.index].Type == 1)
            {
                Singleton<GameAudioManager>.Instance.PlaySound(Singleton<GameAudioManager>.Instance.ConfirmClip, false);
                this.DialogueInUi.SetActive(true);
                this.DialogueInGame.SetActive(false);
                this.ShowDialogue(this.Dialogues[this.index]);
                this.index++;
            }
        }
        else if (!base.IsAction)
        {
            base.StartCoroutine(this.DelayClose());
        }
    }

    [DebuggerHidden]
    private IEnumerator DelayClose()
    {
        _003CDelayClose_003Ec__Iterator0 _003CDelayClose_003Ec__Iterator = new _003CDelayClose_003Ec__Iterator0();
        _003CDelayClose_003Ec__Iterator._0024this = this;
        return _003CDelayClose_003Ec__Iterator;
    }

    private void Clear()
    {
        this.LeftRole.transform.SetParent(this.RoleModelPool);
        this.LeftRole.gameObject.SetActive(false);
        this.LeftRole = null;
        this.RightRole.transform.SetParent(this.RoleModelPool);
        this.RightRole.gameObject.SetActive(false);
        this.RightRole = null;
        for (int i = 0; i < this.LeftContents.Count; i++)
        {
            this.LeftContents[i].gameObject.SetActive(false);
        }
        for (int j = 0; j < this.RightContents.Count; j++)
        {
            this.RightContents[j].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameApp.GetInstance().GetGameScene() != null)
        {
            switch (GameApp.GetInstance().GetGameScene().PlayingState)
            {
                case PlayingState.GamePause:
                    if (Singleton<UiManager>.Instance.CurrentPage.Name == PageName.PausePage)
                    {
                        this.DialogueInGame.SetActive(false);
                    }
                    else
                    {
                        this.AutoPlay();
                    }
                    break;
                case PlayingState.GamePlaying:
                case PlayingState.Changing:
                case PlayingState.WaitForEnd:
                    this.AutoPlay();
                    break;
                default:
                    this.Close();
                    break;
            }
        }
        else if ((UnityEngine.Object)RacingSceneManager.Instance != (UnityEngine.Object)null)
        {
            this.AutoPlay();
        }
    }
}
