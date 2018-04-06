using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectHeroPanel : BasePanel
{
    UITransform back;
    UITransform backImage;
    RttPlayerModel model;

    List<HeroVo> heroVoList;
    HeroInfoPanel heroinfopanel;

    UIText nametext;
    int index = 0;

    InputField inputField;

    UIButton startBtn;
    protected override void InitData()
    {
        base.InitData();
    }
    protected override void LoadCallback()
    {
        back = new UITransform(m_Transform, "back");

        UITransform heroRoot = new UITransform(back.transform, "heroRoot");

        nametext = new UIText(back.transform, "heroname");

        model = new RttPlayerModel("RoomHero", new Vector3(300, 600, 10), heroRoot.transform, new Vector3(0, 0, 0),
           new Vector2(300, 600));

        heroinfopanel = new HeroInfoPanel(back.gameObject, LoadHeroInfoCallback, "heroinfopanel");
        heroinfopanel.LoadResource();

        startBtn = new UIButton(back.transform, "startBtn");
        EventTriggerListener.Get(startBtn.gameObject).onClick = OnStartBtn;

        inputField = back.transform.Find("InputField").GetComponent<InputField>();
    }
    protected void LoadHeroInfoCallback()
    {
        heroinfopanel.root.rectTransform.anchorMin = new Vector2(1, 0);
        heroinfopanel.root.rectTransform.anchorMax = new Vector2(1, 1);
        heroinfopanel.root.rectTransform.offsetMax = new Vector2(0, 0);
        heroinfopanel.root.rectTransform.offsetMin = new Vector2(-350, 0);
    }

    private void OnStartBtn(GameObject go, PointerEventData eventData)
    {
        string name = inputField.text;
        if (name.Equals(""))
        {
            return;
        }
        Globals.player1.key = 0 + "";
        Globals.player1.name = name;
        Globals.player1.AddNewPlayer(heroVoList[index].id, true);
        SceneMgr.GetInstance().SwichScene<RoomScene>();
    }

    public override void Refresh()
    {
        base.Refresh();
        RefreshInfo();
    }
    private void RefreshInfo()
    {
        heroVoList = SelectHeroLogic.GetInstance().GetInitHero();
        nametext.text = heroVoList[index].name;
        heroinfopanel.SetInfo(heroVoList[index]);
        SetRttModel();
    }
    private void SetRttModel()
    {
        HeroVo herovo = heroVoList[index];
        string modelName = CharactorConfig.GetData(herovo.charactor[0] + "").model;
        model.LoadModel(modelName, new Vector3(0, -0.75f, 1.7f), new Vector3(0, 180, 0));
    }
    public override void Destroy(bool once = false)
    {
        base.Destroy(once);
        heroinfopanel.Destroy();
    }
}
