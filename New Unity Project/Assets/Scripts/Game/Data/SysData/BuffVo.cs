using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class BuffVo
{
	[Description("buffid")]
	public string id;
	[Description("buff名字")]
	public string name;
	[Description("buff图标")]
	public string icon;
	[Description("buff描述")]
	public string desc;
	[Description("buff特效")]
	public string effect;
	[Description("buff类型")]
	public int buffType;
	[Description("是否触发点")]
	public int ischufapos;
	[Description("参数")]
	public List<int> param;
	[Description("持续时间")]
	public int time;
	[Description("中相同buffcd时间")]
	public int cd;
	[Description("buff动作")]
	public string buffaction;
	[Description("buff打断类型")]
	public int breaktype;
	[Description("buff并行类型")]
	public List<int> parallel;
	[Description("buff打断列表")]
	public List<int> breakList;
}

