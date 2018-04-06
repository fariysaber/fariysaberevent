using System;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerEntityMono : BaseSceMoEntity
{
    GameScene scene;
    public override void Init(GameScene scene)
    {
        base.Init(scene);
        this.scene = scene;
        EventDispatcher.AddEventListener<Vector3>(GlobalEvents.MAINHERO_MOVE, SetMainMove);
        EventDispatcher.AddEventListener(GlobalEvents.MAINHERO_JUMP, SetMainJump);
        EventDispatcher.AddEventListener(GlobalEvents.MAINHERO_USESKILL1, UseSkill1);
        EventDispatcher.AddEventListener(GlobalEvents.MAINHERO_USESKILL2, UseSkill2);
        EventDispatcher.AddEventListener(GlobalEvents.MAINHERO_USESKILL3, UseSkill3);
        EventDispatcher.AddEventListener(GlobalEvents.RESET_MAINCAMERAL, SetTarget);
    }
    protected override void StartCreateEntity()
    {
        EntityInitData data = null;
        if (entityType == EntityTypes.BattlePlayerMy)
        {
            data = Globals.player1.GetBattleMainEntityInitData();
        }
        data.entityType = EntityTypes.BattlePlayerMy;
        data.initPos = GetPos();
        GameScene scene = SceneMgr.GetInstance().nowGameScene;
        data.parentObject = scene.entityRoot;

        entity = EntityMgr.GetInstance().CreateEntity<BattlePlayerEntity>(data);
        scene.AddEntity(entity.UID, entity);
        SetTarget();
    }
    private void SetTarget()
    {
        if (entityType == EntityTypes.BattlePlayerMy)
        {
            BattleScene scene = SceneMgr.GetInstance().nowGameScene as BattleScene;
            scene.cameraMgr.maincamera.SetPosition(entity.model.Pos);
            scene.SetMainPlayerEntity(battlePlayerEntity);
            if (entity != null)
            {
                if (entity.model.transform != null)
                {
                    scene.cameraMgr.maincamera.SetTarget(entity.model.transform);
                }
            }
        }
    }
    public void SetMainMove(Vector3 step)
    {
        if (entityType != EntityTypes.BattlePlayerMy)
        {
            return;
        }
        if (entity != null)
        {
            if (scene.cameraMgr.maincamera != null)
            {
                step = scene.cameraMgr.maincamera.GetResetPos(step);
            }
            (entity.model as PlayerModel).SetJoyDir(step.normalized);
        }
    }
    /// <summary>
    /// 测试用
    /// </summary>
    public void SetMainJump()
    {
        if (entityType != EntityTypes.BattlePlayerMy)
        {
            return;
        }
        if (entity != null)
        {
            battlePlayerEntity.skillMgr.UseSkill(70001);
        }
    }
    /// <summary>
    /// 测试用
    /// </summary>
    public void UseSkill1()
    {
        if (entityType != EntityTypes.BattlePlayerMy)
        {
            return;
        }
        if (entity != null)
        {
            battlePlayerEntity.skillMgr.UseSkill(10001);
        }
    }
    /// <summary>
    /// 测试用
    /// </summary>
    public void UseSkill2()
    {
        if (entityType != EntityTypes.BattlePlayerMy)
        {
            return;
        }
        if (entity != null)
        {
            battlePlayerEntity.skillMgr.UseSkill(10002);
        }
    }
    /// <summary>
    /// 测试用
    /// </summary>
    public void UseSkill3()
    {
        if (entityType != EntityTypes.BattlePlayerMy)
        {
            return;
        }
        if (entity != null)
        {
            battlePlayerEntity.skillMgr.UseSkill(10003);
        }
    }
    public override void DestroyAll()
    {
        base.DestroyAll();
        EventDispatcher.RemoveEventListener<Vector3>(GlobalEvents.MAINHERO_MOVE, SetMainMove);
        EventDispatcher.RemoveEventListener(GlobalEvents.MAINHERO_JUMP, SetMainJump);

        EventDispatcher.RemoveEventListener(GlobalEvents.MAINHERO_USESKILL1, UseSkill1);
        EventDispatcher.RemoveEventListener(GlobalEvents.MAINHERO_USESKILL2, UseSkill2);
        EventDispatcher.RemoveEventListener(GlobalEvents.MAINHERO_USESKILL3, UseSkill3);
        EventDispatcher.RemoveEventListener(GlobalEvents.RESET_MAINCAMERAL, SetTarget);
    }
}
