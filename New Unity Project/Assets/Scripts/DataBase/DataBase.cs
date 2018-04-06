using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataBase : Singleton<DataBase>
{
    // 后面改成正式路径
    public string dataDir = "Assets/Abres/AbResTemp/data/";
    /// <summary>
    /// 角色所有信息
    /// </summary>
    protected Dictionary<string, object> _dataDic = new Dictionary<string,object>();
    public object GetValue(string keyName,object defaultValue = null)
    {
        if (_dataDic.ContainsKey(keyName))
        {
            return _dataDic[keyName];
        }
        return defaultValue;
    }
    public int GetEntityValue()
    {
        if (!_dataDic.ContainsKey(Globals.entityKeyName))
        {
            _dataDic[Globals.entityKeyName] = 0;
        }
        int value = (int)_dataDic[Globals.entityKeyName];
        value++;
        _dataDic[Globals.entityKeyName] = value;
        return (int)_dataDic[Globals.entityKeyName];
    }
    public int GetValueByUp(string name)
    {
        if (!_dataDic.ContainsKey(name))
        {
            _dataDic[name] = 0;
        }
        int value = (int)_dataDic[name];
        value++;
        _dataDic[name] = value;
        return (int)_dataDic[name];
    }
    public void SetValue(string keyName,object value)
    {
        _dataDic[keyName] = value;
    }
    public void ReloadValue(string basekey)
    {
        string fileUrl = dataDir + basekey + ".xml";

        FileUtils.MakeDirectoryByFile(fileUrl);
        if (!FileUtils.IsFileExists(fileUrl))
        {
            return;
        }
        FileInfo fi = new FileInfo(fileUrl);

        long len = fi.Length;
        FileStream fs = new FileStream(fileUrl, FileMode.Open);
        byte[] buffer = new byte[len];
        fs.Read(buffer, 0, (int)len);
        fs.Close();

        object dataVo = SerializerUtils.BinaryDerialise(buffer);
        _dataDic = dataVo as Dictionary<string, object>;
    }
    public void SaveValue()
    {
        SerializerUtils.Serialize(_dataDic, dataDir + _dataDic["key"] + ".xml");
    }
}
