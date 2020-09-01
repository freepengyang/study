using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
//---------------------------------------------
//工具代码类
//Author LiWeiJia
//Time 2018.6.21
//-----------------------------------------------
public class ToolsTest : MonoBehaviour {

    #region 生成变量代码模板
    /// <summary>
    /// __LK__：左大括号{
    /// __RK__：右大括号}
    /// {0}：成员变量代码
    /// 
    /// UIPreafab命名规则：
    /// UILabel：lab_ 开头
    /// UISprite：spr_ 开头
    /// UIGridContainer：grid_ 开头
    /// GameObject：go_ 开头
    /// UIScrollView：scroll_ 开头
    /// 按钮：btn_开头
    /// UIToggle：tog_ 开头
    /// UISlider：slid_ 开头
    /// </summary>
    public const string BEHAVIOUR_FORMAT =
        "using UnityEngine;\r\n" +
        "using System;\r\n" +
        "using System.Collections;\r\n" +
        "using System.Collections.Generic;\r\n\r\n" +
        "public class FindCode : \r\n" +
        "__LK__\r\n" +
        "   {0}\r\n" +                                                      
        "__RK__\r\n";

    List<string> codeLis = new List<string>();

    [ContextMenu("Execute Generate VariableTemp")]
    private void GenerateFindCode()
    {
        codeLis.Clear();
        FormatFindCodeString(transform);

        string CaseCode = string.Join("\r\n    ", codeLis.ToArray());
        string code = string.Format(BEHAVIOUR_FORMAT, CaseCode);
        code = code.Replace("__LK__", "{");
        code = code.Replace("__RK__", "}");
        string path = Application.dataPath;

        string behaviourCodeFile = Path.Combine(path, "CodeTemp") + ".txt";

        if (File.Exists(behaviourCodeFile))
        {
            File.Delete(behaviourCodeFile);
        }
        
        using (FileStream fs = File.Create(behaviourCodeFile))
        {
            byte[] bytes = Encoding.Default.GetBytes(code);
            fs.Write(bytes, 0, bytes.Length);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void FormatFindCodeString(Transform tran, string path = "")
    {
        int childCount = tran.childCount;
        if (childCount == 0) return;

        for (int i = 0; i < childCount; i++)
        {
            Transform tranChild = tran.GetChild(i);
            FormatString(tranChild.gameObject.name, path + "/" + tranChild.gameObject.name, tranChild);
        }
    }

    private void FormatString(string name, string path, Transform tran)
    {
        if (!name.Contains("_"))
        {
            FormatFindCodeString(tran, path);
            return;
        }

        path = path.Remove(0,1);
        string[] sub = name.Split('_');
        string vName = "";
        codeLis.Add("");
        switch (sub[0])
        {
            case "lab":
                vName = "lab" + sub[1];
                codeLis.Add("   private UILabel " + vName + ";");
                codeLis.Add("   private UILabel mLab" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").GetComponent<UILabel>());" + "__RK____RK__");
                break;
            case "go":
                vName = "go" + sub[1];
                codeLis.Add("   private GameObject " + vName + ";");
                codeLis.Add("   private GameObject mGo" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").gameObject);" + "__RK____RK__");
                break;
            case "btn":
                vName = "btn" + sub[1];
                codeLis.Add("   private GameObject " + vName + ";");
                codeLis.Add("   private GameObject mBtn" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").gameObject);" + "__RK____RK__");
                break;
            case "scroll":
                vName = "scroll" + sub[1];
                codeLis.Add("   private UIScrollView " + vName + ";");
                codeLis.Add("   private UIScrollView mScroll" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").GetComponent<UIScrollView>());" + "__RK____RK__");
                break;
            case "grid":
                vName = "grid" + sub[1];
                codeLis.Add("   private UIGridContainer " + vName + ";");
                codeLis.Add("   private UIGridContainer mGrid" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").GetComponent<UIGridContainer>());" + "__RK____RK__");
                break;
            case "spr":
                vName = "spr" + sub[1];
                codeLis.Add("   private UISprite " + vName + ";");
                codeLis.Add("   private UISprite mSpr" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").GetComponent<UISprite>());" + "__RK____RK__");
                break;
            case "slid":
                vName = "slid" + sub[1];
                codeLis.Add("   private UISlider " + vName + ";");
                codeLis.Add("   private UISlider mSlid" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").GetComponent<UISlider>());" + "__RK____RK__");
                break;
            case "tog":
                vName = "tog" + sub[1];
                codeLis.Add("   private UIToggle " + vName + ";");
                codeLis.Add("   private UIToggle mTog" + sub[1] + "__LK__ get __LK__ return " + vName + " ?? " + "(" + vName + "= transform.Find(" + "\"" + path + "\"" + ").GetComponent<UIToggle>());" + "__RK____RK__");
                break;
        }

        FormatFindCodeString(tran, path);
    }
    #endregion
}
