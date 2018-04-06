using System;
using UnityEditor;
using UnityEngine;

class SceneBaseNodeEditor : Editor
{
    SerializedObject serObj;

    SerializedProperty bornRangeType;
    SerializedProperty boxRange;
    SerializedProperty circleRange;

    void OnEnable()
    {
        serObj = new SerializedObject(target);

        bornRangeType = serObj.FindProperty("bornRangeType");

        boxRange = serObj.FindProperty("boxRange");
        circleRange = serObj.FindProperty("circleRange");
    }


    public override void OnInspectorGUI()
    {
        serObj.Update();
        GUILayout.Label("基础点位信息", EditorStyles.miniBoldLabel);
    }
}