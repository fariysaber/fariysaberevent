using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SceneTools
{
     [MenuItem("fariySaber编辑器/场景编辑器/刷新灯光烘焙效果")]
    public static void OpenLight()
    {

        UnityEngine.Object getObj = Selection.activeObject;
        GameObject gameObject = getObj as GameObject;
        Transform[] form = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform trans in form)
        {
            Light light = trans.GetComponent<Light>();
            if (light)
            {
                light.lightmappingMode = LightmappingMode.Baked;
            }
        }
    }
     [MenuItem("fariySaber编辑器/场景编辑器/保存当前场景信息")]
     public static void SaveSceneFbInfo()
     {
         GameObject fb = GameObject.Find("fb");
         if (fb == null)
         {
             Debugger.Log("未找到fb节点", Color.red);
             return;
         }
         foreach (Transform child in fb.GetComponentsInChildren<Transform>(true))
         {
             if (child.GetComponent<BaseSceneMono>() == null)
             {
                 continue;
             }
             BaseSceAndNodeInfo info = SceneNodeTool.GetSceneNodeInfo(child.gameObject);

             //创建场景信息
             string sceneUrl = info.sceneEngineName + "_sceneInfo";
             sceneUrl = BinTool.GetBinPath(sceneUrl);
             FileUtils.MakeDirectoryByFile(sceneUrl);
             SerializerUtils.Serialize(info.mono, sceneUrl);

             string entityUrl = info.sceneEngineName + "_entityinfo";
             entityUrl = BinTool.GetBinPath(entityUrl);
             FileUtils.MakeDirectoryByFile(entityUrl);
             SerializerUtils.Serialize(info.baseSceMonoEnList, entityUrl);

             AssetDatabase.Refresh();
         }
         AssetDatabase.Refresh();
     }
}