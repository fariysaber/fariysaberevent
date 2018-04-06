using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents
{
    public static readonly string SCENE_ENTER = "scene_enter";

    //操作相关
    public static readonly string MAINHERO_MOVE = "movehero_move";
    public static readonly string MAINHERO_JUMP = "movehero_jump";
    public static readonly string MAINHERO_USESKILL1 = "movehero_juseskilll";
    public static readonly string MAINHERO_USESKILL2 = "movehero_juseskill2";
    public static readonly string MAINHERO_USESKILL3 = "movehero_juseskill3";
    public static readonly string RESET_MAINCAMERAL = "reset_maincamera";

    //UI相关
    //战斗界面
    public static readonly string BATTLE_HEROICON = "battle_heroicon";
    public static readonly string BATTLE_HEROBUFF = "battle_herobuff";
}
