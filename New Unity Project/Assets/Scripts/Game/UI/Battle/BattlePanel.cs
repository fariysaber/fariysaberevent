using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattlePanel : BasePanel
{
    UITransform heroiconList;
    HeroHeadBatIcon herobaticonPanel;
    protected override void InitData()
    {
        base.InitData();
    }
    protected override void LoadCallback()
    {
        heroiconList = new UITransform(Root.transform, "heroiconList");
        herobaticonPanel = new HeroHeadBatIcon(heroiconList.gameObject, null, "heroicon");
        herobaticonPanel.LoadResource();
        EventDispatcher.AddEventListener(GlobalEvents.BATTLE_HEROICON, RefreshHeroIcon);
        EventDispatcher.AddEventListener(GlobalEvents.BATTLE_HEROBUFF, RefreshBuff);
    }
    public void RefreshHeroIcon()
    {
        BattleScene scene = SceneMgr.GetInstance().nowGameScene as BattleScene;
        if (scene.mainPlayerEntity != null)
        {
            herobaticonPanel.SetHeadInfo(scene.mainPlayerEntity.attributeMgr.gameEntitydata.herovo,
                scene.mainPlayerEntity.attributeMgr.gameEntitydata.pifu);
        }
        RefreshBuff();
    }
    public void RefreshBuff()
    {
        BattleScene scene = SceneMgr.GetInstance().nowGameScene as BattleScene;
        if (scene.mainPlayerEntity != null)
        {
            List<int> getBufIds = scene.mainPlayerEntity.buffMgr.GetBuffIds();
            herobaticonPanel.RefreshbuffList(getBufIds);
        }
    }
    public override void Refresh()
    {
        base.Refresh();
        RefreshHeroIcon();
    }
    protected override void RemoveAction()
    {
        base.RemoveAction();
        EventDispatcher.RemoveEventListener(GlobalEvents.BATTLE_HEROICON, RefreshHeroIcon);
        EventDispatcher.RemoveEventListener(GlobalEvents.BATTLE_HEROBUFF, RefreshBuff);
    }
}
