using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LianxiBattleScene : BattleScene
{
    public LianxiBattleScene()
        : base()
    {
    }

    protected override void LoadSceneCallBack()
    {
        base.LoadSceneCallBack();
        GameObject cameraObject = GameObject.Find(Globals.MainCameraName);
        cameraMgr.SwichCamera<ThirdCamera>(cameraObject,new ThirdInitData());
        InputMgr.GetInstance().SwichScene<BattleController>();
        engine = new LianxiBattleEngine();
        engine.Init<LianxiBattleScene>(this);

        EventDispatcher.AddEventListener<Vector3>(GlobalEvents.MAINHERO_MOVE, SetMainMove);
        EventDispatcher.AddEventListener(GlobalEvents.MAINHERO_JUMP, SetMainJump);
        EventDispatcher.AddEventListener(GlobalEvents.MAINHERO_USESKILL1, UseSkill1);
    }

    public void SetMainMove(Vector3 step)
    {
        if (mainPlayerEntity != null)
        {
            if (cameraMgr.maincamera != null)
            {
                step = cameraMgr.maincamera.GetResetPos(step);
            }
            (mainPlayerEntity.model as PlayerModel).SetJoyDir(step.normalized);
        }
    }
    /// <summary>
    /// 测试用
    /// </summary>
    public void SetMainJump()
    {
        if (mainPlayerEntity != null)
        {
            mainPlayerEntity.skillMgr.UseSkill(10003);
        }
    }
    /// <summary>
    /// 测试用
    /// </summary>
    public void UseSkill1()
    {
        if (mainPlayerEntity != null)
        {
            mainPlayerEntity.skillMgr.UseSkill(10002);
        }
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
        EventDispatcher.RemoveEventListener<Vector3>(GlobalEvents.MAINHERO_MOVE, SetMainMove);
        EventDispatcher.RemoveEventListener(GlobalEvents.MAINHERO_JUMP, SetMainJump);

        EventDispatcher.RemoveEventListener(GlobalEvents.MAINHERO_USESKILL1, UseSkill1);
    }
}
