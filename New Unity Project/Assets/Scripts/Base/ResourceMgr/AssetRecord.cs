using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetRecord
{
    public AssetLoader.LoadComplete loadComplete;
    public AssetLoader.LoadError loadError;

    public string allUrl;
    public string fileUrl;
    public Type type;

    public AssetRecord()
    {
        
    }

    public void RemoveComplete()
    {
        loadComplete = null;
    }

    public void RemoveError()
    {
        loadError = null;
    }
    public void DoComplete(AssetLoader loader)
    {
        if (loadComplete != null)
        {
            loadComplete(loader);
        }
    }
    public void DoError(AssetLoader loader)
    {
        if (loadError != null)
        {
            loadError(loader);
        }
    }
}
