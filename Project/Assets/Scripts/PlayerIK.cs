using System;
using RootMotion.FinalIK;
using UnityEngine;
using Zombie3D;

[RequireComponent(typeof(Animator))]
public class PlayerIK : MonoBehaviour
{
    public Vector3 CameraAnchorPos
    {
        get
        {
            return this.cameraAnchor.position;
        }
    }

    public Quaternion CameraAnchorRotate
    {
        get
        {
            return this.cameraAnchor.rotation;
        }
    }

    private void Awake()
    {
        this.aimIK.SetPreUpdate(new IKSolver.UpdateDelegate(this.OnPreUpdate));
        this.aimIK.SetPostUpdate(new IKSolver.UpdateDelegate(this.OnPostUpdate));
        this.SetIKWeight(0f);
    }

    public void SetPlayer(Player _player)
    {
        this.player = _player;
    }

    public void SetGun(Transform _gunTrans, Weapon weapon)
    {
        if (weapon.GetWeaponType() == WeaponType.Sniper)
        {
            this.aimBoneTransCopy = this.aimBoneTransCopy_L;
            this.aimTemplete = this.aimTemplete_L;
        }
        else
        {
            this.aimBoneTransCopy = this.aimBoneTransCopy_R;
            this.aimTemplete = this.aimTemplete_R;
        }
        IKSolver iksolver = this.aimIK.GetIKSolver();
        (iksolver as IKSolverAim).transform = this.aimBoneTransCopy;
        this.animator.SetBool("SnipeHandle", this.player.GetWeapon().SnipeHasHandle());
        this.playerWeapon = weapon;
        this.gunTrans = _gunTrans;
        this.gunOffset = this.gunTrans.localPosition;
        this.gunRotation = this.gunTrans.localRotation;
        this.animator.SetInteger("GunID", weapon.GetGunIndex());
        this.animator.SetTrigger("OnGunOnArms");
    }

    public void SetIKWeight(float finalWeight)
    {
        this.aimIK.solver.IKPositionWeight = finalWeight;
    }

    public float GetIKWeight()
    {
        return this.aimIK.solver.IKPositionWeight;
    }

    public void RecorderPlayerIk()
    {
        this.ikWeightBeforeGunOff = this.aimIK.solver.IKPositionWeight;
    }

    public void SetIkWeightInChangeGun(float percent)
    {
        this.SetIKWeight(Mathf.Lerp(this.ikWeightBeforeGunOff, 0f, percent));
    }

    public void OnPreUpdate()
    {
        if (this.aimBoneTransCopy == null || this.aimTemplete == null)
        {
            return;
        }
        this.aimBoneTransCopy.position = this.aimTemplete.position;
        this.aimBoneTransCopy.rotation = this.aimTemplete.rotation;
        if (this.upperBodyLockedWeight > 0.01f)
        {
            Quaternion rotation = this.upperBodyRoot.rotation;
            Vector3 forward = new Vector3(this.pelvis.forward.x, 0f, this.pelvis.forward.z);
            Vector3 upwards = new Vector3(this.pelvis.up.x, 0f, this.pelvis.up.z);
            Quaternion b = Quaternion.LookRotation(forward, upwards);
            this.upperBodyRoot.rotation = Quaternion.Lerp(this.upperBodyRoot.rotation, b, this.upperBodyLockedWeight);
        }
    }

    public void OnPostUpdate()
    {
        if (this.playerWeapon == null || this.playerWeapon.firePointTransform == null)
        {
            return;
        }
        this.playerWeapon.SetFirePointInfo(this.playerWeapon.firePointTransform.position, this.playerWeapon.firePointTransform.rotation);
    }

    public void DoGunOff()
    {
        this.animator.SetTrigger("OnGunOff");
    }

    public void SetPlayerAnimator(bool isMoving, bool isShooting, bool forceIdle = false)
    {
        this.animator.SetBool("isShooting", isShooting);
        if (isShooting || forceIdle)
        {
            this.animator.SetBool("isAim", isShooting);
        }
    }

    public void SetIsAim(bool isAim)
    {
        this.animator.SetBool("isAim", isAim);
    }

