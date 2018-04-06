using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathMgr : Singleton<AStarPathMgr>
{
    public delegate void OnPathSeeked(List<Vector3> p,int index);
    private OnPathSeeked _onPathSeeked = null;
    public void StartPath(Vector3 pos, Vector3 endPos, OnPathSeeked seak,int index)
    {
        List<Vector3> posList = new List<Vector3>();
        float magni = (endPos - pos).magnitude;
        Vector3 normalized = (endPos - pos).normalized;
        Vector3 newPos = pos;
        while (magni > 1)
        {
            newPos += normalized;
            posList.Add(newPos);
            magni -= 1;
        }
        posList.Add(endPos);
        if (seak != null)
        {
            seak(posList, index);
        }
    }
}