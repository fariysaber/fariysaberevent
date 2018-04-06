using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerItem
{
    //消耗道具
    public XiaohaoItemInfo xiaohaoItemInfo;
    //装备道具
    public EquipItemInfo equipItemInfo;
    public GamePlayerItem()
    {
        xiaohaoItemInfo = new XiaohaoItemInfo();
        equipItemInfo = new EquipItemInfo();
    }
    public void AddXiaoHaoItem(int id,int num)
    {
        xiaohaoItemInfo.Add(id, num);
    }
}
public class PlayerItemInfo
{
    
}
public class PlayerItemData
{
    public string unikey;
    public string name;
    public int count;
}