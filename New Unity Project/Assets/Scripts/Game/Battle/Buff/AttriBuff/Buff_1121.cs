using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1121 : Buff_Add
{
    public override float GetBaoShangBuffNum()
    {
        return buffVo.param[0] / 1000f;
    }
}