using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BinTool
{
    public static Dictionary<string, object> GetBinFile(string path)
    {
        string fileUrl = "Assets/Abres/Zero/XML/BinData/" + path + ".xml";

        FileUtils.MakeDirectoryByFile(fileUrl);
        AssetDatabase.Refresh();
        if (!FileUtils.IsFileExists(fileUrl))
        {
            return null;
        }

        FileInfo fi = new FileInfo(fileUrl);

        long len = fi.Length;
        FileStream fs = new FileStream(fileUrl, FileMode.Open);
        byte[] buffer = new byte[len];
        fs.Read(buffer, 0, (int)len);
        fs.Close();

        object dataVo = SerializerUtils.BinaryDerialise(buffer);
        Dictionary<string, object> _data = (Dictionary<string, object>)dataVo;
        Debugger.Log("还有" + _data.Count + "个" + path + "数据");
        return _data;
    }

    public static string GetBinPath(string path)
    {
        path = "Assets/Abres/Zero/XML/BinData/" + path + ".xml";
        return path;
    }
}
