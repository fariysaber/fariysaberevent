using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 无任何剧情或者说明,打开摄像头直接开战
/// </summary>
public class BaseSceneMono : MonoBehaviour
{
    public string CLASS_NAME = "BaseSceneMono";
    public float startCd = 1f;
    public float endCd = 1f;
    public SceneStartAction sceneStartAction = SceneStartAction.blackCircle;
    public SceneEndAction sceneEndAction = SceneEndAction.blackCircle;
    public int month = 1;
    public int day = 1;
    public int hour = 12;
    public virtual void LoadInfo(BaseSceneMonoD monoD)
    {
        CLASS_NAME = monoD.CLASS_NAME;
        startCd = monoD.startCd;
        endCd = monoD.endCd;
        sceneStartAction = monoD.sceneStartAction;
        sceneEndAction = monoD.sceneEndAction;
        month = monoD.month;
        day = monoD.day;
        hour = monoD.hour;
    }
    public virtual void OnUpdate(float dt)
    {
    }
    public virtual void DestroyAll()
    {
    }
}
[Serializable]
public class BaseSceneMonoD
{
    public string CLASS_NAME = "BaseSceneMono";
    public float startCd = 1f;
    public float endCd = 1f;
    public SceneStartAction sceneStartAction = SceneStartAction.blackCircle;
    public SceneEndAction sceneEndAction = SceneEndAction.blackCircle;
    public int month = 1;
    public int day = 1;
    public int hour = 12;
    public virtual void SaveInfo(BaseSceneMono monoinfo)
    {
        CLASS_NAME = monoinfo.CLASS_NAME;
        startCd = monoinfo.startCd;
        endCd = monoinfo.endCd;
        sceneStartAction = monoinfo.sceneStartAction;
        sceneEndAction = monoinfo.sceneEndAction;
        month = monoinfo.month;
        day = monoinfo.day;
        hour = monoinfo.hour;
    }
}
public enum SceneStartAction
{
    blackCircle = 1,
}
public enum SceneEndAction
{
    blackCircle = 1,
}