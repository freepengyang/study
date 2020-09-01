using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBase
{
    protected string express = string.Empty;
    public virtual string Express
    {
        get
        {
            return express;
        }
    }
    protected string logic = string.Empty;
    public virtual string Logic
    {
        get
        {
            return logic;
        }
    }
    public enum LinkType
    {
        LT_EMOTION = 0,
        LT_LOCATION = 1,
        LT_ITEM = 2,
    }
    protected LinkType eLinkType = LinkType.LT_EMOTION;
    public LinkType Link
    {
        get
        {
            return eLinkType;
        }
    }

    public string Replace(string value)
    {
        int end = value.Length - Express.Length;
        for (int i = 0; i <= end; ++i)
        {
            bool equal = true;
            for(int j = 0; j < Express.Length; ++j)
            {
                if(Express[j] != value[i + j])
                {
                    equal = false;
                    break;
                }
            }

            if (equal)
            {
                string retValue = string.Empty;
                if (i > 0)
                    retValue += value.Substring(0, i);

                retValue += Logic;

                if (i + Express.Length < value.Length)
                    retValue += value.Substring(i + Express.Length);

                return retValue;
            }
        }
        return value;
    }
}

public class ItemLink : LinkBase
{
    private static string pattern = "\\[[0-9A-Fa-f]{6}\\]|\\[-\\]";
    public string bytes { get; set; }
    public TABLE.ITEM Item { get; set; }
    public int quality { get; set; }
    public ItemLink()
    {
        eLinkType = LinkType.LT_ITEM;
        Item = null;
        bytes = string.Empty;
    }

    public override string Express
    {
        get
        {
            if (string.IsNullOrEmpty(express))
            {
                string strName = Item.name;
                if(System.Text.RegularExpressions.Regex.IsMatch(strName, pattern))
                {
                    var matches = System.Text.RegularExpressions.Regex.Matches(strName, pattern);
                    if (matches.Count > 1)
                    {
                        strName = strName.Replace(matches[0].Value, string.Empty).Replace(matches[1].Value, string.Empty);
                    }
                }
                express = CSString.Format(313, strName);
            }
            return express;
        }
    }

    public override string Logic
    {
        get
        {
            if (string.IsNullOrEmpty(logic))
            {
                logic = $"[url=func:{(int)CSLinkFunction.CSLF_ITEM_LINK}:item:{Item.id}:{bytes}][u]{Item.name.BBCode(quality)}[/u][/url]";
            }
            return logic;
        }
    }
}

public class EmotionLink : LinkBase
{
    public string Emotion {get;set;}
    public EmotionLink()
    {
        eLinkType = LinkType.LT_EMOTION;
        Emotion = string.Empty;
    }

    const string EmotionReg = @"\[(emoticon\d+)\]";
    const string EmotionReplace = @"【$1】";

    public override string Express
    {
        get
        {
            if(string.IsNullOrEmpty(express))
            {
                express = System.Text.RegularExpressions.Regex.Replace(Emotion, EmotionReg, EmotionReplace);
            }
            return express;
        }
    }

    public override string Logic
    {
        get
        {
            if (string.IsNullOrEmpty(logic))
            {
                logic = Emotion;
            }
            return logic;
        }
    }
}

public class LocationLink : LinkBase
{
    public int MapID 
    {
        get
        {
            return null == MapInfo ? 0 : MapInfo.id;
        }
    }
    public int CoordX { get; set; }
    public int CoordY { get; set; }
    public TABLE.MAPINFO MapInfo { get; set; }
    public LocationLink()
    {
        eLinkType = LinkType.LT_LOCATION;
        CoordX = 0;
        CoordY = 0;
        MapInfo = null;
    }

    public override string Express
    {
        get
        {
            if(string.IsNullOrEmpty(express) && null != MapInfo)
            {
                express = CSString.Format(312, MapInfo.name, CoordX, CoordY);
            }
            return express;
        }
    }

    public override string Logic
    {
        get
        {
            if(string.IsNullOrEmpty(logic) && null != MapInfo)
            {
                logic = $"[url=func:{(int)CSLinkFunction.CSLF_LOCATION_LINK}:find:{MapInfo.id}:{CoordX}:{CoordY}:{MapInfo.name}]{CSString.Format(333)} [u][00f0ed][{MapInfo.name}{CoordX},{CoordY}][-][/u][/url]";
            }
            return logic;
        }
    }
}