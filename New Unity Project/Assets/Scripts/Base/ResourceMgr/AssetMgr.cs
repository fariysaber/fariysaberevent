using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetMgr
{
    public delegate void AssetLoadComplete();
    private static readonly List<AssetLoader> m_FreeAssetLoaders = new List<AssetLoader>();//保证只有1个AssetLoader
    private static readonly List<AssetRecord> m_WaitAssetDataList = new List<AssetRecord>();
    private static bool m_isInit;

    private static void Init()
    {
        var item = new AssetLoader();
        AssetLoader.LoadComplete complete = LoadAssetComplete;
        item.LoadCompleteFunction = complete;
        AssetLoader.LoadError error = LoadError;
        item.LoadErrorFunction = error;
        m_FreeAssetLoaders.Add(item);
    }

    public static void AddLoad<T>(string url, AssetLoader.LoadComplete complete,AssetLoader.LoadError error = null)
    {
        if (!m_isInit)
        {
            Init();
            m_isInit = true;
        }
        AssetRecord assetRecord = new AssetRecord
        {
            allUrl = GetAllUrl(url),
            fileUrl = url,
            type = typeof(T),
            loadComplete = complete,
            loadError = error
        };
        lock (m_WaitAssetDataList)
        {
            m_WaitAssetDataList.Add(assetRecord);
        }
        StartLoad();
    }

    public static void CancleLoad<T>(string url)
    {
        lock (m_WaitAssetDataList)
        {
            for (int i = 0; i < m_WaitAssetDataList.Count; i++)
            {
                AssetRecord assetRecord = m_WaitAssetDataList[i];
                if (assetRecord.fileUrl == url)
                {
                    assetRecord.RemoveComplete();
                    assetRecord.RemoveError();
                    return;
                }
            }
        }
    }

    public static void Update()
    {
        StartLoad();
    }

    public static void StartLoad()
    {
        if (m_FreeAssetLoaders.Count == 0 || m_WaitAssetDataList.Count == 0)
        {
            return;
        }
        var loader = m_FreeAssetLoaders[m_FreeAssetLoaders.Count - 1];
        lock (m_FreeAssetLoaders)
        {
            m_FreeAssetLoaders.Remove(loader);
        }

        var assetRecord = m_WaitAssetDataList[0];
        lock (m_WaitAssetDataList)
        {
            m_WaitAssetDataList.Remove(assetRecord);
        }
#if UNITY_EDITOR
        loader.Load(assetRecord);
#else
            loader.Load(assetRecord);
#endif
    }

    private static string GetAllUrl(string url)
    {
        string fileExistsPath = string.Format("file://{0}/{1}", Application.persistentDataPath, url);
        if (!FileUtils.IsFileExists(fileExistsPath))
        {
            fileExistsPath = string.Format("{0}/{1}", Application.streamingAssetsPath, url);
        }
#if UNITY_ANDROID
            return fileExistsPath;
#else
        return "file:///" + fileExistsPath;
#endif
    }
    private static void LoadAssetComplete(AssetLoader loader)
    {
        var assetRecord = loader.assetRecord;
        assetRecord.DoComplete(loader);
        lock (m_FreeAssetLoaders)
        {
            m_FreeAssetLoaders.Add(loader);
        }
        StartLoad();
    }
    private static void LoadError(AssetLoader loader)
    {
        var assetRecord = loader.assetRecord;
        Debugger.Log("加载失败" + assetRecord.fileUrl);
        assetRecord.DoError(loader);
        loader.Dispose();
        lock (m_FreeAssetLoaders)
        {
            m_FreeAssetLoaders.Add(loader);
        }
        StartLoad();
    }
}
