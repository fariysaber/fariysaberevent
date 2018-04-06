using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UI;
//资源优化分类
public class AssetFeileiTools
{
    public Dictionary<string,List<string>> assetDic;
    public AssetFeileiTools()
    {
        assetDic = new Dictionary<string, List<string>>();
    }
}
/// <summary>
/// 慎用移动ui图集中引用多的的资源到common,目前只对image起作用
/// </summary>
public class UIAssetFeileiTools : AssetFeileiTools
{
    public string spritePath = Application.dataPath + "/Abres/Zero/UI/Atlas";
    public string prefabPath = Application.dataPath + "/Abres/Zero/UI/Modules";
    public string commonPath = Application.dataPath + "/Abres/Zero/UI/Atlas/Common";
    public string[] ignoreDirector = { "SingleSprite","Common" };
    public string[] needImageName = { "jpg", "png", "bmp" };
    private int _moveNeedNum = 4;
    public UIAssetFeileiTools(int moveNeedNum = 1) :base()
    {
        this._moveNeedNum = moveNeedNum;
        StartFenlei();
    }
    private void StartFenlei()
    {
        assetDic.Clear();
        DirectoryInfo folder = new DirectoryInfo(spritePath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].FullName.EndsWith(".meta"))
            {
                continue;
            }
            string getDirUrl = FileUtils.Replace(files[i].FullName);
            if (files[i] is DirectoryInfo)
            {
                if (FileUtils.IsInEndsName(getDirUrl, ignoreDirector))
                {
                    continue;
                }
                FenleiAtlas(getDirUrl);
            }
        }
        if (assetDic.Count > 0)
        {
            foreach (var item in assetDic)
            {
                if (item.Value.Count >= _moveNeedNum)
                {
                    //三步骤
                    MoveToCommon(item.Key,item.Value);//移动一个到common路径下
                    RefreshPrefabDepend(item.Key);//刷新prefab的新的依赖到common路径
                    DeleteMoveParentFile(item.Value);//刷新prefab的新的依赖到common路径
                }
            }
        }
        assetDic = null;
    }
    private void FenleiAtlas(string direpath)
    {
        List<string> files = FileUtils.GetAllFiles(direpath);
        for (int i = 0; i < files.Count; i++)
        {
            string getName = FileUtils.GetAssetName(files[i]);
            if (!assetDic.ContainsKey(getName))
            {
                assetDic[getName] = new List<string>();
            }
            if (!assetDic[getName].Contains(files[i]))
            {
                assetDic[getName].Add(files[i]);
            }
        }
    }
    private void MoveToCommon(string name,List<string> path)
    {
        Debugger.Log("重复了超过" + _moveNeedNum + "次的资源：" + name, Color.green);
        string[] str = FileUtils.GetReplaceFilePath(path[0], commonPath);
        if (FileUtils.IsFileExists(str[1]))
        {
            Debugger.Log("已经存在了" + str[1], Color.cyan);
        }
        else
        {
            Debugger.Log(str[0] + "到", Color.cyan);
            Debugger.Log(str[1], Color.cyan);
            FileUtil.ReplaceFile(str[0], str[1]);
            FileUtil.ReplaceFile(str[2], str[3]);
        }
        AssetDatabase.Refresh();
    }
    private void RefreshPrefabDepend(string name)
    {
        string nameNoend = name.Replace(Path.GetExtension(name), "");
        string[] filesPath = Directory.GetFiles(prefabPath, "*.prefab", SearchOption.AllDirectories);
        string resPath = commonPath + "/" + name;
        resPath = FileUtils.GetAssetDataPath(resPath);
        Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(resPath, typeof(Sprite));
        for (int i = 0; i < filesPath.Length; i++)
        {
            filesPath[i] = FileUtils.Replace(filesPath[i]);
            filesPath[i] = FileUtils.GetAssetDataPath(filesPath[i]);
            GameObject gameObject = AssetDatabase.LoadAssetAtPath(filesPath[i], typeof(GameObject)) as GameObject;
            Transform[] form = gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform trans in form)
            {
                ReplaceCommont.ReplaceSprite(trans, sprite, nameNoend, gameObject, resPath);
            }
            AssetDatabase.SaveAssets();
        }
        AssetDatabase.Refresh();
    }

    private void DeleteMoveParentFile(List<string> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            string[] str = FileUtils.GetReplaceFilePath(path[i], commonPath);
            FileUtil.DeleteFileOrDirectory(str[0]);
            FileUtil.DeleteFileOrDirectory(str[2]);
        }
        AssetDatabase.Refresh();
    }
}

public class ReplaceCommont
{
    public static void ReplaceSprite(Transform trans, Sprite sprite, string nameNoend, GameObject parent, string resPath)
    {
        Image getImg = trans.GetComponent<Image>();
        if (getImg && getImg.sprite != null && getImg.sprite.name.Equals(nameNoend))
        {
            getImg.sprite = sprite;
            Debugger.Log("将" + parent.name + "上的 " + getImg.name + "依赖到了" + resPath, Color.green);
        }
    }
}