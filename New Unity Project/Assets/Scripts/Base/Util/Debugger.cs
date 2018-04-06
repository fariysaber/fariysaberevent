using UnityEngine;
using System.Collections;

public class Debugger
{
    public static bool enableLog = false;
    public static bool enableWarnLog = true;
    public static bool enableErrorLog = true;

    public static int MAX_LOG_NUM = 64;
    public static void Log(string msg)
    {
        if (enableLog)
        {
            UnityEngine.Debug.Log(msg);
        }
    }

    public static void Log(string msg, Color color)
    {
        if (Debugger.enableLog)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Debugger.Log(HtmlUtil.StringDebugColor(msg, color));
            }
            else
            {
                Debugger.Log(msg);
            }
        }
    }
    public static void LogError(string msg)
    {
        if (enableLog)
        {
            UnityEngine.Debug.LogError(msg);
        }
    }
}
