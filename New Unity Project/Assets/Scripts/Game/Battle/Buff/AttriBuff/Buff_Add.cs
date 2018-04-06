using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Add : BuffBase
{
    protected override void AttachEffectObject()
    {
        buffOffeset = new Vector3(0, 0.1f, 0);
        Transform parent = buffMgr.entity.model.GetEffectObjName(true);
        buffEffect = buffMgr.GetEffect(buffVo.effect, ResourceType.scene, buffOffeset, Vector3.zero, Vector3.one, Globals.normaleffectlayer,
            parent, -1f);
        Debug.Log(buffVo.effect);
    }
}