    public void CancelCurrentAction()
    {
        this.animator.SetTrigger("OnPlayerCancelCurAction");
    }

    public void DoReload()
    {
        this.animator.SetTrigger("OnReload");
    }

    private void DoShoot(bool isShooting)
    {
        this.animator.SetBool("isShooting", isShooting);
    }

    public void DoWalk(bool walk)
    {
        this.animator.SetBool("isMoving", walk);
    }

    public void SetGunOn()
    {
        this.animator.SetTrigger("OnGunOn");
    }

    public void OnHurt()
    {
        this.animator.SetTrigger("OnHurt");
    }

    public void OnDead()
    {
        this.animator.SetTrigger("OnDead");
        this.onlyArmsWeight.Start(0f, 1f, 0f, null);
        this.normalWalkWeight.Start(0f, 1f, 0f, null);
    }

    public void SetReloadTimes(int _times)
    {
        this.animator.SetInteger("ReloadTimes", _times);
    }

    public void ReloadOnce()
    {
        int integer = this.animator.GetInteger("ReloadTimes");
        this.SetReloadTimes(integer - 1);
    }

    public void OnSkill(SkillAction action)
    {
        if (action == SkillAction.ACTION_THROW)
        {
            this.animator.SetTrigger("OnSkill1");
            this.player.PlayAudio("PlayerThrow");
        }
        else if (action == SkillAction.ACTION_CROUCH)
        {
            this.animator.SetTrigger("OnSkill2");
            this.player.PlayAudio("CrouchSetting");
        }
    }

    public void OnRebirth()
    {
        this.animator.SetTrigger("OnRebirth");
    }

    public void OnQTE()
    {
        this.animator.SetTrigger("OnQTE");
        this.anchorAnimator.SetTrigger("OnQTE");
    }

    public void DoSnipeCameraAction(PlayerPose _playerPose)
    {
        this.anchorAnimator.Play("SNIPE_STAND_INTRO");
    }

    public void CamerQTEOver()
    {
        this.anchorAnimator.SetTrigger("OnQTEOver");
    }

    public void SetPlayerGameInfo(AvatarType avatar, PlayerPose _playerPose)
    {
        this.animator.SetInteger("AvatarType", (int)avatar);
        if (_playerPose != PlayerPose.FREE)
        {
            this.animator.SetLayerWeight(0, 0f);
            this.animator.SetLayerWeight(1, 0f);
            this.animator.SetLayerWeight(2, 0f);
        }
        this.animator.SetInteger("PlayerPose", (int)_playerPose);
        this.anchorAnimator.SetInteger("AvatarType", (int)avatar);
        this.anchorAnimator.SetInteger("PlayerPose", (int)_playerPose);
    }

    public void ShowAttackBox()
    {
        this.attackBox.Show();
    }

    public void LateUpdate()
    {
        Quaternion rotation = this.upperBodyRoot.rotation;
        Vector3 forward = new Vector3(this.pelvis.forward.x, 0f, this.pelvis.forward.z);
        Vector3 upwards = new Vector3(this.pelvis.up.x, 0f, this.pelvis.up.z);
        Quaternion b = Quaternion.LookRotation(forward, upwards);
        this.upperBodyRoot.rotation = Quaternion.Lerp(this.upperBodyRoot.rotation, b, this.upperBodyLockedWeight);
        if (this.rotateBone == null)
        {
            return;
        }
        Transform transform = this.player.GetTransform();
        this.rootMatrix.SetTRS(this.rotateBone.position, transform.rotation, Vector3.one);
        this.rootMatrixInverse = this.rootMatrix.inverse;
        this.lockForward = this.rootMatrixInverse * this.rotateBone.forward;
        this.lockUp = this.rootMatrixInverse * this.rotateBone.up;
        Matrix4x4 lhs = Matrix4x4.TRS(this.rotateBone.position, Quaternion.Euler(transform.rotation.eulerAngles + this.finalRotateEular), Vector3.one);
        Vector3 forward2 = lhs * this.lockForward;
        Vector3 upwards2 = lhs * this.lockUp;
        Quaternion b2 = Quaternion.LookRotation(forward2, upwards2);
        this.rotateBone.rotation = Quaternion.Slerp(this.rotateBone.rotation, b2, this.testWeight);
    }

