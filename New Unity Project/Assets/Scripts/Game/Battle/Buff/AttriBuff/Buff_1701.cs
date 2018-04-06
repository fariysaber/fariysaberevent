using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1701 : Buff_Add
{
    public override int GetDefAddpercent()
    {
        return -buffVo.param[0];
    }
    public override int GetAtkAddPercent()
    {
        return -buffVo.param[1];
    }
}