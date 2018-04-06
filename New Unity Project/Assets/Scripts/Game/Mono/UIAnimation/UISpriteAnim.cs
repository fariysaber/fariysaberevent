using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnim : MonoBehaviour {

	// Use this for initialization
    public Sprite[] sprite;
    [SerializeField, Range(0.02f, 100f), Tooltip("设置每帧图片间隔时间")]
    public float delayTime = 0.1f;
    //变化参数
    [SerializeField, Tooltip("设置初始化时间")]
    public float startTime = 0f;
    [SerializeField, Tooltip("设置初始化第几帧图片")]
    public int spriteIndex = 0;
    [SerializeField, Tooltip("暂停动画")]
    public bool Pause;

    //初始化参数保存起来
    protected float initstartTime = 0f;
    protected int initspriteIndex = 0;
    protected Image initImage;

	protected virtual void Awake () 
    {
        initstartTime = startTime;
        initspriteIndex = spriteIndex;
	}

    protected virtual void Start()
    {
        ResetImage();
    }

    public virtual void ResetState()
    {
        startTime = initstartTime;
        spriteIndex = initspriteIndex;
        ResetImage();
    }

    protected virtual void ResetImage()
    {
        initImage = gameObject.GetComponent<Image>();
        if (initImage && sprite != null && sprite.Length > 0)
        {
            spriteIndex = Mathf.Min(sprite.Length - 1, spriteIndex);
            initImage.sprite = sprite[spriteIndex];
        }
    }

    public virtual void SetData(float starttime, int spriteindex,Sprite[] list = null)
    {
        startTime = starttime;
        spriteIndex = spriteindex;
        ResetImage();
        if (list != null)
        {
            sprite = list;
        }
    }
	
	// Update is called once per frame
    protected virtual void Update()
    {
		
	}

    protected virtual void OnDisable()
    {
        ResetState();
    }
}
