using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineMono : BulletMono
{
    public override void Init(int id, BulletVo vo)
    {
        base.Init(id, vo);
        body.constraints = RigidbodyConstraints.FreezeRotation;
        body.useGravity = false;
        Collider collider = body.GetComponent<Collider>();
        collider.isTrigger = true;
    }
    public override void SetRigiInfo(Entity entity)
    {
        Vector3 speed = entity.model.transform.forward.normalized * bulletVo.movespeed / 1000f;
        body.velocity = speed;
    }
    void OnTriggerEnter(Collider collisionInfo)
    {
        if (enterTriger != null)
        {
            enterTriger(collisionInfo);
        }
    }
    void OnTriggerStay(Collider collisionInfo)
    {
        if (stayTriger != null)
        {
            stayTriger(collisionInfo);
        }
    }
}
