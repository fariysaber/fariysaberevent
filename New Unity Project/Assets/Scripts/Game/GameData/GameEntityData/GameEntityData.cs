using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// entity信息
/// </summary>
public class GameEntityData
{
    public string entityKey;
    public string name;
    public int pifu;
    public string entityId;
    public int level = 1;
    public int exp = 0;
    public int star = 0;
    public HeroVo herovo;
}
public class GamePlayerEntityData : GameEntityData
{
    public List<PlayerSkillInfo> playerSkillInfo;
    public PlayerEquipInfo wuqi;
    public PlayerEquipInfo lingzhuang1;
    public PlayerEquipInfo lingzhuang2;
    public PlayerEquipInfo lingzhuang3;
    public List<int> GetLingzhuangBuff()
    {
        List<int> buffIds = new List<int>();
        int addIndex = 0;
        for (int i = 0; i < lingzhuang1.vo.taozhuang.Count; i++)
        {
            if (lingzhuang1.vo.taozhuang[i] == lingzhuang2.id)
            {
                if (lingzhuang1.vo.taozhuangparam.Count > 0)
                {
                    buffIds.Add(lingzhuang1.vo.taozhuangparam[0][1]);
                }
                addIndex++;
            }
            if (lingzhuang1.vo.taozhuang[i] == lingzhuang3.id)
            {
                if (lingzhuang1.vo.taozhuangparam.Count > 1)
                {
                    buffIds.Add(lingzhuang1.vo.taozhuangparam[1][1]);
                }
                addIndex++;
            }
        }
        if (addIndex == 0)
        {
            for (int i = 0; i < lingzhuang2.vo.taozhuang.Count; i++)
            {
                if (lingzhuang2.vo.taozhuang[i] == lingzhuang3.id)
                {
                    if (lingzhuang2.vo.taozhuangparam.Count > 0)
                    {
                        buffIds.Add(lingzhuang2.vo.taozhuangparam[0][1]);
                    }
                }
            }
        }
        return buffIds;
    }
    public void AddWuqiInfo(PlayerEquipInfo info)
    {
        wuqi = info;
    }
    public void AddLingzhuang(int i, PlayerEquipInfo info)
    {
        if (i == 1)
        {
            lingzhuang1 = info;
        }
        else if (i == 2)
        {
            lingzhuang2 = info;
        }
        else if (i == 3)
        {
            lingzhuang3 = info;
        }
    }
    public void AddSkill(int id, int level)
    {
        PlayerSkillInfo info = GetSkill(id);
        if (info != null)
        {
            playerSkillInfo.Remove(info);
        }
        PlayerSkillInfo crtInfo = CreateNewInfo(id, level);
        playerSkillInfo.Add(crtInfo);
    }
    public PlayerSkillInfo CreateNewInfo(int id, int level)
    {
        PlayerSkillInfo info = new PlayerSkillInfo();
        info.id = id;
        info.level = level;
        info.skillVo = SkillConfig.GetData(id + "");
        return info;
    }
    public PlayerSkillInfo GetSkill(int id)
    {
        for (int i = 0; i < playerSkillInfo.Count; i++)
        {
            if (playerSkillInfo[i].id == id)
            {
                return playerSkillInfo[i];
            }
        }
        return null;
    }
    public void RemoveSkillbyId(int id)
    {
        for (int i = 0; i < playerSkillInfo.Count; i++)
        {
            if (playerSkillInfo[i].id == id)
            {
                playerSkillInfo.RemoveAt(i);
            }
        }
    }
    public List<PlayerSkillInfo> GetShowSkillInfo()
    {
        List<PlayerSkillInfo> skillList = new List<PlayerSkillInfo>();
        for (int i = 0; i < playerSkillInfo.Count; i++)
        {
            if (playerSkillInfo[i].skillVo.type == (int)SkillType.normal || playerSkillInfo[i].skillVo.type == (int)SkillType.get ||
                playerSkillInfo[i].skillVo.type == (int)SkillType.juexing)
            {
                skillList.Add(playerSkillInfo[i]);
            }
        }
        return skillList;
    }
    public int GetHeroNextExp()
    {
        if(level >= Globals.maxLevel)
        {
            return -1;
        }
        LevelShuxinVo vo = LevelShuxinConfig.GetData(level + "");
        return vo.jingyan - exp;
    }
    public int nowlevelNeedAllExp()
    {
        if (level >= Globals.maxLevel)
        {
            return -1;
        }
        LevelShuxinVo vo = LevelShuxinConfig.GetData(level + "");
        int lastjingyan = level <= 1 ? 0 : LevelShuxinConfig.GetData((level - 1) + "").jingyan;
        return vo.jingyan - lastjingyan;
    }
    public int GetPingji()
    {
        return herovo.pingji + star;
    }
    //获得生命
    public int GetShengmingpercent()
    {
        int value = herovo.hp;
        for (int i = 0; i < star; i++)
        {
            value += herovo.hpUp[i];
        }
        return value;
    }
    public int GetEquipHp()
    {
        int num = 0;
        if (wuqi != null)
        {
            num += GetLZHp(wuqi, Globals.WuqiLvBeishu);
        }
        if (lingzhuang1 != null && lingzhuang1.suijitype == 3)
        {
            num += GetLZHp(lingzhuang1, Globals.GetLZbeishuByStar(lingzhuang1.vo.star));
        }
        if (lingzhuang2 != null && lingzhuang2.suijitype == 3)
        {
            num += GetLZHp(lingzhuang2, Globals.GetLZbeishuByStar(lingzhuang2.vo.star));
        }
        if (lingzhuang3 != null && lingzhuang3.suijitype == 3)
        {
            num += GetLZHp(lingzhuang3, Globals.GetLZbeishuByStar(lingzhuang3.vo.star));
        }
        //Debug.Log(num);
        return num;
    }
    public int GetLZHp(PlayerEquipInfo info, float beishu)
    {
        return (int)((float)LevelShuxinConfig.GetData(info.level + "").shengming * (float)info.vo.hp / 1000f * beishu);
    }
    public int GetHp()
    {
        int levsm = LevelShuxinConfig.GetData(level + "").shengming;
        levsm *= GetShengmingpercent();
        levsm /= 1000;
        levsm += GetEquipHp();
        return levsm;
    }
    public string GetHpPj()
    {
        return Globals.GetShuxinPJ(GetShengmingpercent(), 100);
    }
    public string GetNextHpPj()
    {
        int value = herovo.hp;
        for (int i = 0; i < star + 1 && i < herovo.hpUp.Count ; i++)
        {
            value += herovo.hpUp[i];
        }
        return Globals.GetShuxinPJ(value, 100);
    }
    public float GetHpShengxing()
    {
        if (herovo.hpUp.Count <= star)
        {
            return 0;
        }
        return herovo.hpUp[star] / 10f;
    }

