using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class LevelShuxinVo
{
	[Description("等级")]
	public string level;
	[Description("生命")]
	public int shengming;
	[Description("攻击")]
	public int gongji;
	[Description("防御")]
	public int fangyu;
	[Description("暴击")]
	public int baoji;
	[Description("暴击伤害")]
	public int baoshang;
	[Description("经验")]
	public int jingyan;
}

