using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillIcon : ChildPanel
{
    private PlayerSkillInfo info;
    private UIText level;
    private UIText tip;
    private UIButton btn;
    public SkillIcon(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void LoadResCallBack()
    {
        level = new UIText(root.transform.Find("level"), "Text");
        tip = new UIText(root.transform.Find("tip"), "Text");
        btn = new UIButton(root.transform, "btn");
        EventTriggerListener.Get(btn.gameObject).onClick = OnBtnClick;
        if (info != null)
            RefreshInfo();
    }
    protected virtual void OnBtnClick(GameObject go, PointerEventData eventData)
    {
        UIMgr.GetInstance().OpenUI<SkillDescPanel>(UILayer.HighLayer,info);
    }
    public void SetInfo(PlayerSkillInfo info)
    {
        this.info = info;
        if (isLoadCompete)
        {
            RefreshInfo();
        }
    }
    private void RefreshInfo()
    {
        level.text = info.level + "";
        tip.text = Globals.GetSkillTypeName(info.skillVo.type);

        UIMgr.GetInstance().GetSprite(btn._image, "", ResourcePath.GetSkillIconPath(info.skillVo.icon));
    }
}