using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class CharactorVo
{
	[Description("外表id")]
	public string id;
	[Description("全身像")]
	public string body;
	[Description("头像")]
	public string head;
	[Description("半身像")]
	public string halfbody;
	[Description("模型")]
	public string model;
	[Description("战斗模型")]
	public string batmodel;
}

