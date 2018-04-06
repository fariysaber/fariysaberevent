using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PackTool : Editor
{
    [MenuItem("fariySaber编辑器/打包资源/打包所有资源")]
    public static void PackAllResource()
    {
        PackLogic.PackAll();
    }

    [MenuItem("fariySaber编辑器/打包资源/（导表）（图集名字）（动画名字）一套完成")]
    public static void PackAndDoListAllResource()
    {
        ConfigTool.ReadExcel();
        CommonTool.RefreshAllAtlas();
        CommonTool.RefreshAllArmature();
        PackLogic.PackAll();
    }
}