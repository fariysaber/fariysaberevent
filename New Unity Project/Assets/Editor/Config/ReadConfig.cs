using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;
using UnityEngine.UI;

public class ReadConfig
{
    public static string saveCsUrl = Application.dataPath + "/Scripts/Game/Data/SysData/";
    public static string saveXMLUrl = Application.dataPath + "/Abres/Zero/XML/Config/";
    public static string assetVo = "Vo";
    public static string csLast = ".cs";
    public static string xmlLast = ".xml";
    public static void ReadExcelToXml(string excelUrl)
    {
        FileStream stream = File.Open(excelUrl, FileMode.Open, FileAccess.Read);
        Debug.Log("qqqqqqqqqqqqqqqqqq" + excelUrl);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //获取文件名并转换
        string assetName = excelUrl.Substring(excelUrl.LastIndexOf("/") + 1);
        assetName = assetName.Replace(Path.GetExtension(assetName), "");
        assetName += assetVo;

        WriteToCs(result, assetName);
        WriteToXml(result, assetName);
    }
    #region 写入CS文件相关
    private static void WriteToCs(DataSet result, string csName)
    {
        StringBuilder value = new StringBuilder();

        StringBuilder dataValue = new StringBuilder();

        List<string> descList = GetExcelDataList(result, 1);
        List<string> nameList = GetExcelDataList(result, 2);
        List<string> typeList = GetExcelDataList(result, 3);
        List<string> keyList = GetExcelDataList(result, 5);
        List<string> oneValueList = GetExcelDataList(result, 6);
        for (int i = 0; i < descList.Count; i++)
        {
            StringBuilder getData = new StringBuilder();
            getData.Append(GetDesStr(descList[i]));
            getData.Append(GetDataStr(typeList[i], nameList[i], keyList[i], oneValueList[i], csName));

            dataValue.Append(getData);
        }

        value.Append(GetReadBaseCs(csName));
        value.Append(dataValue);
        value.Append(GetReadEndCs());

        csName = csName + csLast;
        FileUtils.CreateOrWriteTxtFile(saveCsUrl + csName, value.ToString());
    }

