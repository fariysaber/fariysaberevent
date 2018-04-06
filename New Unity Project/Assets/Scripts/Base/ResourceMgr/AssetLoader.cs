using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AssetLoader
{
    public delegate void LoadComplete(AssetLoader loader);
    public delegate void LoadError(AssetLoader loader);
    private enum DownLoadState
    {
        Init = 0,
        Loading = 1,
        LoadOrComplete = 2,
        Store = 3,
        LoadFail = 4,
        StoreFail = 5,
        Cached = 6,
    }

    private AssetRecord _assetRecord;
    private DownLoadState _state;
    public WWW _loader;
    private float m_Size;
    public LoadComplete LoadCompleteFunction;
    public LoadError LoadErrorFunction;

    public AssetRecord assetRecord
    {
        get { return _assetRecord; }
    }

    public void Load(AssetRecord data)
    {
        _assetRecord = data;
        _state = DownLoadState.Loading;
        CoroutineHelper.GetInstance().StartCoroutine(StartLoad());
    }

    private IEnumerator StartLoad()
    {
        if (GetIsMainest(_assetRecord.fileUrl))
        {
            _loader = new WWW(_assetRecord.allUrl);
        }
        else
        {
            _loader = new WWW(_assetRecord.allUrl + ".ab");
        }
        yield return _loader;
        if (_loader.error != null)
        {
            _state = DownLoadState.LoadFail;
            if (_loader.error.Contains("bust") == false)
            {
                Debugger.Log("加载异常=" + _assetRecord.allUrl.ToLower(),Color.red);
                DispatcherError();
            }
        }
        else
        {
            int index = _assetRecord.fileUrl.LastIndexOf("/");
            string assetName = _assetRecord.fileUrl.Substring(index + 1);
            _state = DownLoadState.LoadOrComplete;
            if (assetName == "abres")
            {
                AssetBundleManifest mab = (AssetBundleManifest)_loader.assetBundle.LoadAsset("AssetBundleManifest");
                ResourceMgr.GetInstance().SetBackManifest(mab);
                _loader.assetBundle.Unload(false);
            }
            else
            {
                m_Size = _loader.bytesDownloaded;
            }
            DispatcherComplete();
        }
    }

    private void DispatcherComplete()
    {
        if (LoadCompleteFunction != null)
        {
            LoadCompleteFunction(this);
        }
    }

    private void DispatcherError()
    {
        if (LoadErrorFunction != null)
        {
            LoadErrorFunction(this);
        }
    }

    public void Dispose()
    {
        if (_state == DownLoadState.LoadFail)
        {
            return;
        }
        if (_loader == null || _loader.assetBundle == null)
        {
            return;
        }
        _loader.assetBundle.Unload(false);
        _loader.Dispose();
        _loader = null;
    }

    private bool GetIsMainest(string url)
    {
        int index = url.LastIndexOf("/");
        string assetName = url.Substring(index + 1);
        if (assetName == "abres")
        {
            return true;
        }
        return false;
    }
}
