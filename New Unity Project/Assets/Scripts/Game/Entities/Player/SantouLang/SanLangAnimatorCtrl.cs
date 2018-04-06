using System;
using System.Collections.Generic;
using UnityEngine;

public class SanLangAnimatorCtrl : AnimationCtrl
{
    protected override void AddAnimationId()
    {
        base.AddAnimationId();

        animationsId.Add(0, "idleLookAround");
        animationsId.Add(1, "run");
        animationsId.Add(2, "idleBreathe");
        animationsId.Add(3, "idleAggresive");
        animationsId.Add(4, "walk");
        animationsId.Add(5, "biteNormal");
        animationsId.Add(6, "jumpBiteNormal");
        animationsId.Add(7, "blowFireAggressive");
        animationsId.Add(8, "getHitNormal");
        animationsId.Add(9, "deathNormal");
    }
    protected override bool isCanReplaceNewState(int newState)
    {
        if (newState == 8)
        {
            if ((Isidle() || _stateAnim == 8) && isRuning == false)
            {
                return true;
            }
            return false;
        }
        return true;
    }
    protected override bool Isidle()
    {
        if (_stateAnim == 0 || _stateAnim == 2 || _stateAnim == 3)
        {
            return true;
        }
        return false;
    }
    protected override bool IsRun()
    {
        if (_stateAnim == 1 || _stateAnim == 4)
        {
            return true;
        }
        return false;
    }
    protected override bool IsHited()
    {
        return _stateAnim == 8;
    }
    //根据待机和移动复写
    protected override int ReplaceState(int newState)
    {
        if (newState == 0)
        {
            newState = GetIdleState();
        }
        if (isRuning && Isidle())
        {
            newState = GetRunState();
        }
        return newState;
    }
    private int GetIdleState()
    {
        BattlePlayerEntity batentity = basemode.playerEntity as BattlePlayerEntity;
        if (batentity.isHited)
        {
            return 0;
        }
        if (batentity.attributeMgr.IsDiyuPercentHp(0.5f))
        {
            return 2;
        }
        return 0;
    }
    private int GetRunState()
    {
        BattlePlayerEntity batentity = basemode.playerEntity as BattlePlayerEntity;
        if (batentity.isHited)
        {
            return 1;
        }
        return 4;
    }
}
