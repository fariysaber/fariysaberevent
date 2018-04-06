using System;
using System.Collections.Generic;
using UnityEngine;
//transform处理相关类
public class TransformUtils
{
    public static void SetParent(Transform child,Transform parent,bool reset = true)
    {
        if (child != null && parent != null)
        {
            child.SetParent(parent);
        }
        if(reset)
            ResetTransform(child.gameObject);
    }
    public static void ResetTransform(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.Euler(0,0,0);
    }
    public static Vector3 RotateByDir(Vector3 source, Vector3 dir)
    {
        if (dir == Vector3.back)
        {
            return new Vector3(-source.x, source.y, source.z);
        }
        else if (dir == Vector3.forward)
        {
            return source;
        }
        else
        {
            return Quaternion.FromToRotation(Vector3.forward, dir) * source;
        }
    }
    public static Component AddComponent(GameObject gameObj, string name)
    {
        Type type = GameTools.GetType(name);
        Component com = gameObj.GetComponent(type);
        if (com == null)
        {
            com = gameObj.AddComponent(type);
        }
        return com;
    }
    public static GameObject FindOrCreateObject(string name, bool initPos = false, bool initScale = false)
    {
        GameObject findObject = GameObject.Find(name);
        if (findObject == null)
        {
            findObject = new GameObject(name);
        }
        if (initPos)
        {
            findObject.transform.position = Vector3.zero;
        }
        if (initScale)
        {
            findObject.transform.localScale = Vector3.one;
        }
        return findObject;
    }
    public static GameObject CreateGameObject(Transform crtObject, Transform parent,
        Vector3 pos, Vector3 scale, string name)
    {
        GameObject getObject = null;
        if (crtObject == null)
        {
            getObject = new GameObject();
        }
        else
        {
            getObject = GameObject.Instantiate(crtObject.gameObject).gameObject;
        }
        getObject.transform.SetParent(parent);
        getObject.transform.localPosition = pos;
        getObject.transform.localScale = scale;
        getObject.transform.localRotation = Quaternion.identity;
        getObject.name = name;
        return getObject;
    }
    public static void SetVisible(GameObject obj, bool value)
    {
        if (obj != null && obj.activeSelf != value)
        {
            obj.SetActive(value);
        }
    }
    public static void DestroyGameObject(GameObject obj, bool immediate = true)
    {
        if (obj == null)
        {
            return;
        }
        if (immediate)
        {
            GameObject.DestroyImmediate(obj);
        }
        else
        {
            GameObject.Destroy(obj);
        }
    }
    public static Transform GetObjectByName(GameObject obj, string name)
    {
        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.ToLower().Equals(name.ToLower()))
            {
                return child;
            }
        }
        return null;
    }
    public static void ResetObjectLayer(GameObject obj, int layer)
    {
        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = layer;
        }
    }
    public static bool IsInGround(Transform transform)
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = -Vector3.up * 3f;
        RaycastHit hitInfo;
        return Physics.Raycast(ray,out hitInfo, 0.4f, Globals.GetDownLayer());
    }
}
