using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    /// <summary>
    /// 作为界面关闭的凭据
    /// </summary>
    private Dictionary<string, bool> _loadDic = new Dictionary<string,bool>();
    UITransform back;
    UITransform dialog;
    UIText desc;
    private float LoadingShowMaxCd = 10f;
    private float LoadingWaitCd = 2f;
    protected override void InitData()
    {
        base.InitData();
        isForverUI = true;
        LoadingShowMaxCd = 10f;
        LoadingWaitCd = 2f;
    }
    protected override void LoadCallback()
    {
        back = new UITransform(m_Transform, "back");
        dialog = new UITransform(back.transform, "dialog");
        desc = new UIText(dialog.transform, "desc");
    }
    public override void Refresh()
    {
        base.Refresh();
    }
    public override void Close()
    {
        base.Close();
        Debugger.Log("关闭LoadingPanel");
    }
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if (LoadState == UILoadingState.LoadComplete && OpenState == OpenState.Open)
        {
            LoadingWaitCd -= dt;
            LoadingShowMaxCd -= dt;
            if ((_loadDic.Count <= 0 && LoadingWaitCd <= 0f) || LoadingShowMaxCd <= 0f)
            {
                Close();
            }
        }
    }
    public override void Destroy(bool once = false)
    {
        base.Destroy(once);
    }
    public void AddLoadAction(string action)
    {
        if (_loadDic.ContainsKey(action))
        {
            return;
        }
        _loadDic[action] = true;
    }
    public void RemoveLoadAction(string action)
    {
        if (_loadDic.ContainsKey(action))
        {
            _loadDic.Remove(action);
        }
        if (_loadDic.Count <= 0)
        {
            if (LoadingWaitCd <= 0f)
            {
                Close();
            }
        }
    }
}
