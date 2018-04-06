using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModel
{
    protected ModelInitData modelInitData;
    public Vector3 bornPos;
    public CapsuleCollider capsuleCollider;
    public BoxCollider boxCollider;
    public SphereCollider sphereCollider;
    public delegate void LoadedModelCallback();
    public LoadedModelCallback entitycallback;
    public virtual void InitBaseData(ModelInitData data,LoadedModelCallback callback = null)
    {
        _entity = data.entity;
        _modelName = data.modelName;
        parent = data.parent;

        Layer = data.layer;
        Pos = data.pos;
        bornPos = Pos;
        Scale = data.scale;
        modelInitData = data;
        entitycallback = callback;
        StartLoad(data.modelData);
    }

    protected virtual void InitBaseCommponent(ModelData modelData)
    {
        string path = modelData.path;
        path = path.Substring(path.LastIndexOf("/model/") + "/model/".Length);
        ModelInfo info = BinDataMgr.GetInstance().GetModelInfo(path);
        if (gameObject != null)
        {
            GameObject.DestroyImmediate(gameObject);
        }
        gameObject = TransformUtils.CreateGameObject(null,
                parent.transform, Pos, Scale, _modelName);
        transform.forward = _forward;
        
        SetColliderInfo(info);
        SetCharactorInfo(info);
        Layer = _layer;
    }
    protected virtual void SetColliderInfo(ModelInfo info)
    {
        
    }
    protected virtual void SetCharactorInfo(ModelInfo info)
    {
        
    }

    public void RefreshModelLayer(int layer)
    {
        if (gameObject == null)
        {
            return;
        }
        if (limitLayer != -1)
        {
            layer = limitLayer;
        }
        if (gameObject)
        {
            gameObject.layer = GetModelBaseLayer();//character用player层级，防止子弹达到
        }
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child.GetComponent<Collider>() && child.GetComponent<Collider>().enabled == true)
            {
                child.gameObject.layer = layer;
            }
            else
            {
                child.gameObject.layer = GetModelBaseLayer();
            }
        }
    }
    protected virtual int GetModelBaseLayer()
    {
        return Globals.playerlayer;
    }

    public void SetAcive(bool active)
    {
        if (gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }
    }

    public virtual void OnUpdate(float dt)
    {
    }
    public virtual void FixedUpdate(float dt)
    {
    }
    public virtual void LateUpdate(float dt)
    {
    }


    #region 模型加载相关
    public virtual void StartLoad(ModelData modelData, bool reLoad = false)
    {
        if (ResourceMgr.GetInstance().GetResourceData(ResourceType.entity, modelData.path) == null)
        {
            InitBaseCommponent(modelData);
        }
        LoadState = ModelLoadState.loading;
        path = modelData.path;
        ResourceMgr.GetInstance().LoadResource(ResourceType.entity, modelData.path, OnFinishLoad);
    }
    public virtual void ReLoad()
    {
        Debugger.Log("BaseModel ReLoad没被override");
    }
    private void OnFinishLoad(ResourceData data)
    {
        if (LoadState != ModelLoadState.loading)
        {
            return;
        }
        GameObject crtObj = data.GetCreateObject();
        if (gameObject != null)
        {
            AttachNewObj(crtObj);
            GameObject.DestroyImmediate(gameObject);
            gameObject = null;
        }
        LoadState = ModelLoadState.loaded;
        crtObj.transform.SetParent(parent.transform);
        crtObj.transform.position = Pos;
        crtObj.transform.localScale = Scale;
        crtObj.transform.forward = _forward;
        crtObj.name = _modelName;
        gameObject = crtObj;
        RefreshColliderInfo();
        Layer = _layer;
        OnStartModelInfo();
        if (_entity.EntityType == EntityTypes.BattlePlayerMy)
        {
            EventDispatcher.TriggerEvent(GlobalEvents.RESET_MAINCAMERAL);
        }
        if (entitycallback != null)
        {
            entitycallback();
        }
    }

    public virtual void AttachEffectObject(GameObject addObj,Vector3 pos,Vector3 rotate, string attachname = "",bool resetAttachLayer = true)
    {
        if(addObj == null)
        {
            return;
        }
        if (transform == null)
        {
            return;
        }
        Transform parentTrans = null;
        if (attachname.Equals(""))
        {
            addObj.transform.SetParent(transform);
            parentTrans = transform;
        }
        else
        {
            Transform attachTransform = TransformUtils.GetObjectByName(gameObject, attachname);
            if (attachTransform != null)
            {
                addObj.transform.SetParent(attachTransform);
                parentTrans = attachTransform;
            }
            else
            {
                addObj.transform.SetParent(transform);
                attachTransform = transform;
            }
        }
        addObj.transform.localPosition = pos;
        addObj.transform.localRotation = Quaternion.Euler(rotate);
        if (resetAttachLayer)
        {
            foreach (Transform child in addObj.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = parentTrans.gameObject.layer;
            }
        }
    }

    public virtual Transform GetEffectObjName(bool isbodyEffectObj = false)
    {
        Transform attachTransform = TransformUtils.GetObjectByName(gameObject, "effectparent");
        if (attachTransform != null && isbodyEffectObj)
        {
            return attachTransform;
        }
        Transform effecttrans = transform.Find("effectobj");
        if (effecttrans == null)
        {
            GameObject crt = new GameObject("effectobj");
            effecttrans = crt.transform;
            TransformUtils.SetParent(effecttrans, transform);
        }
        return effecttrans;
    }
    public virtual Transform GetObjByName(string name)
    {
        Transform getTransform = TransformUtils.GetObjectByName(gameObject, name);
        if (getTransform == null)
        {
            return transform;
        }
        return getTransform;
    }

    public virtual Transform GetAttributeObj()
    {
        Transform trans = GetEffectObjName();
        Transform attriTrans = trans.Find("attriObj");
        if (attriTrans == null)
        {
            GameObject crt = new GameObject("attriObj");
            attriTrans = crt.transform;
            TransformUtils.SetParent(attriTrans, trans);
            CharacterController controller = gameObject.GetComponent<CharacterController>();
            if (controller != null)
            {
                attriTrans.transform.localPosition = new Vector3(0, controller.height + 0.1f, 0);
            }
        }
        return attriTrans;
    }

    public float GetHeight()
    {
        CharacterController controller = gameObject.GetComponent<CharacterController>();
        if (controller != null)
        {
            return controller.height;
        }
        return 1f;
    }

    public Vector3 GetCenterPos()
    {
        return new Vector3(0, GetHeight() / 2f, 0);
    }

    private void AttachNewObj(GameObject newobj)
    {
        Transform effecttrans = transform.Find("effectobj");
        if (effecttrans != null)
        {
            Debugger.Log("移动到模型特效挂节点成功",Color.green);
            TransformUtils.SetParent(effecttrans, newobj.transform);
        }
    }


    protected virtual void OnStartModelInfo()
    {

    }
    private void RefreshColliderInfo()
    {
        collider = gameObject.GetComponent<Collider>();
        if (collider)
        {
            collider.enabled = false;
        }
        string path = modelInitData.modelData.path;
        path = path.Substring(path.LastIndexOf("/model/") + "/model/".Length);
        ModelInfo info = BinDataMgr.GetInstance().GetModelInfo(path);
        SetColliderInfo(info);
    }
    protected void SetCollider(GameObject collObj)
    {
        boxCollider = collObj.GetComponent<BoxCollider>();
        capsuleCollider = collObj.GetComponent<CapsuleCollider>();
        sphereCollider = collObj.GetComponent<SphereCollider>();
    }
    public virtual void DestroyModel()
    {
        if (gameObject != null)
        {
            TransformUtils.DestroyGameObject(gameObject);
        }
        if (LoadState == ModelLoadState.loaded || LoadState == ModelLoadState.loading)
        {
            ResourceMgr.GetInstance().DecreaseLoad(ResourceType.entity, path);
        }
        LoadState = ModelLoadState.destroy;
        gameObject = null;
    }
    #endregion

    #region 基本数据设置
    private ModelLoadState _loadState;
    public ModelLoadState LoadState
    {
        set { _loadState = value; }
        get { return _loadState; }
    }

    private int _layer;
    public int Layer
    {
        set 
        {
            _layer = value;
            RefreshModelLayer(_layer);
        }
        get { return _layer; }
    }
    private int limitLayer = -1;
    private string limitLayerName = "";

    public void SetLimitLayer(int setLimitlayer, string setName)
    {
        limitLayer = setLimitlayer;
        limitLayerName = setName;
        Layer = _layer;
    }
    public void ResetLayer(string setName)
    {
        if (limitLayerName.Equals(setName))
        {
            limitLayer = -1;
            Layer = _layer;
        }
    }

    private Entity _entity;
    public Entity Entity
    {
        set { _entity = value; }
        get { return _entity; }
    }

    public PlayerEntity playerEntity
    {
        get { return _entity as PlayerEntity; }
    }

    private string _modelName;
    public string ModelName
    {
        set { _modelName = value; }
        get { return _modelName; }
    }

    //Render根GameObject
    private GameObject _gameObject = null;
    public GameObject gameObject
    {
        set
        {
            _gameObject = value;
            if (_gameObject != null)
            {
                _transform = _gameObject.transform;
            }
            else
            {
                _transform = null;
            }
        }
        get { return _gameObject; }
    }

    private Transform _transform = null;
    public Transform transform
    {
        set { _transform = value; }
        get { return _transform; }
    }

    public GameObject parent;
    /// <summary>
    /// 强制设位置
    /// </summary>
    private Vector3 _pos;
    public Vector3 Pos
    {
        get
        {
            if (transform)
            {
                _pos = transform.position;
            }
            return _pos;
        }
        set
        {
            _pos = value;
            if (transform)
            {
                transform.position = value;
            }
        }
    }

    public float GetColliderLength()
    {
        if (transform == null)
        {
            return 0;
        }
        if (boxCollider != null)
        {
            if (boxCollider.size.x > boxCollider.size.z)
            {
                return boxCollider.size.x / 2f;
            }
            else
            {
                return boxCollider.size.z / 2f;
            }
        }
        if (capsuleCollider != null)
        {
            return capsuleCollider.radius / 2f;
        }
        if (sphereCollider != null)
        {
            return sphereCollider.radius / 2f;
        }
        return 0f;
    }

    private Vector3 _forward = Vector3.zero;
    public Vector3 Forward
    {
        get
        {
            if (transform)
            {
                _forward = transform.forward;
            }
            return _forward;
        }
    }

    //模型在屏幕中的缩放比例
    private Vector3 _scale = Vector3.zero;
    public Vector3 Scale
    {
        set 
        {
            _scale = value;
            if (gameObject)
            {
                gameObject.transform.localScale = _scale;
            }
        }
        get { return _scale; }
    }

    public Collider collider;

    private string path = "";
    #endregion
}
public class ModelInitData
{
    public Entity entity;
    public GameObject parent;
    public string modelName;
    public Vector3 pos = new Vector3(1000,1000,0);
    public Vector3 scale = new Vector3(1, 1, 1);
    public int layer;
    public ModelData modelData;
    public MoveInfo moveInfo;
}
public class ModelData
{
    public string path;
    public void SetPath(string modelPath, bool isTrueUrl = false)
    {
        if (isTrueUrl)
        {
            path = modelPath;
        }
        else
        {
            path = ResourcePath.GetModel(modelPath);
        }
    }
}
public enum ModelLoadState
{
    none = 0,
    loading = 1,
    loaded = 2,
    destroy = 3,
}