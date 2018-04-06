using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBattleScene : BattleScene
{
    public BigBattleScene()
        : base()
    {
    }

    protected override void LoadSceneCallBack()
    {
        base.LoadSceneCallBack();
        //大世界用第三人称摄像头视角
        GameObject cameraObject = GameObject.Find(Globals.MainCameraName);
        cameraMgr.SwichCamera<ThirdCamera>(cameraObject, new ThirdInitData());
        InputMgr.GetInstance().SwichScene<BattleController>();
        //初始化大世界战斗引擎，用于处理整个战斗逻辑
        engine = new BigBattleEngine();
        engine.Init<BigBattleScene>(this);
    }

    public override void BattleUpdate(float dt)
    {
        base.BattleUpdate(dt);
    }

    public override void BattleFixedUpdate(float dt)
    {
        base.BattleFixedUpdate(dt);
    }

    public override void BattleLateUpdate(float dt)
    {
        base.BattleLateUpdate(dt);
    }

    public override void DestroyOther()
    {
        base.DestroyOther();
    }
}
