using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

public class CommonTool : Editor
{
    [MenuItem("Assets/打开UI编辑场景")]
    public static void OpenUIEditor()
    {
        EditorSceneManager.OpenScene("Assets/AbResTemp/testScene/UIMyScene.unity");
    }
    [MenuItem("Assets/打开游戏进入场景")]
    public static void OpenMainSceneEditor()
    {
        EditorSceneManager.OpenScene("Assets/Resources/sceneResource/Drive.unity");
    }
    [MenuItem("fariySaber编辑器/公共工具/获取引用的图片名和位置/所选择实例")]
    public static void GetImageDependBySelect()
    {
        UnityEngine.Object getObj = Selection.activeObject;
        GameObject gameObject = getObj as GameObject;
        Transform[] form = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform trans in form)
        {
            Image getImg = trans.GetComponent<Image>();
            if (getImg)
            {
                Debug.Log(getImg.sprite.name);
            }
        }
    }

    [MenuItem("Assets/render名字")]
    public static void GetRenderName()
    {
        UnityEngine.Object getObj = Selection.activeObject;
        GameObject gameObject = getObj as GameObject;
        Transform[] form = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform trans in form)
        {
            Renderer renderer = trans.GetComponent<Renderer>();
            if (renderer)
            {
                Debug.Log(renderer.name);
            }
        }
    }

    [MenuItem("fariySaber编辑器/公共工具/获取依赖/所选择资源")]
    public static void GetDependBySelect()
    {
        UnityEngine.Object getObj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(getObj);
        if (path.Equals(""))
        {
            EditorUtility.DisplayDialog("提示", "所选资源必须是Assets目录资源", "关闭", "");
        }
        else
        {
            string[] dependPaths = AssetDatabase.GetDependencies(path);
            Debugger.Log("所选资源依赖为" + dependPaths.Length + "个",Color.green);
            for (int i = 0; i < dependPaths.Length; i++)
            {
                Debugger.Log("依赖分别为" + dependPaths[i], Color.yellow);
            }
        }
    }

    [MenuItem("Assets/所选择资源依赖")]
    public static void GetAssetDependBySelect()
    {
        UnityEngine.Object getObj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(getObj);
        if (path.Equals(""))
        {
            EditorUtility.DisplayDialog("提示", "所选资源必须是Assets目录资源", "关闭", "");
        }
        else
        {
            string[] dependPaths = AssetDatabase.GetDependencies(path);
            Debugger.Log("所选资源依赖为" + dependPaths.Length + "个", Color.green);
            for (int i = 0; i < dependPaths.Length; i++)
            {
                Debugger.Log("依赖分别为" + dependPaths[i], Color.yellow);
            }
        }
    }

    [MenuItem("Assets/所选择目录相同的依赖")]
    public static void GetAssetDependByDir()
    {
        UnityEngine.Object getObj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(getObj);
        List<string> pathList = CommonToolUtil.GetFilePathByDir(path);
        Dictionary<string, int> dependPathList = new Dictionary<string, int>();
        for (int i = 0; i < pathList.Count; i++)
        {
            string[] dependPaths = AssetDatabase.GetDependencies(pathList[i]);
            for (int j = 0; j < dependPaths.Length; j++)
            {
                if (dependPathList.ContainsKey(dependPaths[j]))
                {
                    dependPathList[dependPaths[j]] += 1;
                }
                else
                {
                    dependPathList[dependPaths[j]] = 1;
                }
            }
        }
        foreach (var item in dependPathList)
        {
            Debug.Log(item.Key);
            Debug.Log(item.Value);
        }
    }

    [MenuItem("fariySaber编辑器/公共工具/获取依赖/获取打开文件依赖")]
    public static void GetDependByOpenFile()
    {
        string[] dependPaths = CommonToolUtil.GetDependByOpenFile();
        Debugger.Log("所选资源依赖为" + dependPaths.Length + "个", Color.green);
        for (int i = 0; i < dependPaths.Length; i++)
        {
            Debugger.Log("依赖分别为" + dependPaths[i], Color.yellow);
        }
    }
    [MenuItem("fariySaber编辑器/公共工具/获取依赖/获取打开目录下所有依赖")]
    public static void GetDependByOpenFolder()
    {
        Dictionary<string, string[]> dependPaths = CommonToolUtil.GetDependByOpenFolder();
        foreach (var item in dependPaths)
        {
            Debugger.Log(item.Key + "所选资源依赖为" + item.Value.Length + "个", Color.green);
            for (int i = 0; i < item.Value.Length; i++)
            {
                Debugger.Log("依赖分别为" + item.Value[i], Color.yellow);
            }
        }
    }
    [MenuItem("fariySaber编辑器/ui图集相关/刷新所有sprite")]
    public static void RefreshAllAtlas()
    {
        CommonToolUtil.SetAllAtlas();
    }

    [MenuItem("fariySaber编辑器/ui图集相关/刷新所有Armature")]
    public static void RefreshAllArmature()
    {
        CommonToolUtil.SetFileSpriteByPath(Application.dataPath + "/Abres/Zero/UI/Armature");
    }

    public static string uiprefabPath = Application.dataPath + "/Abres/Zero/UI/Modules";
    public static string[] needUIPrefabName = { "prefab" };
    [MenuItem("fariySaber编辑器/ui图集相关/获取uiprefab的依赖图集")]
    public static void GetUIPrefabAtlas()
    {
        CommonToolUtil.GetDirPrefabDependPath(uiprefabPath, needUIPrefabName);
    }

    [MenuItem("fariySaber编辑器/ui图集相关/分类ui资源(重复多的移动到common)")]
    public static void GetFenleiUIAtlas()
    {
        new UIAssetFeileiTools();
    }
}
