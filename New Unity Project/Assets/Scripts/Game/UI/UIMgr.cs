using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : Singleton<UIMgr>
{
    private Dictionary<string, BasePanel> _uiList;
    public UIMgr()
    {
        _uiList = new Dictionary<string, BasePanel>();
    }
    private GameObject m_UiRoot;
    private GameObject m_Canvas;
    private CanvasScaler m_CanvasScaler;

    private GameObject m_LowerLayer;
    private GameObject m_MiddleLayer;
    private GameObject m_HighLayer;
    private GameObject m_TopLayer;
    private GameObject m_UserLayer;
    public void Init()
    {
        m_UiRoot = TransformUtils.FindOrCreateObject("_UIRoot_", true, true);
        TransformUtils.AddComponent(m_UiRoot, Globals.dontDestroy);
        InitLayer();
    }

    private void InitLayer()
    {
        m_Canvas = m_UiRoot.transform.Find("Canvas").gameObject;
        m_CanvasScaler = m_Canvas.GetComponent<CanvasScaler>();

        m_LowerLayer = m_Canvas.transform.Find("LowerLayer").gameObject;
        m_MiddleLayer = m_Canvas.transform.Find("MiddleLayer").gameObject;
        m_HighLayer = m_Canvas.transform.Find("HighLayer").gameObject;
        m_TopLayer = m_Canvas.transform.Find("TopLayer").gameObject;
        m_UserLayer = m_Canvas.transform.Find("UserLayer").gameObject;

        AdjustCanvasScale();
    }

    private void AdjustCanvasScale()
    {
        float ratioSetting = Screen.width / Screen.height;
        float ratioScreen = Globals.screenOriWidth / Globals.screenOriHeight;
        if (Mathf.Abs(ratioSetting - ratioScreen) < 0.03)
        {
            m_CanvasScaler.matchWidthOrHeight = 1;
        }
        else if (ratioSetting > ratioScreen)
        {
            m_CanvasScaler.matchWidthOrHeight = 1;
        }
        else if (ratioSetting < ratioScreen)
        {
            m_CanvasScaler.matchWidthOrHeight = 0;
        }
    }

    /// <summary>
    /// 根据名字打开界面
    /// </summary>
    /// <param name="viewName"></param>
    public BasePanel OpenUI<T>(UILayer layer = UILayer.MiddleLayer,object data = null) where T : new()
    {
        string name = typeof(T).Name;
        BasePanel panel = GetPanel(name);
        if (panel == null)
        {
            panel = InitPanel<T>(layer, data);
        }
        panel.InitBaseData(layer, name, data);
        return panel;
    }

    public BasePanel InitPanel<T>(UILayer layer = UILayer.MiddleLayer,object data = null) where T : new()
    {
        if (GetResourcePath<T>().Equals(""))
        {
            Debugger.LogError("报错,无法找到" + typeof(T).Name + "文件prefab");
            return null;
        }
        BasePanel createPanel = new T() as BasePanel;
        _uiList[typeof(T).Name] = createPanel;
        return createPanel;
    }

    public BasePanel GetPanel(string name)
    {
        if (_uiList.ContainsKey(name))
        {
            return _uiList[name];
        }
        return null;
    }

    public BasePanel GetPanel<T>()
    {
        return GetPanel(typeof(T).Name);
    }

    public GameObject GetLayerParent(UILayer layer)
    {
        switch(layer)
        {
            case UILayer.LowLayer: return m_LowerLayer; break;
            case UILayer.MiddleLayer: return m_MiddleLayer; break;
            case UILayer.HighLayer: return m_HighLayer; break;
            case UILayer.TopLayer: return m_TopLayer; break;
            case UILayer.User: return m_UserLayer; break;
        }
        return m_MiddleLayer;
    }
    public string GetResourcePath<T>() where T : new()
    {
        string name = typeof(T).Name;
        return GetResourcePath(name);
    }
    public string GetResourcePath(string name)
    {
        switch (name)
        {
            case "LoadingPanel": return "longdingview";
            case "WaitPanel": return "waitview";
            case "RoomPanel": return "roomview";
            case "LoginPanel": return "loginview";
            case "SelectHeroPanel": return "selectheroview";
            case "TeamPanel": return "teamview";
            case "SkillDescPanel": return "descpanel";
            case "EquipDescPanel": return "descpanel";
            case "BattlePanel": return "battleview";
        }
        return "";
    }
    public void Update(float dt)
    {
        List<BasePanel> removeItem = new List<BasePanel>();
        foreach (var item in _uiList.Values)
        {
            if (item.LoadState == UILoadingState.UnLoad)
            {
                removeItem.Add(item);
            }
            else
            {
                item.OnUpdate(dt);
            }
        }
        for (int i = removeItem.Count - 1; i >= 0; i--)
        {
            if (removeItem[i].LoadState == UILoadingState.UnLoad)
            {
                _uiList.Remove(removeItem[i].className);
                removeItem[i].Destroy(true);
            }
        }
    }
    /// <summary>
    /// 从字典移除
    /// </summary>
    public void RemoveDic(string path)
    {
        if (_uiList.ContainsKey(path))
        {
            _uiList.Remove(path);
        }
    }

    public void RemoveDicByType(bool ignoreForver = true,bool isForce = true)
    {
        List<BasePanel> removeItem = new List<BasePanel>();
        foreach (var item in _uiList.Values)
        {
            if (item.isForverUI)
            {
                continue;
            }
            removeItem.Add(item);
        }

        for (int i = removeItem.Count - 1; i >= 0; i--)
        {
            _uiList.Remove(removeItem[i].className);
            removeItem[i].Destroy(isForce);
        }
    }

    public void RemoveDicAndResByType(bool ignoreForver = true)
    {
        RemoveDicByType(ignoreForver, true);
        ResourceMgr.GetInstance().DestroyByResType(ResourceType.ui, false);
    }

    #region 一些通用界面相关逻辑
    public void ShowLoadResourcePanel(string resname)
    {
        LoadingPanel panel = UIMgr.GetInstance().OpenUI<LoadingPanel>(UILayer.TopLayer) as LoadingPanel;
        panel.AddLoadAction(resname);
    }
    public void RemoveLoadResourcePanel(string resname)
    {
        LoadingPanel panel = UIMgr.GetInstance().GetPanel<LoadingPanel>() as LoadingPanel;
        panel.RemoveLoadAction(resname);
    }
    public void GetSprite(Image image,string atlasName,string spitename)
    {
        ResourceData.loadComplete complete = delegate(ResourceData data)
        {
            Sprite sprite = null;
            if (atlasName.Equals(""))
            {
                sprite = data.GetMainSprite();
            }
            else
            {
                sprite = data.GetSprite(spitename);
            }
            image.sprite = sprite;
        };
        string path = spitename;
        if (!atlasName.Equals(""))
        {
            path = ResourcePath.GetUIAtlas(atlasName);
        }
        ResourceMgr.GetInstance().LoadResource(ResourceType.ui, path, complete);
    }
    #endregion
}