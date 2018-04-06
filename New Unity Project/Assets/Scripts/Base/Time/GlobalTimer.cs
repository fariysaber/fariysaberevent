using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class TimerTick
{
    public int count;
    public int time;
    public int startTime;
    public System.Action callback;
}
//时间器单独管理（与资源和场景ui分开，防止冲突）
public class GlobalTimer:SingletonMonoBehaviour<GlobalTimer>
{
    private long m_millSecond;//当前毫秒数
    private UniqueIDHelper uiqueIds;
    private Dictionary<int, TimerTick> timerTicks;
    private bool PauseTime = false;
    private TickMgr ticMgr;

    protected override void Awake()
    {
        base.Awake();
        uiqueIds = new UniqueIDHelper();
        timerTicks = new Dictionary<int, TimerTick>();
        ticMgr = new TickMgr();
        GameObject.DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (PauseTime)
            return;
        ticMgr.OnUpdate(Time.deltaTime);
        List<int> keys = new List<int>(timerTicks.Keys);
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            if (!timerTicks.ContainsKey(keys[i]))
            {
                continue;
            }
            TimerTick tick = timerTicks[keys[i]];
            int delTime = (int)(Time.deltaTime * 1000);
            while (delTime > 0)
            {
                int lastTime = tick.time - delTime;
                if (lastTime > 0)
                {
                    tick.time -= delTime;
                    delTime = 0;
                    break;
                }
                else
                {
                    tick.count -= 1;
                    if (tick.count <= 0)
                    {
                        tick.callback();
                        RemoveTimerTick(keys[i]);
                        break;
                    }
                    else
                    {
                        delTime -= tick.time;
                        tick.time = tick.startTime;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (PauseTime)
            return;
        ticMgr.FixedUpdate(Time.deltaTime);
    }

    void LateUpdate()
    {
        if (PauseTime)
            return;
        ticMgr.LateUpdate(Time.deltaTime);
    }

    public void ClearTimerTick()
    {
        timerTicks.Clear();
    }

    public void RemoveTimerTick(int id)
    {
        if (timerTicks.ContainsKey(id))
        {
            timerTicks.Remove(id);
        }
    }

    public int SetTimer(int interval, System.Action callback, int count = 1)
    {
        int id = uiqueIds.GetUnique();
        TimerTick timertick = new TimerTick();
        timertick.time = interval;
        timertick.count = count;
        timertick.callback = callback;
        timertick.startTime = interval;
        if (interval <= 0)
        {
            callback();
            uiqueIds.RemoveUniqueID(id);
        }
        else
        {
            timerTicks[id] = timertick;
        }
        return id;
    }

    public long MiliSecondNow()
    {
        DateTime dt1970 = new DateTime(1970, 1, 1);
        TimeSpan ts = DateTime.Now - dt1970;
        m_millSecond = (long)ts.TotalMilliseconds;
        return m_millSecond;
    }

    public long secondNow()
    {
        return (long)Math.Ceiling((decimal)(MiliSecondNow()/1000));
    }

    void OnGUI()
    {
       
    }

    public float timeScale
    {
        get { return Time.timeScale; }
        set { Time.timeScale = value; }
    }
}
