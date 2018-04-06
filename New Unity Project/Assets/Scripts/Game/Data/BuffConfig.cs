using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConfig : BaseConfig
{
    public static BuffVo GetData(string key)
    {
        Init<BuffVo>();
        return configDic[typeof(BuffVo).Name][key] as BuffVo;
    }
}
