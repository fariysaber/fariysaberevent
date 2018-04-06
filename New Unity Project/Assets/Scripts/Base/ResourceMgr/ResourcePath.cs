using System;
using System.Collections.Generic;

public class ResourcePath
{
    public const string URL_UI_MODULES = "ui/modules/";
    public const string URL_UI_ATLAS = "ui/atlas/";
    public const string URL_UI_HEAD = "ui/atlas/singlesprite/head/";
    public const string URL_UI_SINGLE = "ui/atlas/singlesprite/";
    public const string URL_MATERIALS = "materials/";
    public const string URL_SHADER = "shader/";
    public const string URL_FONT = "font/";
    public const string URL_MODEL = "model/";
    public const string URL_EFFECT = "effect/";
    public const string URL_XML = "xml/";
    public const string URL_XML_CONFIG = "xml/config/";
    public const string URL_XML_BINDATA = "xml/bindata/"; 
    public const string URL_SOUND = "sound/";
    public const string URL_MOVIE = "movie/";
    public const string URL_PREFAB = "prefab/";
    public const string URL_SCENE = "scene/";
    public const string URL_AINODE = "ainode/";
    public const string URL_EQUIP = "equip/";

    public const string AbRes = "abres";
    public const string initFont = "simhei";

    public const string commonUi = "common";
    public const string buffsprite = "buffsprite";
    public const string battlesprote = "battlesprite";
    public const string numsprite = "numsprite";

    public const string HEADIR = "head/";
    public const string SKILLICON = "skill/";
    public const string EQUIPICON = "equip/";
    public const string BODY = "body/";

    /// <summary>
    /// 获取根目录
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetPath(string path = "")
    {
        return GetRootPath() + "zero/" + path;
    }
    public static string GetRootPath()
    {
        return "abres/";
    }

    /// <summary>
    /// 获取UI预设
    /// </summary>
    /// <param name="path">UI总路径</param>
    /// <param name="name">UI名字</param>
    /// <returns></returns>
    public static string GetUIModules(string path)
    {
        return GetPath() + URL_UI_MODULES + path.ToLower();
    }

    public static string GetUIAtlas(string path)
    {
        return GetPath() + URL_UI_ATLAS + path.ToLower();
    }

    /// <summary>
    /// 获取场景资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetSceneRes(string path)
    {
        return GetPath() + URL_SCENE + path.ToLower();
    }

    public static string GetAiActionRes(string path)
    {
        return GetPath() + URL_AINODE + path.ToLower();
    }
    /// <summary>
    /// 获取依赖关系文件
    /// </summary>
    /// <returns></returns>
    public static string GetManifestPath()
    {
        return GetRootPath() + AbRes;
    }

    public static string GetFontPath(string path)
    {
        return GetPath() + URL_FONT + path.ToLower();
    }

    public static string GetConfig(string path)
    {
        return GetPath() + URL_XML_CONFIG + path.ToLower();
    }

    public static string GetBinData(string path)
    {
        return GetPath() + URL_XML_BINDATA + path.ToLower();
    }

    public static string GetModel(string path)
    {
        return GetPath() + URL_MODEL + path.ToLower();
    }

    public static string GetEquip(string path)
    {
        return GetPath() + URL_EQUIP + path.ToLower();
    }

    public static string GetEffect(string path)
    {
        return GetPath() + URL_EFFECT + path.ToLower();
    }

    public static string GetHeadPath(string path)
    {
        return GetPath() + URL_UI_SINGLE + HEADIR + path.ToLower();
    }

    public static string GetSkillIconPath(string path)
    {
        return GetPath() + URL_UI_SINGLE + SKILLICON + path.ToLower();
    }

    public static string GetEquipIconPath(string path)
    {
        return GetPath() + URL_UI_SINGLE + EQUIPICON + path.ToLower();
    }

    public static string GetBodyPath(string path)
    {
        return GetPath() + URL_UI_SINGLE + HEADIR + path.ToLower();
    }
}
