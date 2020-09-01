using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

public class FPS : CSGameMgrBase2<FPS>
{
    private float mTimer;
    private int mframeCounter;
    private float mFps;
    public float Fps
    {
        get { return mFps; }
        set
        {
            //TODO:ddn
            //if (CSGame.Sington != null && CSGame.Sington.CurCSGameState.State == EGameState.MainScene)     //mFps != value && 
            //{
            //    UIRedPointManager.Instance.UpdateByFPS(value);
            //}
            mFps = value;
            //if (CSDynamicChangeSetting.Instance != null) CSDynamicChangeSetting.Instance.FPS = value;
        }
    }
    //private PerformanceCounter cpuCounter;
    //private PerformanceCounter ramCounter;


    public void Start()
    {
        mTimer = 0;
        mframeCounter = 0;

        //PerformanceCounterCategory.Exists("PerformanceCounter");

        //cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        // ramCounter = new ComputerInfo();

        //cpuCounter = new PerformanceCounter();

        //cpuCounter.CategoryName = "Processor";
        //cpuCounter.CounterName = "% Processor Time";
        //cpuCounter.InstanceName = "_Total";

        //ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        //InvokeRepeating("setData", 1, 1);
        //InvokeRepeating("callAndroid", 2, 2);
    }

    private void setData()
    {
        NGUIDebug.Log();
    }

    //public string getCurrentCpuUsage()
    //{
    //    return cpuCounter.NextValue() + "%";
    //}

    //public string getAvailableRAM()
    //{
    //    return ramCounter.NextValue() + "MB";
    //}

    //public double GetCpuPercent()
    //{
    //    var percentage = cpuCounter.NextValue();
    //    return System.Math.Round(percentage, 2, System.MidpointRounding.AwayFromZero);
    //}
    //public double GetMemoryPercent()
    //{
    //    var usedMem = this.cinf.TotalPhysicalMemory - this.cinf.AvailablePhysicalMemory;//总内存减去可用内存
    //    return Math.Round(
    //             (double)(usedMem / Convert.ToDecimal(this.cinf.TotalPhysicalMemory) * 100),
    //             2,
    //             MidpointRounding.AwayFromZero);
    //}
    void Update()
    {
        mTimer += Time.deltaTime;
        mframeCounter++;

        if (mTimer > CSConstant.UpdateInterval)
        {
            Fps = mframeCounter / mTimer;
            mframeCounter = 0;
            mTimer = 0;
        }
    }


#if true
    void OnGUI()
    {
        if (mFps >= 30)
        {
            GUI.color = Color.green;
        }
        else if (mFps < 10)
        {
            GUI.color = Color.red;
        }
        else
        {
            GUI.color = Color.yellow;
        }

        mFps = (int)mFps;
        //if (CSScene.Sington != null)
        //    GUI.Label(new Rect(0, Screen.height - 200, 250, 50), "client.state:" + CSNetwork.Instance.IsConnect);
        //if (CSScene.Sington != null && CSScene.Sington.AvaterListDic.ContainsKey((int)EAvatarType.Player))
        //    GUI.Label(new Rect(0, Screen.height - 300, 250, 50), "player count:" + CSScene.Sington.AvaterListDic[(int)EAvatarType.Player].Count);
        //GUI.Label(new Rect(0, Screen.height - 65, 250, 50), "Time:" + CSServerTime.Instance.ServerNows.ToString("yyyy-MM-dd HH:mm:ss"));
        GUI.Label(new Rect(0, Screen.height - 50, 250, 50), "FPS:" + mFps);
        GUI.Label(new Rect(0, Screen.height - 75, 250, 50), "analyticPackageCount:   " + CSNetwork.Instance.analyticPackageCount);
        GUI.Label(new Rect(0, Screen.height - 100, 250, 50), "client.state:  " + CSNetwork.Instance.IsConnect);
        GUI.Label(new Rect(0, Screen.height - 125, 250, 50), "thread.IsAlive:  " + CSNetwork.Instance.IsAlive);
        //if (CSScene.Sington != null && CSScene.Sington.EnterScene != null)
        //    GUI.Label(new Rect(0, Screen.height - 150, 250, 50), "Scene Line:  " + CSScene.Sington.EnterScene.line);
        //GUI.Label(new Rect(0, Screen.height - 175, 250, 50), "Server Time:  " + CSServerTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
#endif
}
