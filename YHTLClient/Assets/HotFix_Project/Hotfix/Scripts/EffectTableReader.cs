using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class EffectTableReader
{
    byte[] buffer = new byte[1024];
    Stream stream;

    public void Reset()
    {
        System.Array.Clear(buffer,0,buffer.Length);
    }

    public void ReadInt()
    {
        var path = "C:/work/YHTLProject/Client/Trunk/YHTLClient/Assets/HotFix_Project/Hotfix/Scripts/HotMain.cs";
        using (FileStream fsw = new FileStream(path, FileMode.Open))
        {
            fsw.Seek(0,SeekOrigin.Begin);
            int cnt = 0;
            while ((cnt = fsw.Read(buffer, 0, buffer.Length)) > 0)
                FNDebug.LogFormat("ReadCnt = {0}",cnt);
        }
    }
}

public static class EncodeStreamHelper
{
    public static void Encode(this Stream stream,int value)
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)(value & 0xFF);
        buffer[1] = (byte)((value >> 8) & 0xFF);
        buffer[2] = (byte)((value >> 16) & 0xFF);
        buffer[3] = (byte)(value >> 24);
        stream.Write(buffer,0,buffer.Length);
    }

    public static void Encode(this Stream stream, long value)
    {
        byte[] buffer = new byte[8];
        buffer[0] = (byte)(value & 0xFF);
        buffer[1] = (byte)((value >> 8) & 0xFF);
        buffer[2] = (byte)((value >> 16) & 0xFF);
        buffer[3] = (byte)((value >> 24) & 0xFF);
        buffer[4] = (byte)((value >> 32) & 0xFF);
        buffer[5] = (byte)((value >> 40) & 0xFF);
        buffer[6] = (byte)((value >> 48) & 0xFF);
        buffer[7] = (byte)((value >> 56) & 0xFF);
        stream.Write(buffer, 0, buffer.Length);
    }

    public static void Encode(this Stream stream, uint value)
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)(value & 0xFF);
        buffer[1] = (byte)((value >> 8) & 0xFF);
        buffer[2] = (byte)((value >> 16) & 0xFF);
        buffer[3] = (byte)(value >> 24);
        stream.Write(buffer, 0, buffer.Length);
    }

    public static void Encode(this Stream stream, string value)
    {
        //写入长度
        if (string.IsNullOrEmpty(value))
            value = string.Empty;
        byte[] writeBytes = Encoding.UTF8.GetBytes(value);
        stream.Encode(writeBytes.Length);
        stream.Write(writeBytes,0,writeBytes.Length);
    }

    public static int DecodeInt(this Stream stream,byte[] buffer)
    {
        stream.Read(buffer, 0, 4);
        int v = buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24);
        return v;
    }

    public static string DecodeString(this Stream stream,byte[] buffer)
    {
        stream.Read(buffer, 0, 4);
        int v = buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24);
        stream.Read(buffer, 0, v);
        return Encoding.UTF8.GetString(buffer,0,v);
    }

    public static unsafe int DecodeUnsafeInt(byte[] buffer, int* pos)
    {
        int pv = *(pos);
        int v = 0;
        fixed (byte* x = &buffer[pv])
        {
            v = x[0] | (x[1] << 8) | (x[2] << 16) | (x[3] << 24);
        }
        *(pos) += 4;
        return v;
    }

    public static unsafe string DecodeUnsafeString(byte[] buffer, int* pos)
    {
        int pv = *(pos);
        int v = 0;
        fixed (byte* x = &buffer[pv])
        {
            v = x[0] | (x[1] << 8) | (x[2] << 16) | (x[3] << 24);
            *(pos) += 4 + v;
            return Encoding.UTF8.GetString(&x[4],v);
        }
    }
}