using UnityEngine;
using Zombie3D;

[AddComponentMenu("TPS/TPSSimpleCamera")]
public class TPSSimpleCameraScript : BaseCameraScript, IMessageHandler
{
    public Texture reticle;

    public Texture reticleX;

    public Texture reticleY;

    public Texture reticleX_red;

    public Texture reticleY_red;

    public Texture leftTopReticle;

    public Texture rightTopReticle;

    public Texture leftBottomReticle;

    public Texture rightBottomReticle;

    public Texture[] shootReticle;

    public Texture shootPoint;

    public Texture shootPoint_red;

    public Texture[] shotGunReticle;

    public Texture[] shotGunReticle_red;

    public Texture aimOutRange;

    protected Vector2 reticleScale = Vector2.one;

    protected Shader transparentShader;

    protected Shader solidShader;

    protected float drx;

    protected float dry;

    protected float winTime = -1f;

    protected bool bloomEnable;

    public float rSmooth = 20f;

    public float mSmooth = 100f;

    [CNName("加特林缓动跟随")]
    public float cSmooth = 5f;

    protected bool needShow;

    [CNName("射击准星初始")]
    public float shootReticleOffset;

    [CNName("射击最终")]
    public float shootReticleFinalPos;

    [CNName("扩散速度")]
    public float diffuseSpeed = 5f;

    [CNName("扩散速度-射击")]
    public float diffuse_shoot;

    [CNName("收缩速度-射击")]
    public float shrink_shoot;

    [CNName("移动收缩速度")]
    public float move_shrink;

    private float shootReticleTemp;

    private void Awake()
    {
        base.cameraTransform = base.transform;
        this.shootReticleTemp = this.shootReticleFinalPos;
        Singleton<GlobalMessage>.Instance.RegisterMessageHandler(this);
    }

    public void OnDestroy()
    {
        Singleton<GlobalMessage>.Instance.RemoveMessageHandler(this);
    }

    public override CameraType GetCameraType()
    {
        return CameraType.TPSCamera;
    }

    private void Start()
    {
    }

    public void SetBloomEnable()
    {
        this.bloomEnable = !this.bloomEnable;
        base.bloom.enabled = this.bloomEnable;
        base.colorCurves.enabled = this.bloomEnable;
    }

    public override void SetCameraRotation(Transform trans)
    {
        base.angelV = 0f;
        Vector3 eulerAngles = trans.rotation.eulerAngles;
        base.angelH = eulerAngles.y;
        base.cameraTransform.rotation = Quaternion.Euler(0f - base.angelV, base.angelH, 0f);
        Vector3 position = base.player.GetTransform().TransformPoint(base.cameraDistanceFromPlayer);
        base.cameraTransform.position = position;
    }

    public override void SetCameraPosition(Transform trans)
    {
        base.cameraTransform.position = trans.position;
    }

    public override void ResetCameraPosition()
    {
        Vector3 position = base.player.GetTransform().TransformPoint(base.cameraDistanceFromPlayer);
        base.cameraTransform.position = position;
    }

