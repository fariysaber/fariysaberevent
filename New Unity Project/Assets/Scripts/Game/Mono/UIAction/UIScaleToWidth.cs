using System;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleToWidth : MonoBehaviour
{
    private UIText text;
    private string name;
    public float maxWidth;
    public bool isPipeiParent;
    public float offset = 10f;
    void Awake()
    {
        text = new UIText(transform,"");
        if (maxWidth == 0)
        {
            maxWidth = text._text.preferredWidth;
        }
        if (isPipeiParent)
        {
            maxWidth = text.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        }
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
        float nowWidth = text._text.preferredWidth;
        float getMaxEndWidth = maxWidth - offset;
        if (nowWidth > getMaxEndWidth)
        {
            text.transform.localScale = new Vector3(getMaxEndWidth / nowWidth, getMaxEndWidth / nowWidth, 1);
        }
        else
        {
            text.transform.localScale = Vector3.one;
        }
    }
}
