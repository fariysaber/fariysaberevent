using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameGlobals : SingletonMonoBehaviour<GameGlobals>
{
    public bool PauseTime = false;
    public void Awake()
    {
        Caching.CleanCache();
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitGlobalTimer();
        InitDriveLoad();
        InitUiInfo();
        InitGameResource();
    }

    private void InitGlobalTimer()
    {
        GameObject global = new GameObject();
        global.name = "GameTimer";
        global.AddComponent<GlobalTimer>();
    }
    private void InitDriveLoad()
    {
    }
    private void InitUiInfo()
    {
        UIMgr.GetInstance().Init();
    }
    #region 初始化资源相关
    private void InitGameResource()
    {
        ResourceMgr.GetInstance().LoadResource(ResourceType.main, ResourcePath.GetManifestPath(), LoadMainfestCallBack);
    }
    private void LoadMainfestCallBack(ResourceData data)
    {
        ResourceMgr.GetInstance().LoadResource(ResourceType.font, ResourcePath.GetFontPath(ResourcePath.initFont), LoadFontCallBack);
    }
    private void LoadFontCallBack(ResourceData data)
    {
        ConfigMgr.GetInstance().InitAllConfigResource(LoadConfigCallBack);
    }
    private void LoadConfigCallBack()
    {
        LoadUIRes();
        StartLoadGameData();//开始获取所有游戏开始数据
        SceneMgr.GetInstance().SwichScene<LoginScene>();
        UIMgr.GetInstance().RemoveLoadResourcePanel("ConfigMgr");
    }
    private void LoadUIRes()
    {
        ResourceMgr.GetInstance().LoadResource(ResourceType.uiForever, ResourcePath.GetUIAtlas(ResourcePath.commonUi), LoadUIResCallBack);
        UIMgr.GetInstance().ShowLoadResourcePanel("uiForever");
    }
    private void LoadUIResCallBack(ResourceData data)
    {
        UIMgr.GetInstance().RemoveLoadResourcePanel("uiForever");
    }
    private void StartLoadGameData()
    {
        Globals.ResetPlayer1Data();
    }
    #endregion

    void Update()
    {
        if (PauseTime)
            return;
        AssetMgr.Update();
        InputMgr.GetInstance().Update(Time.deltaTime);
        SceneMgr.GetInstance().Update(Time.deltaTime);
        UIMgr.GetInstance().Update(Time.deltaTime);
    }

    void LateUpdate()
    {
         if (PauseTime)
            return;
         SceneMgr.GetInstance().LateUpdate(Time.deltaTime);
    }
}