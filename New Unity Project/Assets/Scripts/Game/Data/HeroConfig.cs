using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroConfig : BaseConfig
{
    public static HeroVo GetData(string key)
    {
        Init<HeroVo>();
        return configDic[typeof(HeroVo).Name][key] as HeroVo;
    }
    public static HeroVo GetDataIgoreNull(string key)
    {
        Init<HeroVo>();
        if (!configDic[typeof(HeroVo).Name].ContainsKey(key))
        {
            return null;
        }
        return configDic[typeof(HeroVo).Name][key] as HeroVo;
    }
}
