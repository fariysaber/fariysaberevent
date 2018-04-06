using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class BulletVo
{
	[Description("子弹id")]
	public string id;
	[Description("子弹模型")]
	public string model;
	[Description("运动轨迹")]
	public int movetype;
	[Description("运动速度")]
	public int movespeed;
	[Description("持续时间")]
	public int movetime;
	[Description("触发判断类型")]
	public int damagetype;
	[Description("触发判断类型参数")]
	public List<List<string>> damagetypeparam;
	[Description("对于同一个人数判断次数")]
	public int damageplayernum;
	[Description("判断次数")]
	public int damagemaxnum;
	[Description("最多判断人数")]
	public int damagemaxplayer;
	[Description("是否跟随主角")]
	public int follow;
	[Description("是否武器特效")]
	public int iswuqieffect;
	[Description("特效名")]
	public string damageeffect;
	[Description("开始音效")]
	public string startsound;
	[Description("触发音效")]
	public string damagesound;
	[Description("开始buff")]
	public int startbuff;
	[Description("触发buff")]
	public int damagebuff;
	[Description("伤害最大范围")]
	public int damagerange;
	[Description("伤害攻击比例")]
	public int atkdmg;
	[Description("伤害防御比例")]
	public int defenddmg;
	[Description("伤害生命比例")]
	public int hpdmg;
	[Description("伤害暴击比例")]
	public int baojidmg;
	[Description("伤害暴伤比例")]
	public int baoshangdmg;
}

