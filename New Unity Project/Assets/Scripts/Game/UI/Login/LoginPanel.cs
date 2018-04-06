using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginPanel : BasePanel
{
    UITransform back;
    UITransform backImage;
    UITransform startBtn;
    UITransform reloadBtn;

    protected override void InitData()
    {
        base.InitData();
    }
    protected override void LoadCallback()
    {
        back = new UITransform(m_Transform, "back");
        backImage = new UITransform(back.transform, "backImage");
        startBtn = new UITransform(backImage.transform, "restart");
        reloadBtn = new UITransform(backImage.transform, "reload");

        EventTriggerListener.Get(startBtn.gameObject).onClick = OnStartBtn;
        EventTriggerListener.Get(reloadBtn.gameObject).onClick = OnReloadBtn;
    }

    private void OnStartBtn(GameObject go, PointerEventData eventData)
    {
        SceneMgr.GetInstance().SwichScene<SelectHeroScene>();
    }

    private void OnReloadBtn(GameObject go, PointerEventData eventData)
    {

    }

    public override void Refresh()
    {
        base.Refresh();
    }
}
