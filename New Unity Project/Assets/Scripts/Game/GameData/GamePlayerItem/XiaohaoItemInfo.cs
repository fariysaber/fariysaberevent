using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消耗道具类
/// </summary>
public enum XiaoHaoType
{
    huobi = 0,
    plant = 1,
    rock = 2,
    fish = 3,
    seeds = 4,
    all = 101,
}
public class XiaohaoItemInfo : PlayerItemInfo
{
    public Dictionary<XiaoHaoType,Dictionary<int, ShengjiItemData>> _dic;
    public XiaohaoItemInfo()
    {
        _dic = new Dictionary<XiaoHaoType, Dictionary<int, ShengjiItemData>>();
    }
    public int GetNumByKey(int key, XiaoHaoType xiaohaoType = XiaoHaoType.all)
    {
        if (xiaohaoType != XiaoHaoType.all)
        {
            if (_dic.ContainsKey(xiaohaoType) && _dic[xiaohaoType].ContainsKey(key))
            {
                return _dic[xiaohaoType][key].count;
            }
        }
        foreach (var item in _dic)
        {
            if(item.Value.ContainsKey(key))
            {
                return item.Value[key].count;
            }
        }
        return 0;
    }
    public void Add(int id, int num)
    {

    }
    public static int GetSkillUpLevelNum(int level)
    {
        if (level < Globals.skillUpMaxLevel)
        {
            return 5 * level;
        }
        return -1;
    }
    public static int GetEquipLevelNum(int level)
    {
        if (level < Globals.EquipUpMaxLevel)
        {
            LevelShuxinVo vo = LevelShuxinConfig.GetData(level + "");
            int lastjingyan = level <= 1 ? 0 : LevelShuxinConfig.GetData((level - 1) + "").jingyan;
            return vo.jingyan - lastjingyan; 
        }
        return -1;
    }
}
public class XiaohaoItemData : PlayerItemData
{
    
}
public class ShengjiItemData : XiaohaoItemData
{

}