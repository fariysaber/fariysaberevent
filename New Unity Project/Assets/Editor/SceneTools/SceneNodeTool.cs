using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SceneNodeTool
{
    public string name;
    public static BaseSceAndNodeInfo GetSceneNodeInfo(GameObject parent)
    {
        string name = parent.name;
        BaseSceAndNodeInfo scenodeinof = new BaseSceAndNodeInfo();
        BaseSceneMonoD sceneMono = new BaseSceneMonoD();
        sceneMono.SaveInfo(parent.GetComponent<BaseSceneMono>());
        
        scenodeinof.baseSceMonoEnList = GetBaseSceMonoEnList(parent);
        scenodeinof.sceneEngineName = name;
        scenodeinof.mono = sceneMono;
        return scenodeinof;
    }
    private static List<BaseScrMoEntityD> GetBaseSceMonoEnList(GameObject obj)
    {
        List<BaseScrMoEntityD> list = new List<BaseScrMoEntityD>();
        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject == obj || child.GetComponent<BaseSceMoEntity>() == null)
            {
                continue;
            }
            BaseScrMoEntityD scemoenti = new BaseScrMoEntityD();
            scemoenti.SaveInfo(child.GetComponent<BaseSceMoEntity>());
            if (scemoenti != null)
            {
                list.Add(scemoenti);
            }
        }
        return list;
    }
}
