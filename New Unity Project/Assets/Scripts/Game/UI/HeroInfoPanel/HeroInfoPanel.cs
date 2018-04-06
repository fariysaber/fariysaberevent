using System;
using System.Collections.Generic;
using UnityEngine;

public class ChildPanel
{
    public UITransform root;
    public GameObject parent;
    public bool isDestroy;
    public bool isLoadCompete;
    public bool isLoading = false;//是否已经在加载
    public string resource;
    public delegate void LoadCallback();
    public LoadCallback loadback;
    public bool active = true;
    public ChildPanel(GameObject parent, LoadCallback callback, string res)
    {
        this.parent = parent;
        this.resource = res;
        loadback = callback;
    }
    public void JudgeLoad()
    {
        if (isLoading == false)
        {
            LoadResource();
        }
    }
    public virtual void LoadResource()
    {
        isLoading = true;
        ResourceMgr.GetInstance().LoadResource(ResourceType.ui, ResourcePath.GetUIModules(resource), LoadComplete);
    }
    private void LoadComplete(ResourceData data)
    {
        isLoadCompete = true;
        if (isDestroy || parent == null)
        {
            return;
        }
        root = new UITransform(data.GetCreateObject().transform,"");
        root.transform.SetParent(parent.transform);
        TransformUtils.ResetTransform(root.gameObject);
        if (loadback != null)
        {
            loadback();
        }
        LoadResCallBack();
        SetActive(active);
    }
    public void SetActive(bool value)
    {
        active = value;
        if (isLoadCompete)
        {
            TransformUtils.SetVisible(root.gameObject, value);
        }
    }
    protected virtual void LoadResCallBack()
    {
    }
    public virtual void Destroy()
    {
        isDestroy = true;
    }
}

public class HeroInfoPanel : ChildPanel
{
    public HeroVo heroVo;
    private UIText dingweidesc;
    private UITransform dingwei;
    private UIImage leixinicon;
    private UIText pingji;

    private UITransform shuxin;
    private UIText shengming;
    private UIImage shengmingicon;
    private UIText gongji;
    private UIImage gongjiicon;
    private UIText fangyu;
    private UIImage fangyuicon;
    private UIText baojilv;
    private UIImage baojilvicon;
    private UIText baojishanghai;
    private UIImage baojishanghaiicon;
    private UIText nengliang;
    private UIImage nengliangicon;

    private int star;
    public HeroInfoPanel(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void LoadResCallBack()
    {
        dingwei = new UITransform(root.transform, "dingwei");
        dingweidesc = new UIText(dingwei.transform, "dingweidesc");
        leixinicon = new UIImage(root.transform.Find("leixin").Find("leixdesc"), "leixinicon");
        pingji = new UIText(root.transform.Find("pingji").Find("leixdesc"), "Text");

        shuxin = new UITransform(root.transform.Find("shuxin"),"");
        shengming = new UIText(shuxin.transform.Find("shengming"), "pingji");
        shengmingicon = new UIImage(shuxin.transform.Find("shengming").Find("back"), "icon");
        gongji = new UIText(shuxin.transform.Find("gongji"), "pingji");
        gongjiicon = new UIImage(shuxin.transform.Find("gongji").Find("back"), "icon");
        fangyu = new UIText(shuxin.transform.Find("fangyu"), "pingji");
        fangyuicon = new UIImage(shuxin.transform.Find("fangyu").Find("back"), "icon");
        baojilv = new UIText(shuxin.transform.Find("baojilv"), "pingji");
        baojilvicon = new UIImage(shuxin.transform.Find("baojilv").Find("back"), "icon");
        baojishanghai = new UIText(shuxin.transform.Find("baojishanghai"), "pingji");
        baojishanghaiicon = new UIImage(shuxin.transform.Find("baojishanghai").Find("back"), "icon");
        nengliang = new UIText(shuxin.transform.Find("nengliang"), "pingji");
        nengliangicon = new UIImage(shuxin.transform.Find("nengliang").Find("back"), "icon");
        if (this.heroVo != null)
            RefreshInfo();
    }
    public void SetInfo(HeroVo vo,int star = 0)
    {
        this.heroVo = vo;
        this.star = star;
        if (isLoadCompete)
        {
            RefreshInfo();
        }
    }
    public void RefreshInfo()
    {
        dingweidesc.text = "定位：" + Globals.GetBattleTypeName(heroVo.battleType);
        UIMgr.GetInstance().GetSprite(leixinicon._image, ResourcePath.commonUi, Globals.GetBattleShuxin(heroVo.shuxin));
        pingji.text = Globals.GetPingji(heroVo.pingji + star);
        pingji._text.color = Globals.GetPingjiColor(pingji.text);

        RefreshShengming();
        RefreshGongji();
        RefreshFangyu();
        RefreshBaojilv();
        RefreshBaojiShanghai();
        RefreshNengliang();
    }
    public void RefreshShengming()
    {
        int value = heroVo.hp;
        for (int i = 0; i < star; i++)
        {
            value += heroVo.hpUp[i];
        }
        shengming.text = Globals.GetShuxinPJ(value, 100);
        shengming._text.color = Globals.GetPingjiColor(shengming.text);
        shengmingicon.SetPercent(value / Globals.pingjimaxNum);
    }
    public void RefreshGongji()
    {
        int value = heroVo.attck;
        for (int i = 0; i < star; i++)
        {
            value += heroVo.attckUp[i];
        }
        gongji.text = Globals.GetShuxinPJ(value, 100);
        gongji._text.color = Globals.GetPingjiColor(gongji.text);
        gongjiicon.SetPercent(value / Globals.pingjimaxNum);
    }
    public void RefreshFangyu()
    {
        int value = heroVo.defend;
        for (int i = 0; i < star; i++)
        {
            value += heroVo.defendUp[i];
        }
        fangyu.text = Globals.GetShuxinPJ(value, 100);
        fangyu._text.color = Globals.GetPingjiColor(fangyu.text);
        fangyuicon.SetPercent(value / Globals.pingjimaxNum);
    }
    public void RefreshBaojilv()
    {
        int value = heroVo.critRate;
        for (int i = 0; i < star; i++)
        {
            value += heroVo.critRateUp[i];
        }
        baojilv.text = Globals.GetShuxinPJ(value, 200);
        baojilv._text.color = Globals.GetPingjiColor(baojilv.text);
        baojilvicon.SetPercent(value / Globals.pingjimaxNum);
    }
    public void RefreshBaojiShanghai()
    {
        int value = heroVo.critDamage;
        for (int i = 0; i < star; i++)
        {
            value += heroVo.critDamageUp[i];
        }
        baojishanghai.text = Globals.GetShuxinPJ(value, 200);
        baojishanghai._text.color = Globals.GetPingjiColor(baojishanghai.text);
        baojishanghaiicon.SetPercent(value / Globals.pingjimaxNum);
    }
    public void RefreshNengliang()
    {
        int value = heroVo.energy;
        for (int i = 0; i < star; i++)
        {
            value += heroVo.energyUp[i];
        }
        nengliang.text = Globals.GetShuxinPJ(value, 100);
        nengliang._text.color = Globals.GetPingjiColor(nengliang.text);
        nengliangicon.SetPercent(value / Globals.pingjimaxNum);
    }
}