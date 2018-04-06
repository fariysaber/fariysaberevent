using UnityEngine;
using System.Collections;

public class MonoBehaviorExt : MonoBehaviour
{
    protected GameObject FindChild(string path)
    {
        var target = transform.FindChild(path);
        if (target == null)
        {
            Debug.Log("cant find gameChild");
        }
        return target.gameObject;
    }

    protected void OnDisable()
    {
        this.enabled = false;
    }

    protected void OnVisible()
    {
        this.enabled = true;
    }
}
