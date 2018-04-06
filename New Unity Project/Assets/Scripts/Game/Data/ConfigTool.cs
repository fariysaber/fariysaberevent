using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class ConfigTool : Singleton<ConfigTool>
{
    private FileStream _fs;
    private BinaryReader _br;

    private MemoryStream _memory;
    /// <summary>
    /// 解析配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Dictionary<string, object> GetConfig<T>() where T : new()
    {
        Dictionary<string, object> keyValueDic = new Dictionary<string, object>();
        string voName = typeof(T).Name;
        TextAsset text = GetTextAsset(voName);
        try
        {
            _memory = new MemoryStream(text.bytes);
            _br = new BinaryReader(_memory);
        }
        catch(IOException err)
        {
            Debugger.LogError(err.ToString());
            return keyValueDic;
        }
        try
        {
            //读------文件头-----
            string head = ReadString();
            //读-------版本------
            int version = ReadInt();
            //读-------字段数----
            int typeNum = ReadInt();

            //读---------字段名和类型-----------
            List<string> dataNames = new List<string>();
            List<int> dataTypes = new List<int>();
            for (int i = 0; i < typeNum; i++)
            {
                dataNames.Add(ReadString());
                dataTypes.Add(ReadInt());
            }
            while (_br.BaseStream.Position < _br.BaseStream.Length - 1)
            {
                Type type = GameTools.GetType(voName);
                if (type == null)
                {
                    _br.Close();
                    return keyValueDic;
                }
                object getObj = Activator.CreateInstance(type);
                FieldInfo[] infoList = getObj.GetType().GetFields();
                string key = ReadString();

                for (int i = 0; i < typeNum; i++)
                {
                    switch (dataTypes[i])
                    {
                        case 1:
                            {
                                int value = ReadInt();
                                ReadDataToDic(infoList, dataNames[i], value, ref getObj);
                                break;
                            }
                        case 2:
                            {
                                string value = ReadString();
                                ReadDataToDic(infoList, dataNames[i], value, ref getObj);
                                break;
                            }
                        case 3:
                            {
                                int arrayHasNum = ReadInt();
                                int arrayType = ReadInt();
                                //一维数组
                                if (arrayHasNum == 1)
                                {
                                    int getNum = ReadInt();
                                    //整形数组
                                    if (arrayType == 1)
                                    {
                                        List<int> getList = ReadIntList(getNum);
                                        ReadDataToDic(infoList, dataNames[i], getList, ref getObj);
                                    }
                                    else
                                    {
                                        List<string> getList = ReadStringList(getNum);
                                        ReadDataToDic(infoList, dataNames[i], getList, ref getObj);
                                    }
                                }
                                //二维数组
                                else if (arrayHasNum == 2)
                                {
                                    //读取二维数组长度
                                    int listNum = ReadInt();
                                    //整形数组
                                    if (arrayType == 1)
                                    {
                                        List<List<int>> getList = new List<List<int>>();
                                        for (int g = 0; g < listNum; g++)
                                        {
                                            int getNum = ReadInt();
                                            List<int> gList = ReadIntList(getNum);
                                            getList.Add(gList);
                                        }
                                        ReadDataToDic(infoList, dataNames[i], getList, ref getObj);
                                    }
                                    else
                                    {
                                        List<List<string>> getList = new List<List<string>>();
                                        for (int g = 0; g < listNum; g++)
                                        {
                                            int getNum = ReadInt();
                                            List<string> gList = ReadStringList(getNum);
                                            getList.Add(gList);
                                        }
                                        ReadDataToDic(infoList, dataNames[i], getList, ref getObj);
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                _br.Close();
                                return keyValueDic;
                            }
                    }
                }
                keyValueDic.Add(key, getObj);
            }
        }
        catch(Exception err)
        {
            _br.Close();
            return keyValueDic;
        }

        _br.Close();
        return keyValueDic;
    }

    private int ReadInt()
    {
        int value = _br.ReadInt32();
        //Debug.Log(value);
        return value;
    }
    private string ReadString()
    {
        string value = _br.ReadString();
        //Debug.Log(value);
        return value;
    }
    private List<int> ReadIntList(int readNum)
    {
        List<int> getList = new List<int>();
        for (int k = 0; k < readNum; k++)
        {
            int value = ReadInt();
            //Debug.Log(value);
            getList.Add(value);
        }
        return getList;
    }
    private List<string> ReadStringList(int readNum)
    {
        List<string> getList = new List<string>();
        for (int k = 0; k < readNum; k++)
        {
            string value = ReadString();
            //Debug.Log(value);
            getList.Add(value);
        }
        return getList;
    }
    private void ReadDataToDic<T>(FieldInfo[] infos, string name, T value, ref object getObj)
    {
        for (int i = 0; i < infos.Length; i++)
        {
            if (infos[i] != null && infos[i].Name.Equals(name))
            {
                infos[i].SetValue(getObj, value);
                return;
            }
        }
    }

    private TextAsset GetTextAsset(string name)
    {
        name = name.ToLower();
        name = ResourcePath.GetConfig(name);
        ResourceData data = ResourceMgr.GetInstance().GetResourceData(ResourceType.config, name);
        return data.GetTextAsset();
    }
}
