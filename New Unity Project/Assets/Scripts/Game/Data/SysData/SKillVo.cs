using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class SKillVo
{
	[Description("技能id")]
	public string id;
	[Description("技能名字")]
	public string name;
	[Description("技能类型")]
	public int type;
	[Description("技能图标")]
	public string icon;
	[Description("技能描述")]
	public string desc;
	[Description("技能最高等级")]
	public int maxlevel;
	[Description("技能升级强化参数")]
	public List<List<int>> levelUp;
	[Description("使用消耗数据")]
	public List<List<int>> costParam;
	[Description("参数")]
	public List<List<string>> param;
	[Description("技能cd换算时间")]
	public int cdhuansuan;
	[Description("技能cd")]
	public int cd;
	[Description("前摇声效")]
	public string castsound;
	[Description("持续声效")]
	public string middlesound;
	[Description("后摇声效")]
	public string backsound;
	[Description("特效是否跟随")]
	public int follow;
	[Description("前摇特效")]
	public string casteffect;
	[Description("持续期特效")]
	public string middleffect;
	[Description("后摇特效")]
	public string backeffect;
	[Description("前摇动作")]
	public string castaction;
	[Description("持续期动作")]
	public string middleaction;
	[Description("后摇动作")]
	public string backaction;
	[Description("打断类型")]
	public int breakType;
	[Description("前摇时间")]
	public int casttime;
	[Description("持续期时间")]
	public int middletime;
	[Description("后摇时间")]
	public int backtime;
	[Description("前摇可打断类型")]
	public List<int> castBreak;
	[Description("持续期可打断类型")]
	public List<int> middleBreak;
	[Description("后摇可打断类型")]
	public List<int> backBreak;
	[Description("前摇并行类型")]
	public List<int> castparallel;
	[Description("持续期并行类型")]
	public List<int> middleparallel;
	[Description("后摇并行类型")]
	public List<int> backparallel;
	[Description("解锁方式")]
	public List<int> unLock;
	[Description("攻击子弹id")]
	public int bullet;
}

