using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_10001 : Bullet
{
    public float length;
    protected override void SetColliderParam()
    {
        base.SetColliderParam();
        BoxCollider collider = effect.effectObject.GetComponent<BoxCollider>();
        collider.size = new Vector3(1.2f, 1, 1.5f);
        collider.center = new Vector3(0, 0, 0.75f);
    }
}
