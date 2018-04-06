using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : ITick
{
    protected GameObject m_Root;
    public GameObject Root
    {
        set 
        { 
            m_Root = value;
            if (m_Root == null)
                m_Transform = null;
            else
                m_Transform = m_Root.transform;
        }
        get { return m_Root; }
    }
    protected Transform m_Transform;

    private GameObject m_Parent;
    private UILayer m_Layer;
    private OpenState m_OpenState;
    public OpenState OpenState
    {
        get { return m_OpenState; }
    }
    protected UILoadingState m_LoadingState = UILoadingState.Init;
    public UILoadingState LoadState
    {
        get { return m_LoadingState; }
    }

    protected object m_InitData;
    private string CLASS_NAME;
    public string className
    {
        get { return CLASS_NAME; }
    }

    public bool isForverUI = false;

    public void InitBaseData(UILayer layer,string className, object data)
    {
        m_InitData = data;
        m_Layer = layer;
        GameObject newParent = UIMgr.GetInstance().GetLayerParent(layer);
        CLASS_NAME = className;
        if(m_Parent == null || m_Parent != newParent)
             m_Parent = newParent;
        m_OpenState = OpenState.Open;
        InitData();
        if (m_LoadingState == UILoadingState.Init)
        {
            InitResource();
        }
        else if (m_LoadingState == UILoadingState.LoadComplete || m_LoadingState == UILoadingState.UnLoad)
        {
            RefreshBaseData();
        }
    }
    private void ResetScreen()
    {
        RectTransform rectTransform = m_Transform as RectTransform;
        if (rectTransform != null)
        {
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
        }
        m_Transform.localScale = Vector3.one;
    }
    private void RefreshBaseData()
    {
        m_Transform.SetParent(m_Parent.transform);
        TransformUtils.SetVisible(Root, true);
        ResetScreen();
        m_OpenState = OpenState.Open;
        m_LoadingState = UILoadingState.LoadComplete;
        Refresh();
    }
    protected virtual void InitResource()
    {
        string res = UIMgr.GetInstance().GetResourcePath(CLASS_NAME);
        Debugger.Log("加载UIPrefab资源" + res,Color.cyan);
        m_LoadingState = UILoadingState.Loading;
        ResourceType resType = ResourceType.ui;
        if (isForverUI)
        {
            resType = ResourceType.uiForever;
        }
        ResourceMgr.GetInstance().LoadResource(resType, ResourcePath.GetUIModules(res), LoadComplete);
    }
    private void LoadComplete(ResourceData data)
    {
        if (m_LoadingState != UILoadingState.Loading)
        {
            return;
        }
        m_LoadingState = UILoadingState.LoadComplete;
        DestroyRoot();
        Root = data.GetCreateObject();
        m_Transform.SetParent(m_Parent.transform);
        ResetScreen();
        TransformUtils.SetVisible(Root, m_OpenState == OpenState.Open);
        LoadCallback();
        Refresh();
    }
    protected virtual void LoadCallback()
    {

    }
    protected virtual void InitData()
    {

    }
    public virtual void Refresh()
    {

    }
    public virtual void Close()
    {
        if (Root != null)
        {
            TransformUtils.SetVisible(Root,false);
        }
        m_OpenState = OpenState.Close;
    }
    public virtual void Destroy(bool once = false)
    {
        if (once)
        {
            RemoveAction();
            DestroyRoot();
            m_LoadingState = UILoadingState.Destroy;
            UIMgr.GetInstance().RemoveDic(CLASS_NAME);
            ResourceMgr.GetInstance().DecreaseLoad(ResourceType.ui, ResourcePath.GetUIModules(CLASS_NAME));
        }
        else
        {
            Close();
            m_LoadingState = UILoadingState.UnLoad;
        }
    }
    protected virtual void RemoveAction()
    {

    }

    private void DestroyRoot()
    {
        if (m_Root != null)
        {
            GameObject.DestroyImmediate(m_Root);
        }
    }

    public virtual void OnUpdate(float dt)
    {
        
    }

    public virtual void FixedUpdate(float dt)
    {
        
    }

    public virtual void LateUpdate(float dt)
    {
        
    }
}
