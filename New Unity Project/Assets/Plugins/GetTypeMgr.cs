using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class GetTypeMgr
{
    public static Type GetType(string strName)
    {
        Type kType = null;
        Assembly kAssem;
        if (kType == null)
        {
            kAssem = AppDomain.CurrentDomain.Load("UnityEngine");
            kType = kAssem.GetType(strName);
            if (kType == null)
            {
                kType = kAssem.GetType("UnityEngine." + strName);
            }
        }
        if (kType == null)
        {
            kAssem = AppDomain.CurrentDomain.Load("Assembly-CSharp");
            kType = kAssem.GetType(strName);
        }
        if (kType == null)
        {
            kAssem = AppDomain.CurrentDomain.Load("Assembly-CSharp-firstpass");
            kType = kAssem.GetType(strName);
        }
        if (kType == null)
        {
            kAssem = AppDomain.CurrentDomain.Load("System");
            kType = kAssem.GetType(strName);
        }
        return kType;
    }
}
