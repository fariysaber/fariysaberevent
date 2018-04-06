using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowText : MonoBehaviour
{
    private UIText text;
    private string name;
    public float offset = 25f;
    void Awake()
    {
        text = new UIText(transform.parent,"");
    }
    void Update()
    {
        if (text.text.Equals(name))
        {
            return;
        }
        name = text.text;
        ResetWidth();
    }
    private void ResetWidth()
    {
        float width = text._text.preferredWidth;
        gameObject.GetComponent<RectTransform>().localPosition = new Vector2(width + offset, 0);
    }
}
