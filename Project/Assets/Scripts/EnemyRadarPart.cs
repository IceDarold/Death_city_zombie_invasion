//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class EnemyRadarPart : MonoBehaviour
{
    [Header("检测频率")]
    public float RefreshRate;

    [Header("预警范围")]
    public float WarnRange;

    public List<EnemyRadar> Radars = new List<EnemyRadar>();

    private bool Enable;

    private float rate;

    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/EnemyRadar")) as GameObject;
            gameObject.transform.SetParent(base.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            EnemyRadar component = gameObject.GetComponent<EnemyRadar>();
            this.Radars.Add(component);
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        this.Activate();
    }

    public void Activate()
    {
        this.Enable = true;
    }

    public void Close()
    {
        this.Enable = false;
        for (int i = 0; i < this.Radars.Count; i++)
        {
            this.Radars[i].gameObject.SetActive(false);
        }
    }

    private List<Enemy> GetEnemyInRange()
    {
        List<Enemy> list = new List<Enemy>();
        Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
        Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
        IDictionaryEnumerator enumerator = enemies.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Enemy enemy = ((DictionaryEntry)enumerator.Current).Value as Enemy;
                if (enemy.GetState() != Enemy.DEAD_STATE)
                {
                    float num = Vector3.Distance(player.GetTransform().position, enemy.GetTransform().position);
                    Vector3 to = enemy.GetTransform().position - player.GetTransform().position;
                    float num2 = Vector3.Angle(player.GetTransform().forward, to);
                    if (num < this.WarnRange && num2 > 80f)
                    {
                        list.Add(enemy);
                    }
                }
            }
            return list;
        }
        finally
        {
            IDisposable disposable;
            if ((disposable = (enumerator as IDisposable)) != null)
            {
                disposable.Dispose();
            }
        }
    }

    private void ShowEnemyIcon()
    {
        List<Enemy> enemyInRange = this.GetEnemyInRange();
        while (this.Radars.Count < enemyInRange.Count)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/EnemyRadar")) as GameObject;
            gameObject.transform.SetParent(base.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            EnemyRadar component = gameObject.GetComponent<EnemyRadar>();
            this.Radars.Add(component);
        }
        for (int i = 0; i < this.Radars.Count; i++)
        {
            if (i < enemyInRange.Count)
            {
                this.Radars[i].Target = enemyInRange[i];
                this.Radars[i].WarnRange = this.WarnRange;
                this.Radars[i].gameObject.SetActive(true);
            }
            else
            {
                this.Radars[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (this.Enable)
        {
            if (this.rate < this.RefreshRate)
            {
                this.rate += Time.deltaTime;
            }
            else
            {
                this.rate = 0f;
                this.ShowEnemyIcon();
            }
        }
    }
}


