using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class UiTools : Editor
{
    public static string fontPath = Application.dataPath + "/Abres/Zero/Font/simhei.ttf";
    [MenuItem("GameObject/UI/创建自适应text")]
    public static void CreateScaleToWidth()
    {
        UnityEngine.Object getObj = Selection.activeObject;
        GameObject parent = getObj as GameObject;
        GameObject crtobj = new GameObject();
        crtobj.transform.SetParent(parent.transform);
        crtobj.name = "Text";
        Text text = crtobj.AddComponent<Text>();
        RectTransform rectran = crtobj.GetComponent<RectTransform>();
        rectran.anchorMax = new Vector2(1, 1);
        rectran.anchorMin = new Vector2(0, 0);
        rectran.offsetMin = new Vector2(0, 0);
        rectran.offsetMax = new Vector2(0, 0);
        rectran.localScale = Vector3.one;
        Font font = (Font)AssetDatabase.LoadAssetAtPath(FileUtils.GetAssetDataPath(fontPath), typeof(Font));
        text.font = font;
        text.fontSize = 28;
        text.alignment = TextAnchor.MiddleCenter;
        text.resizeTextForBestFit = true;
        text.resizeTextMaxSize = 28;
        text.resizeTextMinSize = 1;
    }
    public static string textPrefab = Application.dataPath + "/Abres/Zero/UI/Modules/text2.prefab";
    [MenuItem("GameObject/UI/创建自适应text2")]
    public static void CreateScaleToWidth2()
    {
        UnityEngine.Object getObj = Selection.activeObject;
        GameObject parent = getObj as GameObject;
        GameObject getobj = (GameObject)AssetDatabase.LoadAssetAtPath(FileUtils.GetAssetDataPath(textPrefab), typeof(GameObject));
        GameObject crtobj = GameObject.Instantiate(getobj, parent.transform);
        crtobj.name = "Text";
        RectTransform rectran = crtobj.GetComponent<RectTransform>();
        rectran.offsetMin = new Vector2(0, 0);
        rectran.offsetMax = new Vector2(0, 0);
        rectran.sizeDelta = new Vector2(160, 50);
    }
}