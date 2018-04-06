using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public BaseModel model;
    private string _uid;
    public string UID
    {
        set { _uid = value; }
        get { return _uid; }
    }

    private string _entityId;
    public string EtityId
    {
        set { _entityId = value; }
        get { return _entityId; }
    }

    private EntityTypes _entityType = EntityTypes.Default;
    public EntityTypes EntityType
    {
        set { _entityType = value; }
        get { return _entityType; }
    }

    /// <summary>
    /// 是否消除，不存在了，下一帧清除掉
    /// </summary>
    private bool _isDispose = false;
    public bool IsDispose
    {
        set { _isDispose = value; }
        get { return _isDispose; }
    }

    /// <summary>
    /// 是否可见
    /// </summary>
    private bool _isVisible = true;
    public bool IsVisible
    {
        set { _isVisible = value; }
        get { return _isVisible; }
    }

    public virtual void InitData(EntityInitData data)
    {
        _uid = data.uid;
        _entityId = data.entityId;
        _entityType = data.entityType;
        _isDispose = false;
        _isVisible = true;
        StartLoadModel(data);
    }

    protected virtual void StartLoadModel(EntityInitData data)
    {
        model = new BaseModel();
        ModelInitData modelinitdata = GetModelInitData(data);
        model.InitBaseData(modelinitdata);
    }

    protected virtual ModelInitData GetModelInitData(EntityInitData data)
    {
        ModelInitData modelInitdata = new ModelInitData();
        modelInitdata.pos = data.initPos;
        modelInitdata.modelName = Globals.entityName + data.uid;
        modelInitdata.layer = EntityMgr.GetInstance().GetLayerByType(data.entityType);
        modelInitdata.entity = this;
        modelInitdata.scale = data.initScale;
        modelInitdata.parent = SceneMgr.GetInstance().nowGameScene.entityRoot;
        modelInitdata.modelData = new ModelData();
        modelInitdata.modelData.SetPath(GetPathByEnitity(data.entityId));
        modelInitdata.moveInfo = data.moveInfo;
        return modelInitdata;
    }

    protected virtual string GetPathByEnitity(string entityIdStr,int pifu = 0)
    {
        HeroVo herovo = HeroConfig.GetData(entityIdStr);
        string path = CharactorConfig.GetData(herovo.charactor[pifu] + "").batmodel;
        return path;
    }

    public virtual void OnUpdate(float dt)
    {
        if (model != null)
        {
            model.OnUpdate(dt);
        }
    }
    public virtual void FixedUpdate(float dt)
    {   
    }
    public virtual void LateUpdate(float dt)
    {
        if (model != null)
        {
            model.LateUpdate(dt);
        }
    }
    public virtual void Destroy()
    {
        if (model != null)
        {
            model.DestroyModel();
        }
        IsDispose = true;
    }
}
public class EntityInitData
{
    public string uid;
    public string name;
    public EntityTypes entityType;
    public string entityId;
    public Vector3 initPos;
    public Vector3 initScale = Vector3.one;
    public GameObject parentObject;
    public MoveInfo moveInfo;
    public List<BattleSkillBaseInfo> skillBaseInfo;
    public GameEntityData gameEntityData;
}
public class MoveInfo
{
    public float baseSpeed = 2;
    public float baseSpeedPercent = 1;
    public float addSpeed = 0;
    public float addSpeedPercent = 0;
    public float maxAnimSpeed = 3f;
}