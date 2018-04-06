using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEngine
{
    protected BaseSceneMono sceneMono;
    protected List<BaseSceMoEntity> sceMoEntity;
    public void Init<T>(GameScene scene) where T : GameScene
    {
        string sceneName = typeof(T).Name;
        BaseSceneMonoD scenemonoD = BinDataMgr.GetInstance().GetSceneMonoD(sceneName);
        List<BaseScrMoEntityD> scMonEntityD = BinDataMgr.GetInstance().GetSceneEntityMonoD(sceneName);
        InitBaseData(scene, sceneName, scenemonoD,scMonEntityD);
    }

    protected virtual void InitBaseData(GameScene scene,string sceneName, BaseSceneMonoD sceneMonoD, List<BaseScrMoEntityD> scMonEntityD)
    {
        GameObject fbscene = GameObject.Find("fbscene");
        if (fbscene != null)
        {
            GameObject.DestroyImmediate(fbscene);
        }
        fbscene = new GameObject("fbscene");
        GameObject sceneMonoGo = new GameObject(sceneName);
        sceneMono = sceneMonoGo.AddComponent<BaseSceneMono>();
        sceneMono.LoadInfo(sceneMonoD);
        TransformUtils.SetParent(sceneMono.transform, fbscene.transform);

        sceMoEntity = new List<BaseSceMoEntity>();
        for (int i = 0; i < scMonEntityD.Count; i++)
        {
            GameObject entityMono = new GameObject("entityMono" + i);
            BaseSceMoEntity moentity = AddBaseMonoEntityCom(entityMono, scMonEntityD[i]);
            moentity.Init(scene);
            moentity.LoadInfo(scMonEntityD[i]);
            TransformUtils.SetParent(moentity.transform, sceneMono.transform, false);
            sceMoEntity.Add(moentity);
        }
    }
    private BaseSceMoEntity AddBaseMonoEntityCom(GameObject entityMono,BaseScrMoEntityD data)
    {
        if (data.entityType == EntityTypes.BattlePlayerMy)
        {
            return entityMono.AddComponent<MyPlayerEntityMono>();
        }
        if (data.entityType == EntityTypes.BattlePlayerZhongli)
        {
            return entityMono.AddComponent<ZhongliPlayerEntityMono>();
        }
        return entityMono.AddComponent<BaseSceMoEntity>();
    }

    public virtual void OnUpdate(float dt)
    {
        if (sceneMono != null)
        {
            sceneMono.OnUpdate(dt);
        }
        if (sceMoEntity != null)
        {
            for (int i = 0; i < sceMoEntity.Count; i++)
            {
                sceMoEntity[i].OnUpdate(dt);
            }
        }
    }

    public virtual void FixedUpdate(float dt)
    {

    }

    public virtual void LateUpdate(float dt)
    {

    }

    public virtual void DestroyEngine()
    {
        if (sceneMono != null)
        {
            sceneMono.DestroyAll();
        }
        if (sceMoEntity != null)
        {
            for (int i = 0; i < sceMoEntity.Count; i++)
            {
                sceMoEntity[i].DestroyAll();
            }
        }
    }
}
