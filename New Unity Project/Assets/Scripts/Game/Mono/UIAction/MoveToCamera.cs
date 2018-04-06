using System;
using System.Collections.Generic;
using UnityEngine;
//物体与摄像头方向一致
public class MoveToCamera : MonoBehaviour
{
    public bool isMoveToCamera = true;
    void Update()
    {
        if (isMoveToCamera)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}