using System;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    main = 0,
    font = 1,
    config = 2,
    uiForever = 3,
    ui = 8,
    scene = 9,
    entity = 11,
    all = 100,
}
public class ResourceMgr : Singleton<ResourceMgr>
{
    private Dictionary<ResourceType, Dictionary<string, ResourceData>> m_ResourceDic;
    private AssetBundleManifest m_Abm = null; 
    public ResourceMgr()
    {
        m_ResourceDic = new Dictionary<ResourceType, Dictionary<string, ResourceData>>();
    }
    public void SetBackManifest(AssetBundleManifest mainfest)
    {
        Debugger.Log("获取总的依赖成功", Color.green);
        m_Abm = mainfest;
    }
    public void LoadResource(ResourceType resType,string resource,ResourceData.loadComplete loadComplete = null,
        bool removeAsset = false, ResourceData.loadFailed failedCallback = null)
    {
        List<string> dependList = LoadDependencies(resType, resource);
        ResourceData data = GetResourceData(resType,resource);
        if (data == null)
        {
            AssetLoader.LoadComplete LoaderComplete = delegate(AssetLoader loader)
            {
                data.LoadCallback(loader);
            };
            AssetLoader.LoadError LoaderError = delegate(AssetLoader loader)
            {
                data.LoadFailed(loader);
            };
            data = AddResourceData(resType, resource, removeAsset, loadComplete, failedCallback);
            AssetMgr.AddLoad<object>(resource, LoaderComplete, LoaderError);
            data.dependList = dependList;
            return;
        }
        data.RefreshRemoveAsset(removeAsset);
        data.CallResource(loadComplete,failedCallback);
    }
    public void DecreaseLoad(ResourceType resType, string path)
    {
        ResourceData data = GetResourceData(resType, path);
        if (data != null)
        {
            data.DecreaseLoad();
        }
    }
    private ResourceData AddResourceData(ResourceType resType, string resource,bool removeAsset = false, ResourceData.loadComplete loadComplete = null,
        ResourceData.loadFailed failedCallback = null)
    {
        if (!m_ResourceDic.ContainsKey(resType))
        {
            m_ResourceDic[resType] = new Dictionary<string, ResourceData>();
        }
        if (!m_ResourceDic[resType].ContainsKey(resource))
        {
            m_ResourceDic[resType][resource] = new ResourceData();
        }
        m_ResourceDic[resType][resource].filePath = resource;
        m_ResourceDic[resType][resource].RefreshRemoveAsset(removeAsset);
        m_ResourceDic[resType][resource].AddComplete(loadComplete);
        m_ResourceDic[resType][resource].AddFail(failedCallback);
        return m_ResourceDic[resType][resource];
    }
    public ResourceData GetResourceData(ResourceType resType, string resource)
    {
        if (m_ResourceDic.ContainsKey(resType))
        {
            if (m_ResourceDic[resType].ContainsKey(resource))
            {
                //如果是同一层级的资源标志位则直接返回
                return m_ResourceDic[resType][resource];
            }
        }
        //查看是否别的层级存在这份资源，如果是的话将资源放在较低级的层级
        foreach (var item in m_ResourceDic)
        {
            if (item.Value.ContainsKey(resource))
            {
                ResourceData getData = item.Value[resource];
                if ((int)resType < (int)item.Key)
                {
                    m_ResourceDic[item.Key].Remove(resource);
                    m_ResourceDic[resType][resource] = getData;
                }
                return getData;
            }
        }
        return null;
    }

