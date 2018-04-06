using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescPanel : BasePanel
{
    protected UITransform tippanel;
    protected UIText name;
    protected UIText level;
    protected UIText levelneed;
    protected UIButton levelbtn;
    protected UIText desc;
    protected UITransform back;
    protected override void InitData()
    {
        base.InitData();
    }
    protected override void LoadCallback()
    {
        back = new UITransform(Root.transform.Find("back"), "");
        tippanel = new UITransform(Root.transform.Find("back").Find("tippanel"), "obj");
        name = new UIText(tippanel.transform, "name");
        level = new UIText(tippanel.transform, "level");
        levelneed = new UIText(tippanel.transform, "levelneed");
        levelbtn = new UIButton(tippanel.transform, "shengbtn");
        desc = new UIText(tippanel.transform, "desc");

        EventTriggerListener.Get(back.gameObject).onClick = OnBtnClick;
    }
    protected virtual void OnBtnClick(GameObject go, PointerEventData eventData)
    {
        Close();
    }
}
public class SkillDescPanel : DescPanel
{
    public PlayerSkillInfo skillInfo;
    public override void Refresh()
    {
        base.Refresh();
        skillInfo = (PlayerSkillInfo)m_InitData;
        name.text = skillInfo.skillVo.name;
        level.text = "等级:" + skillInfo.level;
        int hasNum = Globals.player1.items.xiaohaoItemInfo.GetNumByKey(Globals.SkillUpNeedName);
        int needNum = XiaohaoItemInfo.GetSkillUpLevelNum(skillInfo.level);
        if (needNum == -1)
        {
            levelneed.text = "最大等级";
            TransformUtils.SetVisible(levelbtn.gameObject, false);
        }
        else
        {
            levelneed.text = "升级：" + hasNum + "/" + needNum;
            TransformUtils.SetVisible(levelbtn.gameObject, true);
        }
        desc.text = skillInfo.skillVo.desc;
    }
}
public class EquipDescPanel : DescPanel
{
    public PlayerEquipInfo equipInfo;
    public override void Refresh()
    {
        base.Refresh();
        equipInfo = (PlayerEquipInfo)m_InitData;
        name.text = equipInfo.vo.name;
        level.text = "等级:" + equipInfo.level;
        int hasNum = Globals.player1.items.xiaohaoItemInfo.GetNumByKey(Globals.EquipUpNeedName);
        int needNum = XiaohaoItemInfo.GetEquipLevelNum(equipInfo.level);
        if (needNum == -1)
        {
            levelneed.text = "最大等级";
            TransformUtils.SetVisible(levelbtn.gameObject, false);
        }
        else
        {
            levelneed.text = "升级：" + hasNum + "/" + needNum;
            TransformUtils.SetVisible(levelbtn.gameObject, true);
        }
        desc.text = equipInfo.vo.shuxindesc;
    }
}