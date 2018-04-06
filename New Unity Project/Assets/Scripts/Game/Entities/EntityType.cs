using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityTypes
{
    Default = 0,//普通实体，只起显示作用
    PlayerType = 1,//人物类型
    Plant = 2,//植物类型
    building = 3,//建筑类型,
    BattlePlayerType = 4,//战斗人物
    BattlePlayerMy = 401,//战斗控制自己
    BattlePlayerMyTeam = 402, // 战斗控制成员,
    BattlePlayerFriend = 411,//友方
    BattlePlayerDiRen = 421,//战斗敌方
    BattlePlayerZhongli = 431,//战斗中立
}