    public void DoFollowPlayer(float deltaTime, float rx, float ry)
    {
        float num = base.reticlePosition.x - (float)(Screen.width / 2);
        if (base.allowReticleMove)
        {
            if (Mathf.Abs(num) < base.reticleLogoRange * (float)Screen.width || num * rx < 0f)
            {
                base.reticlePosition = new Vector2(base.reticlePosition.x + rx * base.reticleMoveSpeed, base.reticlePosition.y);
                if (base.limitReticle)
                {
                    if ((!(base.reticlePosition.y <= 40f) || !(ry > 0f)) && (!(base.reticlePosition.y > 310f) || !(ry < 0f)))
                    {
                        base.reticlePosition = new Vector2(base.reticlePosition.x, base.reticlePosition.y - ry * base.reticleMoveSpeed);
                    }
                }
                else
                {
                    base.reticlePosition = new Vector2(base.reticlePosition.x, base.reticlePosition.y - ry * base.reticleMoveSpeed);
                }
            }
            else
            {
                base.angelH += rx * deltaTime * base.cameraSwingSpeed;
                base.reticlePosition = new Vector2(base.reticlePosition.x, base.reticlePosition.y - ry * base.reticleMoveSpeed);
                base.angelV = base.fixedAngelV;
            }
        }
        else
        {
            if (Time.timeScale != 0f)
            {
                base.angelH += rx * 0.03f * base.cameraSwingSpeed * base.swingSpeedHorizontal;
                base.angelV += ry * 0.03f * base.cameraSwingSpeed * base.swingSpeedVertical;
            }
            if (base.isAngelVFixed)
            {
                base.angelV = base.fixedAngelV;
            }
            base.angelV = Mathf.Clamp(base.angelV, base.minAngelV, base.maxAngelV);
        }
        if (base.gameScene.PlayingState == PlayingState.GamePlaying || base.gameScene.PlayingState == PlayingState.WaitForEnd)
        {
            base.cameraTransform.rotation = Quaternion.Lerp(base.cameraTransform.rotation, Quaternion.Euler(0f - base.angelV, base.angelH, 0f), Time.deltaTime * this.rSmooth);
        }
        if (base.gameScene.PlayingState == PlayingState.GamePlaying || base.gameScene.PlayingState == PlayingState.WaitForEnd)
        {
            Transform transform = base.player.GetTransform();
            Vector3 eulerAngles = base.cameraTransform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
            Vector3 eulerAngles2 = base.cameraTransform.rotation.eulerAngles;
            float x = eulerAngles2.x;
            x = ((!(x > 90f)) ? x : (x - 360f));
            float num2 = Mathf.Abs(base.cameraDistanceFromPlayer.z) * Mathf.Sin(Mathf.Abs(x * 0.0174532924f));
            float f = Mathf.Abs(base.cameraDistanceFromPlayer.z) * Mathf.Cos(Mathf.Abs(x * 0.0174532924f));
            if (x < 0f)
            {
                num2 *= -1f;
            }
            Vector3 position = new Vector3(base.cameraDistanceFromPlayer.x, base.cameraDistanceFromPlayer.y + num2, 0f - Mathf.Abs(f));
            base.moveTo = base.player.GetTransform().TransformPoint(position);
            base.cameraTransform.position = base.moveTo;
        }
        if (base.cameraControl != null)
        {
            base.cameraControl(base.cameraComponent.gameObject);
        }
    }

    private void DoFollowCannon(float deltaTime, float rx, float ry)
    {
        if (base.gameScene.PlayingState != 0 && base.gameScene.PlayingState != PlayingState.WaitForEnd)
        {
            return;
        }
        base.cannon.yAxisValue += rx * 0.03f * base.cameraSwingSpeed;
        base.cannon.xAxisValue += ry * 0.03f * base.cameraSwingSpeed;
        base.cannon.yAxisValue = Mathf.Clamp(base.cannon.yAxisValue, base.cannon.minYAxisValue, base.cannon.maxYAxisValue);
        base.cannon.xAxisValue = Mathf.Clamp(base.cannon.xAxisValue, base.cannon.minXAxisValue, base.cannon.maxXAxisValue);
        base.cannon.yAxis.localRotation = Quaternion.Lerp(base.cannon.yAxis.localRotation, Quaternion.Euler(0f, 0f - base.cannon.yAxisValue, 0f), deltaTime * this.cSmooth);
        base.cannon.curXAxisValue = Mathf.Lerp(base.cannon.curXAxisValue, base.cannon.xAxisValue, Time.deltaTime * this.cSmooth);
        base.cannon.xAxis.localRotation = Quaternion.Euler(0f, 0f, base.cannon.curXAxisValue);
        base.cameraTransform.position = base.cannon.GetCameraAnchorPos_BeforeShake();
        base.cameraTransform.rotation = Quaternion.Lerp(base.cameraTransform.rotation, Quaternion.LookRotation(base.cannon.cameraAnchor.forward), deltaTime * this.rSmooth);
    }

    private void DoFollowAnimation(Vector3 pos, Quaternion rotation)
    {
        base.cameraTransform.position = pos;
        base.cameraTransform.rotation = rotation;
    }

