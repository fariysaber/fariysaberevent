using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class EquipVo
{
	[Description("装备id")]
	public string id;
	[Description("装备名字")]
	public string name;
	[Description("武器攻击特效")]
	public string dmgeffect;
	[Description("装备model")]
	public string model;
	[Description("装备图标")]
	public string icon;
	[Description("星级")]
	public int star;
	[Description("类型")]
	public int type;
	[Description("子类型")]
	public int childtype;
	[Description("套装")]
	public List<int> taozhuang;
	[Description("套装参数")]
	public List<List<int>> taozhuangparam;
	[Description("攻击成长")]
	public int atk;
	[Description("防御成长")]
	public int defend;
	[Description("血量成长")]
	public int hp;
	[Description("暴击成长")]
	public int baoji;
	[Description("暴击伤害")]
	public int baojishang;
	[Description("附带buff")]
	public List<int> buffid;
	[Description("效果说明")]
	public string shuxindesc;
	[Description("介绍")]
	public string desctitle;
}

