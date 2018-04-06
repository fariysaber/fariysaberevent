using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr
{
    public BaseCamera maincamera;
    public void SwichCamera<T>(GameObject cameraObj = null, object data = null) where T : new()
    {
        DestroyCamera();
        maincamera = new T() as BaseCamera;
        maincamera.Init(cameraObj, data);
    }
    public void DestroyCamera()
    {
        if (maincamera != null)
        {
            maincamera.DestroyCamera();
            maincamera = null;
        }
    }
    public virtual void LateUpdate(float dt)
    {
        if (maincamera != null)
        {
            maincamera.LateUpdate(dt);
        }
    }
}
