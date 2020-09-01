using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
namespace ExtendEditor
{
    public class SelectionBase : EditorWindowBase<SelectionBase>
    {
        protected static List<string> mPathList = new List<string>();
        protected static List<string> mDirList = new List<string>();
        protected static Object[] mLastSelectObjs = new Object[0];
        protected static Object[] mSelectObjs = new Object[0];
        protected static int mHandleIndex = -1;
        protected virtual bool IsGetAllSelectFile
        {
            get { return true; }
        }

        public override void OnGUI()
        {
            base.OnGUI();
            mSelectObjs = FileUtility.GetFiltered();
            if (mSelectObjs.Length == 0) mHandleIndex = -1;
            DetectFilterChange();
            EditorGUILayout.LabelField("Select Object Num = " + mPathList.Count + " HandleIndex=" + mHandleIndex);
            NGUIEditorTools.DrawSeparator();
        }

        void DetectFilterChange()
        {
            if (mSelectObjs.Length != mLastSelectObjs.Length)
            {
                OnSelectionFilteredChange();
                return;
            }
            for (int i = 0; i < mSelectObjs.Length; i++)
            {
                bool isFind = false;
                for (int j = 0; j < mLastSelectObjs.Length; j++)
                {
                    if (mSelectObjs[i] == mLastSelectObjs[j])
                    {
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                {
                    OnSelectionFilteredChange();
                    break;
                }
            }
        }

        public void BeginHandle()
        {
            mHandleIndex = 0;
        }

        public bool CanHandle()
        {
            return mHandleIndex >= 0 && mHandleIndex < mPathList.Count;
        }

        public string GetHandlePath(int index)
        {
            if (index >= 0 && index < mPathList.Count)
                return mPathList[index];
            return "";
        }

        public string GetCurHandlePath()
        {
            return GetHandlePath(mHandleIndex);
        }


        public Object GetCurHandleObj()
        {
            return GetHandleObject(mHandleIndex);
        }

        public Object GetHandleObject(int index)
        {
            if (index >= 0 && index < mPathList.Count)
            {
                Object obj = AssetDatabase.LoadAssetAtPath(mPathList[index], typeof(Object));
                return obj;
            }
            return null;
        }

        public void MoveHandle()
        {
            if (!CanHandle()) return;
            mHandleIndex++;
            if (!CanHandle())
            {
                mHandleIndex = -1;
            }
        }

        public void End()
        {
            mHandleIndex = -1;
        }

        public override void OnSelectionChange()
        {
            base.OnSelectionChange();
        }

        public virtual void OnSelectionFilteredChange()
        {
            
            mPathList.Clear();
            mDirList.Clear();
            mSelectObjs = FileUtility.GetFiltered();
            mLastSelectObjs = mSelectObjs;
            if (IsGetAllSelectFile)
            {
                int index = 0;
                foreach (Object obj in mSelectObjs)
                {
                    string path = AssetDatabase.GetAssetPath(mSelectObjs[index]);
                    path = Application.dataPath.Replace("/Assets","/") + path;
                    FileUtility.GetDeepAssetPaths(path, mPathList);
                    FileUtility.GetDeepAssetDirs(path, mDirList);
                    index++;
                }
            }
        }
    }
}