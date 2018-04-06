using System;
using System.Collections.Generic;
using UnityEngine;

public class BreakAndParalleMgr
{
    public Dictionary<int, List<int>> breakList;
    public Dictionary<int, List<int>> paralleList;
    public BreakAndParalleMgr()
    {
        breakList = new Dictionary<int, List<int>>();
        paralleList = new Dictionary<int,List<int>>();
    }
    public void AddBreakeList(int type, List<int> breake)
    {
        breakList[type] = breake;
    }
    public void AddParalle(int type, List<int> paralle)
    {
        paralleList[type] = paralle;
    }
    public bool CheckCanBreake(int type,int breakType)
    {
        if (!breakList.ContainsKey(type))
        {
            return false;
        }
        for (int i = 0; i < breakList[type].Count; i++)
        {
            if (breakList[type][i] == breakType)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckCanParalle(int type, int breakType)
    {
        if (!paralleList.ContainsKey(type))
        {
            return false;
        }
        for (int i = 0; i < paralleList[type].Count; i++)
        {
            if (paralleList[type][i] == breakType)
            {
                return true;
            }
        }
        return false;
    }
}
