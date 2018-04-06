using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Drive : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Type type = GetTypeMgr.GetType("GameStart");
        gameObject.AddComponent(type);
    }
}
