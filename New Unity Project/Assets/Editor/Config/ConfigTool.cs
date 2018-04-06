using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ConfigTool : Editor
{
    public static string excelDir = Application.dataPath + "/AbResTemp/xml";
    [MenuItem("fariySaber编辑器/配置表工具/导出全部配置表")]
    public static void ReadExcel()
    {
        List<string> getConfigUrls = FileUtils.GetAllFiles(excelDir);
        for (int i = 0; i < getConfigUrls.Count; i++)
        {
            ReadConfig.ReadExcelToXml(getConfigUrls[i]);
        }
        AssetDatabase.Refresh();
    }
}