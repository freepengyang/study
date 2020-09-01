using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class UtilityColor
{
    public const string White = "[ffffff]";
    public const string Green = "[00ff0c]";
    public const string Bule = "[00fff0]";
    public const string Purple = "[ff00f0]";
    public const string Orange = "[ff9000]";
    public const string Red = "[ff0000]";
    public const string Yellow = "[ffcc00]";
    public const string MainText = "[dcd5b8]";
    public const string SecondaryText = "[cbb694]";
    public const string ImportantText = "[ffcc00]";
    public const string WeakText = "[888580]";
    public const string ToolTipDone = "[00ff0c]";
    public const string ToolTipUnDone = "[ff0000]";
    public static string Propery = "[eee5c3]";
    public static string Title = "[FFCC30]";
    public static string SubTitle = "[b8a586]";
    public static string NPCMainText = "[3d1400]";
    public static string NPCSecondaryText = "[823C18]";
    public static string NPCImportantText = "[007900]";
    public static string ButtonGrey = "[C0C0C0]";
    public static string ButtonBrown = "[CFBFB0]";
    public static string ButtonGreen = "[B0BBCF]";
    public const string SabacTeam = "[2451ad]";
	public static string TabCheck = "[C7C7C7]";
	public static string TabBackground = "[969696]";

	public static Dictionary<int, Color> MonsterNameColorDic = new Dictionary<int, Color>()
    {
        {0, Color.white},
        {1, Color.white},
        {2, CSColor.green},
        {3, CSColor.blue},
        {4, CSColor.purple},
        {5, CSColor.orange},
    };

    public static string GetColorString(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.White:
                return White;
            case ColorType.Green:
                return Green;
            case ColorType.Blue:
                return Bule;
            case ColorType.Purple:
                return Purple;
            case ColorType.Orange:
                return Orange;
            case ColorType.Red:
                return Red;
            case ColorType.Yellow:
                return Yellow;
            case ColorType.MainText:
                return MainText;
            case ColorType.SecondaryText:
                return SecondaryText;
            case ColorType.ImportantText:
                return ImportantText;
            case ColorType.WeakText:
                return WeakText;
            case ColorType.ToolTipDone:
                return ToolTipDone;
            case ColorType.ToolTipUnDone:
                return ToolTipUnDone;
            case ColorType.TitleColor:
                return Title;
            case ColorType.SubTitleColor:
                return SubTitle;
            case ColorType.ProperyColor:
                return Propery;
            case ColorType.NPCMainText:
                return NPCMainText;
            case ColorType.NPCImportantText:
                return NPCImportantText;
            case ColorType.NPCSecondaryText:
                return NPCSecondaryText;
            case ColorType.CommonButtonGrey:
                return ButtonGrey;
            case ColorType.CommonButtonBrown:
                return ButtonBrown;
            case ColorType.CommonButtonGreen:
                return ButtonGreen;
            case ColorType.SabacTeam:
                return SabacTeam;
			case ColorType.TabCheck:
				return TabCheck;
			case ColorType.TabBackground:
				return TabBackground;
		}

        return string.Empty;
    }

    public static Color GetColor(ColorType colorType)
    {
        Color ret = Color.white;

        string colorStr = $"#{GetColorString(colorType)}";
        colorStr = colorStr.Replace("]", "");
        colorStr = colorStr.Replace("[", "");

        ColorUtility.TryParseHtmlString(colorStr, out ret);
        return ret;
    }

    public static string BBCode(this string old, ColorType colorType)
    {
        return $"{GetColorString(colorType)}{old}[-]";
    }

    public static string BBCode(this string old, int quality)
    {
        return $"{GetColorString(GetColorTypeByQuality((int) quality))}{old}[-]";
    }

    public static string GetColorStr(int quality)
    {
        switch (quality)
        {
            case 1: return White;
            case 2: return Green;
            case 3: return Bule;
            case 4: return Purple;
            case 5: return Orange;
        }

        return White;
    }

    public static ColorType GetColorTypeByQuality(int quality)
    {
        switch (quality)
        {
            case 1: return ColorType.White;
            case 2: return ColorType.Green;
            case 3: return ColorType.Blue;
            case 4: return ColorType.Purple;
            case 5: return ColorType.Orange;
        }

        return ColorType.White;
    }

    public static string GetItemNameValue(int quality)
    {
        switch (quality)
        {
            case 1: return GetColorString(ColorType.White);
            case 2: return GetColorString(ColorType.Green);
            case 3: return GetColorString(ColorType.Blue);
            case 4: return GetColorString(ColorType.Purple);
            case 5: return GetColorString(ColorType.Orange);
        }

        return GetColorString(ColorType.White);
    }

    static string[] colorName;

    public static string GetColorName(int qua)
    {
        if (colorName == null)
        {
            colorName = SundryTableManager.Instance.GetSundryEffect(395).Split('#');
        }

        return colorName[qua - 1];
    }

    /// <summary>
    /// 色值码转Color   格式为："#FECEE1"
    /// </summary>
    /// <param name="_hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string _hex)
    {
        Color nowColor;
        ColorUtility.TryParseHtmlString(_hex, out nowColor);
        return nowColor;
    }

    /// <summary>
    /// Color转色值码
    /// </summary>
    /// <param name="_color"></param>
    /// <returns></returns>
    public static string ColorToHex(Color _color)
    {
        return ColorUtility.ToHtmlStringRGB(_color);
    }


    private static string[] stringsQuality;
    /// <summary>
    /// 获取数字对应相应的品质文字
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    public static string GetQualityText(int quality)
    {
        if (stringsQuality == null)
            stringsQuality = UtilityMainMath.StrToStrArr(CSString.Format(1104));
        if (stringsQuality.Length < 6) return String.Empty;

        switch (quality)
        {
            case 1: return stringsQuality[0]; //白色
            case 2: return stringsQuality[1]; //绿色
            case 3: return stringsQuality[2]; //蓝色
            case 4: return stringsQuality[3]; //紫色
            case 5: return stringsQuality[4]; //橙色
            case 6: return stringsQuality[5]; //红色
        }

        return stringsQuality[0];
    }
}

