using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1001 : Buff_Add
{
    public override int GetAtkAddPercent()
    {
        return buffVo.param[0];
    }
}