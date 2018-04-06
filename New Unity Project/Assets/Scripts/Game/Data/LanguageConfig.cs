using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageConfig : BaseConfig
{
    public static LanguageVo GetData(string key)
    {
        Init<LanguageVo>();
        return configDic[typeof(LanguageVo).Name][key] as LanguageVo;
    }
}
