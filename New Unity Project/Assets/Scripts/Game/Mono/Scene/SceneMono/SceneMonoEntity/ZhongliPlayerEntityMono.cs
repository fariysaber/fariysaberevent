using System;
using System.Collections.Generic;
using UnityEngine;

public class ZhongliPlayerEntityMono : BaseSceMoEntity
{
    GameScene scene;
    public override void Init(GameScene scene)
    {
        base.Init(scene);
        this.scene = scene;
        
    }
    protected override void StartCreateEntity()
    {
        CreateBattleEnitity();
        CreateAiComponet();
    }
    
}
