using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConfig
{
    protected static Dictionary<string,Dictionary<string, object>> configDic;
    public static void Init<T>() where T : new ()
    {
        if (configDic == null)
        {
            configDic = new Dictionary<string, Dictionary<string, object>>();
        }
        string name = typeof(T).Name;
        if (!configDic.ContainsKey(name))
        {
            configDic[name] = ConfigTool.GetInstance().GetConfig<T>();
        }
    }
}