    private List<string> LoadDependencies(ResourceType resType, string path)
    {
        List<string> dependList = new List<string>();
        if (m_Abm != null)
        {
            path = path.Replace(ResourcePath.GetRootPath(), "");
            string[] dps = m_Abm.GetAllDependencies(path + ".ab");
            for (int i = 0; i < dps.Length; i++)
            {
                string dependPath = dps[i].Replace(".ab", "");
                dependPath = ResourcePath.GetRootPath() + dependPath;
                AssetLoader.LoadComplete complete = delegate
                {
                    Debugger.Log("加载资源" + path + "的依赖资源" + dependPath);
                };
                LoadResource(resType, dependPath);
                dependList.Add(dependPath);
            }
        }
        return dependList;
    }
    public void DestroyByPath(string path, bool removeForce = false)
    {
        foreach (var item in m_ResourceDic)
        {
            if (item.Value.ContainsKey(path))
            {
                item.Value[path].DestroyResource(removeForce);
                m_ResourceDic[item.Key].Remove(path);
                return;
            }
        }
    }
    public void DestroyByPathAndType(ResourceType resType, string path, bool removeForce = false)
    {
        if (m_ResourceDic.ContainsKey(resType))
        {
            if (m_ResourceDic[resType].ContainsKey(path))
            {
                m_ResourceDic[resType][path].DestroyResource(removeForce);
                m_ResourceDic[resType].Remove(path);
                return;
            }
        }
    }
    public void DestroyByResType(ResourceType resType, bool removeForce = false)
    {
        if (m_ResourceDic.ContainsKey(resType))
        {
            List<ResourceData> removeItem = new List<ResourceData>();
            foreach (var item in m_ResourceDic[resType].Values)
            {
                removeItem.Add(item);
            }
            for (int i = removeItem.Count - 1; i >= 0; i--)
            {
                removeItem[i].DestroyResource(removeForce);
            }
            m_ResourceDic[resType].Clear();
        }
    }

    public void GetPrintAllReource()
    {
        Debugger.Log("~~~~~~~~~~~~~~打印资源池~~~~~~~~~~~~~~", Color.yellow);
        foreach (var item in m_ResourceDic)
        {
            Debugger.Log(item.Key + "", Color.yellow);
            foreach (var item2 in m_ResourceDic[item.Key])
            {
                Debugger.Log(item2.Key, Color.magenta);
            }
        }
    }

    /// <summary>
    /// 只是从字典删除
    /// </summary>
    /// <param name="path"></param>
    public void RemoveResourceDic(string path)
    {
        foreach (var item in m_ResourceDic)
        {
            if (item.Value.ContainsKey(path))
            {
                m_ResourceDic[item.Key].Remove(path);
                return;
            }
        }
    }
}
public class ResourceData
{
    public enum LoaderAction
    {
        none = 0,
        secess = 1,
        fail = 2,
        destroy = 3,
        destroyForce = 4,
    }
    public delegate void loadComplete(ResourceData data);
    public delegate void loadFailed(ResourceData data);
    private List<loadComplete> m_loadComplete;
    private List<loadFailed> m_loadfailed;
    public LoaderAction hasLoaded;
    private int loadCount = 0;
    private bool m_RemoveAsset;

    private bool isPrefabRes;

    private UnityEngine.Object m_Content;
    private UnityEngine.AssetBundle m_AssetBundle;
    public string filePath;
    public List<string> dependList;
    public ResourceData()
    {
        m_loadComplete = new List<loadComplete>();
        m_loadfailed = new List<loadFailed>();
        dependList = new List<string>();
        hasLoaded = LoaderAction.none;
    }
    public void RefreshRemoveAsset(bool removeAsset)
    {
        m_RemoveAsset = removeAsset;
    }
    public void DecreaseLoad()
    {
        loadCount -= 1;
        loadCount = loadCount < 0 ? 0 : loadCount;
    }
    public void AddComplete(loadComplete complete)
    {
        if (complete == null || !m_loadComplete.Contains(complete))
        {
            loadCount += 1;
        }
        if (complete == null)
        {
            return;
        }
        if (!m_loadComplete.Contains(complete))
        {
            m_loadComplete.Add(complete);
        } 
    }
    public void AddFail(loadFailed failed)
    {
        if (failed == null)
        {
            return;
        }
        if (!m_loadfailed.Contains(failed))
        {
            m_loadfailed.Add(failed);
        }
    }
    public void CallResource(loadComplete complete, loadFailed failed)
    {
        AddComplete(complete);
        AddFail(failed);
        if (hasLoaded == LoaderAction.none)
        {
            return;
        }
        if (hasLoaded == LoaderAction.secess && complete != null)
        {
            complete(this);
            return;
        }
        if (hasLoaded == LoaderAction.fail && failed != null)
        {
            failed(this);
            return;
        }
    }

