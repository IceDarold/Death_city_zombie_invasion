using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zombie3D;

public class SceneNPC : SceneMissionItem, EnemyTarget
{
    [CNName("测试")]
    public Transform target;

    public Animator animator;

    public NavMeshAgent nav;

    [CNName("移动速度")]
    public float speed = 3f;

    [CNName("跳下速度")]
    public float jumpSpeed = 1f;

    [CNName("翻越和爬出的速度")]
    public float crossSpeed = 1f;

    [CNName("NPC攻击间隔")]
    public float attackInterval = 2f;

    [CNName("血量")]
    public float maxHp = 15f;

    [CNName("防御力")]
    public float armor = 999f;

    [CNName("攻击半径")]
    public float attackRadius = 3f;

    [CNName("枪火")]
    public ParticleSystem gunFire;

    [CNName("受伤")]
    public AudioClip audioHurt;

    [CNName("死亡")]
    public AudioClip audioDead;

    protected NPCSTATE state = NPCSTATE.NONE;

    protected const string aniKey = "state";

    protected const string aniHurt = "OnHurt";

    protected const string aniDead = "OnDead";

    protected Vector3 targetPos;

    protected float navDuration;

    protected OffMeshLinkData curOffMesh;

    protected bool canDoJumpOffset;

    protected Quaternion specialRotate = Quaternion.identity;

    protected bool isCrossActionOver;

    protected int curPointIndex = -1;

    protected NPCCreater creater;

    protected List<NPCPathPoint> pathPoint;

    protected Mission mission;

    protected bool isInLoopAnimation;

    protected float loopCount;

    protected Enemy targetEnemy;

    protected TimeScheduler shootSchduler;

    protected TimeScheduler checkEnemy;

    protected float hp;

    protected GameScene gameScene;

    protected bool needSetHp2UI;

    protected NPCSTATE preState = NPCSTATE.NONE;

    protected AudioPlayer audioPlayer;

    public List<string> stateList = new List<string>();

    public override SceneMissionItem Init(MissionItemInfo info)
    {
        return base.Init(info);
    }

    public void Init(NPCCreater _creater)
    {
        this.creater = _creater;
        this.gameScene = GameApp.GetInstance().GetGameScene();
        this.SetState(this.creater.startState);
        if ((UnityEngine.Object)this.creater.startTrigger != (UnityEngine.Object)null)
        {
            this.creater.startTrigger.SetOnTriggerEnter(new Action(this.DoWakeUp));
        }
        this.shootSchduler = new TimeScheduler(this.attackInterval, delegate
        {
            this.AttackAction();
        }, null);
        this.checkEnemy = new TimeScheduler(0.5f, new Action(this.CheckGoToShoot), null);
        this.hp = this.maxHp;
        this.audioPlayer = new AudioPlayer();
        Transform folderTrans = base.transform.Find("Audio");
        this.audioPlayer.AddAudio(folderTrans, "Dead");
        this.audioPlayer.AddAudio(folderTrans, "Hurt");
        this.audioPlayer.AddAudio(folderTrans, "Shoot");
    }

