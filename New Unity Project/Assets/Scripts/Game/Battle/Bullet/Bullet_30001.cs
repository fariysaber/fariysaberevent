using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_30001 : Bullet
{
    public float length;
    protected override void SetColliderParam()
    {
        base.SetColliderParam();
        BoxCollider collider = effect.effectObject.GetComponent<BoxCollider>();
        collider.size = new Vector3(1f, 0.81f, 0.9f);
        collider.center = new Vector3(0, -0.1f, 1f);
    }
}