    public virtual void LoadCallback(AssetLoader loader)
    {
        Debugger.Log("加载资源>>>>>>>>>>>>>" + filePath,Color.green);
        if (hasLoaded == LoaderAction.destroy || hasLoaded == LoaderAction.destroyForce)
        {
            bool destroyDepend = (hasLoaded == LoaderAction.destroyForce);
            loader._loader.assetBundle.Unload(destroyDepend);
            return;
        }
        hasLoaded = LoaderAction.secess;
        m_AssetBundle = loader._loader.assetBundle;

        int index = loader.assetRecord.allUrl.LastIndexOf("/");
        string assetName = loader.assetRecord.allUrl.Substring(index + 1);
        if (assetName != "abres" && !filePath.Contains(ResourcePath.GetSceneRes("")))
        {
            try
            {
                m_Content = m_AssetBundle.LoadAsset(assetName);
            }
            catch (Exception ex)
            {
                m_Content = null;
                Debug.Log("没有Content" + assetName);
            }
        }
        if (m_loadComplete.Count <= 0)
        {
            return;
        }
        foreach (var item in m_loadComplete)
        {
            if (item != null)
            {
                item(this);
            }
        }
    }
    public virtual void LoadFailed(AssetLoader loader)
    {
        hasLoaded = LoaderAction.fail;
        if (m_loadfailed.Count <= 0)
        {
            return;
        }
        foreach (var item in m_loadfailed)
        {
            if (item != null)
            {
                item(this);
            }
        }
    }
    public virtual void DestroyResource(bool removeForce = false)
    {
        if (m_AssetBundle != null)
        {
            m_AssetBundle.Unload(removeForce);
            m_AssetBundle = null;
        }
        if (m_Content != null)
        {
            if (!isPrefabRes)
            {
                Resources.UnloadAsset(m_Content);
            }
            m_Content = null;
        }
        DisposeAll(removeForce);
    }
    public void DisposeAll(bool removeForce = false)
    {
        if (hasLoaded == LoaderAction.none)
        {
            AssetMgr.CancleLoad<object>(filePath);
        }
        if (removeForce)
        {
            hasLoaded = LoaderAction.destroyForce;
        }
        else
        {
            hasLoaded = LoaderAction.destroy;
        }
        for (int i = 0; i < dependList.Count; i++)
        {
            ResourceMgr.GetInstance().DecreaseLoad(ResourceType.all, dependList[i]);
        }
        Debugger.Log("卸载>>>>>>>>>>>>>" + filePath, Color.red);
        ResourceMgr.GetInstance().RemoveResourceDic(filePath);
    }
    public GameObject GetCreateObject()
    {
        GameObject createObject = GameObject.Instantiate(m_Content as GameObject);
        isPrefabRes = true;
        return createObject;
    }
    public TextAsset GetTextAsset()
    {
        TextAsset textAsset = m_Content as TextAsset;
        return textAsset;
    }
    public Sprite GetMainSprite()
    {
        string[] fileNames = filePath.Split('/');
        string filename = fileNames[fileNames.Length - 1];
        Sprite sprite = m_AssetBundle.LoadAsset<Sprite>(filename);
        return sprite;
    }
    public Sprite GetSprite(string name)
    {
        Sprite sprite = m_AssetBundle.LoadAsset<Sprite>(name);
        return sprite;
    }
}