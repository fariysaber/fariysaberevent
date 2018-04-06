using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_10006 : Bullet
{
    public float length;
    protected override void SetColliderParam()
    {
        base.SetColliderParam();
        SphereCollider collider = effect.effectObject.GetComponent<SphereCollider>();
        collider.radius = 1.3f;
    }
    //判断特效
    protected override void CalculateEffect(Collider collisionInfo, Vector3 pos, Vector3 normal)
    {
        if (!Globals.IsBattlePlayer(collisionInfo.gameObject.layer))
        {
            return;
        }
        Effect effect = bulletMgr.GetEffect(bulletVo.damageeffect, ResourceType.scene, pos, normal, Vector3.one, Globals.normaleffectlayer,
            SceneMgr.GetInstance().nowGameScene.effectRoot.transform, 1f);
    }

    protected override Vector3 GetPos()
    {
        PlayerEntity entity = bulletMgr.entity;
        Vector3 pos;
        if (bulletVo.follow == 1)
        {
            pos = new Vector3();
        }
        else
        {
            pos = entity.model.Pos;
        }
        return pos;
    }
}
