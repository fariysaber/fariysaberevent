using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RttPlayerModel : RttModel
{
    public RttPlayerModel(string name, Vector3 vec, Transform parent, Vector3 pos, Vector2 size)
        : base(name, vec, parent, pos, size)
    {

    }
    protected override void OnPointerDown(GameObject go, PointerEventData eventData)
    {
        base.OnPointerDown(go, eventData);
        Debug.Log(eventData.position);
    }
    protected override void OnPointerUp(GameObject go, PointerEventData eventData)
    {
        base.OnPointerUp(go, eventData);
        Debug.Log(eventData.position);
    }
}
