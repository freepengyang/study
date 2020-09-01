using System;
using System.Collections;
using System.Collections.Generic;
public class CSNetBase
{
    public virtual void NetCallback(ECM _type, NetInfo obj)
    {

    }

    public virtual void HandByNetCallback(ECM _type, NetInfo obj)
    {

    }
}

public abstract class CSNetFactory
{
    private static Dictionary<string, CSNetBase> netObjs = new Dictionary<string, CSNetBase>(128); //key:Type.name, value: obj
    public static CSNetBase Get(int Id)
    {
        CSNetBase netAction = null;
        try
        {
            var type = NetCallabck.GetCallbackType(Id);

            if (type != null)
            {
                string netName = type.Name;

                if (!netObjs.ContainsKey(netName))
                {
                    netAction = Activator.CreateInstance(type) as CSNetBase;

                    netObjs[netName] = netAction;
                }
                else
                {
                    netAction = netObjs[netName] as CSNetBase;
                }
            }   
        }
        catch (Exception ex)
        {
            FNDebug.LogError("create error:" + ex);
        }
        return netAction;
    }

    public static void Reset()
    {
        netObjs.Clear();
    }
}
