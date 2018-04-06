using System;
using System.Collections.Generic;
using UnityEngine;
public enum DamageType
{
    cast = 1,
    box = 2,
    Sphere = 3,
    spherecast = 4,
    modelcollider = 5,
}
public enum MoveType
{
    freeze = 1,
    moveLine = 2,
}
public class Bullet
{
    public int index;
    public int bulletId;
    public BulletVo bulletVo;
    public Effect effect;
    public int level;
    public BulletMgr bulletMgr;
    public BulletMono bulletMono;

    public bool stop = false;
    public float attackOneTime;
    public float lastTime;
    public bool chufabuff = true;
    public Dictionary<int, int> attackNumList = new Dictionary<int, int>();
    public Dictionary<int, float> attackTimeList = new Dictionary<int, float>();
    public object data = null;
    public Bullet()
    {
        
    }
    public void Init(int index, int id, BulletMgr bulletmgr,int level,bool getchufaBuff,object data = null)
    {
        this.index = index;
        bulletId = id;
        bulletMgr = bulletmgr;
        this.level = level < 1 ? 1 : level;
        bulletVo = BulletConfig.GetData(bulletId + "");
        attackOneTime = bulletVo.movetime / 1000f / bulletVo.damagemaxnum;
        lastTime = 0f;
        chufabuff = getchufaBuff;
        this.data = data;
        CreateModel();
    }
    protected virtual void CreateModel()
    {
        if (bulletVo.model.Equals(""))
        {
            return;
        }
        int layer = GetBulletLayer();
        Vector3 pos = GetPos();
        Vector3 rotate = GetRotate();
        Vector3 scale = GetScale();
        Transform parent = GetParent();
        effect = bulletMgr.GetEffect(bulletVo.model, ResourceType.scene, pos, rotate, scale, layer, parent);
        effect.SetLoadCallback(LoadModelCallback);
    }
    protected virtual Transform GetParent()
    {
        bool follow = bulletVo.follow == 1 ? true : false;
        if (follow)
        {
            return bulletMgr.entity.model.GetEffectObjName();
        }
        return SceneMgr.GetInstance().nowGameScene.bulletRoot.transform;
    }
    protected virtual Vector3 GetPos()
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
        pos.y += (entity.model as PlayerModel).controller.height / 2f;
        if (data != null)
        {
            pos = (Vector3)data;
        }
        return pos;
    }
    protected virtual Vector3 GetRotate()
    {
        Vector3 rotate = Vector3.zero;
        if (bulletVo.follow <= 0)
        {
            rotate = bulletMgr.entity.model.transform.eulerAngles;
        }
        return rotate;
    }
    protected virtual Vector3 GetScale()
    {
        return Vector3.one;
    }
    protected virtual int GetBulletLayer()
    {
        PlayerEntity entity = bulletMgr.entity;
        switch (entity.EntityType)
        {
            case EntityTypes.BattlePlayerMy: return Globals.BattlePlayerMineBulletlayer; break;
            case EntityTypes.BattlePlayerMyTeam: return Globals.BattlePlayerMineBulletlayer; break;
            case EntityTypes.BattlePlayerFriend: return Globals.BattlePlayerMineBulletlayer; break;
            case EntityTypes.BattlePlayerDiRen: return Globals.BattlePlayerDirenBulletlayer; break;
            case EntityTypes.BattlePlayerZhongli: return Globals.BattlePlayerZhongliBulletlayer; break;
        }
        return Globals.normaleffectlayer;
    }
    protected virtual void LoadModelCallback()
    {
        switch (bulletVo.damagetype)
        {
            case (int)DamageType.box:
                CreateBox();
                break;
            case (int)DamageType.Sphere:
                CreateSphere();break;
        }
        SetColliderParam();
        switch (bulletVo.movetype)
        {
            case (int)MoveType.freeze:
                bulletMono = (BulletFreezeMono)TransformUtils.AddComponent(effect.effectObject, typeof(BulletFreezeMono).Name);
                break;
            case (int)MoveType.moveLine:
                bulletMono = (BulletLineMono)TransformUtils.AddComponent(effect.effectObject, typeof(BulletLineMono).Name);
                break;
            default: bulletMono = (BulletMono)TransformUtils.AddComponent(effect.effectObject, typeof(BulletMono).Name); break;
        }
        bulletMono.Init(bulletId, bulletVo);
        bulletMono.SetRigiInfo(bulletMgr.entity);
        bulletMono.enter = Enter;
        bulletMono.stay = Stay;
        bulletMono.exit = Exit;
        bulletMono.enterTriger = EnterTriger;
        bulletMono.stayTriger = StayTriger;
    }
    protected virtual void CreateBox()
    {
        TransformUtils.AddComponent(effect.effectObject, typeof(BoxCollider).Name);
    }
    protected virtual void CreateSphere()
    {
        TransformUtils.AddComponent(effect.effectObject, typeof(SphereCollider).Name);
    }
    protected virtual void SetColliderParam()
    {
        
    }
    protected virtual void Enter(Collision collisionInfo)
    {
        ContactPoint point = collisionInfo.contacts[0];
        Vector3 pos = point.point;
        Judge(collisionInfo.collider, pos,point.normal);
    }
    protected virtual void Stay(Collision collisionInfo)
    {
        ContactPoint point = collisionInfo.contacts[0];
        Vector3 pos = point.point;
        Judge(collisionInfo.collider, pos, point.normal);
    }
    protected virtual void Exit(Collision collisionInfo)
    {
    }
    protected virtual void EnterTriger(Collider collisionInfo)
    {
        Vector3 pos = collisionInfo.transform.position;
        pos.y = effect.effectObject.transform.position.y;
        Judge(collisionInfo, pos, Vector3.up);
    }
    protected virtual void StayTriger(Collider collisionInfo)
    {
        Vector3 pos = collisionInfo.transform.position;
        pos.y = effect.effectObject.transform.position.y;
        Judge(collisionInfo, pos, Vector3.up);
    }

    protected virtual void Judge(Collider collisionInfo,Vector3 pos,Vector3 normal)
    {
        GameObject go = collisionInfo.gameObject;
        int id = go.GetInstanceID();
        if (attackNumList.ContainsKey(id) == false && attackNumList.Count <= bulletVo.damagemaxplayer)
        {
            attackNumList[id] = 1;
            attackTimeList[id] = 0f;
        }
        else
        {
            if (attackNumList[id] >= bulletVo.damageplayernum)
            {
                return;
            }
            if (attackTimeList[id] < attackOneTime)
            {
                return;
            }
            attackNumList[id]++;
            attackTimeList[id] = 0f;
        }
        Calculate(collisionInfo, pos, normal);
    }

    protected virtual void Calculate(Collider collisionInfo, Vector3 pos,Vector3 normal)
    {
        CalculateEffect(collisionInfo, pos, normal);
        CalculateDamage(collisionInfo);
    }
    //判断特效
    protected virtual void CalculateEffect(Collider collisionInfo,Vector3 pos, Vector3 normal)
    {
        string damageEffect = bulletVo.damageeffect;
        BattlePlayerEntity batEntity = bulletMgr.entity as BattlePlayerEntity;
        if (bulletVo.iswuqieffect == 1 && batEntity.attributeMgr.gameEntitydata.wuqi != null && batEntity.attributeMgr.gameEntitydata.wuqi.vo.dmgeffect != "empty")
        {
            damageEffect = batEntity.attributeMgr.gameEntitydata.wuqi.vo.dmgeffect;
        }
        Effect effect = bulletMgr.GetEffect(damageEffect, ResourceType.scene, pos, normal, Vector3.one, Globals.normaleffectlayer,
            SceneMgr.GetInstance().nowGameScene.effectRoot.transform, 1f);
    }
    //计算伤害
    protected virtual void CalculateDamage(Collider collisionInfo)
    {
        if (EntityMgr.GetInstance().IsMinType(bulletMgr.entity.EntityType))
        {
            JudgeDamage(collisionInfo);
        }
        else
        {
            JudgeDamage(collisionInfo);//其他先写一样的
        }
    }
    protected virtual void JudgeDamage(Collider collisionInfo)
    {
        GameObject hitobj = collisionInfo.gameObject;
        //暂时只算战斗单位的数值，后面添加植物类型
        BattlePlayerEntity hitbatEntity = SceneMgr.GetInstance().nowGameScene.GetBattleEntity(hitobj);
        if (hitbatEntity == null)
        {
            return;
        }
        BattlePlayerEntity batEntity = bulletMgr.entity as BattlePlayerEntity;
        float dmg = GetDamage(batEntity, hitbatEntity);

        //hitbatEntity.SetHitDmg(batEntity, (int)dmg);
    }
    //计算伤害
    protected virtual float GetDamage(BattlePlayerEntity batEntity, BattlePlayerEntity hitbatEntity)
    {
        //子弹攻击伤害
        float finalAtk = batEntity.attributeMgr.GetFinalAtk();
        float finalDef = batEntity.attributeMgr.GetFinalDef();
        float finalHp = batEntity.attributeMgr.GetFinalHp();
        float baoji = batEntity.attributeMgr.GetFinalBaoji();
        float baoshang = batEntity.attributeMgr.GetFinalBaoShang();
        float chuantou = batEntity.attributeMgr.GetChuantou();
        float finalShang = finalAtk * bulletVo.atkdmg + finalDef * bulletVo.defenddmg +
            finalHp * bulletVo.hpdmg + baoji * bulletVo.baojidmg + baoshang * bulletVo.baoshangdmg;

        if(level > 0)
        {
            finalShang = finalShang * (1 + (level - 1) * 0.05f);
        }
        finalShang = finalShang / 1000f;

        //对方防御比例
        float hitDef = hitbatEntity.attributeMgr.GetFinalDef();
        hitDef = (1 - chuantou) * hitDef;
        hitDef = hitDef > 0 ? hitDef : 0;
        float defbili =  1 - hitDef / (hitDef + Globals.defendBili);

        //算出比例伤害
        finalShang = finalShang * defbili;
        //算出增伤
        finalShang = finalShang * (1 + batEntity.attributeMgr.GetFinalAddDmg());
        //算出减伤
        finalShang = finalShang * (1 - hitbatEntity.attributeMgr.GetFinalReduDmg());
        finalShang = finalShang > 0 ? finalShang : 0;
        //算出是否暴击
        float random = UnityEngine.Random.Range(0,1000f);
        random /= 1000f;
        bool isBaoji = random > baoji ? false : true;
        if (isBaoji)
        {
            finalShang = finalShang * 2 * (1 + baoshang);
        }
        if (chufabuff)
        {
            batEntity.buffMgr.SetShanghai(this, finalShang, isBaoji, hitbatEntity);
        }
        hitbatEntity.attributeMgr.SetShanghai(this, finalShang, isBaoji);

        hitbatEntity.SetHitDmg(batEntity, (int)finalShang, isBaoji);

        return finalShang;
    }


    //每帧执行
    public virtual void OnUpdate(float dt)
    {
        if (stop)
        {
            return;
        }
        if (bulletMono != null)
        {
            bulletMono.OnUpdate(dt);
        }
        if (attackTimeList.Count > 0)
        {
            List<int> keys = new List<int>();
            foreach (var item in attackTimeList)
            {
                keys.Add(item.Key);
            }
            for (int i = keys.Count - 1; i >= 0; i--)
            {
                attackTimeList[keys[i]] += dt;
            }
        }
        if (lastTime < bulletVo.movetime / 1000f && bulletVo.movetime > 0)
        {
            lastTime += dt;
            if (lastTime >= bulletVo.movetime / 1000f)
            {
                Stop();
            }
        }
    }

    public virtual void Stop()
    {
        stop = true;
        bulletMgr.EndBulletByIndex(index);
    }

    public virtual void Destroy()
    {
        if (effect != null)
        {
            bulletMgr.AddTeampEffect(effect);
        }
    }
}
