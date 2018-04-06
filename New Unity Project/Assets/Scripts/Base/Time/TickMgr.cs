﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TickMgr
{
    private List<ITick> _activeTicks;
    private List<ITick> _needDeleteTicks;
    private List<ITick> _needActiveTicks;
    private ITick tickItem;

    public TickMgr()
    {
        _activeTicks = new List<ITick>();
        _needActiveTicks = new List<ITick>();
        _needDeleteTicks = new List<ITick>();
        _needActiveTicks = new List<ITick>();
    }

    public void refreshUpdate(float dt)
    {
        while (_needDeleteTicks.Count > 0)
        {
            tickItem = _needDeleteTicks[0];
            _needDeleteTicks.RemoveAt(0);
            _activeTicks.Remove(tickItem);
        }
        while (_needActiveTicks.Count > 0)
        {
            tickItem = _needActiveTicks[0];
            _needActiveTicks.RemoveAt(0);
            _activeTicks.Add(tickItem);
        }
        
    }
    public void OnUpdate(float dt)
    {
        refreshUpdate(dt);
        int count = _activeTicks.Count;
        for (int i = 0; i < count; i++)
        {
            _activeTicks[i].OnUpdate(dt);
        }
    }

    public void FixedUpdate(float dt)
    {
        refreshUpdate(dt);
        int count = _activeTicks.Count;
        for (int i = 0; i < count; i++)
        {
            _activeTicks[i].FixedUpdate(dt);
        }
    }

    public void LateUpdate(float dt)
    {
        refreshUpdate(dt);
        int count = _activeTicks.Count;
        for (int i = 0; i < count; i++)
        {
            _activeTicks[i].LateUpdate(dt);
        }
    }

    public void AddTick(ITick tick, bool isonce = false)
    {
        if (_needDeleteTicks.Contains(tick))
        {
            _needDeleteTicks.Remove(tick);
        }
        if (!IsActive(tick))
        {
            if (isonce)
            {
                if (_needActiveTicks.Contains(tick))
                {
                    _needActiveTicks.Remove(tick);
                }
                _activeTicks.Add(tick);
            }
            else
            {
                if (!_needActiveTicks.Contains(tick))
                {
                    _needActiveTicks.Add(tick);
                }
            }
        }
    }

    public void RemoveTick(ITick tick, bool isonce = false)
    {
        if (IsActive(tick))
        {
            if (isonce)
            {
                _activeTicks.Remove(tick);
            }
            else
            {
                _needDeleteTicks.Add(tick);
            }
        }
        else if (_needActiveTicks.Contains(tick))
        {
            _needActiveTicks.Remove(tick);
        }
    }

    public bool IsActive(ITick tick)
    {
        return _activeTicks.Contains(tick);
    }

    public List<ITick> Ticks
    {
        get
        {
            return _activeTicks;
        }
    }
}
