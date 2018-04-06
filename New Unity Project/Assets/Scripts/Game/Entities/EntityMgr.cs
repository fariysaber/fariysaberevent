using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityMgr : Singleton<EntityMgr>
{
    public Entity CreateEntity<T>(EntityInitData data) where T : new()
    {
        Entity newEntity = GetEntityByType(typeof(T).Name, data.entityId);
        newEntity.InitData(data);
        return newEntity;
    }
    public Entity GetEntityByType(string typeName, string entityId)
    {
        if (typeName == typeof(BattlePlayerEntity).Name)
        {
            HeroVo herovo = HeroConfig.GetData(entityId);
            if (herovo != null)
            {
                typeName = herovo.className;
            }
        }
        Type getType = GameTools.GetType(typeName);
        Debugger.Log("创建Entity Class:" + typeName);
        object getObj = Activator.CreateInstance(getType);
        return getObj as Entity;
    }
    public int GetLayerByType(EntityTypes type)
    {
        switch (type)
        {
            case EntityTypes.Default: return Globals.entitylayer; break;
            case EntityTypes.PlayerType: return Globals.playerlayer; break;
            case EntityTypes.Plant: return Globals.plantlayer; break;
            case EntityTypes.building: return Globals.buildinglayer; break;
            case EntityTypes.BattlePlayerType: return Globals.playerlayer; break;
            case EntityTypes.BattlePlayerMy: return Globals.BattlePlayerMylayer; break;
            case EntityTypes.BattlePlayerMyTeam: return Globals.BattlePlayerMyTeamlayer; break;
            case EntityTypes.BattlePlayerFriend: return Globals.BattlePlayerFriendlayer; break;
            case EntityTypes.BattlePlayerDiRen: return Globals.BattlePlayerDiRenlayer; break;
            case EntityTypes.BattlePlayerZhongli: return Globals.BattlePlayerZhonglilayer; break;
        }
        return Globals.entitylayer;
    }
    public bool IsBattleEntity(int layer)
    {
        if(layer == Globals.BattlePlayerMylayer ||
            layer == Globals.BattlePlayerMyTeamlayer ||
            layer == Globals.BattlePlayerFriendlayer ||
            layer == Globals.BattlePlayerDiRenlayer ||
            layer == Globals.BattlePlayerZhonglilayer )
        {
            return true;
        }
        return false;
    }
    public bool IsBattleEntity(EntityTypes type)
    {
        switch (type)
        {
            case EntityTypes.BattlePlayerMy: return true; break;
            case EntityTypes.BattlePlayerMyTeam: return true; break;
            case EntityTypes.BattlePlayerFriend: return true; break;
            case EntityTypes.BattlePlayerDiRen: return true; break;
            case EntityTypes.BattlePlayerZhongli: return true; break;
        }
        return false;
    }
    public bool IsMinType(EntityTypes type)
    {
        switch (type)
        {
            case EntityTypes.BattlePlayerMy: return true; break;
            case EntityTypes.BattlePlayerMyTeam: return true; break;
            case EntityTypes.BattlePlayerFriend: return true; break;
        }
        return false;
    }
}