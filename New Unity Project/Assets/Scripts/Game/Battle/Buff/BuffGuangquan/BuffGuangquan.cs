using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffGuangquan : BuffBase
{
    protected override void AttachEffectObject()
    {
        buffOffeset = new Vector3(0, 0.1f, 0);
        base.AttachEffectObject();
        buffEffect.SetLoadCallback(BuffEffectLoadCallBack);
    }
    private void BuffEffectLoadCallBack()
    {
        int layer = buffMgr.entity.model.Layer;
        Color color;
        if (layer == Globals.BattlePlayerMylayer)
        {
            color = new Color(0f, 1f, 0f);
        }
        else if (layer == Globals.BattlePlayerMyTeamlayer || layer == Globals.BattlePlayerFriendlayer)
        {
            color = new Color(108f / 255f, 1f, 0f);
        }
        else if (layer == Globals.BattlePlayerDiRenlayer)
        {
            color = new Color(1, 120f / 255f, 0);
        }
        else if (layer == Globals.BattlePlayerZhonglilayer)
        {
            color = new Color(155f / 255f, 0f, 1f);
        }
        else
        {
            color = new Color(28f / 255f, 1f, 1f);
        }
        buffEffect.effectObject.GetComponent<Renderer>().material.SetColor("_TintColor", color);
    }
}