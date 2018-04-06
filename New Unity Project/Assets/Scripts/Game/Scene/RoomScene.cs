using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScene : GameScene
{
    public RoomScene() : base()
    {
    }
    public override void InitScene<T>(object data = null)
    {
        base.InitScene<T>(data);
    }

    protected override void LoadSceneCallBack()
    {
        base.LoadSceneCallBack();
        UIMgr.GetInstance().OpenUI<RoomPanel>();
    }
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
    }

    public override void FixedUpdate(float dt)
    {
        base.FixedUpdate(dt);
    }

    public override void LateUpdate(float dt)
    {
        base.LateUpdate(dt);
    }
}
