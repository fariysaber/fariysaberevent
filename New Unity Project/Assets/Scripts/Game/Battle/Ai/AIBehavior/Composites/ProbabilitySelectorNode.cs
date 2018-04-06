using System;
using System.Collections.Generic;
using UnityEngine;

//复合节点
//概率选择节点（按概率选取，当一个为truetrue时候返回true，后面不执行，如果全为为false，则返回false）
public class ProbabilitySelectorNode : CompositeNode
{
    protected int selectCount = 0;
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        nodeIndex = (int)UnityEngine.Random.Range(0, childNodes.Count);
        selectCount = 0;
        childNodes[nodeIndex].StartNode(startdata, CallChilcBack);
    }
    public void CallChilcBack(bool result)
    {
        if (behaviorLoad == BehaviorLoad.destroy)
        {
            return;
        }
        if (result == true)
        {
            SetCallback(true);
            return;
        }
        selectCount++;
        nodeIndex++;
        nodeIndex = nodeIndex % childNodes.Count;
        if (selectCount >= childNodes.Count)
        {
            SetCallback(false);
            return;
        }
        else
        {
            childNodes[nodeIndex].StartNode(startdata, CallChilcBack);
        }
    }
}