using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroHeadPanel : ChildPanel
{
    protected string headPath = "";
    protected UIImage headImg;
    public HeroHeadPanel(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void LoadResCallBack()
    {
        headImg = new UIImage(root.transform, "head");
        if (!this.headPath.Equals(""))
            RefreshInfo();
    }
    public void SetInfo(string headPath)
    {
        this.headPath = headPath;
        if (isLoadCompete)
        {
            RefreshInfo();
        }
    }
    public void SetHeadInfo(HeroVo herovo,int pifu = 0)
    {
        headPath = CharactorConfig.GetData(herovo.charactor[pifu] + "").head;
        SetInfo(headPath);
    }
    protected void RefreshInfo()
    {
        UIMgr.GetInstance().GetSprite(headImg._image, "",ResourcePath.GetHeadPath(headPath));
    }
}