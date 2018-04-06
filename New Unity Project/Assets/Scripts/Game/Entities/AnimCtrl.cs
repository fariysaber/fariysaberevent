using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl
{
    protected BaseModel basemode;
    public virtual void InitAnim(BaseModel model)
    {
        basemode = model;
    }
    protected Animator GetAnimator(GameObject obj)
    {
        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
        {
            Animator anim = child.GetComponent<Animator>();
            if (anim != null)
            {
                return anim;
            }
        }
        Debugger.Log("该角色" + basemode.ModelName + "没有animator");
        return null;
    }
    protected Animation GetAnimation(GameObject obj)
    {
        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
        {
            Animation anim = child.GetComponent<Animation>();
            if (anim != null)
            {
                return anim;
            }
        }
        Debugger.Log("该角色" + basemode.ModelName + "没有Animation");
        return null;
    }
    public virtual void PlayAnimByStateName(string name,bool force = true)
    {
    }
    public virtual void PlayResetIdle()
    {
    }
    public virtual void Update(float dt)
    {

    }
}
