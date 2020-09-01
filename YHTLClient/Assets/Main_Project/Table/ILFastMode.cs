public class TableHandle
{
    public int key;
    public int intOffset;
    public int stringOffset;
    public int longOffset;
    public TableData data;
    public object Value;
}
public class TableData
{
    //public int id { get; set; }
    public int[] intValues;// { get; set; }
    public long[] longValues;// { get; set; }
    public string[] stringValues;// { get; set; }
    public System.Collections.Generic.Dictionary<int,TableHandle> id2offset;
    public TableHandle[] handles;
}