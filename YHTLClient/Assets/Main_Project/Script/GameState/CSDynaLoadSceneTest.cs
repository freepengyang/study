using UnityEngine;
using System.Collections;

public class CSDynaLoadSceneTest : MonoBehaviour {

	// Use this for initialization
    private float intnalTime = 0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > intnalTime)
        {
            intnalTime = Time.time + 1;
            if (FNDebug.developerConsoleVisible) FNDebug.Log("动态加载场景测试=" + intnalTime);
        }
	}
}
