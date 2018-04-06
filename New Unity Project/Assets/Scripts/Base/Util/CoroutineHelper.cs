using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//协程部分
public class CoroutineHelper : Singleton<CoroutineHelper>
{
    sealed class CoroutineCore : MonoBehaviour
    {
        void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
    private GameObject _coroutineHelperObj = null;
    private static string DEFAULT_COROUTINEHELPER = "MainCoroutine";
    private Dictionary<string, CoroutineCore> _core = new Dictionary<string, CoroutineCore>();
    public CoroutineHelper()
    {
        _coroutineHelperObj = new GameObject("CoroutineHelper");
        UnityEngine.Object.DontDestroyOnLoad(_coroutineHelperObj);
    }
    public Coroutine StartCoroutine(string coreName, IEnumerator routine)
    {
        CoroutineCore curCore = GetOrCreateCoroutineCore(coreName);
        return curCore.StartCoroutine(routine);
    }
    public void StopCoroutine(string coreName, IEnumerator routine)
    {
        CoroutineCore curCore = GetCoroutineCore(coreName);
        if (curCore != null)
        {
            curCore.StopCoroutine(routine);
        }
    }
    public void StopAllCoroutines(string coreName)
    {
        CoroutineCore curCore = GetCoroutineCore(coreName);
        if (curCore != null)
        {
            curCore.StopAllCoroutines();
        }
    }
    public void StopAllCoroutines()
    {
        foreach (CoroutineCore curcore in _core.Values)
        {
            curcore.StopAllCoroutines();
        }
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return StartCoroutine(DEFAULT_COROUTINEHELPER, routine);
    }
    public void StopCoroutine(IEnumerator routine)
    {
        StopCoroutine(DEFAULT_COROUTINEHELPER, routine);
    }
    public void DestroyCoroutineCore(string coreName)
    {
        if (_core.ContainsKey(coreName))
        {
            CoroutineCore curCore = _core[coreName];
            curCore.StopAllCoroutines();
            GameObject.DestroyImmediate(curCore.gameObject);
            _core.Remove(coreName);
        }
    }
    private CoroutineCore GetCoroutineCore(string name)
    {
        CoroutineCore cortine = null;
        _core.TryGetValue(name, out cortine);
        return cortine;
    }
    private CoroutineCore CreateGoroutineCore(string name)
    {
        GameObject curCorouObj = new GameObject(name);
        curCorouObj.transform.SetParent(_coroutineHelperObj.transform);

        CoroutineCore cortine = curCorouObj.AddComponent<CoroutineCore>();
        _core.Add(name, cortine);
        return cortine;
    }
    private CoroutineCore GetOrCreateCoroutineCore(string coreName)
    {
        CoroutineCore cucor = GetCoroutineCore(coreName);
        if (cucor == null)
        {
            cucor = CreateGoroutineCore(coreName);
        }
        return cucor;
    }
}