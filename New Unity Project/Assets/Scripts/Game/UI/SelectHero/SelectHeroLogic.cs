using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectHeroLogic : Singleton<SelectHeroLogic>
{
    public List<HeroVo> GetInitHero()
    {
        List<HeroVo> heroVoList = new List<HeroVo>();
        for (int i = 1; i < 10; i++)
        {
            HeroVo vo = HeroConfig.GetDataIgoreNull(i + "");
            if (vo != null)
            {
                heroVoList.Add(vo);
            }
        }
        return heroVoList;
    }
}
