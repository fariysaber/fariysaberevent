using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class itemVo
{
	[Description("道具id")]
	public string id;
	[Description("道具名字")]
	public string name;
	[Description("道具图标")]
	public string icon;
	[Description("效果类型")]
	public int type;
	[Description("效果参数1")]
	public int typeparam1;
}

