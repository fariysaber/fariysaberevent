using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelShuxinConfig : BaseConfig
{
    public static LevelShuxinVo GetData(string key)
    {
        Init<LevelShuxinVo>();
        return configDic[typeof(LevelShuxinVo).Name][key] as LevelShuxinVo;
    }
}
