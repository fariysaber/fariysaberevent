using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class HeroVo
{
	[Description("角色id")]
	public string id;
	[Description("角色性别(1男2女)")]
	public int sex;
	[Description("名字")]
	public string name;
	[Description("人物外表id（没有数据默认选第一个)")]
	public List<int> charactor;
	[Description("人物class")]
	public string className;
	[Description("类型")]
	public int type;
	[Description("战斗定位")]
	public int battleType;
	[Description("初始评级")]
	public int pingji;
	[Description("人物属性")]
	public int shuxin;
	[Description("移速成长")]
	public int speed;
	[Description("暴击率成长")]
	public int critRate;
	[Description("暴击伤害成长")]
	public int critDamage;
	[Description("生命恢复成长")]
	public int hprecover;
	[Description("生命成长")]
	public int hp;
	[Description("能量恢复成长")]
	public int energyrecover;
	[Description("能量成长")]
	public int energy;
	[Description("攻击成长")]
	public int attck;
	[Description("防御成长")]
	public int defend;
	[Description("评级提升移速成长")]
	public List<int> speedUp;
	[Description("评级提升暴击率成长")]
	public List<int> critRateUp;
	[Description("评级提升暴击伤害成长")]
	public List<int> critDamageUp;
	[Description("评级提升生命恢复成长")]
	public List<int> hprecoverUp;
	[Description("评级提升生命成长")]
	public List<int> hpUp;
	[Description("评级提升能量恢复成长")]
	public List<int> energyrecoverUp;
	[Description("评级提升能量成长")]
	public List<int> energyUp;
	[Description("评级提升攻击成长")]
	public List<int> attckUp;
	[Description("评级提升防御成长")]
	public List<int> defendUp;
	[Description("可提升评级次数")]
	public int pingfenUpNum;
	[Description("对应英雄技能表id")]
	public int heroSkillId;
}

