using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipConfig : BaseConfig
{
    public static EquipVo GetData(string key)
    {
        Init<EquipVo>();
        return configDic[typeof(EquipVo).Name][key] as EquipVo;
    }
}
