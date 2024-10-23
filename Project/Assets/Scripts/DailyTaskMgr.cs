//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyTaskMgr : Singleton<DailyTaskMgr>
{
    public int[] DailyEnegery = new int[12] {
        10,
        10,
        10,
        10,
        10,
        20,
        10,
        15,
        15,
        10,
        10,
        20
    };

    public int[] DailyMaxNum = new int[12] {
        3,
        3,
        3,
        3,
        6,
        1,
        1000,
        10,
        2,
        40,
        10,
        1
    };

    public int[] DailyBoxNeedEnegery = new int[5] {
        20,
        40,
        60,
        80,
        100
    };

    protected DateTime recorderDate;

    protected int[] boxIsGot = new int[5];

    protected Dictionary<string, int> dailyTaskTimes = new Dictionary<string, int>();

    protected Dictionary<string, int> dailyTaskIsGot = new Dictionary<string, int>();

    protected bool isLoad;

    public Action<bool> RefreshDayMissionBtn;

    public void LoadDailyTaskRecorder()
    {
        if (!this.isLoad)
        {
            this.isLoad = true;
            string @string = PlayerPrefs.GetString("dailyDateRecorderKey", "1970-1-1");
            this.recorderDate = Convert.ToDateTime(@string);
            for (int i = 0; i < this.boxIsGot.Length; i++)
            {
                this.boxIsGot[i] = PlayerPrefs.GetInt("dailyBoxRecorderKey" + i.ToString(), 0);
            }
            IEnumerator enumerator = Enum.GetValues(typeof(DailyTask)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DailyTask dailyTask = (DailyTask)enumerator.Current;
                    this.dailyTaskTimes.Add(dailyTask.ToString(), PlayerPrefs.GetInt(dailyTask.ToString() + "Times", 0));
                    this.dailyTaskIsGot.Add(dailyTask.ToString(), PlayerPrefs.GetInt(dailyTask.ToString() + "IsGot", 0));
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumerator as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
            }
            DateTime now = DateTime.Now;
            if (this.recorderDate.Year == now.Year && this.recorderDate.Month == now.Month && this.recorderDate.Day == now.Day)
            {
                return;
            }
            this.ClearDailyTaskRecorder(now);
        }
    }

    public int GetDailyEnegeryNum()
    {
        int num = 0;
        IEnumerator enumerator = Enum.GetValues(typeof(DailyTask)).GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                DailyTask dailyTask = (DailyTask)enumerator.Current;
                if (this.dailyTaskTimes[dailyTask.ToString()] >= this.DailyMaxNum[(int)dailyTask])
                {
                    num += this.DailyEnegery[(int)dailyTask];
                }
            }
            return num;
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

    public int GetDailyTaskNum(DailyTask task)
    {
        return this.dailyTaskTimes[task.ToString()];
    }

    public void AddDailyTaskNum(DailyTask task, int num = 1, bool _save = true)
    {
        Dictionary<string, int> dictionary;
        string key = ""; ;
        dictionary = this.dailyTaskTimes; (dictionary)[key = task.ToString()] = dictionary[key] + num;
        if (_save)
        {
            this.SaveDailyTaskRecorder();
        }
    }

    public bool GetDilyTaskGot(DailyTask task)
    {
        return this.dailyTaskIsGot[task.ToString()] == 1;
    }

    public void SetDailyTaskGot(DailyTask task)
    {
        this.dailyTaskIsGot[task.ToString()] = 1;
    }

    public bool GetDailyBoxGot(int index)
    {
        return this.boxIsGot[index] == 1;
    }

    public void SetDailyBoxGot(int index)
    {
        this.boxIsGot[index] = 1;
        if (this.RefreshDayMissionBtn != null)
        {
            this.RefreshDayMissionBtn(this.GetHasReadyBox());
        }
    }

    public bool GetHasReadyBox()
    {
        for (int i = 0; i < this.boxIsGot.Length; i++)
        {
            if (!this.GetDailyBoxGot(i) && this.GetBoxIsReady(i))
            {
                return true;
            }
        }
        return false;
    }

    public bool GetBoxIsReady(int index)
    {
        return this.GetDailyEnegeryNum() >= this.DailyBoxNeedEnegery[index];
    }

    private void ClearDailyTaskRecorder(DateTime date)
    {
        this.recorderDate = new DateTime(date.Year, date.Month, date.Day);
        for (int i = 0; i < this.boxIsGot.Length; i++)
        {
            this.boxIsGot[i] = 0;
        }
        IEnumerator enumerator = Enum.GetValues(typeof(DailyTask)).GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                DailyTask dailyTask = (DailyTask)enumerator.Current;
                this.dailyTaskTimes[dailyTask.ToString()] = 0;
                this.dailyTaskIsGot[dailyTask.ToString()] = 0;
            }
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

    public void SaveDailyTaskRecorder()
    {
        string value = this.recorderDate.Year.ToString() + "-" + this.recorderDate.Month.ToString() + "-" + this.recorderDate.Day.ToString();
        PlayerPrefs.SetString("dailyDateRecorderKey", value);
        for (int i = 0; i < this.boxIsGot.Length; i++)
        {
            PlayerPrefs.SetInt("dailyBoxRecorderKey" + i.ToString(), this.boxIsGot[i]);
        }
        IEnumerator enumerator = Enum.GetValues(typeof(DailyTask)).GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                DailyTask dailyTask = (DailyTask)enumerator.Current;
                PlayerPrefs.SetInt(dailyTask.ToString() + "Times", this.dailyTaskTimes[dailyTask.ToString()]);
                PlayerPrefs.SetInt(dailyTask.ToString() + "IsGot", this.dailyTaskIsGot[dailyTask.ToString()]);
            }
        }
        finally
        {
            IDisposable disposable;
            if ((disposable = (enumerator as IDisposable)) != null)
            {
                disposable.Dispose();
            }
        }
        PlayerPrefs.Save();
    }
}


