using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RttModel
{
    public GameObject rttCamera;
    public Camera camera;
    public RenderTexture renderTexture;
    public bool isDestroy = false;
    public GameObject rttObject;
    public List<string> loadRttObjectRes = new List<string>();
    public Vector3 _offset = new Vector3(0, 0, 0);
    public Vector3 _angle = new Vector3(0, 0, 0);
    public RawImage rawImage;
    public RttModel(string name,Vector3 vec,Transform parent,Vector3 pos,Vector2 size)
    {
        rttCamera = TransformUtils.FindOrCreateObject(name + "Camera");
        TransformUtils.AddComponent(rttCamera, "Camera");
        int index = SceneMgr.GetInstance().nowGameScene.rttIndex;
        rttCamera.transform.position = new Vector3(Globals.maxSceneWidth * index, Globals.maxSceneHeight, Globals.maxSceneHeight);

        ResetCamera();
        CreateRenderTexture(vec);
        CreateRawImage(parent, pos, size);
    }
    private void CreateRenderTexture(Vector3 vec)
    {
        renderTexture = new RenderTexture((int)vec.x, (int)vec.y, (int)vec.z);
        camera.targetTexture = renderTexture;
    }
    private void CreateRawImage(Transform parent,Vector3 pos,Vector2 size)
    {
        rawImage = new GameObject("rttRawImage").AddComponent<RawImage>();
        rawImage.transform.SetParent(parent);
        rawImage.transform.localPosition = pos;
        rawImage.texture = renderTexture;
        rawImage.GetComponent<RectTransform>().sizeDelta = size;
        rawImage.gameObject.layer = Globals.uilayer;

        EventTriggerListener.Get(rawImage.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(rawImage.gameObject).onDown = OnPointerDown;
        EventTriggerListener.Get(rawImage.gameObject).onUp = OnPointerUp;
        TransformUtils.SetVisible(rawImage.gameObject, false);
    }
    protected virtual void OnButtonClick(GameObject go, PointerEventData eventData)
    {
        
    }
    protected virtual void OnPointerDown(GameObject go, PointerEventData eventData)
    {

    }
    protected virtual void OnPointerUp(GameObject go, PointerEventData eventData)
    {

    } 
    public void ResetCamera()
    {
        camera = rttCamera.GetComponent<Camera>();
        camera.targetDisplay = Globals.rttCameraTargetDisplay;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0, 0, 0, 0);
        camera.cullingMask = 1 << Globals.rttLayer;  // 打开层x 
        camera.farClipPlane = 50;
        camera.nearClipPlane = 0.1f;
    }
    public void LoadModel(string path,Vector3 offset,Vector3 angle)
    {
        if (rttObject != null)
        {
            TransformUtils.DestroyGameObject(rttObject);
        }
        _offset = offset;
        _angle = angle;
        loadRttObjectRes.Add(path);
        ResourceMgr.GetInstance().LoadResource(ResourceType.ui, ResourcePath.GetModel(path), LoadComplete);
    }

    private void LoadComplete(ResourceData data)
    {
        if (isDestroy)
        {
            return;
        }
        rttObject = data.GetCreateObject();
        rttObject.transform.position = new Vector3(rttCamera.transform.position.x + _offset.x, rttCamera.transform.position.y + _offset.y, rttCamera.transform.position.z + _offset.z);
        rttObject.transform.localRotation = Quaternion.Euler(_angle.x, _angle.y,_angle.z);
        rttObject.layer = Globals.rttLayer;
        foreach (Transform child in rttObject.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = Globals.rttLayer;
        }
        TransformUtils.SetVisible(rawImage.gameObject, true);
    }

    public void SetRawSize(Vector2 size)
    {
        rawImage.GetComponent<RectTransform>().sizeDelta = size;
    }

    public void SetRawPos(Vector3 pos)
    {
        rawImage.transform.localPosition = pos;
    }

    public void SetActive(bool value)
    {
        TransformUtils.SetVisible(rawImage.gameObject, value);
    }

    public void Destroy()
    {
        if (rttObject != null)
        {
            TransformUtils.DestroyGameObject(rttObject);
        }
        if (rawImage != null)
        {
            TransformUtils.DestroyGameObject(rawImage.gameObject);
        }
        isDestroy = true;
        for (int i = 0; i < loadRttObjectRes.Count; i++)
        {
            ResourceMgr.GetInstance().RemoveResourceDic(loadRttObjectRes[i]);
        }
    }
}
