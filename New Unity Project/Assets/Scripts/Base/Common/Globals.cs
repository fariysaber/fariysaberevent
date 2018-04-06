using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Globals
{
    public static int defaultFrame = 30;
    public readonly static string dontDestroy = "DontDestroyComponent";

    public const float screenOriWidth = 1366;
    public const float screenOriHeight = 768;

    public const int maxSceneWidth = 1000;
    public const int maxSceneHeight = 1000;

    public const int rttCameraTargetDisplay = 3;
    
    //动态数据
    public static int rttLayer = 8;
    public static int uilayer = 5;
    public static int airwalllayer = 11;
    public static int normaleffectlayer = 6;

    public static int waterlayer = 4;
    public static int groundlayer = 9;
    public static int plantlayer = 10;
    public static int entitylayer = 12;
    public static int buildinglayer = 13;
    public static int playerlayer = 14;
    public static int picItemlayer = 15;
    public static int BattlePlayerMylayer = 16;
    public static int BattlePlayerMyTeamlayer = 17;
    public static int BattlePlayerFriendlayer = 18;
    public static int BattlePlayerDiRenlayer = 19;
    public static int BattlePlayerZhonglilayer = 20;

    public static int BattlePlayerMineBulletlayer = 21;
    public static int BattlePlayerDirenBulletlayer = 22;
    public static int BattlePlayerZhongliBulletlayer = 23;

    public static int BattlePlayerMineShanlayer = 24;
    public static int BattlePlayerDirenShanlayer = 25;

    public static int BaseBuffId = 9001;
    //技能升级道具key
    public static int SkillUpNeedName = 200001;
    public static int skillUpMaxLevel = 5;

    public static int GetDownLayer()
    {
        return 1 << groundlayer | 1 << buildinglayer | 1 << plantlayer;
    }

    public static bool IsMineLayer(int layer)
    {
        if (layer == BattlePlayerMylayer || layer == BattlePlayerMyTeamlayer || layer == BattlePlayerFriendlayer)
        {
            return true;
        }
        return false;
    }
    public static bool IsBattlePlayer(int layer)
    {
        if (layer == BattlePlayerMylayer || layer == BattlePlayerMyTeamlayer || layer == BattlePlayerFriendlayer
        || layer == BattlePlayerDiRenlayer || layer == BattlePlayerZhonglilayer || layer == BattlePlayerMineShanlayer
            || layer == BattlePlayerDirenShanlayer)
        {
            return true;
        }
        return false;
    }

    public static string MainCameraName = "MainCamera";

    public static float gravity = 20;

    public static GamePlayer player1;
    public static void ResetPlayer1Data()
    {
        player1 = new GamePlayer();
    }
    //获取技能战斗配置参数
    public static void GetSkillBattleParam(int skillId, ref object getObj)
    {
        SKillVo skillvo = SkillConfig.GetData(skillId + "");
        if (skillvo.param.Count == 0)
        {
            return;
        }
        else if (skillvo.param.Count == 1)
        {
            if (skillvo.param[0].Count == 0)
            {
                return;
            }
        }
        FieldInfo[] infoList = getObj.GetType().GetFields();
        for (int i = 0; i < skillvo.param.Count; i++)
        {
            ReadDataToDic<int>(infoList, skillvo.param[i][0], int.Parse(skillvo.param[i][1]), ref getObj);
        }
    }
    //获取子弹战斗配置参数
    public static void GetBulletBattleParam(int bulletId, ref object getObj)
    {
        BulletVo bulletVo = BulletConfig.GetData(bulletId + "");
        if (bulletVo.damagetypeparam.Count == 0)
        {
            return;
        }
        else if (bulletVo.damagetypeparam.Count == 1)
        {
            if (bulletVo.damagetypeparam[0].Count == 0)
            {
                return;
            }
        }
        FieldInfo[] infoList = getObj.GetType().GetFields();
        for (int i = 0; i < bulletVo.damagetypeparam.Count; i++)
        {
            ReadDataToDic<int>(infoList, bulletVo.damagetypeparam[i][0], int.Parse(bulletVo.damagetypeparam[i][1]), ref getObj);
        }
    }
    public static void ReadDataToDic<T>(FieldInfo[] infos, string name, T value, ref object getObj)
    {
        for (int i = 0; i < infos.Length; i++)
        {
            if (infos[i] != null && infos[i].Name.Equals(name))
            {
                infos[i].SetValue(getObj, value);
                return;
            }
        }
    }

    #region 存储数据库的key值
    public static string entityKeyName = "entityKey";
    public static string ItemUpKey = "itemupkey";
    #endregion

    public static string entityName = "entity";
    //获取角色战斗类型
    public static string GetBattleTypeName(int index)
    {
        switch (index)
        {
            case 1: return "战士"; break;
            case 2: return "输出"; break;
            case 3: return "肉盾"; break;
            case 4: return "辅助"; break;
        }
        return "散人";
    }
    //获取角色战斗属性图标（刚，巧，灵）
    public static string GetBattleShuxin(int index)
    {
        switch (index)
        {
            case 1: return "pic_gang"; break;
            case 2: return "pic_qiao"; break;
            case 3: return "pic_ling"; break;
        }
        return "pic_gang";
    }
    //获取角色评级
    public static string GetPingji(int index)
    {
        switch (index)
        {
            case 1: return "C"; break;
            case 2: return "B"; break;
            case 3: return "A"; break;
            case 4: return "S"; break;
            case 5: return "Z"; break;
        }
        return "C";
    }
    //获取角色下一个评级
    public static string GetNextPingji(int index)
    {
        if(index > 5)
        {
            return "";
        }
        return GetPingji(index + 1);
    }
    //获取评级的颜色
    public static Color GetPingjiColor(string ping)
    {
        switch (ping)
        {
            case "C": return new Color(216f/255f,1f,189f/255f); break;
            case "B": return new Color(0f / 255f, 1f, 0f / 255f); break;
            case "A": return new Color(0f / 255f, 1f, 1f); break;
            case "S": return new Color(1f,1f,0f); break;
            case "Z": return new Color(1f,155f/255f,0f); break;
        }
        return new Color(216f/255f,1f,189f/255f);
    }
    //获取属性值判断当前属性值得评级
    public static float pingjimaxNum = 1000;
    public static float pingjipercentMax = 1200;
    public static string GetShuxinPJ(int hp,int offset = 100)
    {
        int reduceIndex = 3;
        for (reduceIndex = 3; reduceIndex >= -1; reduceIndex--)
        {
            if (hp < pingjimaxNum - reduceIndex * offset + offset / 2)
            {
                break;
            }
        }
        switch (reduceIndex)
        {
            case 3: return "C"; break;
            case 2: return "B"; break;
            case 1: return "A"; break;
            case 0: return "S"; break;
        }
        return "Z";
    }
    //获取升星的碎片数
    public static int maxLevel = 40;
    public static int GetShengxingSuipian(int index)
    {
        switch (index)
        {
            case 1: return 5; break;
            case 2: return 10; break;
            case 3: return 20; break;
            case 4: return 40; break;
            case 5: return 80; break;
        }
        return 999;
    }
    //获取技能的类型图标名称
    public static string GetSkillTypeName(int type)
    {
        switch (type)
        {
            case 1: return "普"; break;
            case 2: return "益"; break;
            case 3: return "觉"; break;
        }
        return "普";
    }

    //装备升级道具key
    public static int EquipUpNeedName = 200101;
    public static int EquipUpMaxLevel = 30;
    //武器对应的攻击，防御，生命对应的人物属性倍数
    public static float WuqiLvBeishu = 1.2f;
    public static float wuqibaojibeishu = 1f;
    public static float wuqibaoshangbeishu = 1.5f;
    //灵装对应的攻击，防御，生命对应的人物属性倍数（根据星级特殊处理）
    public static float defendBili = 300;
    public static float GetLZbeishuByStar(int star)
    {
        if (star == 1)
        {
            return 0.2f;
        }
        else if (star == 2)
        {
            return 0.4f;
        }
        else
        {
            return 0.6f;
        }
    }
    //灵装对应的暴击对应的人物属性倍数（根据星级特殊处理）
    public static float GetLZBaojiByStar(int star)
    {
        if (star == 1)
        {
            return 0.3f;
        }
        else if (star == 2)
        {
            return 0.6f;
        }
        else
        {
            return 1f;
        }
    }
    public static float GetLZBaoShangByStar(int star)
    {
        if (star == 1)
        {
            return 0.5f;
        }
        else if (star == 2)
        {
            return 1f;
        }
        else
        {
            return 1.5f;
        }
    }
    public static string GetEquipTypeName(int type,int childtype)
    {
        if (type == 1)
        {
            return "武";
        }
        if (type == 2)
        {
            if (childtype == 1)
            {
                return "上";
            }
            if (childtype == 2)
            {
                return "中";
            }
            if (childtype == 3)
            {
                return "下";
            }
        }
        return "普";
    }
    public static Color GetEquipColor(int star)
    {
        switch (star)
        {
            case 1: return new Color(216f / 255f, 1f, 189f / 255f); break;
            case 2: return new Color(0f / 255f, 0f, 255f / 255f); break;
            case 3: return new Color(255f/ 255f, 1f, 0f); break;
            case 4: return new Color(1f, 185f, 0f); break;
        }
        return new Color(216f / 255f, 1f, 189f / 255f);
    }

    public static string GetHpBarColor(EntityTypes type)
    {
        switch (type)
        {
            case EntityTypes.BattlePlayerMy: return "pic_yellowbar"; break;
            case EntityTypes.BattlePlayerMyTeam: return "pic_bluebar"; break;
            case EntityTypes.BattlePlayerFriend: return "pic_bluebar"; break;
            case EntityTypes.BattlePlayerDiRen: return "pic_redbar"; break;
            case EntityTypes.BattlePlayerZhongli: return "pic_graybar"; break;
        }
        return "pic_bluebar";
    }

    private static Vector3 GroundPos(Vector3 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(pos.x, pos.y + 0.2f, pos.z), Vector3.down, out hit, 100f,
            1 << Globals.GetDownLayer()))
        {
            return hit.point;
        }
        return new Vector3(10000, 10001, 10000);
    }
}
