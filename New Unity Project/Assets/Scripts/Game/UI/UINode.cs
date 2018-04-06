using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransform
{
    private Transform _transform;
    public Transform transform
    {
        set { _transform = value; }
        get { return _transform; }
    }
    private GameObject _gameObject;
    public GameObject gameObject
    {
        set { _gameObject = value; }
        get { return _gameObject; }
    }
    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        set { _rectTransform = value; }
        get { return _rectTransform; }
    }
    public UITransform(Transform parent, string name)
    {
        if (!name.Equals(""))
        {
            transform = parent.Find(name);
        }
        else
        {
            transform = parent;
        }
        gameObject = transform.gameObject;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
}

public class UIText : UITransform
{
    public Text _text;
    public string text
    {
        set { _text.text = value; }
        get { return _text.text; }
    }
    public UIText(Transform parent, string name)
        : base(parent, name)
    {
        _text = gameObject.GetComponent<Text>();
    }
}

public class UIImage : UITransform
{
    public Image _image;
    public Sprite image
    {
        set { _image.sprite = value; }
        get { return _image.sprite; }
    }
    public UIImage(Transform parent, string name)
        : base(parent, name)
    {
        _image = gameObject.GetComponent<Image>();
    }
    public void SetPercent(float percent)
    {
        _image.fillAmount = percent > 1 ? 1 : percent;
    }
}

public class UIButton : UIImage
{
    private Button _button;
    public Button button
    {
        set { _button = value; }
        get { return _button; }
    }
    public UIButton(Transform parent, string name)
        : base(parent, name)
    {
        button = gameObject.GetComponent<Button>();
    }
}