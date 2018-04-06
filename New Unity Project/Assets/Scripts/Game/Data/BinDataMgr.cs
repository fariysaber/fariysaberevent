using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinDataMgr: Singleton<BinDataMgr>
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();
    public object GetBinInfo<T>() where T : new()
    {
        string name = typeof(T).Name;
        name = name.ToLower();
        if (!_data.ContainsKey(name))
        {
            string resName = ResourcePath.GetBinData(name);
            Debug.Log(resName);
            ResourceData data = ResourceMgr.GetInstance().GetResourceData(ResourceType.config, resName);
            TextAsset text = data.GetTextAsset();
            object dataVo = SerializerUtils.BinaryDerialise(text.bytes);
            _data[name] = dataVo;
        }
        return _data[name];
    }
    public object GetBinInfo(string name)
    {
        name = name.ToLower();
        if (!_data.ContainsKey(name))
        {
            string resName = ResourcePath.GetBinData(name);
            //Debug.Log(resName);
            ResourceData data = ResourceMgr.GetInstance().GetResourceData(ResourceType.config, resName);
            TextAsset text = data.GetTextAsset();
            object dataVo = SerializerUtils.BinaryDerialise(text.bytes);
            _data[name] = dataVo;
            
        }
        return _data[name];
    }
    public ModelInfo GetModelInfo(string path)
    {
        Dictionary<string, object> data = GetBinInfo<ModelInfo>() as Dictionary<string, object>;
        if (data.ContainsKey(path))
        {
            ModelInfo info = data[path] as ModelInfo;
            return info;
        }
        Debugger.LogError("缺少modelinfo：" + path);
        return null;
    }
    public BaseSceneMonoD GetSceneMonoD(string path)
    {
        path = path + "_sceneInfo";
        return GetBinInfo(path) as BaseSceneMonoD;
    }
    public List<BaseScrMoEntityD> GetSceneEntityMonoD(string path)
    {
        path = path + "_entityinfo";
        return GetBinInfo(path) as List<BaseScrMoEntityD>;
    }
}
