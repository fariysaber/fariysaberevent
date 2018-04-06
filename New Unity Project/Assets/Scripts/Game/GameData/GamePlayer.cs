using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class GamePlayer
{
    public string key;
    public string name;

    public GamePlayerItem items = new GamePlayerItem();

    #region 小队基础数据
    public GamePlayerEntityData mainGamePlayerEntityData;
    public List<GamePlayerEntityData> teamGamePlayerEntityData = new List<GamePlayerEntityData>();
    public void AddNewPlayer(string entityId,bool isMain = false, string name = "",int pifu = 0,int level = 1,int star = 0,bool initEquip = true)
    {
        GamePlayerEntityData entityData = CreateNewPlayer(entityId, name, pifu, level, star, initEquip);
        if (isMain)
        {
            mainGamePlayerEntityData = entityData;
        }
        teamGamePlayerEntityData.Add(entityData);
        //测试用
        AddWuqiEquip(mainGamePlayerEntityData, 10004, 20);
    }
    public static GamePlayerEntityData CreateNewPlayer(string entityId, string name = "", int pifu = 0, int level = 1, int star = 0,bool initEquip = true)
    {
        GamePlayerEntityData entityData = new GamePlayerEntityData();
        entityData.entityKey = DataBase.GetInstance().GetEntityValue() + "";
        entityData.name = name;
        entityData.entityId = entityId;
        entityData.pifu = pifu;
        entityData.level = level;
        entityData.star = star;
        entityData.herovo = HeroConfig.GetData(entityId);
        entityData.playerSkillInfo = new List<PlayerSkillInfo>();
        AddPlayerAllSkillInfo(entityData);
        if (initEquip)
        {
            AddInitEquip(entityData);
        }
        
        return entityData;
    }
    public static void AddWuqiEquip(GamePlayerEntityData playerData,int wuqiId,int level)
    {
        PlayerEquipInfo info = new PlayerEquipInfo();
        info.id = wuqiId;
        info.key = DataBase.GetInstance().GetValueByUp(Globals.ItemUpKey) + "";
        info.level = level;
        info.vo = EquipConfig.GetData(info.id + "");
        playerData.AddWuqiInfo(info);
    }
    public static void AddInitEquip(GamePlayerEntityData playerData)
    {
        PlayerEquipInfo info = new PlayerEquipInfo();
        info.id = 10201;
        info.key = DataBase.GetInstance().GetValueByUp(Globals.ItemUpKey) + "";
        info.level = 1;
        info.vo = EquipConfig.GetData(info.id + "");
        playerData.AddWuqiInfo(info);
        //增加初始三件套
        for (int i = 0; i < 3; i++)
        {
            PlayerEquipInfo info1 = new PlayerEquipInfo();
            info1.id = 11025 + i;
            info1.key = DataBase.GetInstance().GetValueByUp(Globals.ItemUpKey) + "";
            info1.level = 1;
            if (i == 0)
                info1.suijitype = 1;
            if (i == 1)
                info1.suijitype = 4;
            if (i == 2)
                info1.suijitype = 3;
            info1.vo = EquipConfig.GetData(info1.id + "");
            playerData.AddLingzhuang(i + 1,info1);
        }
    }
    public static void AddPlayerAllSkillInfo(GamePlayerEntityData playerData)
    {
        HeroSkillVo vo = HeroSkillConfig.GetData(playerData.entityId);
        int jumpId = vo.jumpSkill;
        if (jumpId > 0)
        {
            playerData.AddSkill(jumpId, 1);
        }

        int crashSkill = vo.crashSkill;
        if (crashSkill > 0)
        {
            playerData.AddSkill(crashSkill, 1);
        }

        for (int i = 0; i < vo.active.Count; i++)
        {
            playerData.AddSkill(vo.active[i], 1);
        }
    }
    public string GetMainHeroModel()
    {
        HeroVo herovo = mainGamePlayerEntityData.herovo;
        return CharactorConfig.GetData(herovo.charactor[mainGamePlayerEntityData.pifu] + "").model;
    }
    /// <summary>
    /// 获取战斗数据
    /// </summary>
    /// <returns></returns>
    public EntityInitData GetBattleMainEntityInitData()
    {
        EntityInitData data = GetBattleInitData(mainGamePlayerEntityData);
        return data;
    }
    public static EntityInitData GetBattleInitData(GamePlayerEntityData entitydata)
    {
        EntityInitData data = new EntityInitData();
        data.uid = entitydata.entityKey;
        data.name = entitydata.name;
        data.entityId = entitydata.entityId;
        data.moveInfo = new MoveInfo();
        data.skillBaseInfo = new List<BattleSkillBaseInfo>();
        for (int i = 0; i < entitydata.playerSkillInfo.Count; i++)
        {
            PlayerSkillInfo playerkillInfo = entitydata.playerSkillInfo[i];
            BattleSkillBaseInfo info = new BattleSkillBaseInfo();
            info.skillId = playerkillInfo.id;
            info.skillLevel = playerkillInfo.level;
            data.skillBaseInfo.Add(info);
        }
        data.gameEntityData = entitydata;
        return data;
    }
    #endregion
}