    public void SetHP2UI(bool needSet2UI)
    {
        this.needSetHp2UI = needSet2UI;
        if (needSet2UI)
        {
            this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.FOCUS_NPC_HP, this.hp, this.maxHp);
        }
    }

    public void SetMission(Mission _mission)
    {
        this.mission = _mission;
        this.pathPoint = this.mission.pathPoint;
        if (this.mission.isMoveAction)
        {
            this.DoActive();
        }
        else
        {
            this.SetState(this.mission.loopAnimation);
            this.loopCount = this.mission.limitTime;
        }
        for (int i = 0; i < _mission.pathPoint.Count; i++)
        {
            if ((UnityEngine.Object)_mission.pathPoint[i].activeCollider != (UnityEngine.Object)null)
            {
                _mission.pathPoint[i].activeCollider.SetOnTriggerEnter(new Action(this.DoActive));
            }
        }
        this.isInLoopAnimation = !this.mission.isMoveAction;
    }

    private bool CheckEnemyInAttackRadius()
    {
        bool result = false;
        Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
        object[] array = new object[enemies.Count];
        enemies.Keys.CopyTo(array, 0);
        int i = 0;
        for (int num = array.Length; i < num; i++)
        {
            Enemy enemy = enemies[array[i]] as Enemy;
            if (enemy.GetState() == Enemy.DEAD_STATE)
            {
                break;
            }
            Vector3 position = base.transform.position;
            float y = position.y;
            Vector3 position2 = enemy.GetTransform().position;
            if (!(Mathf.Abs(y - position2.y) >= 0.5f) && (base.transform.position - enemy.GetTransform().position).magnitude < this.attackRadius)
            {
                this.targetEnemy = enemy;
                result = true;
                break;
            }
        }
        return result;
    }

    public override void Update()
    {
        if (this.gameScene.PlayingState != 0 && this.gameScene.PlayingState != PlayingState.WaitForEnd)
        {
            return;
        }
        switch (this.state)
        {
            case NPCSTATE.WALK:
            case NPCSTATE.RUN:
                this.DoMove();
                break;
            case NPCSTATE.JUMP:
                this.DoJump();
                break;
            case NPCSTATE.CLIMB:
            case NPCSTATE.STEPOUT:
                this.DoCross();
                break;
            case NPCSTATE.ATTACK:
                this.DoAttack();
                break;
            case NPCSTATE.IDLE:
                this.DoIdle();
                break;
            case NPCSTATE.OPERATE:
                this.DoOperate(Time.deltaTime);
                break;
        }
        this.CheckLoopAnimationTime(Time.deltaTime);
    }

    private void SetState(NPCSTATE _state)
    {
        this.stateList.Add("NPCSTATE = " + this.state + "---->" + _state);
        switch (_state)
        {
            case NPCSTATE.JUMP:
                this.nav.speed = 0f;
                break;
            case NPCSTATE.ATTACK:
                this.nav.speed = 0f;
                this.RecordPreState(this.state);
                break;
            case NPCSTATE.HURT:
                this.nav.speed = 0f;
                this.RecordPreState(this.state);
                this.animator.SetTrigger("OnHurt");
                break;
            case NPCSTATE.DEAD:
                this.nav.speed = 0f;
                this.RecordPreState(this.state);
                this.animator.SetTrigger("OnDead");
                break;
        }
        this.state = _state;
        this.animator.SetInteger("ActionID", (int)this.state);
        if (this.state != NPCSTATE.JUMP && this.state != NPCSTATE.CLIMB && this.state != NPCSTATE.STEPOUT)
        {
            return;
        }
        this.CaculateSpecialStateRotate();
        this.crossSpeed = (this.curOffMesh.startPos - this.curOffMesh.endPos).magnitude / 2.333f;
    }

    private void RecordPreState(NPCSTATE _state)
    {
        if (_state != NPCSTATE.ATTACK && _state != NPCSTATE.HURT && _state != NPCSTATE.DEAD)
        {
            this.preState = _state;
        }
    }

    private void CaculateSpecialStateRotate()
    {
        Vector3 forward = this.curOffMesh.endPos - this.curOffMesh.startPos;
        forward.y = 0f;
        this.specialRotate = Quaternion.LookRotation(forward);
    }

    private void DoMove()
    {
        this.nav.speed = Mathf.Lerp(this.nav.speed, this.speed, Time.deltaTime * 2f);
        if (this.nav.isOnOffMeshLink)
        {
            this.curOffMesh = this.nav.currentOffMeshLinkData;
            this.nav.SetDestination(this.curOffMesh.startPos);
            Vector3 vector = base.transform.position - this.curOffMesh.startPos;
            vector.y = 0f;
            if (vector.magnitude < 0.1f)
            {
                switch (this.curOffMesh.offMeshLink.area)
                {
                    case 6:
                        this.SetState(NPCSTATE.JUMP);
                        break;
                    case 7:
                        this.SetState(NPCSTATE.CLIMB);
                        break;
                    case 8:
                        this.SetState(NPCSTATE.STEPOUT);
                        break;
                }
            }
            base.transform.position = Vector3.MoveTowards(base.transform.position, this.curOffMesh.startPos, this.speed * Time.deltaTime);
        }
        else
        {
            if ((UnityEngine.Object)this.target != (UnityEngine.Object)null)
            {
                this.nav.SetDestination(this.target.position);
            }
            Vector3 position = base.transform.position;
            float x = position.x;
            Vector3 position2 = this.target.position;
            float num = x - position2.x;
            Vector3 position3 = base.transform.position;
            float x2 = position3.x;
            Vector3 position4 = this.target.position;
            float num2 = num * (x2 - position4.x);
            Vector3 position5 = base.transform.position;
            float z = position5.z;
            Vector3 position6 = this.target.position;
            float num3 = z - position6.z;
            Vector3 position7 = base.transform.position;
            float z2 = position7.z;
            Vector3 position8 = this.target.position;
            if (num2 + num3 * (z2 - position8.z) < 0.01f)
            {
                this.OnArrivePoint();
            }
        }
        this.checkEnemy.DoUpdate(Time.deltaTime);
    }

    private void CheckGoToShoot()
    {
        if (this.CheckEnemyInAttackRadius())
        {
            this.shootSchduler.ResetCounter();
            this.SetState(NPCSTATE.ATTACK);
        }
    }

    private void DoIdle()
    {
        this.checkEnemy.DoUpdate(Time.deltaTime);
    }

    private void DoOperate(float dt)
    {
    }

    private void CheckLoopAnimationTime(float dt)
    {
        if (this.isInLoopAnimation && this.gameScene.PlayingState == PlayingState.GamePlaying)
        {
            this.loopCount -= dt;
            if (this.loopCount <= 0f)
            {
                this.isInLoopAnimation = false;
                this.SetState(NPCSTATE.IDLE);
                this.gameScene.sceneMissions.SubmitMission();
            }
            this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.MISSION_ITEM_PERCENT, this.mission.limitTime - this.loopCount, this.mission.limitTime);
        }
    }

    private void DoAttack()
    {
        if ((this.targetEnemy == null || this.targetEnemy.GetState() == Enemy.DEAD_STATE) && !this.CheckEnemyInAttackRadius())
        {
            this.targetEnemy = null;
            this.SetState(this.preState);
            this.preState = NPCSTATE.NONE;
            return;
        }
        Quaternion b = Quaternion.LookRotation(this.targetEnemy.GetTransform().position - base.transform.position);
        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * 5f);
        this.shootSchduler.DoUpdate(Time.deltaTime);
    }

    private void OnPlayerFireKeyFrame()
    {
        Singleton<GameAudioManager>.Instance.PlaySoundInGame(this.audioPlayer.GetAudio("Shoot"), false);
        this.gunFire.Play();
        if (this.targetEnemy != null && this.targetEnemy.GetState() != Enemy.DEAD_STATE)
        {
            float damage = (this.targetEnemy.GetEnemyProbability() != 0) ? (this.targetEnemy.MaxHp * 0.01f) : (this.targetEnemy.MaxHp * 0.1f);
            this.targetEnemy.OnHit(new DamageProperty(damage), WeaponType.NPC_GUN, Bone.None);
        }
    }

    private void PlayGunAnimation(int id)
    {
    }

    private void AttackAction()
    {
        this.animator.SetTrigger("OnAttack");
    }

    private void DoCross()
    {
        this.nav.SetDestination(this.curOffMesh.endPos);
        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.specialRotate, Time.deltaTime * 5f);
        base.transform.position = Vector3.MoveTowards(base.transform.position, this.curOffMesh.endPos, this.crossSpeed * Time.deltaTime);
        if (this.isCrossActionOver)
        {
            this.nav.CompleteOffMeshLink();
            this.SetState(NPCSTATE.WALK);
            this.isCrossActionOver = false;
        }
    }

    private void DoJump()
    {
        this.nav.SetDestination(this.curOffMesh.endPos);
        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.specialRotate, Time.deltaTime * 5f);
        if (this.canDoJumpOffset)
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, this.curOffMesh.endPos, this.jumpSpeed * Time.deltaTime);
            if ((base.transform.position - this.curOffMesh.endPos).magnitude < 0.1f)
            {
                this.animator.SetTrigger("OnGround");
                this.nav.CompleteOffMeshLink();
                this.SetState(NPCSTATE.WALK);
            }
        }
    }

    private void RotateToTarget()
    {
        if ((UnityEngine.Object)this.target != (UnityEngine.Object)null)
        {
            Vector3 forward = default(Vector3);
            Vector3 vector = this.target.position - base.transform.position;
            forward.x = vector.x;
            forward.y = 0f;
            Vector3 vector2 = this.target.position - base.transform.position;
            forward.z = vector2.z;
            Quaternion b = Quaternion.LookRotation(forward);
            base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * 5f);
        }
    }

    private void RotateToPlayer()
    {
        Player player = this.gameScene.GetPlayer();
        Vector3 forward = default(Vector3);
        Vector3 vector = player.GetTransform().position - base.transform.position;
        forward.x = vector.x;
        forward.y = 0f;
        Vector3 vector2 = this.target.position - base.transform.position;
        forward.z = vector2.z;
        Quaternion b = Quaternion.LookRotation(forward);
        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * 5f);
    }

    public void DoJumpOffset(int canJump)
    {
        this.canDoJumpOffset = (canJump == 1);
    }

    public void OnActionOverEvent(int actionID)
    {
        switch (actionID)
        {
            case 11:
                this.SetState(NPCSTATE.IDLE);
                break;
            case 9:
                if (!this.isInLoopAnimation)
                {
                    if (this.pathPoint.Count > 0 && this.pathPoint[this.curPointIndex].autoActive)
                    {
                        this.DoActive();
                    }
                    else
                    {
                        this.SetState(NPCSTATE.IDLE);
                        if (this.pathPoint.Count > 0)
                        {
                            this.pathPoint[this.curPointIndex].activeCollider.gameObject.SetActive(true);
                        }
                    }
                }
                break;
            case 5:
                this.SetState(this.preState);
                break;
        }
    }

    public void DoCrossActionOver()
    {
        this.isCrossActionOver = true;
    }

    public void OnArrivePoint()
    {
        GameApp.GetInstance().GetGameScene().gamePlotManager.ShowPlot(this.pathPoint[this.curPointIndex].arrivePlot, null);
        if (this.pathPoint[this.curPointIndex].submitMission)
        {
            GameApp.GetInstance().GetGameScene().sceneMissions.SubmitMission();
        }
        else if (this.pathPoint[this.curPointIndex].autoActive && this.pathPoint[this.curPointIndex].arriveState == NPCSTATE.IDLE)
        {
            this.DoActive();
        }
        else
        {
            this.SetState(this.pathPoint[this.curPointIndex].arriveState);
            if ((UnityEngine.Object)this.pathPoint[this.curPointIndex].activeCollider != (UnityEngine.Object)null && this.pathPoint[this.curPointIndex].arriveState == NPCSTATE.IDLE)
            {
                this.pathPoint[this.curPointIndex].activeCollider.gameObject.SetActive(true);
            }
        }
    }

    [ContextMenu("激活")]
    public void DoActive()
    {
        this.curPointIndex++;
        if (this.curPointIndex == this.pathPoint.Count)
        {
            UnityEngine.Debug.LogError("已到达最后一个点!");
        }
        else
        {
            this.target = this.pathPoint[this.curPointIndex].trans;
            NPCSTATE moveAction = this.pathPoint[this.curPointIndex].moveAction;
            if (this.state == NPCSTATE.ATTACK || this.state == NPCSTATE.HURT || this.state == NPCSTATE.DEAD)
            {
                this.preState = moveAction;
            }
            else
            {
                this.SetState(moveAction);
            }
        }
    }

    public void DoWakeUp()
    {
        this.Reset();
        NPCSTATE nPCSTATE = this.state;
        if (nPCSTATE == NPCSTATE.DEPEND)
        {
            this.SetState(NPCSTATE.GETUP);
        }
        else
        {
            this.SetState(NPCSTATE.IDLE);
        }
    }

    private void Reset()
    {
        this.curPointIndex = -1;
    }

    public void OnHit(float damage, bool isCritical, AttackType type)
    {
        if (this.state != NPCSTATE.DEAD && this.state != NPCSTATE.JUMP && this.state != NPCSTATE.CLIMB && this.state != NPCSTATE.STEPOUT && this.gameScene.PlayingState == PlayingState.GamePlaying && !(this.hp <= 0f))
        {
            damage -= this.armor;
            float num = Mathf.Clamp(damage, 1f, damage);
            this.hp -= num;
            if (this.needSetHp2UI)
            {
                this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.FOCUS_NPC_HP, this.hp, this.maxHp);
            }
            if (this.hp <= 0f)
            {
                this.SetState(NPCSTATE.DEAD);
                this.gameScene.DoGameResult(false);
                this.audioPlayer.PlayAudio("Dead");
            }
            else
            {
                this.audioPlayer.PlayAudio("Hurt");
                this.SetState(NPCSTATE.HURT);
                this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.NPC_ONHIT);
            }
        }
    }

    [ContextMenu("死亡")]
    public void OnDead()
    {
        this.OnHit(1E+08f, true, AttackType.NORMAL_UNDODGGE);
    }

    [ContextMenu("复活")]
    public void OnRevive()
    {
    }

    public void OnOperate()
    {
        this.SetState(NPCSTATE.OPERATE);
    }

    [ContextMenu("受伤")]
    public void DoHurtTest()
    {
        this.OnHit(1f, true, AttackType.NORMAL_UNDODGGE);
    }

    public Transform GetTransform()
    {
        return base.transform;
    }

    public bool IsVisible()
    {
        return base.gameObject.activeInHierarchy;
    }

    public EnemyTargetType GetTargetType()
    {
        return EnemyTargetType.NPC;
    }

    public void DoRevive()
    {
        this.hp = this.maxHp;
        this.SetState(this.preState);
        this.preState = NPCSTATE.NONE;
        this.gameScene.SetUIDisplayEvnt(UIDisplayEvnt.FOCUS_NPC_HP, this.hp, this.maxHp);
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }

    public void DoPause()
    {
        this.nav.speed = 0f;
        if (this.state != NPCSTATE.DEAD)
        {
            this.animator.speed = 0f;
        }
    }

    public void DoResume()
    {
        this.animator.speed = 1f;
    }

    public void OnGUI()
    {
    }
}


