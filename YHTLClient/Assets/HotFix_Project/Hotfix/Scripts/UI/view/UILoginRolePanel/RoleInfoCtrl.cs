using UnityEngine;
using System.Collections;

public class RoleInfoCtrl {

    public GameObject obj;

    public GameObject obj_CreateRole;
    public GameObject obj_ChooseRole;
    public bool hasRole = false;
    public GameObject obj_highlight;
    public UILabel name;
    public UILabel level;
    public UISprite icon;
    public UISprite iconHL;

    public RoleInfoCtrl(GameObject go)
    {

        obj = go;
        Start();
    }

    public void Start()
    {
        obj_CreateRole = obj.transform.Find("CreateRole").gameObject;
        obj_ChooseRole = obj.transform.Find("ChooseRole").gameObject;
        obj_highlight = obj_ChooseRole.transform.Find("high_light").gameObject;
        icon = obj.transform.Find("ChooseRole/not_choose").GetComponent<UISprite>();
        iconHL = obj.transform.Find("ChooseRole/high_light").GetComponent<UISprite>();
        name = obj_ChooseRole.transform.Find("name").GetComponent<UILabel>();
        level = obj_ChooseRole.transform.Find("level").GetComponent<UILabel>();
    }
}
