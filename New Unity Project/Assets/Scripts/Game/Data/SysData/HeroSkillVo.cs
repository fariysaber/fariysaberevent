using System;
using System.Collections.Generic;
using System.ComponentModel;
[Serializable]
public class HeroSkillVo
{
	[Description("角色id")]
	public string id;
	[Description("跳跃技能")]
	public int jumpSkill;
	[Description("闪避技能")]
	public int crashSkill;
	[Description("被动技能")]
	public List<int> passive;
	[Description("主动技能")]
	public List<int> active;
}

