using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameScene
{
    public GameObject entityRoot;
    public GameObject effectRoot;
    public GameObject bulletRoot;
    public CameraMgr cameraMgr;
    private Dictionary<string, Entity> _entityDic;
    public Dictionary<int, List<GameObject>> _numsDic;
    private GameObject numsRoot;
    private bool isLoaded = false;
    private bool isDestroy = false;
    private string sceneName;
    private int _rttIndex = 0;

    public int sceneInfoId = 0;
    public int nowEventIndex = -1;
    public List<int> events;
    public int rttIndex
    {
        set { _rttIndex = value; }
        get { return ++_rttIndex; }
    }
    public GameScene()
    {
        _entityDic = new Dictionary<string, Entity>();
        _numsDic = new Dictionary<int, List<GameObject>>();
        cameraMgr = new CameraMgr();
    }
    public virtual void InitScene<T>(object data = null) where T : new()
    {
        sceneName = SceneMgr.GetInstance().GetResourcePath(typeof(T).Name);
    }
    public virtual void LoadScene<T>() where T : new()
    {
        sceneName = SceneMgr.GetInstance().GetResourcePath(typeof(T).Name);
        UIMgr.GetInstance().ShowLoadResourcePanel(sceneName);
        ResourceMgr.GetInstance().LoadResource(ResourceType.scene, ResourcePath.GetSceneRes(sceneName), LoadResComplete);
    }
    private void LoadResComplete(ResourceData data)
    {
        if (isDestroy)
        {
            return;
        }
        CoroutineHelper.GetInstance().StartCoroutine(StartEnter());
    }
    private IEnumerator StartEnter()
    {
        AsyncOperation ao = Application.LoadLevelAsync(sceneName);
        yield return ao;
        LoadSceneJudge();
        UIMgr.GetInstance().RemoveLoadResourcePanel(sceneName);
    }

    private void LoadSceneJudge()
    {
        if (isDestroy)
        {
            return;
        }
        LoadSceneCallBack();
    }

    protected virtual void LoadSceneCallBack()
    {
        isLoaded = true;
        entityRoot = TransformUtils.FindOrCreateObject("_entityRoot", true, true);
        effectRoot = TransformUtils.FindOrCreateObject("_effectRoot", true, true);
        bulletRoot = TransformUtils.FindOrCreateObject("_bulletRoot", true, true);
        numsRoot = TransformUtils.FindOrCreateObject("_numsRoot", true, true);
        ResourceMgr.GetInstance().GetPrintAllReource();
    }

    public BattlePlayerEntity GetBattleEntity(GameObject obj)
    {
        string uid = obj.name.Replace(Globals.entityName, "");
        if (_entityDic.ContainsKey(uid))
        {
            if (EntityMgr.GetInstance().IsBattleEntity(_entityDic[uid].EntityType))
            {
                return _entityDic[uid] as BattlePlayerEntity;
            }
        }
        return null;
    }
    public virtual void AddEntity(string uid, Entity entity, bool destroyLast = true)
    {
        if (destroyLast)
        {
            RemoveEntity(uid);
        }
        else if (_entityDic.ContainsKey(uid))
        {
            return;
        }
        _entityDic[uid] = entity;
    }
    public virtual void RemoveEntity(string uid)
    {
        if(_entityDic.ContainsKey(uid))
        {
            _entityDic[uid].Destroy();
            _entityDic.Remove(uid);
        }
    }
    public GameObject GetNumsObject(int num)
    {
        GameObject go = null;
        if (_numsDic.ContainsKey(num) && _numsDic[num].Count > 0)
        {
            go = _numsDic[num][0];
            _numsDic[num].RemoveAt(0);
        }
        TransformUtils.SetVisible(go, true);
        return go;
    }
    public void AddNumsObject(int num,GameObject numObject)
    {
        if (!_numsDic.ContainsKey(num))
        {
            _numsDic[num] = new List<GameObject>();
        }
        _numsDic[num].Add(numObject);
        numObject.transform.SetParent(numsRoot.transform);
        numObject.transform.localPosition = new Vector3(1000, 1000, 1000);
    }

    public virtual void OnUpdate(float dt)
    {
        foreach (var item in _entityDic)
        {
            item.Value.OnUpdate(dt);
        }
    }


    public virtual void FixedUpdate(float dt)
    {
        
    }

    public virtual void LateUpdate(float dt)
    {
        cameraMgr.LateUpdate(dt);
        foreach (var item in _entityDic)
        {
            item.Value.LateUpdate(dt);
        }
    }

    public virtual void DestroyScene()
    {
        if (_entityDic != null)
        {
            foreach (var item in _entityDic)
            {
                item.Value.Destroy();
            }
        }
        cameraMgr.DestroyCamera();
        UIMgr.GetInstance().RemoveDicAndResByType();
        ResourceMgr.GetInstance().DestroyByResType(ResourceType.scene, true);
        ResourceMgr.GetInstance().DestroyByResType(ResourceType.entity, true);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}
