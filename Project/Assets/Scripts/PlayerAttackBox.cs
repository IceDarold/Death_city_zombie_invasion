using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class PlayerAttackBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log(other.gameObject.name);
        string[] array = other.gameObject.name.Split(new char[]
        {
            '|'
        });
        Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(array[0]);
        if (!this.hitEnemy.Contains(enemyByID))
        {
            this.hitEnemy.Add(enemyByID);
        }
    }

    public void Show()
    {
        base.gameObject.SetActive(true);
        base.StartCoroutine(this.Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(0.1f);
        base.gameObject.SetActive(false);
        Transform playerTrans = GameApp.GetInstance().GetGameScene().GetPlayer().GetTransform();
        for (int i = 0; i < this.hitEnemy.Count; i++)
        {
            DamageProperty damageProperty = new DamageProperty(this.hitEnemy[i].MaxHp, this.force, this.hitEnemy[i].GetTransform().position - playerTrans.position);
            if (this.hitEnemy[i].GetEnemyProbability() == EnemyProbability.NORMAL)
            {
                damageProperty.damage = this.hitEnemy[i].MaxHp;
            }
            else if (this.hitEnemy[i].GetEnemyProbability() == EnemyProbability.ELITE)
            {
                damageProperty.damage = this.hitEnemy[i].MaxHp * 0.3f;
            }
            else if (this.hitEnemy[i].GetEnemyProbability() == EnemyProbability.BOSS)
            {
                damageProperty.damage = this.hitEnemy[i].MaxHp * 0.05f;
            }
            this.hitEnemy[i].OnHit(damageProperty, WeaponType.PLAYER_QTE, Bone.MiddleSpine);
        }
        this.hitEnemy.Clear();
        yield break;
    }

    [CNName("击飞力度")]
    public float force = 500f;

    protected List<Enemy> hitEnemy = new List<Enemy>();
}
