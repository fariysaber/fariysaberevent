using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : Singleton<SceneMgr>
{
    public GameScene nowGameScene;
    public void SwichScene<T>(object data = null) where T : new ()
    {
        DestroyNowScene();
        nowGameScene = new T() as GameScene;
        nowGameScene.InitScene<T>(data);
        nowGameScene.LoadScene<T>();
    }
    public void DestroyNowScene()
    {
        if (nowGameScene != null)
        {
            nowGameScene.DestroyScene();
            nowGameScene = null;
        }
        InputMgr.GetInstance().SwichScene<InputController>();
    }
    public void Update(float dt)
    {
        if (nowGameScene != null)
        {
            nowGameScene.OnUpdate(dt);
        }
    }
    public void LateUpdate(float dt)
    {
        if (nowGameScene != null)
        {
            nowGameScene.LateUpdate(dt);
        }
    }
    public string GetResourcePath(string name)
    {
        switch (name)
        {
            case "RoomScene": return "roomscene";
            case "BigBattleScene": return "bigscene";
            case "LoginScene": return "loginscene";
            case "SelectHeroScene": return "selectheroscene";
        }
        return "";
    }
}

public enum SceneNodeRequire
{
    sceneLoaded = 0,
}