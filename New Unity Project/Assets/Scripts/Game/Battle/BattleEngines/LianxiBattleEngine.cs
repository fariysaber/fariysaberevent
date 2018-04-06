using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LianxiBattleEngine : BattleEngine
{
    public LianxiBattleScene scene;
    public float startCd;
    public float endCd;
    //public SceneBaseNode mainNode;
    //public List<SceneBaseNode> otherNode;

    /*protected override void InitBaseData(GameScene scene)
    {
        base.InitBaseData(scene, gameTransform);
        this.scene = scene as LianxiBattleScene;

        FreeBattle battle = gameTransform.GetComponent<FreeBattle>();
        startCd = battle.startCd;
        endCd = battle.endCd;

        GameObject mainObject = battle.mainNode;
        mainNode = mainObject.GetComponent<SceneBaseNode>();
        otherNode = new List<SceneBaseNode>();
        for (int i = 0; i < battle.monsterNode.Count; i++)
        {
            otherNode.Add(battle.monsterNode[i].GetComponent<SceneBaseNode>());
        }
    }*/
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        DoMainAction(dt);
    }

    #region 主角相关
    private void DoMainAction(float dt)
    {
        /*if (scene.mainPlayerEntity == null)
        {
            mainNode.startRefreshCdTime += dt;
            if (mainNode.startRefreshCdTime > mainNode.startRefreshCd)
            {
                EntityInitData data = Globals.player1.GetBattleMainEntityInitData();
                data.entityType = EntityTypes.BattlePlayerMy;
                data.initPos = mainNode.GetPos();
                data.parentObject = scene.entityRoot;
                BattlePlayerEntity entity = EntityMgr.GetInstance().CreateEntity<BattlePlayerEntity>(data) as BattlePlayerEntity;
                scene.AddEntity(entity.UID,entity);
                scene.cameraMgr.maincamera.SetPosition(entity.model.Pos);

                scene.SetMainPlayerEntity(entity);
            }
        }
        if (scene.mainPlayerEntity != null)
        {
            if (scene.mainPlayerEntity.model.transform != null)
            {
                scene.cameraMgr.maincamera.SetTarget(scene.mainPlayerEntity.model.transform);
            }
        }*/
    }

    #endregion

    public override void DestroyEngine()
    {
        base.DestroyEngine();
    }
}
