using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1111 : Buff_Add
{
    public override float GetBaojiBuffNum()
    {
        return buffVo.param[0] / 1000f;
    }
}