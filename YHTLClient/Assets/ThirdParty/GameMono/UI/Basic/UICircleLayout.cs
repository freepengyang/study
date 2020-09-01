using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class UICircleLayout : MonoBehaviour 
{
	public float radius;
	public bool arrangeInCenter;
	public List<Transform> List = new List<Transform>();
	[Range(0,9)]
	public int offset;
	[Range(0f,360f)]
	public float firstAngle;
	public bool HideInactive = true;

	public List<Transform> GetChildList()
	{
		List.Clear();
		for(int i=0;i<transform.childCount;i++)
		{
			Transform t = transform.GetChild(i);
			if(t==null || (HideInactive&&!t.gameObject.activeSelf)) continue;
			List.Add(t);
		}
		return List;
	}

	[ContextMenu("Execute")]
	void Reposition()
	{
		GetChildList();
		int count = arrangeInCenter ? List.Count - 1 : List.Count;
		float averageAngle = count == 0 ? 2 * Mathf.PI : 2 * Mathf.PI / count;
		for(int i = 0; i < List.Count; i++)
		{
			Transform t = List[i];
			UILabel key = t.Find("key").GetComponent<UILabel>();
			if(key != null) key.text = string.Format("[fff7af]{0}", i);
			
			float angle = arrangeInCenter ? averageAngle * (i - 1) : averageAngle * i;
//			angle += offset * averageAngle;
			angle += firstAngle*Mathf.Deg2Rad;
			float r = arrangeInCenter && i == 0 ? 0 : radius;
			
			t.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * r;
		}
	}
	
	void OnValidate()
	{
		Reposition();
	}
}
