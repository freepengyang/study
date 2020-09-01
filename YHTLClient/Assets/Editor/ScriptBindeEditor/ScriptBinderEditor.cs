using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using System.Text;
using Object = UnityEngine.Object;

[CustomEditor(typeof(ScriptBinder))]
public class ScriptBinderEditor : Editor
{
    protected SerializedProperty components = null;
    protected SerializedProperty labelSpace = null;
    protected SerializedProperty scriptStatus = null;
    protected SerializedProperty labelTypeID = null;
    protected string mInitializeCode = string.Empty;
    protected List<string> mInitializeCodeGUI = new List<string>();
    protected List<string> mDeclareCodeGUI = new List<string>();

    protected string getDeclareCode()
    {
        string ret = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < mDeclareCodeGUI.Count; ++i)
        {
            stringBuilder.Append(mDeclareCodeGUI[i]);
            if (i != mDeclareCodeGUI.Count - 1)
            {
                stringBuilder.Append("\r\n");
            }
        }

        ret = stringBuilder.ToString();
        return ret;
    }

    protected string getInitializeCode()
    {
        string ret = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("protected override void _InitScriptBinder()\n");
        stringBuilder.Append("{\n");
        for (int i = 0; i < mInitializeCodeGUI.Count; ++i)
        {
            stringBuilder.Append("\t");
            stringBuilder.Append(mInitializeCodeGUI[i]);
            if (i != mInitializeCodeGUI.Count - 1)
            {
                stringBuilder.Append("\r\n");
            }
        }

        stringBuilder.Append("}\n");
        ret = stringBuilder.ToString();
        return ret;
    }

    public void OnEnable()
    {
        components = serializedObject.FindProperty("scriptItems");
        labelSpace = serializedObject.FindProperty("labelSpace");
        scriptStatus = serializedObject.FindProperty("scriptStatus");
        labelTypeID = serializedObject.FindProperty("labelTypeId");
        createInitializeCodes();
        createDeclareCodes();
    }

    void _menuFunction(object value)
    {
        var argv = value as object[];
        if (null != argv && argv.Length == 2)
        {
            try
            {
                ScriptBinderItem component = argv[1] as ScriptBinderItem;
                if (null != component)
                {
                    component.component = argv[0] as UnityEngine.Object;
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogErrorFormat(ex.ToString());
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.color = new Color32(0x6a, 0xff, 0x5e, 0xFF);
        GUILayout.BeginVertical("GroupBox");

        GUI.color = Color.gray;
        GUILayout.BeginVertical("GroupBox");
        GUI.color = Color.magenta;
        EditorGUILayout.LabelField("ClientScriptBinder(神器)", GUILayout.MinWidth(100));
        GUILayout.EndVertical();

        GUI.color = Color.gray;
        GUILayout.BeginVertical("GroupBox");
        GUI.color = Color.white;
        base.OnInspectorGUI();
        GUILayout.EndVertical();

        EditorGUI.BeginChangeCheck();
        SearchObjectGUI();
        OnScriptItemGUI();
        OnAppyClassGUI();
        //OnDeclareCodeGUI();
        //OnInitializedCodeGUI();
        OnScriptStatusGUI();
        OnDragAddItemGUI();


        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            createInitializeCodes();
            createDeclareCodes();
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }


    protected void OnScriptItemGUI()
    {
        GUILayout.BeginVertical("GroupBox");
        for (int i = 0; i < components.arraySize; ++i)
        {
            var scriptBindItem = components.GetArrayElementAtIndex(i);
            if (null != scriptBindItem)
            {
                GUI.color = Color.gray;
                //GUILayout.BeginVertical("GroupBox");

                //ScriptBinderItem
                //SerializedProperty hashCode = scriptBindItem.FindPropertyRelative("iHashCode");
                SerializedProperty component = scriptBindItem.FindPropertyRelative("component");
                SerializedProperty varName = scriptBindItem.FindPropertyRelative("varName");
                //SerializedProperty locked = scriptBindItem.FindPropertyRelative("locked");
                ScriptBinderItem scriptItem = null;
                if (i < (target as ScriptBinder).scriptItems.Length)
                {
                    scriptItem = (target as ScriptBinder).scriptItems[i];
                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                GUI.color = Color.white;
                //locked.boolValue = EditorGUILayout.Toggle(locked.boolValue);
                //GUI.enabled = locked.boolValue;
                varName.stringValue = EditorGUILayout.TextField(varName.stringValue);
                GUI.enabled = true;
                //hashCode.intValue = varName.stringValue.GetHashCode();
                GUI.color = Color.white;
                //EditorGUILayout.LabelField(hashCode.intValue.ToString(), GUILayout.MaxWidth(80));

                //EditorGUILayout.EndHorizontal();

                //EditorGUILayout.BeginHorizontal();
                component.objectReferenceValue =
                    EditorGUILayout.ObjectField(component.objectReferenceValue, typeof(UnityEngine.Object), true) as
                        UnityEngine.Object;
                if (null != component.objectReferenceValue)
                {
                    GameObject gameObject = component.objectReferenceValue as GameObject;
                    if (null == gameObject)
                    {
                        if (component.objectReferenceValue is UnityEngine.Component unityComponent)
                        {
                            gameObject = unityComponent.gameObject;
                        }
                    }

                    if (GUILayout.Button("S", "GV Gizmo DropDown", GUILayout.MaxWidth(30)))
                    {
                        UnityEngine.Component[] coms = gameObject.GetComponents<UnityEngine.Component>();

                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("GameObject"), component.objectReferenceValue is GameObject,
                            _menuFunction, new object[] {gameObject, scriptItem});
                        if (null != coms)
                        {
                            for (int j = 0; j < coms.Length; ++j)
                            {
                                menu.AddItem(new GUIContent(coms[j].GetType().Name),
                                    component.objectReferenceValue == coms[j], _menuFunction,
                                    new object[] {coms[j], scriptItem});
                            }
                        }

                        menu.ShowAsContext();
                    }
                }
                //EditorGUILayout.EndHorizontal();

                //EditorGUILayout.BeginHorizontal();
                /*if (GUILayout.Button("insert"))
                {
                    if (i > 0)
                    {
                        components.InsertArrayElementAtIndex(i - 1);
                    }
                    else
                    {
                        components.InsertArrayElementAtIndex(i);
                    }
                }
                if (GUILayout.Button("append"))
                {
                    components.InsertArrayElementAtIndex(i);
                }
                GUI.enabled = !string.IsNullOrEmpty(varName.stringValue);
                if (GUILayout.Button("getcode"))
                {
                    string codeInfo = getCopyString(component, varName,false);
                    if (!string.IsNullOrEmpty(codeInfo))
                    {
                        GUIUtility.systemCopyBuffer = codeInfo;
                        UnityEngine.Debug.LogErrorFormat("<color=#00ff00>copy succeed : {0}</color>", codeInfo);
                    }
                }*/
                //GUI.enabled = true;
                if (GUILayout.Button("X"))
                {
                    components.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndHorizontal();

                //GUILayout.EndVertical();
            }
        }

        GUILayout.EndVertical();

        GUI.color = Color.gray;
        EditorGUILayout.BeginVertical("GroupBox");
        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal();
        GUI.color = Color.green;
        EditorGUILayout.LabelField("Object TotalCount:", GUILayout.MinWidth(60));
        GUI.color = Color.white;
        GUI.enabled = false;
        EditorGUILayout.IntField(components.arraySize, GUILayout.MinWidth(60));
        GUI.enabled = true;
        if (GUILayout.Button("  +   ", GUILayout.MinWidth(60)))
        {
            components.InsertArrayElementAtIndex(components.arraySize);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void OnDragAddItemGUI()
    {
        var eventType = Event.current.type;
        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var o in DragAndDrop.objectReferences)
                {
                    AddReference(o.name, o);
                }
            }

            Event.current.Use();
        }
    }

    private string objectName = "";

    private void SearchObjectGUI()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Search", GUILayout.MaxWidth(50));
        objectName = EditorGUILayout.TextField(objectName);
        EditorGUILayout.EndHorizontal();
        if (!string.IsNullOrEmpty(objectName))
        {
            int size = components.arraySize;
            for (int i = 0; i < size; i++)
            {
                var element = components.GetArrayElementAtIndex(i);
                if (element.FindPropertyRelative("component").objectReferenceValue.name == objectName)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty component = element.FindPropertyRelative("component");
                    component.objectReferenceValue =
                        EditorGUILayout.ObjectField(component.objectReferenceValue, typeof(UnityEngine.Object), true) as
                            UnityEngine.Object;
                    if (component.objectReferenceValue != null)
                    {
                        EditorGUILayout.TextField(element.FindPropertyRelative("varName").stringValue);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    private void AddReference(string key, Object obj)
    {
        int index = components.arraySize;
        components.InsertArrayElementAtIndex(index);
        var element = components.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("component").objectReferenceValue = obj;
        element.FindPropertyRelative("varName").stringValue = key;
    }

    protected string getCopyString(SerializedProperty component, SerializedProperty varName, bool bDeclare)
    {
        if (null != component && null != varName && null != component.objectReferenceValue)
        {
            string enumVarName = varName.stringValue;
            string componentName = component.objectReferenceValue.GetType().FullName;
            string fmtContent = string.Empty;
            if (!bDeclare)
            {
                fmtContent = "m{0} = ScriptBinder.GetObject(\"{0}\") as {1};";
            }
            else
            {
                fmtContent = "{1} m{0};";
            }

            fmtContent = string.Format(fmtContent, varName.stringValue, componentName);
            return fmtContent;
        }

        return string.Empty;
    }

    protected void OnScriptStatusGUI()
    {
        if (scriptStatus.arraySize > 0)
        {
            for (int i = 0; i < scriptStatus.arraySize; ++i)
            {
                var component = scriptStatus.GetArrayElementAtIndex(i);
                if (null != component)
                {
                    GUI.color = Color.gray;
                    EditorGUILayout.BeginVertical("GroupBox");
                    GUI.color = Color.white;

                    var hashCode = component.FindPropertyRelative("iHashCode");
                    var statusName = component.FindPropertyRelative("statusName");
                    var action = component.FindPropertyRelative("action");
                    var locked = component.FindPropertyRelative("locked");

                    GUI.color = Color.white;
                    EditorGUILayout.BeginHorizontal();
                    locked.boolValue = EditorGUILayout.Toggle(locked.boolValue);
                    GUI.enabled = locked.boolValue;
                    statusName.stringValue = EditorGUILayout.TextField(statusName.stringValue);
                    GUI.enabled = true;
                    hashCode.intValue = statusName.stringValue.GetHashCode();
                    GUI.color = Color.white;
                    EditorGUILayout.LabelField(hashCode.intValue.ToString(), GUILayout.MaxWidth(80));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.PropertyField(action);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("insert"))
                    {
                        if (i > 0)
                            scriptStatus.InsertArrayElementAtIndex(i - 1);
                        else
                            scriptStatus.InsertArrayElementAtIndex(i);
                    }

                    if (GUILayout.Button("append"))
                    {
                        scriptStatus.InsertArrayElementAtIndex(i);
                    }

                    if (GUILayout.Button("execute action"))
                    {
                        var script = (target as ScriptBinder);
                        script._SetAction(statusName.stringValue);
                    }

                    if (GUILayout.Button("-"))
                    {
                        scriptStatus.DeleteArrayElementAtIndex(i);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
            }
        }

        GUI.color = Color.gray;
        EditorGUILayout.BeginVertical("GroupBox");
        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal();
        GUI.color = Color.green;
        EditorGUILayout.LabelField("Status TotalCount:", GUILayout.MinWidth(60));
        GUI.color = Color.white;
        GUI.enabled = false;
        EditorGUILayout.IntField(scriptStatus.arraySize, GUILayout.MinWidth(60));
        GUI.enabled = true;
        if (GUILayout.Button("  +   ", GUILayout.MinWidth(60)))
        {
            scriptStatus.InsertArrayElementAtIndex(scriptStatus.arraySize);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    StringBuilder stringBuilder = new StringBuilder(1024);
    protected const string FixedHint = "Apply Class Code";

    protected void OnAppyClassGUI()
    {
        if (null == this.target as ScriptBinder)
        {
            return;
        }

        var hintText = $"生成[{(this.target as ScriptBinder).gameObject.name}] 基类=> UIBase";
        GUI.color = Color.gray;
        EditorGUILayout.BeginVertical("GroupBox");
        GUI.color = Color.white;
        if (GUILayout.Button(hintText))
        {
            GenerateClassCode("UIBase");
        }

        hintText = $"生成[{(this.target as ScriptBinder).gameObject.name}] 基类=> UIBasePanel";
        if (GUILayout.Button(hintText))
        {
            GenerateClassCode("UIBasePanel");
            TryCreateHandbyCode();
        }

        hintText = $"生成[{(this.target as ScriptBinder).gameObject.name}]";
        if (GUILayout.Button(hintText))
        {
            GenerateClassCode("");
        }

        EditorGUILayout.EndVertical();
        GUI.color = Color.white;
    }

    protected void TryCreateHandbyCode()
    {
        ScriptBinder binder = this.target as ScriptBinder;
        if (null == binder)
            return;

        var className = binder.gameObject.name;
        if (!className.StartsWith("UI") || !className.EndsWith("Panel"))
        {
            UnityEngine.Debug.LogErrorFormat(
                "<color=#ff0000>generate class file failed class name must match UI***Panel!</color>");
            return;
        }

        var filePath = Application.dataPath + "/HotFix_Project/Hotfix/Scripts/UI/view";
        var files = System.IO.Directory.GetFiles(filePath, $"{className}.cs", System.IO.SearchOption.AllDirectories);
        if(files.Length > 0)
        {
            return;
        }

        for(int i = 0; i < files.Length; ++i)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(files[i]);
            if (fileName.Equals(className))
            {
                FNDebug.LogFormat("<color=#00ff00>[{0}] already existed</color>",fileName);
                return;
            }
        }

        var storeFileName = $"{filePath}/{className}.cs";

        stringBuilder.Clear();

        stringBuilder.AppendLine($"public partial class {className} : UIBasePanel");
        stringBuilder.AppendLine("{");

        stringBuilder.AppendLine("\tpublic override void Init()");
        stringBuilder.AppendLine("\t{");
        stringBuilder.AppendLine("\t\tbase.Init();");
        stringBuilder.AppendLine("\t}");
        stringBuilder.AppendLine("\t");

        stringBuilder.AppendLine("\tpublic override void Show()");
        stringBuilder.AppendLine("\t{");
        stringBuilder.AppendLine("\t\tbase.Show();");
        stringBuilder.AppendLine("\t}");
        stringBuilder.AppendLine("\t");

        stringBuilder.AppendLine("\tprotected override void OnDestroy()");
        stringBuilder.AppendLine("\t{");
        stringBuilder.AppendLine("\t\tbase.OnDestroy();");
        stringBuilder.AppendLine("\t}");
        
        stringBuilder.AppendLine("}");

        System.IO.File.WriteAllText(storeFileName, stringBuilder.ToString(), Encoding.UTF8);
        FNDebug.LogFormat("<color=#00ff00>generate [{0}] for handby code succeed</color>", className);
    }

    protected void GenerateClassCode(string baseClassName = "UIBase")
    {
        ScriptBinder binder = this.target as ScriptBinder;
        if (null != binder)
        {
            var repeatedValue = string.Empty;
            if (checkVarNameRepeated(ref repeatedValue))
            {
                UnityEngine.Debug.LogErrorFormat(
                    "<color=#ff0000>generate class file failed repeated name = [<color=#00ff00>{0}</color>]!</color>",
                    repeatedValue);
                return;
            }

            var className = binder.gameObject.name;
            if (!className.StartsWith("UI") || !className.EndsWith("Panel"))
            {
                UnityEngine.Debug.LogErrorFormat(
                    "<color=#ff0000>generate class file failed class name must match UI***Panel!</color>");
                return;
            }

            stringBuilder.Clear();
            if (string.IsNullOrEmpty(baseClassName))
                stringBuilder.AppendLine($"public partial class {binder.gameObject.name}");
            else
                stringBuilder.AppendLine($"public partial class {binder.gameObject.name} : {baseClassName}");
            stringBuilder.AppendLine("{");

            for (int i = 0; i < mDeclareCodeGUI.Count; ++i)
            {
                stringBuilder.Append("\tprotected ");
                stringBuilder.Append(mDeclareCodeGUI[i]);
                stringBuilder.Append("\n");
            }

            stringBuilder.Append("\tprotected override void _InitScriptBinder()\n");
            stringBuilder.Append("\t{\n");
            for (int i = 0; i < mInitializeCodeGUI.Count; ++i)
            {
                stringBuilder.Append("\t\t");
                stringBuilder.Append(mInitializeCodeGUI[i]);
                stringBuilder.Append("\n");
            }

            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine("}");

            try
            {
                var storePath = Application.dataPath +
                                $"/HotFix_Project/Hotfix/Scripts/UI/Generated/view/{className}.cs";
                System.IO.File.WriteAllText(storePath, stringBuilder.ToString(), Encoding.UTF8);
                AssetDatabase.Refresh();
                UnityEngine.Debug.LogFormat("<color=#00ff00>generate {0} succeed !</color>", className);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogErrorFormat(
                    "<color=#ff0000>generate class file failed for {0} reason:{1}!</color>", className, e.Message);
            }
        }
    }

    protected void OnDeclareCodeGUI()
    {
        if (mDeclareCodeGUI.Count > 0)
        {
            GUI.color = Color.gray;
            EditorGUILayout.BeginVertical("GroupBox");
            GUI.color = Color.green;
            for (int i = 0; i < mDeclareCodeGUI.Count; ++i)
            {
                EditorGUILayout.LabelField(mDeclareCodeGUI[i]);
            }

            GUI.color = Color.white;
            if (GUILayout.Button("copy declaration code"))
            {
                GUIUtility.systemCopyBuffer = string.Empty;
                var repeatedValue = string.Empty;
                if (!checkVarNameRepeated(ref repeatedValue))
                {
                    GUIUtility.systemCopyBuffer = getDeclareCode();
                    UnityEngine.Debug.LogErrorFormat("<color=#00ff00>copy succeed !</color>");
                }
                else
                {
                    GUIUtility.systemCopyBuffer = string.Format("copy failed repeated name = [{0}]!", repeatedValue);
                    UnityEngine.Debug.LogErrorFormat(
                        "<color=#ff0000>copy failed repeated name = [<color=#00ff00>{0}</color>]!</color>",
                        repeatedValue);
                }
            }

            EditorGUILayout.EndVertical();
            GUI.color = Color.white;
        }
    }

    protected void OnInitializedCodeGUI()
    {
        if (mInitializeCodeGUI.Count > 0)
        {
            GUI.color = Color.gray;
            EditorGUILayout.BeginVertical("GroupBox");
            GUI.color = Color.green;
            for (int i = 0; i < mInitializeCodeGUI.Count; ++i)
            {
                EditorGUILayout.LabelField(mInitializeCodeGUI[i]);
            }

            GUI.color = Color.white;
            if (GUILayout.Button("copy initialize code"))
            {
                GUIUtility.systemCopyBuffer = string.Empty;
                var repeatedValue = string.Empty;
                if (!checkVarNameRepeated(ref repeatedValue))
                {
                    GUIUtility.systemCopyBuffer = getInitializeCode();
                    UnityEngine.Debug.LogErrorFormat("<color=#00ff00>copy succeed !</color>");
                }
                else
                {
                    GUIUtility.systemCopyBuffer = string.Format("copy failed repeated name = [{0}]!", repeatedValue);
                    UnityEngine.Debug.LogErrorFormat(
                        "<color=#ff0000>copy failed repeated name = [<color=#00ff00>{0}</color>]!</color>",
                        repeatedValue);
                }
            }

            EditorGUILayout.EndVertical();
            GUI.color = Color.white;
        }
    }

    protected bool checkVarNameRepeated(ref string repeatValue)
    {
        List<string> varNames = new List<string>();
        for (int i = 0; i < components.arraySize; ++i)
        {
            var scriptBindItem = components.GetArrayElementAtIndex(i);
            if (null != scriptBindItem)
            {
                SerializedProperty varName = scriptBindItem.FindPropertyRelative("varName");
                if (!string.IsNullOrEmpty(varName.stringValue))
                {
                    if (varNames.Contains(varName.stringValue))
                    {
                        repeatValue = varName.stringValue;
                        return true;
                    }

                    varNames.Add(varName.stringValue);
                }
            }
        }

        return false;
    }

    protected void createInitializeCodes()
    {
        mInitializeCodeGUI.Clear();
        for (int i = 0; i < components.arraySize; ++i)
        {
            var scriptBindItem = components.GetArrayElementAtIndex(i);
            if (null != scriptBindItem)
            {
                SerializedProperty component = scriptBindItem.FindPropertyRelative("component");
                SerializedProperty varName = scriptBindItem.FindPropertyRelative("varName");
                if (!string.IsNullOrEmpty(varName.stringValue))
                {
                    var copyString = getCopyString(component, varName, false);
                    if (!string.IsNullOrEmpty(copyString))
                    {
                        mInitializeCodeGUI.Add(copyString);
                    }
                }
            }
        }
    }

    protected void createDeclareCodes()
    {
        mDeclareCodeGUI.Clear();
        for (int i = 0; i < components.arraySize; ++i)
        {
            var scriptBindItem = components.GetArrayElementAtIndex(i);
            if (null != scriptBindItem)
            {
                SerializedProperty component = scriptBindItem.FindPropertyRelative("component");
                SerializedProperty varName = scriptBindItem.FindPropertyRelative("varName");
                if (!string.IsNullOrEmpty(varName.stringValue))
                {
                    var copyString = getCopyString(component, varName, true);
                    if (!string.IsNullOrEmpty(copyString))
                    {
                        mDeclareCodeGUI.Add(copyString);
                    }
                }
            }
        }
    }
}