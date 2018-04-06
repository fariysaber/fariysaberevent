using System;
using System.Text;
using UnityEngine;
public class ColorUtil
{
    public static readonly Color NormalTextColor = ColorUtil.HexToColor(1580404991u);

    public static readonly Color TitleTextColor = ColorUtil.HexToColor(4292639743u);

    public static string ColorToHex(Color color)
    {
        int num = 16777215 & ColorUtil.ColorToInt(color) >> 8;
        return ColorUtil.DecimalToHex(num);
    }

    public static string ColorToHex(ColorUtil.ColorEnum color)
    {
        return ColorUtil.ColorToHex(ColorUtil.GetColor(color));
    }

    public static int ColorToInt(Color c)
    {
        int num = 0;
        num |= Mathf.RoundToInt(c.r * 255f) << 24;
        num |= Mathf.RoundToInt(c.g * 255f) << 16;
        num |= Mathf.RoundToInt(c.b * 255f) << 8;
        return num | Mathf.RoundToInt(c.a * 255f);
    }

    public static string ColorToString(Color color)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("#");
        stringBuilder.Append(((int)(color.r * 255f)).ToString("X2"));
        stringBuilder.Append(((int)(color.g * 255f)).ToString("X2"));
        stringBuilder.Append(((int)(color.b * 255f)).ToString("X2"));
        stringBuilder.Append(((int)(color.a * 255f)).ToString("X2"));
        return stringBuilder.ToString();
    }

    public static string DecimalToHex(int num)
    {
        return num.ToString("X6");
    }

    public static Color GetColor(ColorUtil.ColorEnum color)
    {
        Color result = ColorUtil.NormalTextColor;
        switch (color)
        {
            case ColorUtil.ColorEnum.White:
                result = ColorUtil.HexToColor(4294967295u);
                break;
            case ColorUtil.ColorEnum.Green:
                result = ColorUtil.HexToColor(754915071u);
                break;
            case ColorUtil.ColorEnum.Blue:
                result = ColorUtil.HexToColor(12255231u);
                break;
            case ColorUtil.ColorEnum.Purple:
                result = ColorUtil.HexToColor(4278216447u);
                break;
            case ColorUtil.ColorEnum.Orange:
                result = ColorUtil.HexToColor(4292346111u);
                break;
            case ColorUtil.ColorEnum.Darkgold:
                result = ColorUtil.HexToColor(4290283019u);
                break;
            case ColorUtil.ColorEnum.Red:
                result = Color.red;
                break;
            case ColorUtil.ColorEnum.Yellow:
                result = Color.yellow;
                break;
            default:
                if (color != ColorUtil.ColorEnum.NormalText)
                {
                    if (color == ColorUtil.ColorEnum.TitleText)
                    {
                        result = ColorUtil.TitleTextColor;
                    }
                }
                else
                {
                    result = ColorUtil.NormalTextColor;
                }
                break;
        }
        return result;
    }

    public static Color GetColor(uint colorIndex)
    {
        Color result = ColorUtil.NormalTextColor;
        switch (colorIndex)
        {
            case 1u:
                result = ColorUtil.HexToColor(4294967295u);
                break;
            case 2u:
                result = ColorUtil.HexToColor(754915071u);
                break;
            case 3u:
                result = ColorUtil.HexToColor(12255231u);
                break;
            case 4u:
                result = ColorUtil.HexToColor(4278216447u);
                break;
            case 5u:
                result = ColorUtil.HexToColor(4292346111u);
                break;
            case 6u:
                result = ColorUtil.HexToColor(4290283263u);
                break;
            case 7u:
                result = Color.red;
                break;
            case 8u:
                result = Color.yellow;
                break;
        }
        return result;
    }

    public static string GetColorName(ColorUtil.ColorEnum color)
    {
        string result = string.Empty;
        switch (color)
        {
            case ColorUtil.ColorEnum.White:
                result = "白色";
                break;
            case ColorUtil.ColorEnum.Green:
                result = "绿色";
                break;
            case ColorUtil.ColorEnum.Blue:
                result = "蓝色";
                break;
            case ColorUtil.ColorEnum.Purple:
                result = "紫色";
                break;
            case ColorUtil.ColorEnum.Orange:
                result = "橙色";
                break;
            case ColorUtil.ColorEnum.Darkgold:
                result = "暗金";
                break;
            case ColorUtil.ColorEnum.Red:
                result = "红色";
                break;
            case ColorUtil.ColorEnum.Yellow:
                result = "黄色";
                break;
        }
        return result;
    }

    public static string GetStringNormalColor(string name)
    {
        return string.Concat(new string[]
		{
			"[",
			ColorUtil.ColorToHex (ColorUtil.NormalTextColor),
			"]",
			name,
			"[-]"
		});
    }

    public static string GetStringTitleColor(string name)
    {
        return string.Concat(new string[]
		{
			"[",
			ColorUtil.ColorToHex (ColorUtil.TitleTextColor),
			"]",
			name,
			"[-]"
		});
    }

    public static string GetStringWithColor(string name, ColorUtil.ColorEnum color = ColorUtil.ColorEnum.NormalText)
    {
        return string.Concat(new string[]
		{
			"[",
			ColorUtil.ColorToHex (ColorUtil.GetColor (color)),
			"]",
			name,
			"[-]"
		});
    }

    public static string GetStringWithColorIndex(string name, uint index)
    {
        return string.Concat(new string[]
		{
			"[",
			ColorUtil.ColorToHex (ColorUtil.GetColor (index)),
			"]",
			name,
			"[-]"
		});
    }

    public static Color HexToColor(uint val)
    {
        return ColorUtil.IntToColor((int)val);
    }

    public static Color IntToColor(int val)
    {
        float num = 0.003921569f;
        Color black = Color.black;
        black.r = num * (float)(val >> 24 & 255);
        black.g = num * (float)(val >> 16 & 255);
        black.b = num * (float)(val >> 8 & 255);
        black.a = num * (float)(val & 255);
        return black;
    }

    //
    // Nested Types
    //
    public enum ColorEnum
    {
        None,
        White,
        Green,
        Blue,
        Purple,
        Orange,
        Darkgold,
        Red,
        Yellow,
        NormalText = 100,
        TitleText
    }
}