    public void DoFollowSnipePoint(float deltaTime, float rx, float ry)
    {
        if ((Object)base.snipeFinalBulletAnchor != (Object)null)
        {
            base.cameraTransform.position = base.snipeFinalBulletAnchor.position;
            base.cameraTransform.rotation = base.snipeFinalBulletAnchor.rotation;
        }
        else
        {
            if (Time.timeScale != 0f)
            {
                float num = base.cameraComponent.fieldOfView / 60f;
                base.angelH += rx * 0.03f * base.cameraSwingSpeed * base.swingSpeedHorizontal * num;
                base.angelV += ry * 0.03f * base.cameraSwingSpeed * base.swingSpeedVertical * num;
                base.angelV = Mathf.Clamp(base.angelV, base.snipeMinAngelV, base.snipeMaxAngelV);
                base.angelH = Mathf.Clamp(base.angelH, base.snipeStartAngelH - base.snipeLimitAngelH, base.snipeStartAngelH + base.snipeLimitAngelH);
            }
            if (base.gameScene.PlayingState != 0 && base.gameScene.PlayingState != PlayingState.WaitForEnd)
            {
                return;
            }
            base.cameraTransform.rotation = Quaternion.Lerp(base.cameraTransform.rotation, Quaternion.Euler(0f - base.angelV, base.angelH, 0f), Time.deltaTime * this.rSmooth);
            Vector3 position = base.player.GetTransform().TransformPoint(base.snipeCameraAnchor) + -base.cameraTransform.forward * base.snipeCameraDistance;
            base.cameraTransform.position = position;
        }
    }

    public override void ChangeCameraControl(FollowMode _fmode)
    {
        base.fMode = _fmode;
    }

    public override void DoShootOffset()
    {
        base.angelH += Random.Range(base.gunShootStabilityHMin, base.gunShootStabilityHMax) * Mathf.Sign(Random.Range(-1f, 1f));
        base.angelV -= 0f - Random.Range(base.gunShootStabilityVMin, base.gunShootStabilityVMax);
    }

    public void HandleMessage(MessageType type)
    {
        if (type == MessageType.SensitivityChanged)
        {
            base.cameraSwingSpeed = (float)Singleton<GlobalData>.Instance.Sensitivity;
            UnityEngine.Debug.LogError("Camera Sensitivity Changed :" + base.cameraSwingSpeed);
        }
    }

