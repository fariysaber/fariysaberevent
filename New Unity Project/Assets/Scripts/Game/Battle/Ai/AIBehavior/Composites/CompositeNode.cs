using System;
using System.Collections.Generic;
using UnityEngine;

//复合节点

public class CompositeNode : BehaviorNode
{
    protected List<BehaviorNode> childNodes;
    public List<GameObject> childObjs;
    protected int nodeIndex = 0;
    public override void InitNode(object data)
    {
        base.InitNode(data);
        childNodes = new List<BehaviorNode>();
        for (int i = 0; i < childObjs.Count; i++)
        {
            childNodes.Add(childObjs[i].GetComponent<BehaviorNode>());
        }
        for (int i = 0; i < childNodes.Count; i++)
        {
            childNodes[i].InitNode(data);
        }
    }
    public override void Tick(float dt)
    {
        base.Tick(dt);
        for (int i = 0; i < childNodes.Count; i++)
        {
            childNodes[i].Tick(dt);
        }
    }
    public override void DestroyAll()
    {
        base.DestroyAll();
        for (int i = 0; i < childNodes.Count; i++)
        {
            childNodes[i].DestroyAll();
        }
    }
}