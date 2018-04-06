using System;
using System.Collections.Generic;
using UnityEngine;
//复合节点
//平行节点（同时执行所有点，后面的不执行）
public enum ParallelPolicy
{
    firstfailure = 1,
    firstsuccess = 2,
    firstsuccessorfailure = 3,
}
public class ParallelNode : CompositeNode
{
    public ParallelPolicy policy = ParallelPolicy.firstfailure;
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        nodeIndex = 0;
        for (int i = 0; i < childNodes.Count; i++)
        {
            childNodes[i].StartNode(startdata, CallChilcBack);
        }
    }
    public void CallChilcBack(bool result)
    {
        if (behaviorLoad == BehaviorLoad.destroy)
        {
            return;
        }
        if (result == false && (policy == ParallelPolicy.firstfailure || policy == ParallelPolicy.firstsuccessorfailure))
        {
            SetCallback(false);
            for (int i = 0; i < childNodes.Count; i++)
            {
                childNodes[i].DestroyAll();
            }
            return;
        }
        if (result == true && (policy == ParallelPolicy.firstsuccess || policy == ParallelPolicy.firstsuccessorfailure))
        {
            SetCallback(true);
            for (int i = 0; i < childNodes.Count; i++)
            {
                childNodes[i].DestroyAll();
            }
            return;
        }
        nodeIndex++;
        if (nodeIndex >= childNodes.Count)
        {
            SetCallback(result);
            return;
        }
    }
}