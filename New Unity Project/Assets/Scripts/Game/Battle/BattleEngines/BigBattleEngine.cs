using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//初始化大世界战斗引擎，用于处理整个战斗逻辑
public class BigBattleEngine : BattleEngine
{
    public BigBattleScene scene;

    protected override void InitBaseData(GameScene scene, string sceneName, BaseSceneMonoD sceneMonoD, List<BaseScrMoEntityD> scMonEntityD)
    {
        base.InitBaseData(scene, sceneName, sceneMonoD, scMonEntityD);
        this.scene = scene as BigBattleScene;
    }
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
    }

    public override void DestroyEngine()
    {
        base.DestroyEngine();
    }
}
