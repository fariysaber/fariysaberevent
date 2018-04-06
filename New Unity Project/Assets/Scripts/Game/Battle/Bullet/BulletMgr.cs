using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletMgr
{
    public PlayerEntity entity;
    private List<Bullet> _bulletList;
    protected EffectMgr effectMgr;
    int index = 0;

    public BulletMgr(PlayerEntity entity)
    {
        this.entity = entity;
        _bulletList = new List<Bullet>();
        effectMgr = new EffectMgr();
    }
   
    public Effect GetEffect(string path, ResourceType resType, Vector3 localPos, Vector3 rotate, Vector3 scale, int layer, Transform parent, float time = -1,
        bool resetPause = true)
    {
        Effect effect = effectMgr.GetEffect(path);
        effect.InitEffect(path, resType, localPos, rotate, scale, layer, parent, time, parent);
        return effect;
    }
    public void AddTeampEffect(Effect effect)
    {
        effectMgr.Addtemp(effect);
    }
    public Bullet CreateBullet(int id,int skillId,int skillLevel,bool chufaBuff = true,object data = null)
    {
        Type getType = GameTools.GetType("Bullet_" + id);
        object getBullet = Activator.CreateInstance(getType);

        Globals.GetBulletBattleParam(id, ref getBullet);

        Bullet bullet = getBullet as Bullet;
        index++;
        bullet.Init(index, id, this, skillLevel, chufaBuff, data);
        _bulletList.Add(bullet);
        return bullet;
    }
    public virtual void OnUpdate(float dt)
    {
        for (int i = _bulletList.Count - 1; i >= 0; i--)
        {
            _bulletList[i].OnUpdate(dt);
        }
        effectMgr.OnUpdate(dt);
    }
    public void EndBulletByIndex(int index)
    {
        for (int i = _bulletList.Count - 1; i >= 0; i--)
        {
            if (_bulletList[i].index == index)
            {
                _bulletList[i].Destroy();
                _bulletList.RemoveAt(i);
                return;
            }
        }
    }
    public void DestroyAll()
    {
        for (int i = _bulletList.Count - 1; i >= 0; i--)
        {
            _bulletList[i].Destroy();
        }
        _bulletList.Clear();

        effectMgr.DestroyAll();
        effectMgr = null;
    }
}
