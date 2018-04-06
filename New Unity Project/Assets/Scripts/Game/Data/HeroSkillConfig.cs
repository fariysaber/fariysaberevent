using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSkillConfig : BaseConfig
{
    public static HeroSkillVo GetData(string key)
    {
        Init<HeroSkillVo>();
        return configDic[typeof(HeroSkillVo).Name][key] as HeroSkillVo;
    }
}
