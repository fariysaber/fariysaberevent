using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroHeadBatIcon : HeroHeadPanel
{
    List<HeroBuffIcon> bufIcon = new List<HeroBuffIcon>();
    private UITransform buffList;
    private List<int> buffIds = new List<int>();
    public HeroHeadBatIcon(GameObject parent, LoadCallback callback, string res)
        : base(parent, callback, res)
    {

    }
    protected override void LoadResCallBack()
    {
        headImg = new UIImage(root.transform, "icon");
        buffList = new UITransform(root.transform, "buffList");
        if (!headPath.Equals(""))
            RefreshInfo();
        if (buffIds.Count > 0)
        {
            RefreshbuffList(buffIds);
        }
    }
    public virtual void RefreshbuffList(List<int> buffs)
    {
        int index = 0;
        buffIds = buffs;
        if (isLoadCompete == false)
        {
            return;
        }
        for (index = 0; index < buffs.Count; index++)
        {
            HeroBuffIcon icon = null;
            if (bufIcon.Count <= index)
            {
                icon = new HeroBuffIcon(buffList.gameObject, null, "buffIcon");
                icon.LoadResource();
                icon.SetHeadInfo(BuffConfig.GetData(buffs[index] + ""));
                bufIcon.Add(icon);
            }
            else
            {
                icon = bufIcon[index];
                if (!icon.buffVo.id.Equals(buffs[index] + ""))
                {
                    icon.SetHeadInfo(BuffConfig.GetData(buffs[index] + ""));
                }
            }
            icon.SetActive(true);
        }
        for (int i = index; i < bufIcon.Count; i++)
        {
            bufIcon[i].SetActive(false);
        }
    }
}
