using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletConfig : BaseConfig
{
    public static BulletVo GetData(string key)
    {
        Init<BulletVo>();
        return configDic[typeof(BulletVo).Name][key] as BulletVo;
    }
}
