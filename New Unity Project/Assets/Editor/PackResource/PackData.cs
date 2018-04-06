using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class PackLogic 
{
    public static string AssetBundlesOutputPath = "Assets/StreamingAssets/" + "abres";

    private static string fontResource = Application.dataPath + "/AbRes/Zero/Font";
    public static void SetFontAssetName()
    {
        List<string> endsName = new List<string>();
        endsName.Add(".ttf");
        SearchDirectory(fontResource, endsName);
    }
    private static string movieResource = Application.dataPath + "/AbRes/Zero/Movie";
    public static void SetMovieAssetName()
    {
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(fontResource, endsName);
    }
    private static string uiResource = Application.dataPath + "/AbRes/Zero/UI/";
    public static void SetUIAsset()
    {
        string uiurl = uiResource + "Armature";
        SetPrefabByDir(uiurl);
        uiurl = uiResource + "Modules";
        SetPrefabByDir(uiurl);

        //sprite资源特殊特殊
        string atlasUrl = uiResource + "Atlas";
        List<string> endsName = new List<string>();
        endsName.Add(".png");
        endsName.Add(".jpg");
        endsName.Add(".bmp");
        List<string> endsDirName = new List<string>();
        endsDirName.Add("SingleSprite");

        DirectoryInfo folder = new DirectoryInfo(atlasUrl);
        FileSystemInfo[]  files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (!FileUtils.IsInEndsName(files[i].FullName, endsName) && !(files[i] is DirectoryInfo))
            {
                continue;
            }
            string url = FileUtils.Replace(files[i].FullName);
            if (files[i] is DirectoryInfo)
            {
                if (files[i] is DirectoryInfo && FileUtils.IsInEndsName(files[i].FullName, endsDirName))
                {
                    url = "";
                }
                SearchDirectory(files[i].FullName, endsName, url);
            }
            else
            {
                File(url, url);
            }
        }
    }

    private static void SetPrefabByDir(string path)
    {
        List<string> endsName = new List<string>();
        endsName.Add(".prefab");

        DirectoryInfo folder = new DirectoryInfo(path);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (!FileUtils.IsInEndsName(files[i].FullName, endsName) && !(files[i] is DirectoryInfo))
            {
                continue;
            }
            string url = FileUtils.Replace(files[i].FullName);
            if (files[i] is DirectoryInfo)
            {
                SearchDirectory(files[i].FullName, endsName, url);
            }
            else
            {
                File(url, url);
            }
        }
    }

    private static string configResource = Application.dataPath + "/AbRes/Zero/XML/";
    public static void SetConfigAssetName()
    {
        string resource = configResource + "Config";
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(resource, endsName);
    }
    public static void SetBinDataAssetName()
    {
        string resource = configResource + "BinData";
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(resource, endsName);
    }

    private static string sceneResource = Application.dataPath + "/AbRes/Zero/Scene";
    public static void SetSceneAssetName()
    {
        string resource = sceneResource;
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(resource, endsName);
    }

    public static string modelResource = Application.dataPath + "/AbRes/Zero/model";
    public static void SetModelReource()
    {
        string resource = modelResource;
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(resource, endsName);
    }

    public static string equipResource = Application.dataPath + "/AbRes/Zero/equip";
    public static void SetEquipReource()
    {
        string resource = equipResource;
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(resource, endsName);
    }

    public static string effectResource = Application.dataPath + "/AbRes/Zero/effect";
    public static void SetEffectReource()
    {
        string resource = effectResource;
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(resource, endsName);
    }

    public static string ainodeesource = Application.dataPath + "/AbRes/Zero/AiNode";
    public static void SetAiReource()
    {
        string resource = ainodeesource;
        List<string> endsName = new List<string>();
        endsName.Add("all");
        SearchDirectory(resource, endsName);
    }

    private static void SearchDirectory(string filePath,List<string> endsName,string getpackName = "")
    {
        DirectoryInfo folder = new DirectoryInfo(filePath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (!FileUtils.IsInEndsName(files[i].FullName, endsName) && !(files[i] is DirectoryInfo))
            {
                continue;
            }
            string url = FileUtils.Replace(files[i].FullName);
            string urlName = FileUtils.Replace(files[i].Name);
            string packName = getpackName == "" ? url : getpackName + ".ab";
            if (files[i] is DirectoryInfo)
            {
                SearchDirectory(files[i].FullName, endsName, getpackName);
            }
            else
            {
                File(url, packName);
            }
        }
    }

    private static void File(string url,string packName)
    {
        string _assetPath = "Assets" + url.Substring(Application.dataPath.Length);
        string _assetPath2 = packName.Substring(Application.dataPath.Length + 1);

        //在代码中给资源设置AssetBundleName  
        AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath);
        string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
        assetName = assetName.Replace(Path.GetExtension(assetName), ".ab");
        if (assetImporter != null)
        {
            assetImporter.assetBundleName = assetName;
        }
    }
    public static void SearchAll()
    {
        SetFontAssetName();
        SetMovieAssetName();
        SetUIAsset();
        SetConfigAssetName();
        SetBinDataAssetName();
        SetSceneAssetName();
        SetModelReource();
        SetEquipReource();
        SetEffectReource();
        SetAiReource();
    }
    public static void PackAll()
    {
        AssetDatabase.Refresh();
        ClearAssetBundlesName();
        SearchAll();
        string outputPath = AssetBundlesOutputPath;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();

        Debug.Log("打包完成");
    }

    private static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }
        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
    }
}
