using System;
using System.Collections.Generic;
using UnityEngine;


//叶子节点
//行为节点
public class UseSkillActionNode : ActionNode
{
    public SkillType skillType = SkillType.normal;
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        bool useSkill = batentity.skillMgr.UseSkillByType(skillType);
        SetCallback(useSkill);
    }
}