using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
    /// <summary>
    /// 序列化工具类
    /// </summary>
public class SerializerUtils
{
    /// <summary>
    /// 基于二进制反序列化
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static object BinaryDerialise(byte[] bytes)
    {
        try
        {
            MemoryStream ms = new MemoryStream(bytes);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new UBinder();
            object data = formatter.Deserialize(ms);
            ms.Close();
            ms.Dispose();
            return data;
        }
        catch (Exception ex)
        {
            Debugger.Log("反序列化失败" + ex.ToString());
        }
        return null;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="savePath"></param>
    public static void Serialize(object obj, string savePath)
    {
        if (obj == null)
        {
            Debugger.Log("序列化对象为空，保存路径" + savePath);
            return;
        }
        try
        {
            FileStream fs = new FileStream(savePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
            Debugger.Log("序列化成功，保存路径" + savePath);
        }
        catch (Exception ex)
        {
            Debugger.Log("序列化失败，保存路径" + savePath);
        }
    }
}
