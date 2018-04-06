using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMgr : Singleton<ConfigMgr>
{
    private Dictionary<string, bool> _loadDic = new Dictionary<string,bool>();
    public delegate void loadAllComplete();
    private loadAllComplete _loadallComplete;
    public void InitAllConfigResource(loadAllComplete loadallComplete)
    {
        _loadallComplete = loadallComplete;
        _loadDic.Clear();
        UIMgr.GetInstance().ShowLoadResourcePanel("ConfigMgr");
        InitResource<LanguageVo>();
        InitResource<HeroVo>();
        InitResource<CharactorVo>();
        InitResource<HeroSkillVo>();
        InitResource<SKillVo>();
        InitResource<BuffVo>();
        InitResource<BulletVo>();
        InitResource<LevelShuxinVo>();
        InitResource<EquipVo>();

        InitBinDataResource<ModelInfo>();
        InitSceneBinDataResource<BigBattleScene>();
    }
    private void InitResource<T>() where T : new()
    {
        string name = typeof(T).Name;
        name = ResourcePath.GetConfig(name);
        _loadDic[name] = true;
        ResourceMgr.GetInstance().LoadResource(ResourceType.config, name, LoadCallBack);
    }
    private void InitBinDataResource<T>() where T : new()
    {
        string name = typeof(T).Name;
        name = ResourcePath.GetBinData(name);
        _loadDic[name] = true;
        ResourceMgr.GetInstance().LoadResource(ResourceType.config, name, LoadCallBack);
    }
     private void InitSceneBinDataResource<T>() where T : new()
    {
        string name = typeof(T).Name;
        string name1 = name + "_sceneInfo";
        string name2 = name + "_entityinfo";
        name1 = ResourcePath.GetBinData(name1);
        _loadDic[name1] = true;
        ResourceMgr.GetInstance().LoadResource(ResourceType.config, name1, LoadCallBack);

        name2 = ResourcePath.GetBinData(name2);
        _loadDic[name2] = true;
        ResourceMgr.GetInstance().LoadResource(ResourceType.config, name2, LoadCallBack);
    }
    private void LoadCallBack(ResourceData data)
    {
        if (_loadDic.ContainsKey(data.filePath))
        {
            _loadDic.Remove(data.filePath);
        }
        CheckAllLoaded();
    }
    private void CheckAllLoaded()
    {
        if (_loadDic.Count <= 0)
        {
            if (_loadallComplete != null)
            {
                _loadallComplete();
            }
        }
    }
}