    private void LateUpdate()
    {
        if (base.started)
        {
            base.deltaTime = Time.deltaTime;
            Vector2 cameraRotation = base.player.InputController.CameraRotation;
            float x = cameraRotation.x;
            Vector2 cameraRotation2 = base.player.InputController.CameraRotation;
            float y = cameraRotation2.y;
            this.CheckCameraCollider(base.deltaTime);
            if (base.fMode == FollowMode.PlayerMode)
            {
                this.DoFollowPlayer(base.deltaTime, x, y);
            }
            else if (base.fMode == FollowMode.CannonMode)
            {
                this.DoFollowCannon(base.deltaTime, x, y);
            }
            else if (base.fMode == FollowMode.Animation)
            {
                this.DoFollowAnimation(base.player.GetPlayerIK().CameraAnchorPos, base.player.GetPlayerIK().CameraAnchorRotate);
            }
            else if (base.fMode == FollowMode.SnipeMode)
            {
                this.DoFollowSnipePoint(base.deltaTime, x, y);
            }
            if ((Object)base.gameScene.GetEnemyManager() != (Object)null)
            {
                base.gameScene.GetEnemyManager().transform.position = base.player.GetTransform().position;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (base.fMode == FollowMode.SnipeMode)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(base.player.GetTransform().TransformPoint(base.snipeCameraAnchor), 0.05f);
            Gizmos.DrawLine(base.cameraTransform.position, base.cameraTransform.position + base.cameraTransform.forward * base.snipeCameraDistance);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(base.cameraTransform.position, 0.05f);
            Gizmos.color = Color.white;
        }
    }

    private void OnGUI()
    {
        if (Time.time != 0f && Time.timeScale != 0f && base.gameScene != null && base.gameScene.PlayingMode != GamePlayingMode.Cannon && base.player != null && base.player.GetState() != Player.CHARGER_STATE && base.gameScene.PlayingMode != GamePlayingMode.SnipeMode && (GameApp.GetInstance().GetGameScene().PlayingState == PlayingState.GamePlaying || GameApp.GetInstance().GetGameScene().PlayingState == PlayingState.WaitForEnd) && base.player.InputController.EnableShootingInput)
        {
            Weapon weapon = base.player.GetWeapon();
            if (weapon != null && !base.player.BulletEmpty())
            {
                AimState aimState = base.player.AimState;
                Texture texture = (aimState == AimState.Aimed) ? this.shootPoint_red : this.shootPoint;
                GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)texture.width * 0.5f * base.screenXScale, base.reticlePosition.y - (float)texture.height * 0.5f * base.screenYScale, (float)texture.width * base.screenXScale, (float)texture.height * base.screenYScale), texture);
                bool isMoving = base.player.IsMoving;
                bool isShooting = base.player.IsShooting;
                float expectReticleOffset = weapon.GetExpectReticleOffset(!isMoving, isShooting, Time.deltaTime, this.move_shrink);
                float num = (!weapon.ExpandReticle) ? ((!isShooting) ? this.move_shrink : this.shrink_shoot) : this.diffuse_shoot;
                this.reticleScale = Vector2.Lerp(this.reticleScale, Vector2.one * expectReticleOffset, Time.deltaTime * num);
                if (weapon.GetWeaponType() == WeaponType.ShotGun)
                {
                    Texture image = (aimState == AimState.Aimed) ? this.shotGunReticle_red[0] : this.shotGunReticle[0];
                    Texture image2 = (aimState == AimState.Aimed) ? this.shotGunReticle_red[1] : this.shotGunReticle[1];
                    Texture texture2 = (aimState == AimState.Aimed) ? this.shotGunReticle_red[2] : this.shotGunReticle[2];
                    Texture texture3 = (aimState == AimState.Aimed) ? this.shotGunReticle_red[3] : this.shotGunReticle[3];
                    GUI.DrawTexture(new Rect(base.reticlePosition.x - ((float)this.shotGunReticle[0].width * 0.5f + 15f * this.reticleScale.x) * base.screenXScale, base.reticlePosition.y - (float)this.shotGunReticle[0].height * 0.5f * base.screenYScale, (float)this.shotGunReticle[0].width * base.screenXScale, (float)this.shotGunReticle[0].height * base.screenYScale), image);
                    GUI.DrawTexture(new Rect(base.reticlePosition.x - ((float)this.shotGunReticle[1].width * 0.5f - 15f * this.reticleScale.x) * base.screenXScale, base.reticlePosition.y - (float)this.shotGunReticle[1].height * 0.5f * base.screenYScale, (float)this.shotGunReticle[1].width * base.screenXScale, (float)this.shotGunReticle[1].height * base.screenYScale), image2);
                }
                else
                {
                    Texture image3 = (aimState == AimState.Aimed) ? this.reticleX_red : this.reticleX;
                    Texture image4 = (aimState == AimState.Aimed) ? this.reticleY_red : this.reticleY;
                    GUI.DrawTexture(new Rect(base.reticlePosition.x - ((float)this.reticleX.width * 0.5f + 15f * this.reticleScale.x) * base.screenXScale, base.reticlePosition.y - (float)this.reticleX.height * 0.5f * base.screenYScale, (float)this.reticleX.width * base.screenXScale, (float)this.reticleX.height * base.screenYScale), image3);
                    GUI.DrawTexture(new Rect(base.reticlePosition.x - ((float)this.reticleX.width * 0.5f - 15f * this.reticleScale.x) * base.screenXScale, base.reticlePosition.y - (float)this.reticleX.height * 0.5f * base.screenYScale, (float)this.reticleX.width * base.screenXScale, (float)this.reticleX.height * base.screenYScale), image3);
                    GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)this.reticleY.width * 0.5f * base.screenXScale, base.reticlePosition.y - ((float)this.reticleY.height * 0.5f + 15f * this.reticleScale.y) * base.screenYScale, (float)this.reticleY.width * base.screenXScale, (float)this.reticleY.height * base.screenYScale), image4);
                    GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)this.reticleY.width * 0.5f * base.screenXScale, base.reticlePosition.y - ((float)this.reticleY.height * 0.5f - 15f * this.reticleScale.y) * base.screenYScale, (float)this.reticleY.width * base.screenXScale, (float)this.reticleY.height * base.screenYScale), image4);
                }
                if (aimState == AimState.OutRange)
                {
                    GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)this.aimOutRange.width * 0.5f, base.reticlePosition.y - (float)this.aimOutRange.height * 0.5f, (float)this.aimOutRange.width, (float)this.aimOutRange.height), this.aimOutRange);
                }
                if (!base.player.HasShootedEnemy)
                {
                    if (!(Mathf.Abs(this.shootReticleFinalPos - this.shootReticleTemp) < 1f))
                    {
                        this.shootReticleTemp = Mathf.Lerp(this.shootReticleTemp, this.shootReticleFinalPos, Time.deltaTime * this.diffuseSpeed);
                        float num2 = this.shootReticleTemp + this.shootReticleOffset;
                        float a = 1f - (this.shootReticleTemp - this.shootReticleOffset) / (this.shootReticleFinalPos - this.shootReticleOffset);
                        Color color = GUI.color;
                        GUI.color = new Color(color.r, color.g, color.b, a);
                        GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)this.shootReticle[0].width * 0.5f - num2, base.reticlePosition.y - (float)this.shootReticle[0].height * 0.5f - num2, (float)this.shootReticle[0].width, (float)this.shootReticle[0].height), this.shootReticle[0]);
                        GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)this.shootReticle[0].width * 0.5f + num2, base.reticlePosition.y - (float)this.shootReticle[0].height * 0.5f - num2, (float)this.shootReticle[0].width, (float)this.shootReticle[0].height), this.shootReticle[1]);
                        GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)this.shootReticle[0].width * 0.5f + num2, base.reticlePosition.y - (float)this.shootReticle[0].height * 0.5f + num2, (float)this.shootReticle[0].width, (float)this.shootReticle[0].height), this.shootReticle[2]);
                        GUI.DrawTexture(new Rect(base.reticlePosition.x - (float)this.shootReticle[0].width * 0.5f - num2, base.reticlePosition.y - (float)this.shootReticle[0].height * 0.5f + num2, (float)this.shootReticle[0].width, (float)this.shootReticle[0].height), this.shootReticle[3]);
                        GUI.color = color;
                    }
                }
                else
                {
                    base.player.HasShootedEnemy = false;
                    this.shootReticleTemp = this.shootReticleOffset;
                }
            }
        }
    }

    private void TestBtn()
    {
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 40;
        gUIStyle.normal.textColor = Color.white;
        if (GUI.Button(new Rect(400f, 0f, 150f, 75f), "滤镜"))
        {
            this.SetBloomEnable();
        }
        if (GUI.Button(new Rect(0f, 0f, 150f, 75f), "Vertical --"))
        {
            base.swingSpeedVertical -= 0.05f;
        }
        GUI.Label(new Rect(160f, 0f, 100f, 50f), base.swingSpeedVertical.ToString(), gUIStyle);
        if (GUI.Button(new Rect(240f, 0f, 150f, 75f), "Vertical ++"))
        {
            base.swingSpeedVertical += 0.05f;
        }
        if (GUI.Button(new Rect(0f, 90f, 150f, 75f), "H --"))
        {
            base.swingSpeedHorizontal -= 0.05f;
        }
        GUI.Label(new Rect(160f, 90f, 100f, 50f), base.swingSpeedHorizontal.ToString(), gUIStyle);
        if (GUI.Button(new Rect(240f, 90f, 150f, 75f), "H ++"))
        {
            base.swingSpeedHorizontal += 0.05f;
        }
        if (GUI.Button(new Rect(0f, 180f, 150f, 75f), "X --"))
        {
            base.player.InputController.cameraLimitMin -= 0.1f;
        }
        GUI.Label(new Rect(160f, 180f, 100f, 50f), base.player.InputController.cameraLimitMin.ToString(), gUIStyle);
        if (GUI.Button(new Rect(240f, 180f, 150f, 75f), "X ++"))
        {
            base.player.InputController.cameraLimitMin += 0.1f;
        }
        if (GUI.Button(new Rect(0f, 270f, 150f, 75f), "Y --"))
        {
            base.player.InputController.cameraLimitMax -= 0.1f;
        }
        GUI.Label(new Rect(160f, 270f, 100f, 50f), base.player.InputController.cameraLimitMax.ToString(), gUIStyle);
        if (GUI.Button(new Rect(240f, 270f, 150f, 75f), "Y ++"))
        {
            base.player.InputController.cameraLimitMax += 0.1f;
        }
        if (GUI.Button(new Rect(0f, 360f, 150f, 75f), "controlMin --"))
        {
            base.player.InputController.cameraControlRatioMin -= 0.1f;
        }
        GUI.Label(new Rect(160f, 360f, 100f, 50f), base.player.InputController.cameraControlRatioMin.ToString(), gUIStyle);
        if (GUI.Button(new Rect(240f, 360f, 150f, 75f), "controlMin ++"))
        {
            base.player.InputController.cameraControlRatioMin += 0.1f;
        }
        if (GUI.Button(new Rect(0f, 450f, 150f, 75f), "limitRatio --"))
        {
            base.player.InputController.limitRatio -= 0.001f;
        }
        GUI.Label(new Rect(160f, 450f, 100f, 50f), base.player.InputController.limitRatio.ToString(), gUIStyle);
        if (GUI.Button(new Rect(240f, 450f, 150f, 75f), "limitRatio ++"))
        {
            base.player.InputController.limitRatio += 0.001f;
        }
        if (GUI.Button(new Rect(0f, 540f, 150f, 75f), "controlRatio --"))
        {
            base.player.InputController.controlRatio -= 0.001f;
        }
        GUI.Label(new Rect(160f, 540f, 100f, 50f), base.player.InputController.controlRatio.ToString(), gUIStyle);
        if (GUI.Button(new Rect(240f, 540f, 150f, 75f), "controlRatio ++"))
        {
            base.player.InputController.controlRatio += 0.001f;
        }
        if (GUI.Button(new Rect(0f, 630f, 150f, 75f), "limitDegree --"))
        {
            base.player.InputController.limitDegree -= 1f;
        }
        GUI.Label(new Rect(160f, 630f, 100f, 50f), base.player.InputController.limitDegree.ToString(), gUIStyle);
        if (GUI.Button(new Rect(320f, 630f, 150f, 75f), "limitDegree ++"))
        {
            base.player.InputController.limitDegree += 1f;
        }
        if (GUI.Button(new Rect(0f, 720f, 150f, 75f), "rSmooth --"))
        {
            this.rSmooth -= 5f;
        }
        GUI.Label(new Rect(160f, 720f, 100f, 50f), this.rSmooth.ToString(), gUIStyle);
        if (GUI.Button(new Rect(320f, 720f, 150f, 75f), "rSmooth ++"))
        {
            this.rSmooth += 5f;
        }
    }

    private void TestGunShoot()
    {
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 20;
        gUIStyle.normal.textColor = Color.white;
        if (GUI.Button(new Rect(0f, base.screenYScale * 670f, 100f * base.screenXScale, 50f * base.screenYScale), "--"))
        {
            this.needShow = !this.needShow;
        }
        if (this.needShow)
        {
            this.DrawGUI(100f, ref ShakeCamera.Instance.shootBackRange, gUIStyle, 0.01f, "后移幅度");
            this.DrawGUI(150f, ref ShakeCamera.Instance.shootRiseAngel, gUIStyle, 0.1f, "仰角幅度");
            this.DrawGUI(200f, ref ShakeCamera.Instance.lerpSpeed2, gUIStyle, 0.5f, "位移回归速度");
            this.DrawGUI(250f, ref ShakeCamera.Instance.lerpSpeed3, gUIStyle, 0.5f, "仰角回归速度");
            this.DrawGUI(300f, ref base.gunShootStabilityHMin, gUIStyle, 0.1f, "水平稳定Min");
            this.DrawGUI(350f, ref base.gunShootStabilityHMax, gUIStyle, 0.1f, "水平稳定Max");
            this.DrawGUI(400f, ref base.gunShootStabilityVMin, gUIStyle, 0.1f, "垂直稳定Min");
            this.DrawGUI(450f, ref base.gunShootStabilityVMax, gUIStyle, 0.1f, "垂直稳定Max");
        }
    }

    private void DrawGUI(float height, ref float _value, GUIStyle style, float delta, string desc)
    {
        GUI.Label(new Rect(0f * base.screenXScale, (height + 15f) * base.screenYScale, 100f * base.screenXScale, 20f * base.screenYScale), desc, style);
        if (GUI.Button(new Rect(130f * base.screenXScale, height * base.screenYScale, 100f * base.screenXScale, 50f * base.screenYScale), "--"))
        {
            _value -= delta;
        }
        GUI.Label(new Rect(230f * base.screenXScale, (height + 15f) * base.screenYScale, 100f * base.screenXScale, 20f * base.screenYScale), _value.ToString(), style);
        if (GUI.Button(new Rect(330f * base.screenXScale, height * base.screenYScale, 100f * base.screenXScale, 50f * base.screenYScale), "++"))
        {
            _value += delta;
        }
    }
}


