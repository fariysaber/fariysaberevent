using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GameStart : SingletonMonoBehaviour<GameStart>
{
    private void Start()
    {
        StartCoroutine(GameStartLoad());
    }
    IEnumerator GameStartLoad()
    {
        AsyncOperation ao = Application.LoadLevelAsync("Start");
        yield return ao;
        GameObject MainObject = new GameObject("GameGlobals");
        Type type = GameTools.GetType("GameGlobals");
        MainObject.AddComponent(type);
        Destroy(gameObject);
    }
}
