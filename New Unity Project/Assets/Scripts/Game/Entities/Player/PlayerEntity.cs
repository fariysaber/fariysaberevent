using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity:Entity
{
    public Locker skillLocker = new Locker();
    /// <summary>
    /// 是否死亡
    /// </summary>
    private bool isDead = false;
    public bool IsDead
    {
        set { isDead = value; }
        get { return isDead; }
    }
    public bool IsINGround()
    {
        if (model != null)
        {
            PlayerModel playerModel = model as PlayerModel;
            DownGround down = playerModel.actMoveCtrl.downGround;
            if (down.startSpeed >= 0 && down.isInGroud)
            {
                return true;
            }
        }
        return false;
    }
    public bool isLockPos()
    {
        if (model != null)
        {
            PlayerModel playerModel = model as PlayerModel;
            if (playerModel.posLocker.HasLocker())
            {
                return true;
            }
        }
        return false;
    }
    public string name;

    public override void InitData(EntityInitData data)
    {
        isDead = false;
        name = data.name;
        base.InitData(data);
        InitChildData(data);
    }
    protected virtual void InitChildData(EntityInitData data)
    {
    }
    protected override void StartLoadModel(EntityInitData data)
    {
        model = new PlayerModel();
        model.Entity = this;
        ModelInitData modelinitdata = GetModelInitData(data);
        model.InitBaseData(modelinitdata);
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        OnBattleUpdate(dt);
    }
    protected virtual void OnBattleUpdate(float dt)
    {

    }
    public override void Destroy()
    {
        base.Destroy();
        DestroyOther();
    }
    protected virtual void DestroyOther()
    {

    }
}