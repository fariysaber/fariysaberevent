using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1731 : Buff_Add
{
    protected override void InitInfo(BuffBaseInfo info)
    {
        base.InitInfo(info);
        PlayerModel playermodel = buffMgr.entity.model as PlayerModel;
        playermodel.moveLocker.AddLocker("Buff_1731");
        playermodel.AddRotationLocker("Buff_1731");
    }
    public override void DestroyAll()
    {
        base.DestroyAll();
        PlayerModel playermodel = buffMgr.entity.model as PlayerModel;
        playermodel.moveLocker.RemoveLocker("Buff_1731");
        playermodel.rotationLocker.RemoveLocker("Buff_1731");
    }
}