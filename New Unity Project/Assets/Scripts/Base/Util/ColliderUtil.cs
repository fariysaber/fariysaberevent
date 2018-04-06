using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ColliderType
{
    noCollider = 0,
    boxCollider = 1,
    capsuleCollider = 2,
    sphereCollider = 3,
    meshCollider = 4,
}

public class ColliderUtil
{
    public static ColliderType GetColliderType(GameObject go)
    {
        if (go.GetComponent<BoxCollider>())
        {
            return ColliderType.boxCollider;
        }
        if (go.GetComponent<CapsuleCollider>())
        {
            return ColliderType.capsuleCollider;
        }
        if (go.GetComponent<SphereCollider>())
        {
            return ColliderType.sphereCollider;
        }
        if (go.GetComponent<MeshCollider>())
        {
            return ColliderType.meshCollider;
        }
        return ColliderType.noCollider;
    }

    public static List<float> GetColliderData(GameObject go)
    {
        List<float> data = new List<float>();
        if (go.GetComponent<BoxCollider>())
        {
            BoxCollider collider = go.GetComponent<BoxCollider>();
            data = BoxColliderData.GetColliderData(collider);
            return data;
        }
        if (go.GetComponent<CapsuleCollider>())
        {
            CapsuleCollider collider = go.GetComponent<CapsuleCollider>();
            data = CapsuleColliderData.GetColliderData(collider);
            return data;
        }
        if (go.GetComponent<SphereCollider>())
        {
            SphereCollider collider = go.GetComponent<SphereCollider>();
            data = SphereColliderData.GetColliderData(collider);
            return data;
        }
        return data;
    }

    public static GameObject SetGameObjectCollider(GameObject go, ModelInfo info)
    {
        ColliderType colliderType = (ColliderType)info.colliderType;
        GameObject colliderObj = new GameObject(go.name);
        if (colliderType == ColliderType.boxCollider)
        {
            BoxCollider collider = colliderObj.AddComponent<BoxCollider>();
            BoxColliderData.SetColliderData(collider, info.colliderData);
           
        }
        if (colliderType == ColliderType.capsuleCollider)
        {
            CapsuleCollider collider = colliderObj.AddComponent<CapsuleCollider>();
            CapsuleColliderData.SetColliderData(collider, info.colliderData);
            
        }
        if (colliderType == ColliderType.sphereCollider)
        {
            SphereCollider collider = colliderObj.AddComponent<SphereCollider>();
            SphereColliderData.SetColliderData(collider, info.colliderData);
            
        }
        TransformUtils.SetParent(colliderObj.transform, go.transform);
        return colliderObj;
    }
}

public class CharacterControllerUtil
{
    public static List<float> GetControllerData(GameObject go)
    {
        List<float> data = new List<float>();
        if (go.GetComponent<CharacterController>())
        {
            CharacterController controller = go.GetComponent<CharacterController>();
            data = CharacterControllerData.GetControllerData(controller);
            return data;
        }
        return data;
    }

    public static void SetControllerData(GameObject go,ModelInfo info)
    {
        CharacterController controller = go.AddComponent<CharacterController>();
        CharacterControllerData.SetControllerData(controller, info.cotrollerData);
    }
}

public class BoxColliderData
{
    public static List<float> GetColliderData(BoxCollider collider)
    {
        List<float> data = new List<float>();
        bool isTriger = collider.isTrigger;
        data.Add(isTriger ? 1 : -1);
        data.Add(collider.center.x);
        data.Add(collider.center.y);
        data.Add(collider.center.z);
        data.Add(collider.size.x);
        data.Add(collider.size.y);
        data.Add(collider.size.z);
        return data;
    }
    public static void SetColliderData(BoxCollider collider, List<float> data)
    {
        if (data == null)
        {
            return;
        }
        collider.isTrigger = data[0] > 0 ? true : false;
        collider.center = new Vector3(data[1], data[2], data[3]);
        collider.size = new Vector3(data[4], data[5], data[6]);
    }
}

public class CapsuleColliderData
{
    public static List<float> GetColliderData(CapsuleCollider collider)
    {
        List<float> data = new List<float>();
        bool isTriger = collider.isTrigger;
        data.Add(isTriger ? 1 : -1);
        data.Add(collider.center.x);
        data.Add(collider.center.y);
        data.Add(collider.center.z);
        data.Add(collider.radius);
        data.Add(collider.height);
        return data;
    }
    public static void SetColliderData(CapsuleCollider collider, List<float> data)
    {
        if (data == null)
        {
            return;
        }
        collider.isTrigger = data[0] > 0 ? true : false;
        collider.center = new Vector3(data[1], data[2], data[3]);
        collider.radius = data[4];
        collider.height = data[5];
    }
}

public class SphereColliderData
{
    public static List<float> GetColliderData(SphereCollider collider)
    {
        List<float> data = new List<float>();
        bool isTriger = collider.isTrigger;
        data.Add(isTriger ? 1 : -1);
        data.Add(collider.center.x);
        data.Add(collider.center.y);
        data.Add(collider.center.z);
        data.Add(collider.radius);
        return data;
    }
    public static void SetColliderData(SphereCollider collider, List<float> data)
    {
        if (data == null)
        {
            return;
        }
        collider.isTrigger = data[0] > 0 ? true : false;
        collider.center = new Vector3(data[1], data[2], data[3]);
        collider.radius = data[4];
    }
}

public class CharacterControllerData
{
    public static List<float> GetControllerData(CharacterController controller)
    {
        List<float> data = new List<float>();
        data.Add(controller.slopeLimit);
        data.Add(controller.stepOffset);
        data.Add(controller.skinWidth);
        data.Add(controller.center.x);
        data.Add(controller.center.y);
        data.Add(controller.center.z);
        data.Add(controller.radius);
        data.Add(controller.height);
        return data;
    }

    public static void SetControllerData(CharacterController controller,List<float> data)
    {
        if (controller == null)
        {
            return;
        }
        controller.slopeLimit = data[0];
        controller.stepOffset = data[1];
        controller.skinWidth = data[2];
        controller.center = new Vector3(data[3], data[4], data[5]);
        controller.radius = data[6];
        controller.height = data[7];
    }
}