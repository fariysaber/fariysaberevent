using System;
using System.Text.RegularExpressions;
using UnityEngine;
public class HtmlUtil
{
    public static string StringDebugColor(string msg, string strColor = "#ADFF2F")
    {
        return string.Concat(new string[]
		{
			"<color=",
			strColor,
			">",
			msg,
			"</color>"
		});
    }

    public static string StringDebugColor(string msg, Color color)
    {
        return HtmlUtil.StringDebugColor(msg, ColorUtil.ColorToString(color));
    }
}
