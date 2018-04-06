using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroBuffIcon : ChildPanel
{
    public BuffVo buffVo;
    protected UIImage headImg;
    public HeroBuffIcon(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    public void SetHeadInfo(BuffVo vo)
    {
        buffVo = vo;
        if (isLoadCompete)
        {
            RefreshInfo();
        }
    }
    protected override void LoadResCallBack()
    {
        headImg = new UIImage(root.transform, "");
        if (buffVo != null)
            RefreshInfo();
    }
    protected void RefreshInfo()
    {
        UIMgr.GetInstance().GetSprite(headImg._image, ResourcePath.buffsprite, buffVo.icon);
    }
}
