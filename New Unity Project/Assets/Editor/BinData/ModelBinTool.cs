using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ModelBinTool
{
    public static string modelAssetPath = Application.dataPath + "/Abres/Zero/model";
    [MenuItem("fariySaber编辑器/bin数据/导出modelinfo")]
    public static void SetModelInfo()
    {
        Dictionary<string, object> data = BinTool.GetBinFile("ModelInfo");
        if (data == null)
        {
            data = new Dictionary<string, object>();
        }
        List<string> pathList = FileUtils.GetAllFiles(modelAssetPath);
        for (int i = 0; i < pathList.Count; i++)
        {
            string assetPath = FileUtils.GetAssetDataPath(pathList[i]);
            GameObject gameObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            ModelInfo info = new ModelInfo();
            string name = gameObject.name.ToLower();
            info.name = name;
            info.path = GetInfoPath(assetPath);
            info.colliderType = (int)ColliderUtil.GetColliderType(gameObject);
            info.colliderData = ColliderUtil.GetColliderData(gameObject);
            info.hasController = gameObject.GetComponent<CharacterController>() ? true : false;
            info.cotrollerData = CharacterControllerUtil.GetControllerData(gameObject);
            data[name] = info;
            Debugger.Log("导出modelInfo: " + info.name, Color.green);
        }
        string fileUrl = BinTool.GetBinPath("ModelInfo");
        SerializerUtils.Serialize(data, fileUrl);
        AssetDatabase.Refresh();
    }
    public static string GetInfoPath(string path)
    {
        path = path.Substring(path.LastIndexOf("/model/") + "/model/".Length);
        return path;
    }
}