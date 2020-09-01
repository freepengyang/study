using UnityEngine;
using System.Collections;

public class CSInput
{
    public static float LeftJoystickX
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public static float LeftJoystickY
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }

    public static float RightJoystickX
    {
        get
        {
            return Input.GetAxis("Right Joystick X");
        }
    }

    public static float RightJoystickY
    {
        get
        {
            return Input.GetAxis("Right Joystick Y") * -1;
        }
    }

    public static float DPadX
    {
        get
        {
            return Input.GetAxis("DPad X");
        }
    }

    public static float DPadY
    {
        get
        {
            return Input.GetAxis("DPad Y");
        }
    }

    public static bool JoystickKeyDownA
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton0);
        }
    }

    public static bool JoystickKeyUpA
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton0);
        }
    }

    public static bool JoystickKeyDownB
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton1);
        }
    }

    public static bool JoystickKeyUpB
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton1);
        }
    }

    public static bool JoystickKeyDownX
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton2);
        }
    }

    public static bool JoystickKeyUpX
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton2);
        }
    }

    public static bool JoystickKeyDownY
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton3);
        }
    }

    public static bool JoystickKeyUpY
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton3);
        }
    }

    public static bool JoystickKeyDownLB
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton4);
        }
    }

    public static bool JoystickKeyUpLB
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton4);
        }
    }

    public static bool JoystickKeyDownRB
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton5);
        }
    }

    public static bool JoystickKeyUpRB
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton5);
        }
    }

    public static bool JoystickKeyDownLT
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton6);
        }
    }

    public static bool JoystickKeyUpLT
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton6);
        }
    }

    public static bool JoystickKeyDownRT
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton7);
        }
    }

    public static bool JoystickKeyUpRT
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton7);
        }
    }

    public static bool JoystickKeyLS
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton8);
        }
    }

    public static bool JoystickKeyRS
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton9);
        }
    }

    public static bool JoystickKeyStart
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton10);
        }
    }

    public static bool JoystickKeyDownBack
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton11);
        }
    }

    public static bool JoystickKeyUpBack
    {
        get
        {
            return Input.GetKeyUp(KeyCode.JoystickButton11);
        }
    }

    public static bool JoystickKey12
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton12);
        }
    }

    public static bool JoystickKey13
    {
        get
        {
            return Input.GetKeyDown(KeyCode.JoystickButton13);
        }
    }
}