    //获取攻击
    public int GetAtkPercent()
    {
        int value = herovo.attck;
        for (int i = 0; i < star; i++)
        {
            value += herovo.attckUp[i];
        }
        return value;
    }
    public int GetAtk()
    {
        int levgj = LevelShuxinConfig.GetData(level + "").gongji;
        levgj *= GetAtkPercent();
        levgj /= 1000;
        levgj += GetEquipAtk();
        return levgj;
    }
    public int GetEquipAtk()
    {
        int num = 0;
        if (wuqi != null)
        {
            num += GetLZAtk(wuqi, Globals.WuqiLvBeishu);
        }
        if (lingzhuang1 != null && lingzhuang1.suijitype == 1)
        {
            num += GetLZAtk(lingzhuang1, Globals.GetLZbeishuByStar(lingzhuang1.vo.star));
        }
        if (lingzhuang2 != null && lingzhuang2.suijitype == 1)
        {
            num += GetLZAtk(lingzhuang2, Globals.GetLZbeishuByStar(lingzhuang2.vo.star));
        }
        if (lingzhuang3 != null && lingzhuang3.suijitype == 1)
        {
            num += GetLZAtk(lingzhuang3, Globals.GetLZbeishuByStar(lingzhuang3.vo.star));
        }
        //Debug.Log(num);
        return num;
    }
    public int GetLZAtk(PlayerEquipInfo info, float beishu)
    {
        return (int)((float)LevelShuxinConfig.GetData(info.level + "").gongji * (float)info.vo.atk / 1000f * beishu);
    }
    public string GetAtkPj()
    {
        return Globals.GetShuxinPJ(GetAtkPercent(), 100);
    }
    public float GetAtkShengxing()
    {
        if (herovo.attckUp.Count <= star)
        {
            return 0;
        }
        return herovo.attckUp[star] / 10f;
    }
    public string GetNextAtkPj()
    {
        int value = herovo.attck;
        for (int i = 0; i < star + 1 && i < herovo.attckUp.Count; i++)
        {
            value += herovo.attckUp[i];
        }
        return Globals.GetShuxinPJ(value, 100);
    }
    //获取防御
    public int GetDefendPercent()
    {
        int value = herovo.defend;
        for (int i = 0; i < star; i++)
        {
            value += herovo.defendUp[i];
        }
        return value;
    }
    public int GetDefend()
    {
        int levgj = LevelShuxinConfig.GetData(level + "").fangyu;
        levgj *= GetDefendPercent();
        levgj /= 1000;
        levgj += GetEquipDefend();
        return levgj;
    }
    public int GetEquipDefend()
    {
        int num = 0;
        if (wuqi != null)
        {
            num += GetLZDef(wuqi, Globals.WuqiLvBeishu);
        }
        if (lingzhuang1 != null && lingzhuang1.suijitype == 2)
        {
            num += GetLZDef(lingzhuang1, Globals.GetLZbeishuByStar(lingzhuang1.vo.star));
        }
        if (lingzhuang2 != null && lingzhuang2.suijitype == 2)
        {
            num += GetLZDef(lingzhuang2, Globals.GetLZbeishuByStar(lingzhuang2.vo.star));
        }
        if (lingzhuang3 != null && lingzhuang3.suijitype == 2)
        {
            num += GetLZDef(lingzhuang3, Globals.GetLZbeishuByStar(lingzhuang3.vo.star));
        }
        //Debug.Log(num);
        return num;
    }
    public int GetLZDef(PlayerEquipInfo info, float beishu)
    {
        return (int)((float)LevelShuxinConfig.GetData(info.level + "").fangyu * (float)info.vo.defend / 1000f * beishu);
    }
    public string GetDefendPj()
    {
        return Globals.GetShuxinPJ(GetDefendPercent(), 100);
    }
    public float GetDefendShengxing()
    {
        if (herovo.defendUp.Count <= star)
        {
            return 0;
        }
        return herovo.defendUp[star] / 10f;
    }
    public string GetNextDefendPj()
    {
        int value = herovo.defend;
        for (int i = 0; i < star + 1 && i < herovo.defendUp.Count; i++)
        {
            value += herovo.defendUp[i];
        }
        return Globals.GetShuxinPJ(value, 100);
    }

