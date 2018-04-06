using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class CommonToolUtil : Editor
{
    #region 一些基础的工具接口
    public static string[] GetDependByOpenFile()
    {
        string path = EditorUtility.OpenFilePanel("选择文件", "", "");
        if(!path.Contains("Assets"))
        {
            return new string[0];
        }
        path = FileUtils.GetAssetDataPath(path);
        string[] str = AssetDatabase.GetDependencies(path);
        return str;
    }
    public static Dictionary<string, string[]> GetDependByOpenFolder()
    {
        Dictionary<string, string[]> list = new Dictionary<string, string[]>();
        string path = EditorUtility.OpenFolderPanel("选择文件夹", "", "");
        List<string> filePaths = FileUtils.GetAllFiles(path);
        for (int i = 0; i < filePaths.Count; i++)
        {
            if (!path.Contains("Assets"))
            {
                continue;
            }
            string assetPath = FileUtils.GetAssetDataPath(filePaths[i]);
            list[assetPath] = AssetDatabase.GetDependencies(assetPath);
        }
        return list;
    }
    #endregion

    #region 刷新ui图集信息
    public static string spritePath = Application.dataPath + "/Abres/Zero/UI/Atlas";
    public static string[] SingleSprite = { "SingleSprite"};
    public static string[] needImageName = {"jpg","png","bmp"};
    public static void SetAllAtlas()
    {
        DirectoryInfo folder = new DirectoryInfo(spritePath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].FullName.EndsWith(".meta"))
            {
                continue;
            }
            string getDirUrl = FileUtils.Replace(files[i].FullName);
            string name = files[i].Name;
            if (files[i] is DirectoryInfo)
            {
                if (FileUtils.IsInEndsName(files[i].Name, SingleSprite))
                {
                    name = "";
                }
                SetAtlas(getDirUrl, name);
            }
            else
            {
                SetFileSprite(getDirUrl, "");
            }
        }
    }
    public static void SetAtlas(string direpath,string packName)
    {
        List<string> files = FileUtils.GetAllFiles(direpath);
        for (int i = 0; i < files.Count; i++)
        {
            SetFileSprite(files[i], packName);
        }
    }
    public static void SetFileSprite(string path, string packName)
    {
        if (!FileUtils.IsInEndsName(path, needImageName))
        {
            return;
        }
        path = FileUtils.GetAssetDataPath(path);
        Debugger.Log("转换" + path + "....tag...." + packName, Color.green);
        TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
        texture.textureType = TextureImporterType.Sprite;
        texture.spritePackingTag = packName;
        texture.maxTextureSize = 1024;
        texture.filterMode = FilterMode.Trilinear;
        texture.mipmapEnabled = false;
        AssetDatabase.ImportAsset(path);
    }

    public static void SetFileSpriteByPath(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].FullName.EndsWith(".meta"))
            {
                continue;
            }
            string getDirUrl = FileUtils.Replace(files[i].FullName);
            string name = files[i].Name;
            if (files[i] is DirectoryInfo)
            {
                SetFileSpriteByPath(getDirUrl);
            }
            else
            {
                SetFileSprite(getDirUrl, "");
            }
        }
    }
    #endregion

    #region 获取资源的依赖相关
    public static void GetDirPrefabDependPath(string path,string[] endName)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].FullName.EndsWith(".meta"))
            {
                continue;
            }
            if (!FileUtils.IsInEndsName(files[i].FullName, endName))
            {
                continue;
            }
            string getDirUrl = FileUtils.Replace(files[i].FullName);
            if (files[i] is DirectoryInfo)
            {
                GetDirPrefabDependPath(getDirUrl, endName);
            }
            else
            {
                GetDependPathName(getDirUrl);
            }
        }
    }
    public static void GetDependPathName(string path)
    {
        path = FileUtils.GetAssetDataPath(path);
        string[] dependRes = AssetDatabase.GetDependencies(path);
        if (dependRes.Length > 1)
        {
            Debugger.Log(path + "资源资源", Color.yellow);
        }
        for (int i = 0; i < dependRes.Length; i++)
        {
            if (path.Equals(dependRes[i]))
            {
                continue;
            }
            if (FileUtils.IsInEndsName(dependRes[i], needImageName))
            {
                TextureImporter texture = AssetImporter.GetAtPath(dependRes[i]) as TextureImporter;
                if (texture.spritePackingTag != "")
                {
                    Debugger.Log("资源的图片资源图集" + texture.spritePackingTag, Color.green);
                    continue;
                }
            }
            Debugger.Log("依赖的单个资源" + dependRes[i], Color.green);
        }
    }
    #endregion

    /// <summary>
    /// 根据文件获取其父目录下的文件路径
    /// </summary>
    public static List<string> GetFilePathByDir(string path)
    {
        List<string> pathList = new List<string>();
        path = FileUtils.AssetsToTruePath(path);
        DirectoryInfo folder = new DirectoryInfo(path);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].FullName.EndsWith(".meta"))
            {
                continue;
            }
            string getDirUrl = FileUtils.Replace(files[i].FullName);
            string name = files[i].Name;
            if (files[i] is DirectoryInfo)
            {

            }
            else
            {
                getDirUrl = FileUtils.GetAssetDataPath(getDirUrl);
                pathList.Add(getDirUrl);
            }
        }
        return pathList;
    }
}
