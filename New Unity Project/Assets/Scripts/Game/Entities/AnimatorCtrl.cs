using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCtrl : AnimCtrl
{
    public Animator animator;
    protected Dictionary<int, string> animatorsId;
    protected Dictionary<int, int> animatorLayer;
    protected string triggerName = "trigger";
    protected float continueTime;
    public override void InitAnim(BaseModel mode)
    {
        base.InitAnim(mode);
        animator = GetAnimator(basemode.gameObject);
        _animSpeed = animator.speed;
        _stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        SetAnimPercent(_animPercent);
        AddAnimatorId();
        InitInfo();
        PlayResetIdle();
    }

    protected virtual void InitInfo()
    {
        triggerName = "trigger";
    }

    protected virtual void AddAnimatorId()
    {
        if (animatorsId == null)
        {
            animatorsId = new Dictionary<int, string>();
        }
        else
        {
            animatorsId.Clear();
        }
        if (animatorLayer == null)
        {
            animatorLayer = new Dictionary<int, int>();
        }
        else
        {
            animatorLayer.Clear();
        }
    }

    public bool CanSetTrigger(int newState)
    {
        if (animatorsId.ContainsKey(newState) && isCanReplaceNewState(newState))
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
    ///正在播放的对应的AnimatorStateInfo
    ///</summary>
    private AnimatorStateInfo _stateInfo;

    public int _state = 0;


    ///<summary>
    ///立即播放指定状态的指定帧，并设置速度
    ///</summary>
    protected virtual void PlayAnimByName(string name,float rate = 0f, float speedPercent = -1f)
    {
        if (animator == null)
        {
            return;
        }
        animator.Play(name, 0, rate);
        if (speedPercent >= 0)
        {
            SetAnimPercent(speedPercent);
        }
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
        if (animator == null)
        {
            return;
        }
        if (forceSameStart)
        {
            if (_state == newState)
            {
                animator.Play(_stateInfo.fullPathHash, 0, 0);
            }
        }
        animator.SetInteger(triggerName, newState);
        _state = newState;
    }

    ///<summary>
    ///外部接口根据状态
    ///</summary>
    public virtual void PlayAnimByState(int newState,float rate = 0f, float speedPercent = -1f,float getContinueTime = -1)
    {
        if (!CanSetTrigger(newState))
        {
            return;
        }
        if (animator == null)
        {
            return;
        }
        string name = animatorsId[newState];
        PlayAnimByName(name, rate, speedPercent);
        animator.SetInteger(triggerName, newState);
        _state = newState;
        continueTime = getContinueTime;
    }
    /// <summary>
    /// 外部接口根据名字
    /// </summary>
    /// <param name="name"></param>
    /// <param name="force"></param>
    public override void PlayAnimByStateName(string name,bool force = true)
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

    public int GetStateByName(string name)
    {
        foreach (var item in animatorsId)
        {
            if (item.Value == name)
            {
                return item.Key;
            }
        }
        return -1;
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        _stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        UpdateAnimator(dt);
    }
    protected virtual void UpdateAnimator(float dt)
    {

    }

    #region 设置动作速度相关
    public void SetStopAnim(bool isStopAnim = true)
    {
        stopAnim = isStopAnim ? 0 : 1;
        SetAnimPercent(_animPercent);
    }
    private float _animPercent = 1f;
    private float _animSpeed = 1f;
    private int stopAnim = 1;//设置是否停止动作,0为停止，1为开始
    /// <summary>
    /// 设置动画速度百分比
    /// </summary>
    /// <param name="speed"></param>
    public void SetAnimPercent(float percent)
    {
        _animPercent = percent;
        if (animator)
        {
            animator.speed = _animSpeed * _animPercent * stopAnim;
        }
    }
    #endregion
    public virtual void Destroy()
    {
        if (animator)
        {
            animator.speed = _animSpeed;
        }
        animator = null;
    }
}