using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BaseSceMoEntity : MonoBehaviour
{

    #region 激活相关
    [SerializeField, Tooltip("是否已激活")]
    public bool isActive = false;
    [SerializeField, Tooltip("激活所需")]
    public List<ActiveRequireType> activerequiretype;
    [SerializeField, Tooltip("第一次激活等待cd")]
    public float startRefreshCd = 0f;//第一次刷新等待cd
    public float startRefreshCdTime = 0f;
    protected virtual bool IsRequireActiveType()//是否满足刷新条件
    {
        return true;
    }
    protected virtual void UpdateActive(float dt)
    {
        if (isActive)
        {
            return;
        }
        if (IsRequireActiveType())
        {
            startRefreshCdTime += dt;
            if (startRefreshCdTime >= startRefreshCd)
            {
                isActive = true;
                startRefreshCdTime = 0f;
            }
        }
    }
    #endregion

    #region 激活后出生相关
    [SerializeField, Tooltip("出生位置类型")]
    public BornRangeType bornRangeType;
    [SerializeField, Tooltip("出生为box类型的范围")]
    public List<float> boxRange;
    [SerializeField, Tooltip("出生为circle类型的范围")]
    public float circleRange;

    [SerializeField, Tooltip("总共刷新次数（大于10000为无限）")]
    public int refreshNum = 1;//总共刷新次数

    [SerializeField, Tooltip("消除后刷新cd")]
    public float resetCd = -1;//消除后刷新cd
    public float resetCdTime = 0f;
    public bool isLife = false;//是否生存

    [SerializeField, Tooltip("刷新类型，一个，顺序，随机")]
    public RefreshNodeType refreshNodeType;
    [SerializeField, Tooltip("id（10000为角色）（20000为怪物）")]
    public List<int> id;
    [SerializeField, Tooltip("当前的id")]
    public int selectId = -1;
    [SerializeField, Tooltip("entity类型")]
    public EntityTypes entityType;
     [SerializeField, Tooltip("当前的位置")]
    public List<float> nowPos;
    [SerializeField, Tooltip("entity等级")]
    public int entitylevel = 1;

    [SerializeField, Tooltip("ai寻路位点")]
    public List<GameObject> xunluPosObj;

    public int aiActionId = 1001;

    //进入场景保存的私有数据
    protected bool isCreateEntity;
    public Entity entity;
    public BattlePlayerEntity battlePlayerEntity
    {
        get { return entity as BattlePlayerEntity; }
    }
    protected BehaviorNode aiactionNode;

    protected virtual void UpdateLife(float dt)
    {
        if (isLife)
        {
            if (isCreateEntity == false)
            {
                CreateEntity();
            }
            return;
        }
        if (refreshNum <= 0)
        {
            return;
        }
        resetCdTime += dt;
        if (resetCdTime >= resetCd)
        {
            CreateEntity();
            if (refreshNum < 10000)
            {
                refreshNum--;
            }
        }
    }
    protected void CreateEntity()
    {
        StartCreateEntity();
        isLife = true;
        resetCdTime = 0;
        isCreateEntity = true;
    }
    protected virtual void StartCreateEntity()
    {

    }
    #endregion
    public virtual void Init(GameScene scene)
    {

    }
    public virtual void OnUpdate(float dt)
    {
        UpdateActive(dt);
        if (!isActive)//没激活不执行
        {
            return;
        }
        UpdateLife(dt);
        UpdateAiAction(dt);
    }
    public virtual void DestroyAll()
    {

    }

    public virtual void LoadInfo(BaseScrMoEntityD basescrmonoentityD)
    {
        isActive = basescrmonoentityD.isActive;
        activerequiretype = basescrmonoentityD.activerequiretype;
        startRefreshCd = basescrmonoentityD.startRefreshCd;
        startRefreshCdTime = basescrmonoentityD.startRefreshCdTime;
        bornRangeType = basescrmonoentityD.bornRangeType;
        boxRange = basescrmonoentityD.boxRange;
        circleRange = basescrmonoentityD.circleRange;
        refreshNum = basescrmonoentityD.refreshNum;
        resetCd = basescrmonoentityD.resetCd;
        resetCdTime = basescrmonoentityD.resetCdTime;
        isLife = basescrmonoentityD.isLife;
        refreshNodeType = basescrmonoentityD.refreshNodeType;
        id = basescrmonoentityD.id;
        selectId = basescrmonoentityD.selectId;
        entityType = basescrmonoentityD.entityType;
        nowPos = basescrmonoentityD.nowPos;
        xunluPosObj = new List<GameObject>();
        for (int i = 0; i < basescrmonoentityD.xunluPosx.Count; i++)
        {
            GameObject xunludian = new GameObject("xunluo" + i);
            TransformUtils.SetParent(xunludian.transform, transform);
            xunludian.transform.localPosition = new Vector3(basescrmonoentityD.xunluPosx[i], 0, basescrmonoentityD.xunluPosz[i]);
            xunluPosObj.Add(xunludian);
        }
        aiActionId = basescrmonoentityD.aiActionId;
        entitylevel = basescrmonoentityD.entitylevel;

        transform.position = new Vector3(basescrmonoentityD.activePos[0],
            basescrmonoentityD.activePos[1], basescrmonoentityD.activePos[2]);
    }

    public Vector3 GetPos(BornRangeType getType = BornRangeType.none)
    {
        if (getType == BornRangeType.none)
        {
            getType = bornRangeType;
        }
        if (getType == BornRangeType.pos)
        {
            return gameObject.transform.position;
        }
        if (getType == BornRangeType.box)
        {
            Vector3 pos = gameObject.transform.position;
            pos.x = pos.x + UnityEngine.Random.Range(-boxRange[0], boxRange[0]);
            pos.y = pos.y + UnityEngine.Random.Range(-boxRange[1], boxRange[1]);
            pos.z = pos.z + UnityEngine.Random.Range(-boxRange[2], boxRange[2]);
            return pos;
        }
        if (getType == BornRangeType.circle)
        {
            float posx = UnityEngine.Random.Range(-circleRange, circleRange);
            float posz = (float)System.Math.Sqrt(circleRange * circleRange - posx * posx);
            int mark = UnityEngine.Random.Range(-1, 1) >= 0 ? 1 : -1;
            posz *= mark;
            return new Vector3(posx, gameObject.transform.position.y, posz);
        }
        return Vector3.zero;
    }

    protected void CreateBattleEnitity(bool isLastAlive = false)
    {
        EntityInitData data = new EntityInitData();
        int crtId = 0;
        if (refreshNodeType == RefreshNodeType.one)
        {
            crtId = id[0];
        }
        else if (refreshNodeType == RefreshNodeType.order)
        {
            if (isLastAlive && selectId >= 0)
            {
            }
            else
            {
                selectId += 1;
                selectId = selectId % id.Count;
            }
            crtId = id[selectId];
        }
        else if (refreshNodeType == RefreshNodeType.order)
        {
            if (isLastAlive && selectId >= 0)
            {
            }
            else
            {
                selectId = (int)UnityEngine.Random.Range(0, id.Count);
                selectId = selectId % id.Count;
            }
            crtId = id[selectId];
        }
        data = GamePlayer.GetBattleInitData(GamePlayer.CreateNewPlayer(crtId + ""));

        data.entityType = entityType;
        data.initPos = GetPos();
        GameScene scene = SceneMgr.GetInstance().nowGameScene;
        data.parentObject = scene.entityRoot;

        entity = EntityMgr.GetInstance().CreateEntity<BattlePlayerEntity>(data) as BattlePlayerEntity;
        scene.AddEntity(entity.UID, entity);
    }

    #region ai行为树相关
    protected void CreateAiComponet()
    {
        string path = "ai" + aiActionId;
        if (aiactionNode != null)
        {
            aiactionNode.DestroyAll();
            GameObject.DestroyImmediate(aiactionNode.gameObject);
            aiactionNode = null;
        }
        ResourceMgr.GetInstance().LoadResource(ResourceType.scene, ResourcePath.GetAiActionRes(path), LoadComplete);
    }
    protected void LoadComplete(ResourceData data)
    {
        if (gameObject == null)
        {
            return;
        }
        GameObject crtobj = data.GetCreateObject();
        aiactionNode = crtobj.GetComponent<BehaviorNode>();
        TransformUtils.SetParent(aiactionNode.transform, transform);
        aiactionNode.InitNode(this);
        isOpeAiAction = true;//加载后执行行为树
    }
    protected bool isOpeAiAction = false;
    protected virtual void UpdateAiAction(float dt)
    {
        if (aiactionNode == null)
        {
            return;
        }
        aiactionNode.Tick(dt);
        if (isOpeAiAction && aiactionNode != null)
        {
            aiactionNode.StartNode(null, AiactionCallback);
        }
    }
    private void AiactionCallback(bool result)
    {
        isOpeAiAction = true;
    }
    #endregion
}
public enum ActiveRequireType
{
    jiejinActive = 1,
    datatimeActive = 2,
}
public enum BornRangeType
{
    none = -1,
    pos = 0,
    box = 1,
    circle = 2,
}
public enum RefreshNodeType
{
    one = 0,
    order = 1,
    random = 2,
}
[Serializable]
public class BaseScrMoEntityD
{
    public bool isActive = false;
    public List<ActiveRequireType> activerequiretype;
    public float startRefreshCd = 0f;//第一次刷新等待cd
    public float startRefreshCdTime = 0f;

