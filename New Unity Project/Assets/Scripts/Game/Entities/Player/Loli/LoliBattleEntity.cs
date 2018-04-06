using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoliBattleEntity : BattlePlayerEntity
{
    protected override void StartLoadModel(EntityInitData data)
    {
        model = new LoliModel();
        model.Entity = this;
        ModelInitData modelinitdata = GetModelInitData(data);
        model.InitBaseData(modelinitdata, LoadModelCallback);
    }
    public void LoadModelCallback()
    {
        attributeMgr.StartLoadEquip();
    }
}