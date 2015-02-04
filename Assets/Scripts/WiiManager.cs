using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;
using System.Collections.Generic;
/**This class will parse the Wii JSON coming from the plugin*/
class WiiManager
{
    private static int joyX;
    private static int joyY;
    private static Boolean buttonC;
    private static Boolean buttonZ;

    public delegate void JoyChangeAction(WiiAxisEnum joyAxis, int joyValue);//any subscriber must take two paras and return void
    public static event JoyChangeAction JoyChanged;// static, no need to instantiate the class
    
    public delegate void WiiButtonChangeAction(WiiButtonCodeEnum code, Boolean buttonValue);//any subscriber must take two paras and return void
    /**Subsriber's call back function take two paras (WiiButtonCodeEnum code, Boolean buttonValue)*/
    public static event WiiButtonChangeAction WiiButtonChanged;// static, no need to instantiate the class

    public static int JoyX
    {
        get
        {
            return joyX;
        }
        set
        {
            if (joyX != value)
            {
                joyX = value;
                if (JoyChanged != null)
                {
                    JoyChanged(WiiAxisEnum.AXIS_X, value);
                }
            }
        }
    }
    public static int JoyY
    {
        get
        {
            return joyY;
        }
        set
        {
            if (joyY != value)
            {
                joyY = value;
                if (JoyChanged != null)
                {
                    JoyChanged(WiiAxisEnum.AXIS_Y, value);
                }
            }
        }
    }

    public static Boolean ButtonC
    {
        get
        {
            return buttonC;
        }
        set
        {
            if (buttonC != value)
            {
                if (buttonC != value)
                {
                    buttonC = value;
                    if (WiiButtonChanged != null)
                    {
                        WiiButtonChanged(WiiButtonCodeEnum.BUTTON_C, value);
                    }
                }
            }
        }
    }


    public static Boolean ButtonZ
    {
        get
        {
            return buttonZ;
        }
        set
        {
            if (buttonZ != value)
            {
                buttonZ = value;
                if (WiiButtonChanged != null)
                {
                    WiiButtonChanged(WiiButtonCodeEnum.BUTTON_Z, value);
                }
            }
        }
    }

    public static void setWiiData(string wiiJSON)
    {
        var parsedWiiJSON = JSONNode.Parse(wiiJSON);
        JoyX = int.Parse(parsedWiiJSON["a"]);
        JoyY = int.Parse(parsedWiiJSON["b"]);
        ButtonC = int.Parse(parsedWiiJSON["c"]) == 1;
        ButtonZ = int.Parse(parsedWiiJSON["d"]) == 1;
    }
}

