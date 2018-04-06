using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamera : BaseCamera
{
    private float m_MoveSpeed;
    private float m_TurnSpeed;
    private float m_TiltMax;
    private float m_TiltMin;
    public float m_TurnSmoothing;

    private float m_LookAngle;
    private float m_TiltAngle;

    private Vector3 m_PivotEulers;
    private Quaternion m_PivotTargetRot;
    private Quaternion m_TransformTargetRot;

    private Transform m_CameMoveParent;
    private Transform m_Pivot;

    public override void Init(GameObject cameraObj = null, object data = null)
    {
        base.Init(cameraObj, data);
        GameObject cameMoveParentObject = GameObject.Find("ThirdCameraParent");
        if (cameMoveParentObject == null)
        {
            cameMoveParentObject = new GameObject("cameMoveParentObject");
        }
        m_CameMoveParent = cameMoveParentObject.transform;
        // Lock or unlock the cursor.
        m_Pivot = m_CameMoveParent.transform.Find("m_Pivot");
        if (m_Pivot == null)
        {
            m_Pivot = TransformUtils.CreateGameObject(null, m_CameMoveParent, Vector3.zero, Vector3.one, "m_Pivot").transform;
        }

        ThirdInitData thirdData = data as ThirdInitData;
        m_Pivot.transform.localPosition = new Vector3(0, thirdData.initPivotPosY, 0);
        m_Pivot.transform.localRotation = Quaternion.Euler(thirdData.initPivotRotationX, 0, 0);
        m_CameMoveParent.transform.localRotation = Quaternion.Euler(0, thirdData.parentRotationY, 0);
        m_TurnSmoothing = thirdData.turnSmoothing;

        m_TiltMax = thirdData.tiltMax;
        m_TiltMin = thirdData.tiltMin;

        m_MoveSpeed = thirdData.moveSpeed;
        m_TurnSpeed = thirdData.turnSpeed;

        m_PivotEulers = m_Pivot.rotation.eulerAngles;
        m_PivotTargetRot = m_Pivot.transform.localRotation;
        m_TransformTargetRot = m_CameMoveParent.localRotation;

        m_LookAngle = m_TransformTargetRot.eulerAngles.y;
        m_TiltAngle = m_PivotEulers.x;

        cameraObject.transform.SetParent(m_Pivot);
        TransformUtils.ResetTransform(cameraObject);
        cameraObject.transform.localPosition = new Vector3(0, 0, thirdData.initCameraZ);
    }
    public override void SetPosition(Vector3 pos)
    {
        m_CameMoveParent.position = pos;
    }
    public override void LateUpdate(float dt)
    {
        if (target == null)
        {
            return;
        }
        FollowTarget(Time.deltaTime);
        HandleRotationMovement();
    }
    protected virtual void FollowTarget(float dt)
    {
        if (m_MoveSpeed < 0)
        {
            m_CameMoveParent.position = target.position;
        }
        else
        {
            m_CameMoveParent.position = Vector3.Lerp(m_CameMoveParent.position, target.position, dt * m_MoveSpeed);
        }
    }
    private void HandleRotationMovement()
    {

        m_TransformTargetRot = m_CameMoveParent.localRotation;

        m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y, m_PivotEulers.z);

        if (m_TurnSmoothing > 0)
        {
            m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
            m_CameMoveParent.localRotation = Quaternion.Slerp(m_CameMoveParent.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
        }
        else
        {
            m_Pivot.localRotation = m_PivotTargetRot;
            m_CameMoveParent.localRotation = m_TransformTargetRot;
        }
    }

    public override Vector3 GetResetPos(Vector3 pos)
    {
        float angleY = m_CameMoveParent.localRotation.eulerAngles.y;
        float cosNum = Mathf.Cos(Mathf.Deg2Rad * angleY);
        float sinNum = Mathf.Sin(Mathf.Deg2Rad * angleY);
        Vector3 newPos = new Vector3();
        newPos.x = pos.x * cosNum + pos.z * sinNum;
        newPos.z = pos.z * cosNum - pos.x * sinNum;
        return newPos;
    }
}
public class ThirdInitData
{
    public float initPivotPosY = 0.5f;
    public float initPivotRotationX = 60;
    public float initCameraZ = -6f;
    public float parentRotationY = 0f;

    public float tiltMax = 75f;
    public float tiltMin = 10f;

    public float moveSpeed = 10;
    public float turnSpeed = 1.5f;

    public float turnSmoothing = 0;
}