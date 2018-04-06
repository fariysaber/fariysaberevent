using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntity : BuildEntity
{
    protected override void StartLoadModel(EntityInitData data)
    {
        model = new TreeModel();
        model.Entity = this;
        ModelInitData modelinitdata = GetModelInitData(data);
        model.InitBaseData(modelinitdata);
    }
}
