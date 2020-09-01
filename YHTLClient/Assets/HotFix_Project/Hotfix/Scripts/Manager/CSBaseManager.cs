using System.Collections.Generic;

public class CSBaseManager : Singleton<CSBaseManager>
{
    public Dictionary<string, CSBase> BaseDic = new Dictionary<string, CSBase>();
    
    public void AddBase(string name, CSBase info)
    {
        if(!BaseDic.ContainsKey(name))
        {
            BaseDic.Add(name, info);
        }
    }
    
    public void  Reset()
    {
        var csBaseDic = BaseDic.GetEnumerator();
        while (csBaseDic.MoveNext())
        {
            csBaseDic.Current.Value.OnDispose();
        }

        csBaseDic.Dispose();
        BaseDic.Clear();
    }
}