    /// <summary>
    /// 写入信息文件cs基本骨架
    /// </summary>
    /// <returns></returns>
    private static string GetReadBaseCs(string csName)
    {
        StringBuilder value = new StringBuilder();
        value.Append("using System;\n");
        value.Append("using System.Collections.Generic;\n");
        value.Append("using System.ComponentModel;\n");
        value.Append("[Serializable]\n");
        value.Append("public class " + csName + "\n");
        value.Append("{\n");
        return value.ToString();
    }
    private static string GetReadEndCs()
    {
        StringBuilder value = new StringBuilder();
        value.Append("}\n");
        return value.ToString();
    }
    private static string GetDesStr(string str)
    {
        StringBuilder value = new StringBuilder();
        value.Append("\t");
        value.Append("[Description(\"");
        value.Append(str);
        value.Append("\")]");
        value.Append("\n");
        return value.ToString();
    }
    private static string GetDataStr(string typeStr, string nameStr, string key, string dataValue, string assetName = "")
    {
        StringBuilder value = new StringBuilder();
        value.Append("\t");
        value.Append("public");
        value.Append(" ");
        if (key.Equals("key"))
        {
            value.Append("string");
        }
        else
        {
            if (typeStr.Equals("array"))
            {
                int listNum = 0;
                if (dataValue.Contains("{"))
                {
                    listNum++;
                }
                if (dataValue.Contains("["))
                {
                    listNum++;
                }
                string getType = "string";
                if (IsNumArray(dataValue))
                {
                    getType = "int";
                }
                if (listNum == 1)
                {
                    value.Append("List<");
                    value.Append(getType);
                    value.Append(">");
                }
                else if (listNum == 2)
                {
                    value.Append("List<List<");
                    value.Append(getType);
                    value.Append(">>");
                }
                else
                {
                    value.Append(typeStr);
                    Debugger.LogError("错误array" + assetName + "配置表" + nameStr + "字段");
                }
            }
            else
            {
                value.Append(typeStr);
            }
        }
        value.Append(" ");
        value.Append(nameStr);
        value.Append(";");
        value.Append("\n");

        return value.ToString();
    }
    public static bool IsNumber(string s, int precision, int scale)
    {
        if ((precision == 0) && (scale == 0))
        {
            return false;
        }
        string pattern = @"(^\d{1," + precision + "}";
        if (scale > 0)
        {
            pattern += @"\.\d{0," + scale + "}$)|" + pattern;
        }
        pattern += "$)";
        return Regex.IsMatch(s, pattern);
    }
    private static bool IsNumArray(string str)
    {
        string changeStr = str;
        changeStr = changeStr.Replace("{", "");
        changeStr = changeStr.Replace("}", "");
        changeStr = changeStr.Replace("[", "");
        changeStr = changeStr.Replace("]", "");
        string[] getNumStr = changeStr.Split(',');
        for (int i = 0; i < getNumStr.Length; i++)
        {
            if (getNumStr[i].Equals(""))
            {
                continue;
            }
            if (IsNumber(getNumStr[i], 10, 0) == false)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    public static void WriteToXml(DataSet result, string xmlName)
    {
        string fileName = xmlName;
        xmlName = xmlName + xmlLast;
        //文件相关
        FileUtils.MakeDirectoryByFile(saveXMLUrl + xmlName);
        FileStream fs = new FileStream(saveXMLUrl + xmlName, FileMode.OpenOrCreate);
        BinaryWriter bw = new BinaryWriter(fs);

        //写入-----文件头------
        string head = fileName;
        Write(bw, head);

        //写入-----版本------
        int version = 1;
        Write(bw, version);

        WriteXmlData(bw, result);

        bw.Close();
        fs.Close();
    }
    private static void WriteXmlData(BinaryWriter bw, DataSet result)
    {
        int columns = result.Tables[0].Columns.Count;
        int rows = result.Tables[0].Rows.Count;
        List<string> nameList = GetExcelDataList(result, 2);
        List<string> typeList = GetExcelDataList(result, 3);
        List<string> keyList = GetExcelDataList(result, 5);

        WriteXmlTypeAndName(bw, nameList, typeList, keyList);

        for (int i = 6; i < rows; i++)
        {
            for (int keyj = 0; keyj < columns; keyj++)
            {
                bool isKey = keyList[keyj].Equals("key");
                if (isKey)
                {
                    //采用单key模式
                    string nvalue = result.Tables[0].Rows[i][keyj].ToString();
                    Write(bw, nvalue);
                    break;
                }
            }
            for (int j = 0; j < columns; j++)
            {
                bool isKey = keyList[j].Equals("key");
                int typeInt = GetTypeInt(typeList[j], isKey);
                string nvalue = result.Tables[0].Rows[i][j].ToString();
                switch (typeInt)
                {
                    case 1: WriteToInt(bw, nvalue); break;
                    case 2: Write(bw, nvalue); break;
                    case 3: WriteToArray(bw, nvalue); break;
                }
            }
        }
    }
    private static void WriteXmlTypeAndName(BinaryWriter bw, List<string> nameList, List<string> typeList, List<string> keyList)
    {
        int length = nameList.Count;
        //写入-----长度------
        Write(bw, length);
        for (int i = 0; i < length; i++)
        {
            //写入-----属性名-----
            Write(bw, nameList[i]);
            //写入------类型------
            bool isKey = keyList[i].Equals("key");
            int typeInt = GetTypeInt(typeList[i], isKey);
            Write(bw, typeInt);
        }
    }
    private static void WriteToInt(BinaryWriter bw, string num)
    {
        int getNum = 0;
        int.TryParse(num, out getNum);
        Write(bw, getNum);
    }
    private static void WriteToArray(BinaryWriter bw, string num)
    {
        int arrayType = 1;
        if (num.Contains("["))
        {
            arrayType = 2;
        }
        //写入-----------数组的类型（1 一维 2二维）------
        Write(bw, arrayType);
        //写入-----------数组的属性类型（1 int 2string）------
        int numTypeInt = 1;
        if (IsNumArray(num))
        {
            numTypeInt = 1;
        }
        else
        {
            numTypeInt = 2;
        }
        Write(bw, numTypeInt);
        //写入----------二维数组的长度--------
        if (arrayType == 2)
        {
            int getNum = GetArrayNum(num);
            Write(bw, getNum);
        }

        int searchLeft = 0;
        while (searchLeft != -1)
        {
            searchLeft = num.IndexOf("{", searchLeft);
            if (searchLeft < 0)
            {
                break;
            }
            searchLeft++;
            int searchRight = num.IndexOf("}", searchLeft);
            string getOneValue = num.Substring(searchLeft, searchRight - searchLeft);
            string[] oneValueNum = getOneValue.Split(',');
            int arrayOneNum = 0;
            for (int i = 0; i < oneValueNum.Length; i++)
            {
                if (oneValueNum[i].Equals(""))
                {
                    continue;
                }
                arrayOneNum++;
            }
            //写入---------数据内元素个数-------
            Write(bw, arrayOneNum);
            for (int i = 0; i < oneValueNum.Length; i++)
            {
                if (oneValueNum[i].Equals(""))
                {
                    continue;
                }
                if (numTypeInt == 1)
                {
                    WriteToInt(bw, oneValueNum[i]);
                }
                else if (numTypeInt == 2)
                {
                    Write(bw, oneValueNum[i]);
                }
            }
        }
    }
    private static void Write(BinaryWriter bw, int num)
    {
        bw.Write(num);
        //Debug.Log(num + "" + num.GetType());
    }
    private static void Write(BinaryWriter bw, string num)
    {
        bw.Write(num);
        //Debug.Log(num + "" + num.GetType());
    }
    private static int GetTypeInt(string typeStr, bool isKey)
    {
        if (isKey)
        {
            return 2;//字符串
        }
        if (typeStr.Equals("int"))
        {
            return 1;//int型
        }
        if (typeStr.Equals("string"))
        {
            return 2;//string型
        }
        if (typeStr.Equals("array"))
        {
            return 3;//array型
        }
        return 2;//新的类型当做string处理
    }
    private static int GetArrayNum(string num)
    {
        int searchLeft = 0;
        int getNum = 0;
        while (searchLeft != -1)
        {
            searchLeft = num.IndexOf("{", searchLeft);
            if (searchLeft < 0)
            {
                break;
            }
            searchLeft++;
            getNum++;
        }
        return getNum;
    }

    /// <summary>
    /// 获取配置表对应行的信息
    /// </summary>
    /// <param name="result"></param>
    /// <param name="idx">对应的行数</param>
    /// <returns></returns>
    private static List<string> GetExcelDataList(DataSet result, int idx)
    {
        List<string> getList = new List<string>();
        int columns = result.Tables[0].Columns.Count;
        int rows = result.Tables[0].Rows.Count;
        for (int j = 0; j < columns; j++)
        {
            string nvalue = result.Tables[0].Rows[idx][j].ToString();
            getList.Add(nvalue);
        }
        return getList;
    }
}
