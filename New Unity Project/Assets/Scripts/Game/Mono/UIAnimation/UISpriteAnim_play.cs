using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnim_play : UISpriteAnim
{
    [SerializeField, Tooltip("是否循环")]
    public bool isLoop = true;
    [SerializeField, Tooltip("一次播放是否保持最后一帧")]
    public bool keedEnd = false;
    [SerializeField, Tooltip("动画数量")]
    public int[] index;
    [SerializeField, Tooltip("选择的是哪个动画")]
    public int selectAnimIndex = 0;

    protected int initSelectAnimIndex = 0;
    protected bool initIsLoop = true;
    protected bool initKeepEnd = false;

    private int selectSpriteIndex = 0;
    private int selectMaxSpriteIndex = 0;
    protected override void Awake()
    {
        base.Awake();
        initSelectAnimIndex = selectAnimIndex;
        initIsLoop = isLoop;
        initKeepEnd = keedEnd;
    }

    protected override void ResetImage()
    {
        initImage = gameObject.GetComponent<Image>();
        if (initImage && sprite != null && sprite.Length > 0 && index.Length > 0)
        {
            selectAnimIndex = Mathf.Min(index.Length - 1, selectAnimIndex);
            GetSelectSpriteIndex();
            GetMaxSelectSpriteIndex();
            initImage.sprite = sprite[selectSpriteIndex];

            Debug.Log(selectSpriteIndex);
            Debug.Log(selectMaxSpriteIndex);
        }
    }

    private int GetSelectSpriteIndex()
    {
        selectSpriteIndex = 0;
        int i = 0;
        for (i = 0; i < selectAnimIndex && i < index.Length; i++)
        {
            selectSpriteIndex += index[i];
        }
        spriteIndex = Mathf.Min(index[i] - 1, spriteIndex);
        selectSpriteIndex += spriteIndex;
        return selectSpriteIndex;
    }

    private int GetMaxSelectSpriteIndex()
    {
        selectMaxSpriteIndex = 0;
        int i = 0;
        for (i = 0; i < selectAnimIndex && i < index.Length; i++)
        {
            selectMaxSpriteIndex += index[i];
        }
        selectMaxSpriteIndex += index[i];
        return selectMaxSpriteIndex;
    }

    public override void ResetState()
    {
        startTime = initstartTime;
        spriteIndex = initspriteIndex;
        selectAnimIndex = initSelectAnimIndex;
        isLoop = initIsLoop;
        keedEnd = initKeepEnd;
        ResetImage();
    }

	protected override void Update () 
    {
        if (Pause)
        {
            return;
        }
        if (IsSpriteErro())
        {
            return;
        }
        if (JudgeLoopAndOnceAction())
        {
            return;
        }
        startTime += Time.deltaTime;
        DoTimeAction();
	}
    private void DoTimeAction()
    {
        if (selectSpriteIndex == selectMaxSpriteIndex - 1 && isLoop == false && keedEnd)
        {
            startTime = delayTime;
            return;
        }
        
        bool nextSprite = false;
        while (startTime > delayTime)
        {
            selectSpriteIndex += 1;
            if (selectSpriteIndex < selectMaxSpriteIndex)
            {
                nextSprite = true;
                startTime -= delayTime;
            }
            else
            {
                if (tempAnimIndex >= 0)
                {
                    SetTempInfo();
                    return;
                }
                if (isLoop)
                {
                    nextSprite = true;
                    selectSpriteIndex = GetSelectSpriteIndex();
                    startTime -= delayTime;
                }
                else
                {
                    if (keedEnd == false)
                    {
                        ResetState();
                        return;
                    }
                }
            }
        }
        if (nextSprite)
        {
            initImage.sprite = sprite[selectSpriteIndex];
        }
    }
    private bool IsSpriteErro()
    {
        if (sprite == null)
        {
            return true;
        }
        if (initImage == null || sprite.Length <= 1 || index.Length < 1)
        {
            return true;
        }
        return false;
    }
    private bool JudgeLoopAndOnceAction()
    {
        if (isLoop)
        {
            return false;
        }
        if (selectSpriteIndex < selectMaxSpriteIndex)
        {
            return false;
        }
        return true;
    }

    private int tempAnimIndex = -1;
    private int tempSpriteIndex = -1;
    private int temploopOrOnceIndex = -1;
    private int tempKeepEnd = -1;
    private float tempStartTime = -1;
    /// <summary>
    /// 处理选择的动画
    /// </summary>
    /// <param name="isloopAni">是否循环</param>
    /// <param name="isForceAnim">是否强制开始动画</param>
    public void PlayAnim(int animIndex,bool isloopAni = false,bool isForceAnim = true,bool isKeepEnd = false,int getSpriteIndex = 0,float starttime = 0)
    {
        if (isForceAnim)
        {
            selectAnimIndex = animIndex;
            isLoop = isloopAni;
            keedEnd = isKeepEnd;
            spriteIndex = getSpriteIndex;
            startTime = starttime;
            ResetTemp();
            ResetImage();
        }
        else
        {
            tempAnimIndex = animIndex;
            tempSpriteIndex = getSpriteIndex;
            temploopOrOnceIndex = isloopAni == true ? 1 : 0;
            tempKeepEnd = isKeepEnd == true ? 1 : 0;
            tempStartTime = starttime;
        }
    }
    private void SetTempInfo()
    {
        selectAnimIndex = tempAnimIndex;
        spriteIndex = tempSpriteIndex;
        isLoop = temploopOrOnceIndex == 1 ? true : false;
        keedEnd = tempKeepEnd == 1 ? true : false;
        startTime = tempStartTime;
        ResetTemp();
        ResetImage();
    }

    private void ResetTemp()
    {
        tempAnimIndex = -1;
        tempSpriteIndex = -1;
        temploopOrOnceIndex = -1;
        tempKeepEnd = -1;
        tempStartTime = -1;
    }
}
