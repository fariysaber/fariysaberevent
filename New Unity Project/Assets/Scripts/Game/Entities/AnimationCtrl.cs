using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCtrl : AnimCtrl
{
    public Animation animation;
    protected Dictionary<int, string> animationsId;
    protected int _stateAnim;
    protected string _stateAnimName;
    protected bool isRuning;
    protected float continueTime = -1f;
    public override void InitAnim(BaseModel mode)
    {
        base.InitAnim(mode);
        animation = GetAnimation(basemode.gameObject);
        AddAnimationId();
        PlayResetIdle();
    }

    protected virtual void AddAnimationId()
    {
        if (animationsId == null)
        {
            animationsId = new Dictionary<int, string>();
        }
        else
        {
            animationsId.Clear();
        }
    }
    public bool CanSetTrigger(int newState)
    {
        if (animationsId.ContainsKey(newState) && isCanReplaceNewState(newState))
        {
            return true;
        }
        return false;
    }
    protected virtual bool isCanReplaceNewState(int newState)
    {
        return true;
    }
    ///<summary>
    ///立即播放指定状态的指定帧，并设置速度
    ///</summary>
    protected virtual void PlayAnimByName(string name, float rate = 0f, float speedPercent = -1f)
    {
        if (animation == null)
        {
            return;
        }
        animation.Stop();
        animation.Play(name, AnimationPlayMode.Mix);
    }

    ///<summary>
    ///设置播放状态
    ///</summary>
    protected virtual void SetAnimTrigger(int newState, bool forceSameStart = false)
    {
        if (!CanSetTrigger(newState))
        {
            return;
        }
        if (animation == null)
        {
            return;
        }
        newState = ReplaceState(newState);
        _stateAnimName = animationsId[newState];
        if (forceSameStart)
        {
            if (_stateAnim == newState)
            {
                animation.Stop();
            }
        }
        animation.Play(_stateAnimName);
        _stateAnimName = animationsId[newState];
        _stateAnim = newState;
    }

    ///<summary>
    ///外部接口根据状态
    ///</summary>
    public virtual void PlayAnimByState(int newState, float rate = 0f, float speedPercent = -1f, float getContinueTime = -1)
    {
        if (!CanSetTrigger(newState))
        {
            return;
        }
        if (animation == null)
        {
            return;
        }
        newState = ReplaceState(newState);
        string name = animationsId[newState];
        PlayAnimByName(name, rate, speedPercent);
        _stateAnimName = animationsId[newState];
        _stateAnim = newState;
        continueTime = getContinueTime;
    }

    //根据待机和移动复写
    protected virtual int ReplaceState(int newState)
    {
        if (isRuning && newState == 0)
        {
            newState = 1;
        }
        return newState;
    }

    /// <summary>
    /// 外部接口根据名字
    /// </summary>
    /// <param name="name"></param>
    /// <param name="force"></param>
    public override void PlayAnimByStateName(string name, bool force = true)
    {
        int state = GetStateByName(name);
        if (state == -1)
        {
            Debugger.Log("没找到动作名" + name);
        }
        if (force)
        {
            PlayAnimByState(state);
        }
        else
        {
            SetAnimTrigger(state, true);
        }
    }

    public override void PlayResetIdle()
    {
        SetAnimTrigger(0, true);
    }

    protected int GetStateByName(string name)
    {
        foreach (var item in animationsId)
        {
            if (item.Value == name)
            {
                return item.Key;
            }
        }
        return -1;
    }
    //根据待机和移动复写
    public virtual void SetRuning(bool value)
    {
        if (isRuning == value)
        {
            if (value == true && !IsRun())
            {
            }
            else
            {
                return;
            }
        }
        int newState = 0;
        if (value)
        {
            newState = 1;
        }
        if (IsHited() || IsIdleOrRun())
        {
            SetAnimTrigger(newState, true);
        }
        isRuning = value;
    }

    protected virtual bool IsHited()
    {
        return false;
    }

    protected virtual bool Isidle()
    {
        if (_stateAnim == 0)
        {
            return true;
        }
        return false;
    }
    protected virtual bool IsRun()
    {
        if (_stateAnim == 1)
        {
            return true;
        }
        return false;
    }
    protected virtual bool IsIdleOrRun()
    {
        if (Isidle() || IsRun())
        {
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        if (continueTime > 0)
        {
            continueTime -= dt;
            if (continueTime <= 0)
            {
                SetAnimTrigger(0, true);
            }
        }
    }
}