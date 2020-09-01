using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.Text;
using System.IO;
using System.Linq.Expressions;

public class FileTool
{

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="bytes"></param>
    public static void CreateFile(string filePath, byte[] bytes)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
          
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Close();
        }
    }

    /// <summary>
    /// 创建文本
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="text"></param>
    public static void CreateFile(string filePath, string text)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        using (StreamWriter stream = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            stream.Write(text);
        }
    }

    public static string ReadFile(string path)
    {
        string strContent = "";
        try
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                StreamReader strRead = new StreamReader(stream);
                strContent = strRead.ReadToEnd();
                stream.Close();
            }
        }
        catch {

        }

        return strContent;
    }

    public static void CheckFile(string path)
    {
        if (!File.Exists(path))
        {
            try
            {
                FileInfo file = new FileInfo(path);
                var dir = file.Directory;
                if(dir != null && !dir.Exists)
                    dir.Create();
                using (FileStream version = new FileStream(path, FileMode.Create))
                {
                    version.Close();
                }
            } 
            catch (IOException ex)
            {
                UnityEngine.Debug.LogError("Create File Exception: " + ex.Message.ToString());
            }
        }
    }
}
