using UnityEngine;
using System.Collections;

public class TableScale : MonoBehaviour
{
    public delegate void OnTableScaleClick(GameObject go);
    public OnTableScaleClick onTableScaleClick;

    public delegate void OnToolInitOver();
    public event OnToolInitOver onToolInitOver;
    public bool IsInitOver = false;

    public string Notice="注意：btns 有顺序的改动和程序确认下";
    public GameObject[] btns;
    public BetterList<UIPlayTween> twpList = new BetterList<UIPlayTween>();
    public BetterList<GameObject> arrowList = new BetterList<GameObject>();
    private GameObject tmpObj;


    public int CurPlayIndex = 0;
    private Vector3 RoatVecor90 = new Vector3(0, 0, 270);


    void Awake()
    {
        if (btns != null)
        {
            twpList.Clear();
            arrowList.Clear();

            for (int i = 0; i < btns.Length; i++)
            {
                if(btns[i] != null)
                {
                    UIPlayTween tp = btns[i].transform.GetComponent<UIPlayTween>();
                    //tmpObj = btns[i].transform.FindChild("arrow").gameObject;

                    if (tmpObj != null)
                    {
                        arrowList.Add(tmpObj);
                    }

                    if (tp != null)
                    {
                        twpList.Add(tp);
                        EventDelegate.Add(tp.onFinished, () =>
                        {
                            SetAllBtnCollider(true);
                        });
                    }
                    else
                    {
                        if (Debug.developerConsoleVisible) Debug.LogError(btns[i].name + ": no tweenposition in children ");
                    }

                    UIEventListener.Get(btns[i]).parameter = i;
                    UIEventListener.Get(btns[i].gameObject).onClick += OnBtnClick;
                }
            }

            if(onToolInitOver != null)
                onToolInitOver();

            IsInitOver = true;
        }
    }

    void OnDestroy()
    {
        if (btns != null)
        {
            for (int i = 0; i < btns.Length; i++)
            {
                if (btns[i] != null)
                {
                    UIEventListener.Get(btns[i].gameObject).onClick -= OnBtnClick;
                }
            }           
        }

        twpList.Clear();
        arrowList.Clear();
        tmpObj = null;
    }

    public void OnBtnClick(int index)
    {
        if (btns.Length > index)
        {
            tmpObj = btns[index];

            OnBtnClick(tmpObj);
        }
    }

    private int curIndex = -1;
    public void OnBtnClick(GameObject btn)
    {
        object obj = UIEventListener.Get(btn).parameter;
        if (obj == null) return;
        int index = (int)obj;       

        for (int i = 0; i < twpList.Count; i++)
        {
            if (twpList[i].tweenTarget.activeSelf && twpList[i].gameObject != btn.gameObject)
            {
                twpList[i].Play(false);     //设置打开的非点击收缩

            }
        }
        //SetAllBtnCollider(false);

        if(onTableScaleClick != null)
        {
            onTableScaleClick(btn);
        }

        curIndex = index;
        for (int i = 0; i < arrowList.Count; i++)
        {
            tmpObj = arrowList[i];

            if (curIndex == i)
            {
                tmpObj.transform.localEulerAngles = tmpObj.transform.localEulerAngles == RoatVecor90 ? (RoatVecor90 * 2) : RoatVecor90;
            }
            else
            {
                tmpObj.transform.localEulerAngles = RoatVecor90;
            }
        }
    }

    private void SetAllBtnCollider(bool state)
    {
        BoxCollider collider = null;
        if (btns != null)
        {
            for (int i = 0; i < btns.Length; i++)
            {
                if(btns[i] != null)
                {
                    collider = btns[i].GetComponent<BoxCollider>();
                    if (collider != null)
                    {
                        collider.enabled = state;
                    }
                }
            }
        }
    }

    //public void PlayByIndex(int index)
    //{
    //    if(twpList != null && twpList.Count > index && twpList[index].tweenTarget.activeSelf == false)
    //    {
    //        twpList[index].tweenTarget.SetActive(true);
    //        twpList[index].Play(true);
    //    }
    //    SetAllBtnCollider(true);
    //}
}
