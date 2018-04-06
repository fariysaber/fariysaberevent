using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamInfoPanel : ChildPanel
{
    protected GamePlayerEntityData data;
    public TeamInfoPanel(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void LoadResCallBack()
    {
        root.rectTransform.anchorMin = new Vector2(1, 0);
        root.rectTransform.anchorMax = new Vector2(1, 1);
        root.rectTransform.offsetMax = new Vector2(0, 0);
        root.rectTransform.offsetMin = new Vector2(-350, 0);
        InitNode();
        if (data != null)
        {
            RefreshInfo();
        }
    }
    protected virtual void InitNode()
    {
    }
    public virtual void SetInfo(GamePlayerEntityData data)
    {
        this.data = data;
        if(isLoadCompete)
        {
            RefreshInfo();
        }
    }
    protected virtual void RefreshInfo()
    {
        Debugger.Log(data.entityKey);
    }
}
public class TeamInfoZonglanPanel : TeamInfoPanel
{
    private UIText m_Name;
    private UIText m_dingweidesc;
    private UIImage leixinicon;
    private UIText lvtext;

    private UIText exptext;
    private UIImage expimg;

    private UIText nowpingji;
    private UIText nextpingji;

    private UIImage pingjiicon;
    private UIText jingdu;

    private UIText zhandouli;

    private UITransform skilllist;
    private List<SkillIcon> skillIcon = new List<SkillIcon>();
    public TeamInfoZonglanPanel(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void InitNode()
    {
        m_Name = new UIText(root.transform, "name");
        m_dingweidesc = new UIText(root.transform.Find("dingwei"), "dingweidesc");
        leixinicon = new UIImage(root.transform.Find("leixin").Find("leixdesc"), "leixinicon");
        lvtext = new UIText(root.transform.Find("dengji").Find("dengjidesc"), "Text");
        exptext = new UIText(root.transform.Find("dengji"), "need");
        expimg = new UIImage(root.transform.Find("dengji").Find("back"), "icon");
        nowpingji = new UIText(root.transform.Find("pingji"), "nowpingji");
        nextpingji = new UIText(root.transform.Find("pingji"), "nextpingji");
        pingjiicon = new UIImage(root.transform.Find("pingji").Find("back"), "icon");
        jingdu = new UIText(root.transform.Find("pingji"), "jingdu");

        zhandouli = new UIText(root.transform.Find("zhanli").Find("zhanlidesc"), "Text");

        skilllist = new UITransform(root.transform, "skilllist");
    }
    protected override void RefreshInfo()
    {
        base.RefreshInfo();
        HeroVo heroVo = data.herovo;
        m_Name.text = heroVo.name;
        m_dingweidesc.text = "定位：" + Globals.GetBattleTypeName(heroVo.battleType);
        UIMgr.GetInstance().GetSprite(leixinicon._image, ResourcePath.commonUi, Globals.GetBattleShuxin(heroVo.shuxin));
        lvtext.text = data.level + "";
        int nextexp = data.GetHeroNextExp();
        if (nextexp < 0)
        {
            exptext.text = "";
        }
        else
        {
            exptext.text = data.GetHeroNextExp() + "";
        }
        expimg._image.fillAmount = 1 - (float)nextexp / (float)data.nowlevelNeedAllExp();
        nowpingji.text = Globals.GetPingji(data.GetPingji());
        nowpingji._text.color = Globals.GetPingjiColor(nowpingji.text);
        nextpingji.text = Globals.GetNextPingji(data.GetPingji());
        nextpingji._text.color = Globals.GetPingjiColor(nextpingji.text);
        pingjiicon._image.fillAmount = 0 / Globals.GetShengxingSuipian(data.GetPingji());
        jingdu.text = "0/" + Globals.GetShengxingSuipian(data.GetPingji());

        zhandouli.text = data.GetZhandouli() + "";

        RefreshSkillInfo();
    }
    private void RefreshSkillInfo()
    {
        List<PlayerSkillInfo> showSkillInfo = data.GetShowSkillInfo();
        int i = 0;
        for (i = 0; i < showSkillInfo.Count; i++)
        {
            SkillIcon icon = null;
            if (skillIcon.Count <= i)
            {
                icon = new SkillIcon(skilllist.gameObject, null, "skillIcon");
                icon.LoadResource();
                skillIcon.Add(icon);
            }
            else
            {
                icon = skillIcon[i];
            }
            icon.SetInfo(showSkillInfo[i]);
        }
        for (int j = i; j < skillIcon.Count; j++)
        {
            skillIcon[j].SetActive(false);
        }
    }
    public override void Destroy()
    {
        for (int j = 0; j < skillIcon.Count; j++)
        {
            skillIcon[j].Destroy();
        }
        base.Destroy();
    }
}
public class TeamInfoShuxinPanel : TeamInfoPanel
{
    private UIText shengming;
    private UIText shengmingpj;

    private UIText atk;
    private UIText atkpj;

    private UIText defend;
    private UIText defendpj;

    private UIText baoji;
    private UIText baojipj;

    private UIText baojish;
    private UIText baojishpj;

    private UIText chuantou;
    private UIText chuantoupj;

    private UIText gedang;
    private UIText gedangpj;
    public TeamInfoShuxinPanel(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void InitNode()
    {
        shengming = new UIText(root.transform.Find("shengming").Find("num"), "num");
        shengmingpj = new UIText(root.transform.Find("shengming").Find("num"), "pingji");

        atk = new UIText(root.transform.Find("atk").Find("num"), "num");
        atkpj = new UIText(root.transform.Find("atk").Find("num"), "pingji");

        defend = new UIText(root.transform.Find("defend").Find("num"), "num");
        defendpj = new UIText(root.transform.Find("defend").Find("num"), "pingji");

        baoji = new UIText(root.transform.Find("baoji").Find("num"), "num");
        baojipj = new UIText(root.transform.Find("baoji").Find("num"), "pingji");

        baojish = new UIText(root.transform.Find("baojishanghai").Find("num"), "num");
        baojishpj = new UIText(root.transform.Find("baojishanghai").Find("num"), "pingji");

        chuantou = new UIText(root.transform.Find("chuantou").Find("num"), "num");
        chuantoupj = new UIText(root.transform.Find("chuantou").Find("num"), "pingji");

        gedang = new UIText(root.transform.Find("gedang").Find("num"), "num");
        gedangpj = new UIText(root.transform.Find("gedang").Find("num"), "pingji");
    }
    protected override void RefreshInfo()
    {
        base.RefreshInfo();
        shengming.text = data.GetHp() + "";
        shengmingpj.text = data.GetHpPj() + "";
        shengmingpj._text.color = Globals.GetPingjiColor(shengmingpj.text);

        atk.text = data.GetAtk() + "";
        atkpj.text = data.GetAtkPj() + "";
        atkpj._text.color = Globals.GetPingjiColor(atkpj.text);

        defend.text = data.GetDefend() + "";
        defendpj.text = data.GetDefendPj() + "";
        defendpj._text.color = Globals.GetPingjiColor(defendpj.text);

        baoji.text = data.GetBao() * 100 + "%";
        baojipj.text = data.GetBaojiPj() + "";
        baojipj._text.color = Globals.GetPingjiColor(baojipj.text);

        baojish.text = data.GetBaoshang() * 100 + "%";
        baojishpj.text = data.GetBaojiShangPj() + "";
        baojishpj._text.color = Globals.GetPingjiColor(baojishpj.text);

        chuantou.text = data.GetGedang() + "%";
        TransformUtils.SetVisible(chuantoupj.gameObject, false);
        gedang.text = data.GetGedang() + "%";
        TransformUtils.SetVisible(gedangpj.gameObject, false);
    }
}
public class TeamInfoShengxingPanel : TeamInfoPanel
{
    private UIText shengming;
    private UIText shengmingpj;

    private UIText atk;
    private UIText atkpj;

    private UIText defend;
    private UIText defendpj;

    private UIText baoji;
    private UIText baojipj;

    private UIText baojish;
    private UIText baojishpj;

    private UITransform skilllist;
    private List<SkillIcon> skillIcon = new List<SkillIcon>();
    public TeamInfoShengxingPanel(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void InitNode()
    {
        shengming = new UIText(root.transform.Find("shengming").Find("num"), "num");
        shengmingpj = new UIText(root.transform.Find("shengming").Find("num"), "pingji");

        atk = new UIText(root.transform.Find("atk").Find("num"), "num");
        atkpj = new UIText(root.transform.Find("atk").Find("num"), "pingji");

        defend = new UIText(root.transform.Find("defend").Find("num"), "num");
        defendpj = new UIText(root.transform.Find("defend").Find("num"), "pingji");

        baoji = new UIText(root.transform.Find("baoji").Find("num"), "num");
        baojipj = new UIText(root.transform.Find("baoji").Find("num"), "pingji");

        baojish = new UIText(root.transform.Find("baojishanghai").Find("num"), "num");
        baojishpj = new UIText(root.transform.Find("baojishanghai").Find("num"), "pingji");

        skilllist = new UITransform(root.transform, "skilllist");
    }
    protected override void RefreshInfo()
    {
        base.RefreshInfo();
        shengming.text = "+" + data.GetHpShengxing() + "%";
        shengmingpj.text = data.GetNextHpPj();
        shengmingpj._text.color = Globals.GetPingjiColor(shengmingpj.text);

        atk.text = "+" + data.GetAtkShengxing() + "%";
        atkpj.text = data.GetNextAtkPj();
        atkpj._text.color = Globals.GetPingjiColor(atkpj.text);

        defend.text = "+" + data.GetDefendShengxing() + "%";
        defendpj.text = data.GetNextDefendPj();
        defendpj._text.color = Globals.GetPingjiColor(defendpj.text);

        baoji.text = "+" + data.GetBaojiShengxing() + "%";
        baojipj.text = data.GetNextBaojiPj();
        baojipj._text.color = Globals.GetPingjiColor(baojipj.text);

        baojish.text = "+" + data.GetBaoShangShengxing() + "%";
        baojishpj.text = data.GetNextBaoShangPj();
        baojishpj._text.color = Globals.GetPingjiColor(baojishpj.text);

        RefreshSkillInfo();
    }
    private void RefreshSkillInfo()
    {
        List<PlayerSkillInfo> showSkillInfo = data.GetShowSkillInfo();
        int i = 0;
        for (i = 0; i < showSkillInfo.Count; i++)
        {
            SkillIcon icon = null;
            if (skillIcon.Count <= i)
            {
                icon = new SkillIcon(skilllist.gameObject, null, "skillIcon");
                icon.LoadResource();
                skillIcon.Add(icon);
            }
            else
            {
                icon = skillIcon[i];
            }
            icon.SetInfo(showSkillInfo[i]);
        }
        for (int j = i; j < skillIcon.Count; j++)
        {
            skillIcon[j].SetActive(false);
        }
    }
    public override void Destroy()
    {
        for (int j = 0; j < skillIcon.Count; j++)
        {
            skillIcon[j].Destroy();
        }
        base.Destroy();
    }
}
public class TeamInfoZhuangbeiPanel : TeamInfoPanel
{
    EquipIcon wuqiIcon;
    EquipIcon ling1;
    EquipIcon ling2;
    EquipIcon ling3;
    public TeamInfoZhuangbeiPanel(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void InitNode()
    {
        wuqiIcon = new EquipIcon(root.transform.Find("wuqiobj").gameObject, null, "equip");
        wuqiIcon.LoadResource();
        wuqiIcon.SetInfo(data.wuqi);

        ling1 = new EquipIcon(root.transform.Find("ling1").gameObject, null, "equip");
        ling1.LoadResource();
        ling2 = new EquipIcon(root.transform.Find("ling2").gameObject, null, "equip");
        ling2.LoadResource();
        ling3 = new EquipIcon(root.transform.Find("ling3").gameObject, null, "equip");
        ling3.LoadResource();
    }

    protected override void RefreshInfo()
    {
        wuqiIcon.SetInfo(data.wuqi);
        ling1.SetInfo(data.lingzhuang1);
        ling2.SetInfo(data.lingzhuang2);
        ling3.SetInfo(data.lingzhuang3);
    }
}