    //获取暴击率
    public int GetBaoPercent()
    {
        int value = herovo.critRate;
        for (int i = 0; i < star; i++)
        {
            value += herovo.critRateUp[i];
        }
        return value;
    }
    public float GetBao()
    {
        int levgj = LevelShuxinConfig.GetData(level + "").baoji;
        levgj *= GetBaoPercent();
        levgj /= 1000;
        levgj += GetEquipBao();
        return (float)levgj / 10000f;
    }
    public int GetEquipBao()
    {
        int num = 0;
        num += GetLZBbaoji(wuqi, Globals.wuqibaojibeishu);
        if (lingzhuang1 != null && lingzhuang1.suijitype == 4)
        {
            num += GetLZBbaoji(lingzhuang1, Globals.GetLZBaojiByStar(lingzhuang1.vo.star));
        }
        if (lingzhuang2 != null && lingzhuang2.suijitype == 4)
        {
            num += GetLZBbaoji(lingzhuang2, Globals.GetLZBaojiByStar(lingzhuang2.vo.star));
        }
        if (lingzhuang3 != null && lingzhuang3.suijitype == 4)
        {
            num += GetLZBbaoji(lingzhuang3, Globals.GetLZBaojiByStar(lingzhuang3.vo.star));
        }
        //Debug.Log(num);
        return num;
    }
    public int GetLZBbaoji(PlayerEquipInfo info, float beishu)
    {
        return (int)((float)info.vo.baoji * (float)info.level / (float)Globals.EquipUpMaxLevel * beishu);
    }
    public string GetBaojiPj()
    {
        return Globals.GetShuxinPJ(GetBaoPercent(), 200);
    }
    public float GetBaojiShengxing()
    {
        if (herovo.critRateUp.Count <= star)
        {
            return 0;
        }
        return herovo.critRateUp[star] / 10f;
    }
    public string GetNextBaojiPj()
    {
        int value = herovo.critRate;
        for (int i = 0; i < star + 1 && i < herovo.critRateUp.Count; i++)
        {
            value += herovo.critRateUp[i];
        }
        return Globals.GetShuxinPJ(value, 200);
    }
    //获取暴击伤害
    public int GetBaoShangPercent()
    {
        int value = herovo.critDamage;
        for (int i = 0; i < star; i++)
        {
            value += herovo.critDamageUp[i];
        }
        return value;
    }
    public float GetBaoshang()
    {
        int levgj = LevelShuxinConfig.GetData(level + "").baoshang;
        levgj *= GetBaoShangPercent();
        levgj /= 1000;
        levgj += GetEquipBaoshang();
        return (float)levgj / 10000f;
    }
    public int GetEquipBaoshang()
    {
        int num = 0;
        num += GetLZBbaoShang(wuqi, Globals.wuqibaoshangbeishu);
        if (lingzhuang1 != null && lingzhuang1.suijitype == 5)
        {
            num += GetLZBbaoShang(lingzhuang1, Globals.GetLZBaoShangByStar(lingzhuang1.vo.star));
        }
        if (lingzhuang2 != null && lingzhuang2.suijitype == 5)
        {
            num += GetLZBbaoShang(lingzhuang2, Globals.GetLZBaoShangByStar(lingzhuang2.vo.star));
        }
        if (lingzhuang3 != null && lingzhuang3.suijitype == 5)
        {
            num += GetLZBbaoShang(lingzhuang3, Globals.GetLZBaoShangByStar(lingzhuang3.vo.star));
        }
        //Debug.Log(num);
        return num;
    }
    public int GetLZBbaoShang(PlayerEquipInfo info, float beishu)
    {
        return (int)((float)info.vo.baojishang * (float)info.level / (float)Globals.EquipUpMaxLevel * beishu);
    }
    public string GetBaojiShangPj()
    {
        return Globals.GetShuxinPJ(GetBaoShangPercent(), 200);
    }
    public float GetBaoShangShengxing()
    {
        if (herovo.critDamageUp.Count <= star)
        {
            return 0;
        }
        return herovo.critDamageUp[star] / 10f;
    }
    public string GetNextBaoShangPj()
    {
        int value = herovo.critDamage;
        for (int i = 0; i < star + 1 && i < herovo.critDamageUp.Count; i++)
        {
            value += herovo.critDamageUp[i];
        }
        return Globals.GetShuxinPJ(value, 200);
    }
    //穿透
    public float GetEquipChuantou()
    {
        return 0;
    }
    public float GetChuantou()
    {
        float chuantou = GetEquipChuantou();
        return chuantou;
    }
    //减伤
    public float GetEquipGedang()
    {
        return 0;
    }
    public float GetGedang()
    {
        float gedang = GetEquipGedang();
        return gedang;
    }
    public int baojiShBeishu = 2;
    //战斗力
    public int GetZhandouli()
    {
        float zhandouli = GetHp() * 0.1f + GetAtk() + GetDefend() * 2 + (GetAtk() * GetBao()) * (baojiShBeishu + GetBaoshang());
        Debug.Log(GetHp());
        Debug.Log(GetAtk());
        Debug.Log(GetDefend());
        Debug.Log((GetAtk() * GetBao()));
        return (int)zhandouli;
    }
}
/// <summary>
/// entity技能信息
/// </summary>
public class PlayerSkillInfo
{
    public int id;
    public int level;
    public SKillVo skillVo;
}
public class PlayerEquipInfo
{
    public string key;
    public int id;
    public int level;
    public EquipVo vo;
    public int suijitype = 0;
}
public enum SkillType
{
    normal = 1,
    get = 2,
    juexing = 3,
    crash = 7,
    jump = 8,
    move = 9,
}