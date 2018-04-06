using System;
using System.Collections.Generic;
using UnityEngine;

public class Buff_1131 : Buff_Add
{
    public override int GetChuantou()
    {
        return buffVo.param[0];
    }
}