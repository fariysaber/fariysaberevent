using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomPanel : BasePanel
{
    UITransform back;
    //左边rtt模型相关操作
    RttPlayerModel model;
    RawImage rawImage;
    
    //右边选项操作
    UITransform rightPanel;
    HeroHeadPanel heroheadPanel;

    private UIText nameTxt;
    protected override void InitData()
    {
        base.InitData();
    }
    protected override void LoadCallback()
    {
        back = new UITransform(m_Transform, "back");
        InitRttModel();

        InitRightPanel();

        InitLeftTopPanel();
    }
    private void InitRttModel()
    {
        rawImage = back.transform.Find("rawImage").GetComponent<RawImage>();
        model = new RttPlayerModel("RoomHero", new Vector3(300, 600, 10), back.transform, new Vector3(-390, -86, 0),
            new Vector2(300, 600));
        //ResourceMgr.GetInstance().GetPrintAllReource();
    }
    private void InitLeftTopPanel()
    {
        heroheadPanel = new HeroHeadPanel(back.transform.Find("leftbottom").gameObject, HeadCallback, "headpanel");
        heroheadPanel.LoadResource();
        nameTxt = new UIText(back.transform.Find("leftbottom"), "Text");
    }
    private void HeadCallback()
    {
        heroheadPanel.root.transform.localPosition = new Vector3(55, -55);
    }
    private void InitRightPanel()
    {
         rightPanel = new UITransform(back.transform, "rightPanel");
         TeamBtn team = new TeamBtn(rightPanel.transform.Find("teamObj").gameObject);
         LianxiBtn lianxi = new LianxiBtn(rightPanel.transform.Find("lianxiobj").gameObject);
    }

    public override void Refresh()
    {
        base.Refresh();
        RefreshMainHeroInfo();
    }
    public void RefreshMainHeroInfo()
    {
        SetRttModel();
        SetLeftTopInfo();
    }
    private void SetLeftTopInfo()
    {
        nameTxt.text = Globals.player1.name;
        heroheadPanel.SetHeadInfo(Globals.player1.mainGamePlayerEntityData.herovo, Globals.player1.mainGamePlayerEntityData.pifu);
    }
    private void SetRttModel()
    {
        string modelName = Globals.player1.GetMainHeroModel();
        model.LoadModel(modelName, new Vector3(0, -0.75f, 1.7f), new Vector3(0, 180, 0));
    }
    public override void Close()
    {
        base.Close();
    }
    public override void Destroy(bool once = false)
    {
        if (model != null)
        {
            model.Destroy();
        }
        base.Destroy(once);
    }
}
