using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 装备道具类
/// </summary>
public class EquipItemInfo : PlayerItemInfo
{
    public static int GetUpLevelNum(int level)
    {
        if (level == 1)
        {
            return 5;
        }
        else if (level == 2)
        {
            return 10;
        }
        else if (level == 3)
        {
            return 15;
        }
        else if (level == 4)
        {
            return 20;
        }
        else
        {
            return -1;
        }
    }
}
