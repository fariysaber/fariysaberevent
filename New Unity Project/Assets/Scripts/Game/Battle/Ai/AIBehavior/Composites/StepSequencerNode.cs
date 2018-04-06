using System;
using System.Collections.Generic;
using UnityEngine;
//复合节点
//顺序选择节点（每次update执行一个节点，执行到最后一个从新执行，必须保证每个点有返回，标志位前面的其中一个为false返回，并结束所有的事件
//否则标志位包括它后面的所有的为true的时候返回true）
public class StepSequencerNode : CompositeNode
{
    public int waitnodeIndex = 1;
    protected bool m_LoopOpen = false;
    public override void StartNode(object data, CallBehaviorBack becallback)
    {
        base.StartNode(data, becallback);
        nodeIndex = 0;
        m_LoopOpen = false;
        childNodes[nodeIndex].StartNode(startdata, CallChilcBack);
        StartUpdate();
    }
    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        if (m_LoopOpen)
        {
            for (int i = nodeIndex + 1; i < childNodes.Count - 1; i++)
            {
                //去除标志位后面的事件，因为当前事件受到阻塞无法执行，导致后面的也没法执行
                childNodes[i].DestroyAll();
            }
            nodeIndex = 0;
            m_LoopOpen = false;
            childNodes[nodeIndex].StartNode(startdata, CallChilcBack);
        }
    }
    public void CallChilcBack(bool result)
    {
        m_LoopOpen = false;
        if (behaviorLoad == BehaviorLoad.destroy)
        {
            return;
        }
        if (nodeIndex < waitnodeIndex && result == false)
        {
            SetCallback(false);
            return;
        }
        if (nodeIndex >= childNodes.Count - 1 && result)
        {
            SetCallback(true);
            return;
        }
        if (nodeIndex >= waitnodeIndex - 1)
        {
            m_LoopOpen = true;//循环时下帧执行，防止死循环
        }
        nodeIndex++;
        childNodes[nodeIndex].StartNode(startdata, CallChilcBack);
    }
}