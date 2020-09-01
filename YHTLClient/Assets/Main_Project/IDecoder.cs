using System.Data;
using System.IO;
using UnityEngine.Rendering;

public interface ITable
{

}

public interface IDecoder
{
    ITable DeEncode(System.IO.Stream stream, byte[] buffer);
}

public static class GDecoder
{
    public static int[] intValues;
    public static int intLength;
    public static string[] stringValues;
    public static long[] longValues;
    public static int[] varIntValues;
    public static long[] varLongValues;
    public static string[] varStringValues;
    public static int stringLength;
    public static int longLength;
    public static int pos;
    static byte[] contents;
    static byte[] rules;
    static int cnt;
    static int readCnt;
    static int intValueLength;
    static int stringValueLength;
    static int longValueLength;

    public static int[] intValuesExtend = new int[128];
    public static string[] stringValuesExtend = new string[128];
    public static long[] longValuesExtend = new long[128];

    public static TableData LoadTable(byte[] cs, byte[] rs)
    {
        int cnt = Begin(cs, rs);
        TableData iLFastModeGItem = new TableData();
        var gintValues = iLFastModeGItem.intValues = new int[intValueLength * cnt];
        var gstringValues = iLFastModeGItem.stringValues = new string[stringValueLength * cnt];
        var glongValues = iLFastModeGItem.longValues = new long[longValueLength * cnt];
        intLength = 0;
        stringLength = 0;
        longLength = 0;
        iLFastModeGItem.id2offset = new System.Collections.Generic.Dictionary<int,TableHandle>(cnt);
        iLFastModeGItem.handles = new TableHandle[cnt];
        int ent = rules.Length;
        for(int i = 0; i < cnt;++i)
        {
            for (int j = 0; j < ent; ++j)
            {
                if (rules[j] == 1)
                {
                    gintValues[intLength] = ReadInt(contents, ref pos);
                    if (j == 0)
                    {
                        var handle = new TableHandle
                        {
                            data = iLFastModeGItem,
                            intOffset = i * intValueLength,
                            stringOffset = i * stringValueLength,
                            longOffset = i * longValueLength,
                        };
                        handle.key = gintValues[intLength];
                        iLFastModeGItem.handles[i] = handle;
                        iLFastModeGItem.id2offset.Add(gintValues[intLength], handle);
                    }
                    ++intLength;
                    continue;
                }

                if (rules[j] == 2)
                {
                    gstringValues[stringLength] = ReadString(contents, ref pos);
                    ++stringLength;
                    continue;
                }

                if (rules[j] == 3)
                {
                    glongValues[longLength] = ReadLong(contents, ref pos);
                    ++longLength;
                    continue;
                }
            }
        }
        return iLFastModeGItem;
    }

    public static int Begin(byte[] cs, byte[] rs)
    {
        pos = 0;
        intLength = 0;
        stringLength = 0;
        longLength = 0;
        contents = cs;
        rules = rs;
        readCnt = 0;
        varIntValues = null;
        varStringValues = null;
        varLongValues = null;

        cnt = ReadInt(contents, ref pos);
        if(cnt > 0)
            varIntValues = new int[cnt];
        for(int i = 0,max = cnt; i < max;++i)
            varIntValues[i] = ReadInt(contents, ref pos);

        cnt = ReadInt(contents, ref pos);
        if (cnt > 0)
            varStringValues = new string[cnt]; 
        for (int i = 0, max = cnt; i < max; ++i)
            varStringValues[i] = ReadString(contents, ref pos);

        cnt = ReadInt(contents, ref pos);
        if (cnt > 0)
            varLongValues = new long[cnt];
        for (int i = 0, max = cnt; i < max; ++i)
            varLongValues[i] = ReadLong(contents, ref pos);

        cnt = ReadInt(contents, ref pos);
        intValueLength = ReadInt(contents, ref pos);
        stringValueLength = ReadInt(contents, ref pos);
        longValueLength = ReadInt(contents, ref pos);

        return cnt;
    }

    public static void ReadRawContent()
    {
        intLength = 0;
        stringLength = 0;
        longLength = 0;

        intValues = new int[intValueLength];
        stringValues = new string[stringValueLength];
        longValues = new long[longValueLength];
        int ent = rules.Length;
        for (int j = 0; j < ent; ++j)
        {
            if (rules[j] == 1)
            {
                intValues[intLength] = ReadInt(contents, ref pos);
                ++intLength;
                continue;
            }

            if (rules[j] == 2)
            {
                stringValues[stringLength] = ReadString(contents, ref pos);
                ++stringLength;
                continue;
            }

            if (rules[j] == 3)
            {
                longValues[longLength] = ReadLong(contents, ref pos);
                ++longLength;
                continue;
            }
        }
        ++readCnt;
    }

    static int ReadInt(byte[] contents,ref int pos)
    {
        int v = contents[pos] | (contents[pos + 1] << 8) | (contents[pos + 2] << 16) | (contents[pos + 3] << 24);
        pos += 4;
        return v;
    }

    static long ReadLong(byte[] contents, ref int pos)
    {
        int key = ReadInt(contents, ref pos);
        long value = ReadInt(contents, ref pos);
        long val = ((value & 0xFFFFFFFF) << 32) | ((long)key);
        return val;
    }

    static string ReadString(byte[] contents, ref int pos)
    {
        int length = contents[pos] | (contents[pos + 1] << 8) | (contents[pos + 2] << 16) | (contents[pos + 3] << 24);
        pos += 4;
        string v = System.Text.Encoding.UTF8.GetString(contents,pos,length);
        pos += length;
        return v;
    }
}