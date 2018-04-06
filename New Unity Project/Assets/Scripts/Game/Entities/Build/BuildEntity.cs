using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildEntity : Entity
{
    protected override string GetPathByEnitity(string entityIdStr, int pifu = 0)
    {
        HeroVo herovo = HeroConfig.GetData(entityIdStr);
        string path = CharactorConfig.GetData(herovo.charactor[pifu] + "").batmodel;
        return path;
    }
}
