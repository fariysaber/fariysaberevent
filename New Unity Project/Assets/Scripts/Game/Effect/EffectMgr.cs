using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr
{
    public int index = 0;
    public Dictionary<string, Effect> effectDic = new Dictionary<string, Effect>();
    public Dictionary<string, List<Effect>> tempDic = new Dictionary<string, List<Effect>>(); 
    public Effect GetEffect(string path)
    {
        Effect effect = null;
        if (tempDic.ContainsKey(path) && tempDic[path].Count > 0)
        {
            effect = tempDic[path][0];
            tempDic[path].RemoveAt(0);
        }
        if (effect == null)
        {
            effect = new Effect();
            effect.Init(GetEffectKey(path));
        }
        effectDic[effect.effectkey] = effect;
        return effect;
    }
    public void Addtemp(Effect effect)
    {
        effect.Stop();
        Debugger.Log("特效加入缓存" + effect.loadpath);
        if (tempDic.ContainsKey(effect.loadpath) == false)
        {
            tempDic[effect.loadpath] = new List<Effect>();
        }
        if (effectDic.ContainsKey(effect.effectkey))
        {
            effectDic.Remove(effect.effectkey);
        }
        tempDic[effect.loadpath].Add(effect);
    }
    public void OnUpdate(float dt)
    {
        List<Effect> removeList = new List<Effect>();
        foreach (var item in effectDic.Values)
        {
            item.Update(dt);
            if (item.addtemp)
            {
                removeList.Add(item);
            }
        }
        for (int i = removeList.Count - 1; i >= 0; i--)
        {
            Addtemp(removeList[i]);
        }
        removeList.Clear();
    }
    public void DestroyAll()
    {
        foreach (var item in tempDic)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                item.Value[i].Destroy();
            }
        }
        tempDic.Clear();
    }
    private string GetEffectKey(string path)
    {
        index ++;
        return path + index;
    }
}