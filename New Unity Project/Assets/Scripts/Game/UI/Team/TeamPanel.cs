using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TeamSelectType
{
    zonglan = 0,
    shuxin = 1,
    shengxin = 2,
    zhuangbei = 3,
    caineng = 4,
    liwu = 5,
}

public class TeamPanel : BasePanel
{
    private UITransform back;
    private UITransform heroselectinfopanel;
    private UIButton returnBtn;

    private List<TeamPanelInfo> teampanelInfoList;
    public int teamindex = 0;
    public TeamSelectType selecttype = TeamSelectType.zonglan;

    RttPlayerModel model;
    protected override void InitData()
    {
        base.InitData();
    }
    protected override void LoadCallback()
    {
       back = new UITransform(m_Transform,"back");
       returnBtn = new UIButton(back.transform, "returnBtn");
       heroselectinfopanel = new UITransform(back.transform, "heroselectinfopanel");
       EventTriggerListener.Get(returnBtn.gameObject).onClick = OnBtnClickReturn;

       InitHeroModel();
       teampanelInfoList = new List<TeamPanelInfo>();
       InitZonglan();
       InitShuxin();
       InitShengxing();
       InitZhuangbei();
       InitCaineng();
       InitLiwu();
    }
    private void InitHeroModel()
    {
        UITransform heroRoot = new UITransform(back.transform, "heroRoot");

        model = new RttPlayerModel("TeamHero", new Vector3(300, 600, 10), heroRoot.transform, new Vector3(0, 0, 0),
           new Vector2(300, 600));
    }
    private void SetRttModel()
    {
        GamePlayerEntityData getData = Globals.player1.teamGamePlayerEntityData[teamindex];
        string modelName = CharactorConfig.GetData(getData.herovo.charactor[getData.pifu] + "").model;
        model.LoadModel(modelName, new Vector3(0, -0.75f, 1.7f), new Vector3(0, 180, 0));
    }
    private void InitZonglan()
    {
        UIButton btn = new UIButton(heroselectinfopanel.transform.Find("zonglan"), "btn");
        TeamInfoZonglanPanel panel = new TeamInfoZonglanPanel(back.gameObject, null, "herozonglan");
        AddNewTeamPanelInfo(btn, panel);
    }
    private void InitShuxin()
    {
        UIButton btn = new UIButton(heroselectinfopanel.transform.Find("baseshuxin"), "btn");
        TeamInfoShuxinPanel panel = new TeamInfoShuxinPanel(back.gameObject, null, "shuxinpanel");
        AddNewTeamPanelInfo(btn, panel);
    }
    private void InitShengxing()
    {
        UIButton btn = new UIButton(heroselectinfopanel.transform.Find("shengxing"), "btn");
        TeamInfoShengxingPanel panel = new TeamInfoShengxingPanel(back.gameObject, null, "shengxingpanel");
        AddNewTeamPanelInfo(btn, panel);
    }
    private void InitZhuangbei()
    {
        UIButton btn = new UIButton(heroselectinfopanel.transform.Find("zhuangbei"), "btn");
        TeamInfoZhuangbeiPanel panel = new TeamInfoZhuangbeiPanel(back.gameObject, null, "zhuangbei");
        AddNewTeamPanelInfo(btn, panel);
    }
    private void InitCaineng()
    {
        TransformUtils.SetVisible(heroselectinfopanel.transform.Find("caineng").gameObject, false);
    }
    private void InitLiwu()
    {
        TransformUtils.SetVisible(heroselectinfopanel.transform.Find("liwu").gameObject, false);
    }
    private void AddNewTeamPanelInfo(UIButton btn, TeamInfoPanel panel)
    {
        TeamPanelInfo info = new TeamPanelInfo(btn, panel, OnClickTeamInfoBtnBack);
        info.index = teampanelInfoList.Count;
        teampanelInfoList.Add(info);
    }
    private void OnClickTeamInfoBtnBack(int index)
    {
        if (selecttype == (TeamSelectType)index)
        {
            return;
        }
        selecttype = (TeamSelectType)index;
        RefreshInfo();
    }
    public override void Refresh()
    {
        base.Refresh();
        SetRttModel();
        RefreshInfo();
    }
    private void RefreshInfo()
    {
        for (int i = 0; i < teampanelInfoList.Count; i++)
        {
            if (i == (int)selecttype)
            {
                teampanelInfoList[i].infopanel.SetActive(true);
                teampanelInfoList[i].infopanel.JudgeLoad();
                teampanelInfoList[i].infopanel.SetInfo(Globals.player1.teamGamePlayerEntityData[teamindex]);
                teampanelInfoList[i].btn._image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                teampanelInfoList[i].infopanel.SetActive(false);
                teampanelInfoList[i].btn._image.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }
    
    protected virtual void OnBtnClickReturn(GameObject go, PointerEventData eventData)
    {
        Close();
    }
    public override void Destroy(bool once = false)
    {
        for (int i = 0; i < teampanelInfoList.Count; i++)
        {
            teampanelInfoList[i].infopanel.Destroy();
        }
        base.Destroy(once);
    }
}
public class TeamPanelInfo
{
    public UIButton btn;
    public int index;
    public TeamInfoPanel infopanel;
    public delegate void OnClickCallback(int dex);
    public OnClickCallback callback;
    public TeamPanelInfo(UIButton btn, TeamInfoPanel infopanel, OnClickCallback callback)
    {
        this.btn = btn;
        this.infopanel = infopanel;
        this.callback = callback;
        EventTriggerListener.Get(btn.gameObject).onClick = OnBtnClick;
    }
    protected virtual void OnBtnClick(GameObject go, PointerEventData eventData)
    {
        callback(index);
    }
}