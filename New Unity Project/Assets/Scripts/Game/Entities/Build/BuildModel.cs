using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildModel : BaseModel
{
    //是否已经毁坏
    private bool isDead = false;
    public bool IsDead
    {
        set { isDead = value; }
        get { return isDead; }
    }
}