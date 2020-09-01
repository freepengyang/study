using UnityEngine;
using System.Collections;

public class CSInputController : MonoBehaviour {

    public string KeyOne;
    public GameObject One;

    public string KeyTwo;
    public GameObject Two;

    public string KeyThree;
    public GameObject Three;

    public string KeyFour;
    public GameObject Four;

    public string KeyFive;
    public GameObject Five;

    public string KeySix;
    public GameObject Six;
	
	// Update is called once per frame
	void Update () 
	{
        if (KeyOne != "" && Input.GetKeyDown(KeyOne))
        {
            if (One != null)
            {
                //UIShotcutConfigTemplate temp = One.GetComponent<UIShotcutConfigTemplate>();
                //if (temp != null)
                //{
                //    UIMainSkillPanel.Singleton.OnClickShortcut(temp);
                //}
            }
        }

        if (KeyTwo != "" && Input.GetKeyDown(KeyTwo))
        {
            if (Two != null)
            {
                //UIShotcutConfigTemplate temp = Two.GetComponent<UIShotcutConfigTemplate>();
                //if (temp != null)
                //{
                //    UIMainSkillPanel.Singleton.OnClickShortcut(temp);
                //}
            }
        }

        if (KeyThree != "" && Input.GetKeyDown(KeyThree))
        {
            if (Three != null)
            {
                //UIShotcutConfigTemplate temp = Three.GetComponent<UIShotcutConfigTemplate>();
                //if (temp != null)
                //{
                //    UIMainSkillPanel.Singleton.OnClickShortcut(temp);
                //}
            }
        }

        if (KeyFour != "" && Input.GetKeyDown(KeyFour))
        {
            if (Four != null)
            {
                //UIShotcutConfigTemplate temp = Four.GetComponent<UIShotcutConfigTemplate>();
                //if (temp != null)
                //{
                //    UIMainSkillPanel.Singleton.OnClickShortcut(temp);
                //}
            }
        }

        if (KeyFive != "" && Input.GetKeyDown(KeyFive))
        {
            if (Five != null)
            {
                //UIShotcutConfigTemplate temp = Five.GetComponent<UIShotcutConfigTemplate>();
                //if (temp != null)
                //{
                //    UIMainSkillPanel.Singleton.OnClickShortcut(temp);
                //}
            }
        }

        if (KeySix != "" && Input.GetKeyDown(KeySix))
        {
            if (Six != null)
            {
                //UIShotcutConfigTemplate temp = Six.GetComponent<UIShotcutConfigTemplate>();
                //if (temp != null)
                //{
                //    UIMainSkillPanel.Singleton.OnClickShortcut(temp);
                //}
            }
        }
    }
	
}