    public void Update()
    {
        this.SetIKWeight(Mathf.Lerp(this.aimIK.solver.IKPositionWeight, this.requireIKWeight, Time.deltaTime * 15f));
        this.upperBodyLockedWeight = Mathf.Lerp(this.upperBodyLockedWeight, this.requireLockbodyWeight, Time.deltaTime * 15f);
    }

    public void SetRequiredIKWeight(float ikweight, float _upperbodyLockedWeight)
    {
        this.requireIKWeight = ikweight;
        this.requireLockbodyWeight = _upperbodyLockedWeight;
    }

    [ContextMenu("调整标准骨骼")]
    private void FixAimTemplete()
    {
        if (this.aimTemplete == null || this.aimBoneTransCopy == null)
        {
            return;
        }
        this.aimTemplete.position = this.aimBoneTransCopy.position;
        this.aimTemplete.rotation = this.aimBoneTransCopy.rotation;
    }

    public void SetPickUpRadius(float radius)
    {
        this.pickUpCollider.localScale = Vector3.one * radius;
    }

    [Space(3f)]
    [SerializeField]
    [Header("Final IK")]
    public Animator animator;

    public AimIK aimIK;

    public Transform aimTemplete_R;

    public Transform aimTemplete_L;

    public Transform aimBoneTransCopy_R;

    public Transform aimBoneTransCopy_L;

    public Transform root;

    public Transform aimTarget;

    [CNName("瞄准IK生效曲线")]
    public AnimationCurve ikWeightCurve;

    public Vector3 gunOffset;

    public Quaternion gunRotation;

    [CNName("左手挂点")]
    public Transform leftHandGunRoot;

    [CNName("右手挂点")]
    public Transform rightHandGunRoot;

    [CNName("瞄准持续时间")]
    public float aimDuration = 3f;

    [CNName("上身根节点")]
    public Transform upperBodyRoot;

    [CNName("胯骨")]
    public Transform pelvis;

    [CNName("投掷点")]
    public Transform throwPoint;

    [CNName("弹夹左手")]
    public Transform leftHand;

    [CNName("弹夹右手")]
    public Transform rightHand;

    [CNName(0f, 1f, "锁定上半身权重")]
    public float upperBodyLockedWeight;

    [CNName("攻击框")]
    public PlayerAttackBox attackBox;

    public Transform bone01;

    public Transform bone02;

    public Transform bone03;

    public Transform rotateBone;

    public Vector3 finalRotateEular;

    [CNName(0f, 1f, "测试权重")]
    public float testWeight;

    [CNName("测试")]
    public RuntimeAnimatorController aniController;

    [CNName("相机动画Transform")]
    public Transform cameraAnchor;

    [CNName("相机动画")]
    public Animator anchorAnimator;

    [CNName("普通移速")]
    public float normalSpeed = 1f;

    [CNName("瞄准移速")]
    public float aimSpeed = 1f;

    [CNName("加血特效")]
    public ParticleSystem effectHp;

    public Transform pickUpCollider;

    private Matrix4x4 rootMatrix = default(Matrix4x4);

    private Matrix4x4 rootMatrixInverse;

    private Vector3 lockForward;

    private Vector3 lockUp;

    protected Transform gunTrans;

    protected float requireIKWeight;

    protected float requireLockbodyWeight;

    protected float lerpGunTime;

    protected Vector3 lerpStartPos;

    protected Quaternion lerpStartRotation;

    protected bool transitionStart;

    protected bool transitionEnd;

    protected LerpClass onlyArmsWeight = new LerpClass(1f);

    protected LerpClass upperBodyWeight = new LerpClass(0f);

    protected LerpClass normalWalkWeight = new LerpClass(1f);

    protected const int BASE_LAYER = 0;

    protected const int ONLY_ARMS = 1;

    protected const int UPPER_BODY = 2;

    protected const int FULL_BODY = 3;

    protected Transform aimTemplete;

    protected Transform aimBoneTransCopy;

    protected float ikWeightBeforeGunOff;

    protected Weapon playerWeapon;

    protected Player player;
}
