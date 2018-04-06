using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
public class FileUtils
{
    private static string _root = null;

    //路径获取
    public static string root
    {
        get
        {
            if (_root == null)
            {
                _root = Application.streamingAssetsPath;
            }
            return _root;
        }
    }

    private static string _resRoot = null;

    public static string resRoot
    {
        get
        {
            if (_resRoot == null)
            {
                _resRoot = string.Format("{0}", root);
            }
            return _resRoot;
        }
    }

    //返回目录的完整路径的字符串
    public static string GetFileDir(string filePath)
    {
        FileInfo fi = new FileInfo(filePath);
        return fi.DirectoryName;
    }

    private static bool CreateFile(string path, string fileName, byte[] bytes)
    {
        Stream stream = null;
        string text = path + "/" + fileName;
        string path2 = text.Substring(0, text.LastIndexOf("/",StringComparison.Ordinal));
        bool result = false;
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path2);
            FileInfo fileInfo = new FileInfo(text);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            else
            {
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
            Debug.Log("创建文件:" + text);
            stream = new FileStream(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.Write);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            result = true;
        }
        catch (IOException ex)
        {
            Debug.LogError(ex.Message);
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
        }
        return result;
    }

    //创建文件夹
    public static void MakeSureDirExists(string dir)
    {
        try
        {
            if (!Directory.Exists(dir))
            {
                Debug.Log(dir);
                Directory.CreateDirectory(dir);
            }
        }
        catch (Exception e)
        {
            Debugger.Log("创建文件失败");
        }
    }
    public static void MakeDirectoryByFile(string dir)
    {
        MakeSureDirExists(GetFileDir(dir));
    }
    //是否存在文件
    public static bool IsFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
    //删除文件
    public static bool DeleteFile(string filePath)
    {
        if (!IsFileExists(filePath))
        {
            return true;
        }
        try
        {
            File.Delete(filePath);
            return true;
        }
        catch (Exception e)
        {
            Debugger.Log("删除文件失败url:" + filePath + ",错误信息:" + e.Message);
            return false;
        }
    }
    //删除文件夹
    public static bool DeleteDir(string dirPath, bool isSync = true)
    {
        if (!Directory.Exists(dirPath))
        {
            Debugger.Log("删除目录失败,不存在目录:" + dirPath);
            return true;
        }
        try
        {
            Directory.Delete(dirPath, true);
            if (isSync)
            {
                bool bDeleted = false;
                do
                {
                    //表示是否删除完
                    bDeleted = !Directory.Exists(dirPath);
                    if (!bDeleted)
                    {
                        Debugger.Log("删除目录进行中" + dirPath);
                    }
                }
                while (bDeleted);
            }
            return true;
        }
        catch (Exception e)
        {
            Debugger.Log("删除目录失败" + dirPath + "错误信息" + e.Message);
            return false;
        }
    }

    public static void CreateOrWriteTxtFile(string filePath, string content)
    {
        MakeSureDirExists(GetFileDir(filePath));
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }
            fs.Close();
        }
    }

    public static string ReadTxtFromFile(string filePath)
    {
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string content = sr.ReadToEnd();
                return content;
            }
        }
        catch (Exception e)
        {
            Debugger.Log("无法读取txt文件" + filePath + "错误信息" + e.Message);
            return null;
        }
    }

    public static bool CreateBinFile(string filePath, byte[] content)
    {
        MakeSureDirExists(GetFileDir(filePath));
        if (IsFileExists(filePath))
        {
            if (!DeleteFile(filePath))
            {
                return false;
            }
        }
        try
        {
            FileInfo fi = new FileInfo(filePath);
            FileStream fs = fi.Create();
            fs.Write(content, 0, content.Length);
            fs.Close();

            return true;
        }
        catch(Exception e)
        {
            Debugger.Log("创建二进制文件失败" + filePath + "错误信息" + e.Message);
            return false;
        }
    }

    public static string GemFileMD5(string filePath)
    {
        if (!IsFileExists(filePath))
        {
            return null;
        }
        FileStream fs = new FileStream(filePath,FileMode.Open);
        string md5 = "";
        //md5 = MD5Hash.Get(fs);
        fs.Close();
        return md5;
    }

    //获取文件大小
    public static float GetFileSize(string filePath)
    {
        float size = 0;
        if (IsFileExists(filePath))
        {
            size = new FileInfo(filePath).Length;
        }
        return size;
    }

    //获取路径的文件名
    public static string GetAssetName(string filePath)
    {
        //先获取路径指定字符最后一次出现索引
        var index = filePath.LastIndexOf("/", StringComparison.Ordinal);
        //根据索引找到其后面的字符串路径
        return filePath.Substring(index + 1);
    }
    public static string GetAssetDataPath(string filePath)
    {
        filePath = "Assets" + filePath.Substring(filePath.LastIndexOf("Assets") + "Assets".Length);
        return filePath;
    }

    ///<summary>
    ///解析版本文件
    ///</summary>
    ///<param name = "versionString"></param>
    ///<return></return>
    public static string[] GetVersionStrings(string versionString)
    {
        char[] end = { '\r','\n' };
        //设置清除空值后的分割字符串组
        string[] all = versionString.Split(end, StringSplitOptions.RemoveEmptyEntries);
        var versionStrings = new string[2];
        if (all.Length == 2)
        {
            versionStrings[0] = all[0].Split(':')[1];
            versionStrings[1] = all[1].Split(':')[1];
        }
        return versionStrings;
    }

    //更换文件名
    public static void ReNameFile(string oldName, string newName)
    {
        Directory.Move(oldName, newName);
    }
        
    //获取路径下的所有文件信息
    public static List<string> GetAllFiles(string dirUrl,string[] ends = null)
    {
        List<string> getFiles = new List<string>();
        DirectoryInfo folder = new DirectoryInfo(dirUrl);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].FullName.EndsWith(".meta"))
            {
                continue;
            }
            if (ends != null)
            {
                bool hasEnds = false;
                for (int k = 0; k < ends.Length; k++)
                {
                    if(files[i].FullName.EndsWith(ends[k]))
                    {
                        hasEnds = true;
                    }
                }
                if (hasEnds == false)
                {
                    continue;
                }
            }
            string getDirUrl = Replace(files[i].FullName);
            if (files[i] is DirectoryInfo)
            {
                getFiles.AddRange(GetAllFiles(getDirUrl));
            }
            else
            {
                getFiles.Add(getDirUrl);
            }
        }
        return getFiles;
    }

    public static bool IsInEndsName(string path, List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals("all"))
            {
                return true;
            }
            if (path.EndsWith(list[i]))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsInEndsName(string path, string[] list)
    {
        if (list == null)
        {
            return false;
        }
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].Equals("all"))
            {
                return true;
            }
            if (path.EndsWith(list[i]))
            {
                return true;
            }
        }
        return false;
    }
    public static string Replace(string url)
    {
        return url.Replace("\\", "/");
    }

    /// <summary>
    /// 移动文件
    /// </summary>
    /// <param name="path">需要移动的文件</param>
    /// <param name="targetDirPath">移动的目标文件夹（实例：aaa/bbbb/cc）</param>
    public static void MoveFileToPath(string path, string targetDirPath)
    {
        string name = GetAssetName(path);
        targetDirPath = targetDirPath + "/" + name;
        MakeDirectoryByFile(targetDirPath);
        File.Move(path, targetDirPath);

        string metaPath = path + ".meta";
        targetDirPath += ".meta";
        File.Move(metaPath, targetDirPath);
    }

    public static string[] GetReplaceFilePath(string path, string targetDirPath,bool createDirec = true)
    {
        string[] str = new string[4];
        string name = GetAssetName(path);
        targetDirPath = targetDirPath + "/" + name;

        if (createDirec)
        {
            MakeDirectoryByFile(targetDirPath);
        }

        str[0] = path;
        str[1] = targetDirPath;
        str[2] = path + ".meta";
        str[3] = targetDirPath + ".meta";
        return str;
    }
    public static string AssetsToTruePath(string path)
    {
        path = Application.dataPath + path.Substring(path.LastIndexOf("Assets") + "Assets".Length);
        path = GetFileDir(path);
        path = Replace(path);
        return path;
    }
}
