using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_10003 : Skill_FourAtk
{
    protected override void createSkillEffect1()
    {
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        Vector3 pos = new Vector3(0, 0.91f, 0);
        string effect = "attack1";

        Effect attackEffect = skillMgr.GetEffect(effect, ResourceType.scene, pos, new Vector3(0f, 0f, 0f), Vector3.one,
            Globals.normaleffectlayer, playermodel.GetEffectObjName(), 1f);
    }
    protected override void createSkillEffect2()
    {
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        Vector3 pos = new Vector3(0, 0.91f, 0);
        string effect = "lianzhao1";

        Effect attackEffect = skillMgr.GetEffect(effect, ResourceType.scene, pos, new Vector3(0f, 0f, 0f), Vector3.one,
            Globals.normaleffectlayer, playermodel.GetEffectObjName(), 1f);
    }
    protected override void createSkillEffect3()
    {
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        Vector3 pos = new Vector3(0, 0.91f, 0);
        string effect = "lianzhao1";

        Effect attackEffect = skillMgr.GetEffect(effect, ResourceType.scene, pos, new Vector3(0f, 0f, 0f), Vector3.one,
            Globals.normaleffectlayer, playermodel.GetEffectObjName(), 1f);
    }
    protected override void createSkillEffect4()
    {
        PlayerModel playermodel = skillMgr.entity.model as PlayerModel;
        Vector3 pos = playermodel.Pos;
        pos.y += 0.05f;
        string effect = "boom1";

        Effect attackEffect = skillMgr.GetEffect(effect, ResourceType.scene, pos, new Vector3(0f, 0f, 0f), Vector3.one,
            Globals.normaleffectlayer, SceneMgr.GetInstance().nowGameScene.effectRoot.transform, 5f);
    }
}
