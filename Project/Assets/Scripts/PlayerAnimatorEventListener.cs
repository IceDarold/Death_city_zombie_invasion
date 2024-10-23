using System;
using UnityEngine;
using Zombie3D;

public class PlayerAnimatorEventListener : MonoBehaviour
{
    public void SetPlayer(Player _player)
    {
        this.player = _player;
    }

    private void OnPlayerFireKeyFrame()
    {
        if (this.player == null)
        {
            return;
        }
        this.player.Fire(0f);
    }

    private void OnPlayerChargeOver()
    {
        this.player.OnChargeOver();
        Singleton<GlobalMessage>.Instance.SendGlobalMessage(MessageType.PlayerChargeOver);
    }

    private void OnPlayerChargeOnce()
    {
        this.player.DoReloadOnce();
    }

    private void OnPlayerChangeGunKeyFrame()
    {
        this.player.DoChangeGun();
    }

    private void OnPlayerSkillSpecialKeyFrame()
    {
        this.player.OnSpecialSkillKeyFrame();
    }

    private void ChangeGunHoldPoint(int holdPoint)
    {
    }

    private void OnGunOffCallback()
    {
        this.player.OnGunOffOver();
    }

    private void OnGunOnCallback()
    {
        this.player.OnGunOnOver();
    }

    private void OnFullbodyActionOver()
    {
        this.player.OnFullbodyActionOver();
    }

    private void OnPickOutCharger()
    {
        if (this.player.GetState() == Player.CHARGER_STATE)
        {
            this.player.PickOutCharger();
        }
    }

    private void OnPickOnCharger()
    {
        if (this.player.GetState() == Player.CHARGER_STATE)
        {
            this.player.PickOnCharger();
        }
    }

    private void DropCharger()
    {
        if (this.player.GetState() == Player.CHARGER_STATE)
        {
            this.player.DropCharger();
        }
    }

    private void ShowCharger()
    {
        if (this.player.GetState() == Player.CHARGER_STATE)
        {
            this.player.ShowCharger();
        }
    }

    private void PlayGunAnimation(int id)
    {
        this.player.PlayGunAnimation(id);
    }

    private void ShowAttackBox()
    {
        this.player.ShowAttackBox();
    }

    protected Player player;
}
