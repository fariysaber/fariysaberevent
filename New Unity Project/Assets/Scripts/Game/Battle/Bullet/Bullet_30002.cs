using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_30002 : Bullet
{
    public float length;
    protected override void SetColliderParam()
    {
        base.SetColliderParam();

    }
    protected override Transform GetParent()
    {
        return bulletMgr.entity.model.GetObjByName("zuiba");
    }
    protected override Vector3 GetPos()
    {
        return Vector3.zero;
    }
}
