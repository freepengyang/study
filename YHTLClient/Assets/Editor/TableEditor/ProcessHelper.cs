using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using Path = System.IO.Path;

public static class ProcessHelper
{
    public static Process Run(string exe, string arguments, string workingDirectory = ".", bool waitExit = false)
    {
        try
        {
            bool redirectStandardOutput = true;
            bool redirectStandardError = true;
            bool useShellExecute = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                redirectStandardOutput = false;
                redirectStandardError = false;
                useShellExecute = true;
            }

            if (waitExit)
            {
                redirectStandardOutput = true;
                redirectStandardError = true;
                useShellExecute = false;
            }

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = useShellExecute,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = redirectStandardOutput,
                RedirectStandardError = redirectStandardError,
            };

            Process process = Process.Start(info);

            if (waitExit)
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
                }
            }

            return process;
        }
        catch (Exception e)
        {
            throw new Exception($"dir: {Path.GetFullPath(workingDirectory)}, command: {exe} {arguments}", e);
        }
    }
    public static void ProtoToCS(string[] names)
    {
        try
        {
            string batName = "proto2cs.bat";
            var workDirectory = System.IO.Path.GetFullPath(Application.dataPath + "/../../ProtoGen/");
            if (!System.IO.Directory.Exists(workDirectory))
            {
                FNDebug.Log($"directory not exist for {workDirectory}");
                return;
            }
            var content = string.Empty;
            foreach (var key in names)
            {
                content += string.Format("protogen.exe -i:protos\\{0}.proto -o:table\\{0}.cs\n", key);
            }
            var storePath = workDirectory + batName;
            if (!string.IsNullOrEmpty(content))
            {
                System.IO.File.WriteAllText(storePath, content);
            }
            ExecuteBatFiles(batName, workDirectory, workDirectory);

            FNDebug.Log($"客户端协议转换成功");
        }
        catch (System.Exception e)
        {
            FNDebug.LogError($"客户端协议转换失败 : {e.Message}");
        }
    }

    public static System.Diagnostics.Process CreateShellExProcess(string cmd, string args, string workingDir = "") 
    { 
        var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd); 
        pStartInfo.Arguments = args; pStartInfo.CreateNoWindow = false; 
        pStartInfo.UseShellExecute = true; 
        pStartInfo.RedirectStandardError = false; 
        pStartInfo.RedirectStandardInput = false; 
        pStartInfo.RedirectStandardOutput = false; 
        if (!string.IsNullOrEmpty(workingDir)) 
            pStartInfo.WorkingDirectory = workingDir; 
        return System.Diagnostics.Process.Start(pStartInfo);
    }

    public static void ExecuteBatFiles(string batfile, string args, string workingDir = "")
    { 
        try
        {
            using (var p = CreateShellExProcess(batfile, args, workingDir))
            {
                p.WaitForExit();
                p.Close();
            }
        }
        catch(Exception e)
        {
            FNDebug.LogErrorFormat("ExecuteBatFiles {0} Failed,Exception e = {1}", batfile,e.Message);
        }
    }
}