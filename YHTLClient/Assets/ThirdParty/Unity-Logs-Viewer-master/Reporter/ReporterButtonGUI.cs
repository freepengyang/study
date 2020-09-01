using UnityEngine;
using System.Collections;
using System.Text;

public class ReporterButtonGUI : MonoBehaviour
{
    Reporter reporter;
    GUIContent errorContent;
    GUIStyle nonStyle;
    private Rect _rect;

    void Awake()
    {
        float height = (Screen.height *1.0f ) / 640 * 20;
        _rect = new Rect(0, Screen.height - height, height, height);
        GameObject obj = Resources.Load<GameObject>("Reporter");
        if (obj == null) return;
        GameObject reporterObj = Object.Instantiate(obj, transform, true);
        reporter = reporterObj.GetComponent<Reporter>();
        if (reporter == null)
        {
            reporter = reporterObj.AddComponent<Reporter>();
        }

        if (reporter.images != null)
            errorContent = new GUIContent("0", reporter.images.errorImage, "show or hide errors");
        nonStyle = new GUIStyle();
        nonStyle.clipping = TextClipping.Clip;
        nonStyle.normal.textColor = Color.red;
        nonStyle.fontSize = 20;
        nonStyle.fontStyle = FontStyle.Bold;
        nonStyle.alignment = TextAnchor.LowerLeft;
    }

    void OnGUI()
    {
        if (reporter == null || errorContent == null) return;

        if (Time.frameCount % 100 == 0)
        {
            errorContent.text = reporter.numOfLogsError.ToString();
        }

        if (GUI.Button(_rect, errorContent, nonStyle))
        {
            reporter.doShow();
        }
    }
}