    public BornRangeType bornRangeType;
    public List<float> boxRange;
    public float circleRange;
    public int refreshNum = 1;//总共刷新次数

    public float resetCd = -1;//消除后刷新cd
    public float resetCdTime = 0f;
    public bool isLife = false;//是否生存

    public RefreshNodeType refreshNodeType;
    public List<int> id;
    public int selectId = -1;
    public EntityTypes entityType;
    public List<float> nowPos;

    public List<float> xunluPosx;
    public List<float> xunluPosz;
    public int aiActionId = 1001;

    //特殊拥有的
    //点的位置
    public List<float> activePos;

    public int entitylevel;

    public virtual void SaveInfo(BaseSceMoEntity basescrmonoentityD)
    {
        isActive = basescrmonoentityD.isActive;
        activerequiretype = basescrmonoentityD.activerequiretype;
        startRefreshCd = basescrmonoentityD.startRefreshCd;
        startRefreshCdTime = basescrmonoentityD.startRefreshCdTime;
        bornRangeType = basescrmonoentityD.bornRangeType;
        boxRange = basescrmonoentityD.boxRange;
        circleRange = basescrmonoentityD.circleRange;
        refreshNum = basescrmonoentityD.refreshNum;
        resetCd = basescrmonoentityD.resetCd;
        resetCdTime = basescrmonoentityD.resetCdTime;
        isLife = basescrmonoentityD.isLife;
        refreshNodeType = basescrmonoentityD.refreshNodeType;
        id = basescrmonoentityD.id;
        selectId = basescrmonoentityD.selectId;
        entityType = basescrmonoentityD.entityType;
        nowPos = basescrmonoentityD.nowPos;
        xunluPosx = new List<float>();
        xunluPosz = new List<float>();
        for (int i = 0; i < basescrmonoentityD.xunluPosObj.Count; i++)
        {
            xunluPosx.Add(basescrmonoentityD.xunluPosObj[i].transform.localPosition.x);
            xunluPosz.Add(basescrmonoentityD.xunluPosObj[i].transform.localPosition.z);
        }
        aiActionId = basescrmonoentityD.aiActionId;
        entitylevel = basescrmonoentityD.entitylevel;

        activePos = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                activePos.Add(basescrmonoentityD.transform.position.x);
            }
            if (i == 1)
            {
               activePos.Add(basescrmonoentityD.transform.position.y);
            }
            if (i == 2)
            {
                activePos.Add(basescrmonoentityD.transform.position.z);
            }
        }

    }
}