public class UtilityCsColor : Singleton<UtilityCsColor>
{
    public Color GetColor(int quality)
    {
        switch (quality)
        {
            case 1: return CSColor.white;
            case 2: return CSColor.green;
            case 3: return CSColor.blue;
            case 4: return CSColor.purple;
            case 5: return CSColor.orange;
        }

        return CSColor.white;
    }
}

public struct CSColor
{
    /// <summary>
    /// 米色
    /// </summary>
    static Color _beige = new Color(220 / 255f, 213 / 255f, 184 / 255f);

    public static Color beige
    {
        get { return _beige; }
    }

    static Color _green = new Color(0f, 1f, 12 / 255f);

    public static Color green
    {
        get { return _green; }
    }

    static Color _blue = new Color(0f, 1f, 240 / 255f);

    public static Color blue
    {
        get { return _blue; }
    }

    static Color _purple = new Color(1f, 0f, 240 / 255f);

    public static Color purple
    {
        get { return _purple; }
    }

    static Color _orange = new Color(1f, 144 / 255f, 0f);

    public static Color orange
    {
        get { return _orange; }
    }

    static Color _white = new Color(0.933f, 0.933f, 0.933f);

    public static Color white
    {
        get { return _white; }
    }

    static Color _gray = new Color(136 / 255f, 133 / 255f, 128 / 255f);

    public static Color gray
    {
        get { return _gray; }
    }

    static Color _red = new Color(1f, 0f, 0f);

    public static Color red
    {
        get { return _red; }
    }

    static Color _yellow = new Color(1f, 204 / 255f, 0f);

    public static Color yellow
    {
        get { return _yellow; }
    }
}