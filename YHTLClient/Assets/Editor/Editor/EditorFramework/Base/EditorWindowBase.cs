using UnityEngine;
using System.Collections;
using UnityEditor;
namespace ExtendEditor
{
    public interface IEditorWindowBase
    {
        void OnEnable();
        void OnGUI();
        void OnInspectorUpdate();
        void OnSelectionChange();
        void OnProjectChange();
        void OnDestroy();
    }

    public class EditorWindowBase<T> : EditorWindow, IEditorWindowBase
    {
        public virtual void OnEnable()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        public virtual void OnGUI()
        {

        }

        public virtual void OnInspectorUpdate()
        {
            Repaint();
        }

        public virtual void OnSelectionChange()
        {

        }

        public virtual void OnProjectChange()
        {

        }

        public virtual void OnDestroy()
        {
            EditorUtil.IsDealingTexFormat = false;
            EditorUtility.UnloadUnusedAssetsImmediate();
            System.GC.Collect();
        }
    }
}