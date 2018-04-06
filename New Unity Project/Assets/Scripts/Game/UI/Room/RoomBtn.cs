using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomBtn
{
    GameObject go;
    public RoomBtn(GameObject obj)
    {
        go = obj;
        GameObject btnobj = obj.transform.Find("btn").gameObject;
        EventTriggerListener.Get(btnobj).onClick = OnBtnClick;
    }
    protected virtual void OnBtnClick(GameObject go, PointerEventData eventData)
    {
        
    }
}
public class TeamBtn : RoomBtn
{
    public TeamBtn(GameObject obj)
        : base(obj)
    { 
    }
    protected override void OnBtnClick(GameObject go, PointerEventData eventData)
    {
        UIMgr.GetInstance().OpenUI<TeamPanel>();
    }
}
public class LianxiBtn : RoomBtn
{
    public LianxiBtn(GameObject obj) : base(obj)
    { 
    }
    protected override void OnBtnClick(GameObject go, PointerEventData eventData)
    {
        SceneMgr.GetInstance().SwichScene<BigBattleScene>(1001);
    }
}