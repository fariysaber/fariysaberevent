using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public string effectkey;
    public string loadpath;
    public bool Playing = false;
    public bool Pause = false;
    public bool destroy = false;
    public bool addtemp = false;
    public float cdTime;
    public float hasGoTime;
    private Transform parent;
    private Vector3 localPos;
    private Vector3 scale;
    private int layer;
    private Vector3 rotate;
    public GameObject effectObject;

    public delegate void LoadCallBack();
    public LoadCallBack loadCallback;
    public void Init(string effectkey)
    {
        this.effectkey = effectkey;
    }
    public void InitEffect(string path,ResourceType resType,Vector3 localPos,Vector3 rotate,Vector3 scale,int layer,Transform parent,float time = -1,
        bool resetPause = true)
    {
        Playing = true;
        cdTime = time;
        hasGoTime = 0;
        addtemp = false;
        this.loadpath = path;
        this.localPos = localPos;
        this.scale = scale;
        if (parent == null)
        {
            parent = SceneMgr.GetInstance().nowGameScene.effectRoot.transform;
        }
        this.parent = parent;
        this.layer = layer;
        this.rotate = rotate;
        this.Pause = resetPause == true ? false : this.Pause;
        path = ResourcePath.GetEffect(path);
        if (effectObject != null)
        {
            Play();
        }
        else
        {
            LoadEffect(path, resType);
        }
    }
    protected void LoadEffect(string path,ResourceType type)
    {
        if (path.ToLower().EndsWith("box") || path.ToLower().EndsWith("sphere"))
        {
            LoadNormalEffectComplete();
            return;
        }
        ResourceMgr.GetInstance().LoadResource(type, path, LoadComplete);
    }
    public void SetLoadCallback(Effect.LoadCallBack callback)
    {
        if (effectObject != null)
        {
            callback();
        }
        else
        {
            loadCallback = callback;
        }
    }
    protected void LoadNormalEffectComplete()
    {
        if (effectObject != null)
        {
            TransformUtils.DestroyGameObject(effectObject);
        }
        if (destroy)
        {
            return;
        }
        effectObject = new GameObject();
        LoadCompleteEnd();
    }
    protected void LoadComplete(ResourceData data)
    {
        if (effectObject != null)
        {
            TransformUtils.DestroyGameObject(effectObject);
        }
        if(destroy)
        {
            return;
        }
        effectObject = data.GetCreateObject();
        LoadCompleteEnd();
    }
    private void LoadCompleteEnd()
    {
        if (loadCallback != null)
        {
            loadCallback();
            loadCallback = null;
        }
        if (Playing)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    protected void Play()
    {
        TransformUtils.SetVisible(effectObject,true);
        effectObject.transform.SetParent(parent);
        effectObject.transform.localPosition = localPos;
        effectObject.transform.localRotation = Quaternion.Euler(rotate);
        effectObject.transform.localScale = scale;
        RefreshLayer();
        if (Pause == false)
        {
            PlayEffect();
        }
        else
        {
            PauseEffect();
        }
    }

    protected void PlayEffect()
    {
        Pause = false;
        if (effectObject == null)
        {
            return;
        }
        foreach (ParticleSystem child in effectObject.GetComponentsInChildren<ParticleSystem>(true))
        {
            child.Play();
        }
    }

    protected void RefreshLayer()
    {
        TransformUtils.ResetObjectLayer(effectObject, layer);
    }

    public virtual void Update(float dt)
    {
        if (cdTime < 0)
        {
            return;//无限时间
        }
        if (addtemp)
        {
            return;
        }
        if (cdTime > 0 && hasGoTime < cdTime)
        {
            hasGoTime += dt;
            if (hasGoTime >= cdTime)
            {
                addtemp = true;
            }
        }
    }

    public void Stop()
    {
        Playing = false;
        if (effectObject != null)
        {
            effectObject.SetActive(false);
        }
        StopEffect();
    }
    public void PauseEffect()
    {
        Pause = true;
        if (effectObject == null)
        {
            return;
        }
        foreach (ParticleSystem child in effectObject.GetComponentsInChildren<ParticleSystem>(true))
        {
            child.Pause();
        }
    }
    protected void StopEffect()
    {
        if (effectObject == null)
        {
            return;
        }
        foreach (ParticleSystem child in effectObject.GetComponentsInChildren<ParticleSystem>(true))
        {
            child.Stop();
        }
    }
    public void Destroy()
    {
        Playing = false;
        destroy = true;
        if (effectObject != null)
        {
            TransformUtils.DestroyGameObject(effectObject);
        }
        effectObject = null;
    }
}