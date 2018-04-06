using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : GameScene
{
    protected BattleEngine engine;
    public BattlePlayerEntity mainPlayerEntity;
    public BattleScene()
        : base()
    {
    }
    public void SetMainPlayerEntity(BattlePlayerEntity mainentity)
    {
        mainPlayerEntity = mainentity;
        EventDispatcher.TriggerEvent(GlobalEvents.BATTLE_HEROICON);
    }

    protected override void LoadSceneCallBack()
    {
        base.LoadSceneCallBack();
        UIMgr.GetInstance().OpenUI<BattlePanel>();
    }

    public override void InitScene<T>(object data = null)
    {
        base.InitScene<T>(data);
        if (data != null)
        {
            sceneInfoId = (int)data;
        }
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        BattleUpdate(dt);
        if (engine != null)
        {
            engine.OnUpdate(dt);
        }
    }

    public virtual void BattleUpdate(float dt)
    {

    }

    public override void FixedUpdate(float dt)
    {
        base.FixedUpdate(dt);
        BattleFixedUpdate(dt);
        if (engine != null)
        {
            engine.FixedUpdate(dt);
        }
    }

    public virtual void BattleFixedUpdate(float dt)
    {

    }

    public override void LateUpdate(float dt)
    {
        base.LateUpdate(dt);
        BattleLateUpdate(dt);
        if (engine != null)
        {
            engine.LateUpdate(dt);
        }
    }

    public virtual void BattleLateUpdate(float dt)
    {

    }

    public override void DestroyScene()
    {
        base.DestroyScene();
        DestroyOther();
        engine.DestroyEngine();
    }
    public virtual void DestroyOther()
    { 
    }
}
