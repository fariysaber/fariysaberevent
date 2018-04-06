using System;
using System.Collections.Generic;
using UnityEngine;

//复合节点
//翻转选择节点（按顺序从左至右，当一个为truetrue时候返回true，再将这个子节点放在后面，后面不执行，如果全为为false，则返回false）
public class FilpSelectorNode : CompositeNode
{
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        nodeIndex = 0;
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
            BehaviorNode getnode = childNodes[nodeIndex];
            childNodes.RemoveAt(nodeIndex);
            childNodes.Add(getnode);
            return;
        }
        nodeIndex++;
        if (nodeIndex >= childNodes.Count)
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