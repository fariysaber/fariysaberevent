using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipIcon : ChildPanel
{
    private PlayerEquipInfo info;
    private UIText level;
    private UIText tip;
    private UIImage tipBack;
    private UIButton btn;
    public EquipIcon(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void LoadResCallBack()
    {
        level = new UIText(root.transform.Find("level"), "Text");
        tipBack = new UIImage(root.transform.Find("tip"), "");
        tip = new UIText(root.transform.Find("tip"), "Text");
        btn = new UIButton(root.transform, "btn");
        EventTriggerListener.Get(btn.gameObject).onClick = OnBtnClick;
        if (info != null)
            RefreshInfo();
    }
    protected virtual void OnBtnClick(GameObject go, PointerEventData eventData)
    {
        if (info == null)
        {
            return;
        }
        UIMgr.GetInstance().OpenUI<EquipDescPanel>(UILayer.HighLayer, info);
    }
    public void SetInfo(PlayerEquipInfo info)
    {
        this.info = info;
        if (isLoadCompete)
        {
            RefreshInfo();
        }
    }
    private void RefreshInfo()
    {
        if (info == null)
        {
            level.text = "1";
            tip.text = "空";
            return;
        }
        level.text = info.level + "";
        tip.text = Globals.GetEquipTypeName(info.vo.type,info.vo.childtype);
        tipBack._image.color = Globals.GetEquipColor(info.vo.star);
        root.transform.GetComponent<Image>().color = Globals.GetEquipColor(info.vo.star);
        UIMgr.GetInstance().GetSprite(btn._image, "", ResourcePath.GetEquipIconPath(info.vo.icon));
    }
}