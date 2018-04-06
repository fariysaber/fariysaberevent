using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletFreezeMono : BulletMono
{
    public override void Init(int id, BulletVo vo)
    {
        base.Init(id, vo);
        body.constraints = RigidbodyConstraints.FreezeAll;
    }
}
