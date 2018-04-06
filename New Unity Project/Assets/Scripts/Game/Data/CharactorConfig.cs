using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorConfig : BaseConfig
{
    public static CharactorVo GetData(string key)
    {
        Init<CharactorVo>();
        return configDic[typeof(CharactorVo).Name][key] as CharactorVo;
    }
}
