using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletMono : MonoBehaviour
{
    public int bulletId;
    public BulletVo bulletVo;
    public Rigidbody body;
    public virtual void Init(int id,BulletVo vo)
    {
        bulletId = id;
        bulletVo = vo;
        body = (Rigidbody)TransformUtils.AddComponent(gameObject, typeof(Rigidbody).Name);
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
    public virtual void SetRigiInfo(Entity entity)
    {
    }
    public virtual void OnUpdate(float dt)
    {
    }

    public delegate void OnEnter(Collision collisionInfo);
    public OnEnter enter;

    public delegate void OnStay(Collision collisionInfo);
    public OnStay stay;

    public delegate void OnExit(Collision collisionInfo);
    public OnExit exit;

    public delegate void OnEnterTriger(Collider collisionInfo);
    public OnEnterTriger enterTriger;

    public delegate void OnStayTriger(Collider collisionInfo);
    public OnStayTriger stayTriger;

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (enter != null)
        {
            enter(collisionInfo);
        }
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        if (stay != null)
        {
            stay(collisionInfo);
        }
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        if (exit != null)
        {
            exit(collisionInfo);
        }
    }
}
