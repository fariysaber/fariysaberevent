using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera
{
    public GameObject cameraObject;
    public Camera camera;
    public Transform target;
    public virtual void Init(GameObject cameraObj = null, object data = null)
    {
        cameraObject = cameraObj;
        RefreshCameraData();
    }

    private void ResetCameraParent()
    {
        GameObject cameraParent = GameObject.Find("cameraParent");
        if (cameraParent == null)
        {
            cameraParent = new GameObject("cameraParent");
        }
        cameraObject.transform.SetParent(cameraParent.transform);
    }

    public virtual void ResetCamera(GameObject cameraObj)
    {
        cameraObject = cameraObj;
        RefreshCameraData();
    }

    protected virtual void RefreshCameraData()
    {
        ResetCameraParent();
        camera = cameraObject.GetComponent<Camera>();
        camera.cullingMask = 1 << Globals.waterlayer |
            1 << Globals.groundlayer | 1 << Globals.plantlayer | 1 << Globals.entitylayer |
            1 << Globals.buildinglayer | 1 << Globals.playerlayer | 1 << Globals.picItemlayer |
            1 << Globals.BattlePlayerMylayer | 1 << Globals.BattlePlayerMyTeamlayer |
            1 << Globals.BattlePlayerFriendlayer | 1 << Globals.BattlePlayerDiRenlayer |
            1 << Globals.BattlePlayerZhonglilayer | 1 << Globals.normaleffectlayer |
            1 << Globals.BattlePlayerDirenShanlayer | 1 << Globals.BattlePlayerMineShanlayer |
            1 << Globals.BattlePlayerMineBulletlayer | 1 << Globals.BattlePlayerDirenBulletlayer |
            1 << Globals.BattlePlayerZhongliBulletlayer;
    }

    public void SetTarget(Transform getTarget)
    {
        target = getTarget;
    }

    public virtual void SetPosition(Vector3 pos)
    {
        cameraObject.transform.position = pos;
    }

    public virtual Vector3 GetResetPos(Vector3 pos)
    {
        return pos;
    }

    public virtual void DestroyCamera()
    {
        target = null;
    }

    public virtual void OnUpdate(float dt)
    {
       
    }

    public virtual void FixedUpdate(float dt)
    {
        
    }

    public virtual void LateUpdate(float dt)
    {
        
    }
}
