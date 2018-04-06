using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanLangBattleEntity : BattlePlayerEntity
{
    protected override void StartLoadModel(EntityInitData data)
    {
        model = new SanLangModel();
        model.Entity = this;
        ModelInitData modelinitdata = GetModelInitData(data);
        model.InitBaseData(modelinitdata);
    }
}