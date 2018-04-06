using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfig : BaseConfig
{
    public static SKillVo GetData(string key)
    {
        Init<SKillVo>();
        return configDic[typeof(SKillVo).Name][key] as SKillVo;
    }
}
