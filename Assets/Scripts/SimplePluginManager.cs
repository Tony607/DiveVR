using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class SimplePluginManager : MonoBehaviour
{
#if !UNITY_EDITOR
    AndroidJavaClass pluginTutorialActivityJavaClass;
#endif
    static public SimplePluginManager instance; //the instance of our class that will do the work
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
#if !UNITY_EDITOR
            Debug.Log("Calling AndroidJNI.AttachCurrentThread()");
            AndroidJNI.AttachCurrentThread();
            pluginTutorialActivityJavaClass = new AndroidJavaClass("com.zcw607.motion.PluginTest");
#endif

    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Emulate a cardboard trigger event
            cardboardTrigger("X");
        }

        string str;
        if (Input.GetMouseButtonDown(0))
        {
            str = "{\"a\":16,\"b\":0}";
            setGloveDataJSON(str);
            Time.timeScale = 2f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            str = "{\"a\":0,\"b\":0}";
            setGloveDataJSON(str);
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Emulate a closed hand and glove contact push
            str = "{\"a\":16,\"b\":1}";
            setGloveDataJSON(str);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            //Emulate a closed hand and glove contact push
            str = "{\"a\":16,\"b\":0}";
            setGloveDataJSON(str);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //Emulate a Crouch
            str = "{\"a\":0,\"b\":0,\"c\":1,\"d\":0}";
            setWiiDataJSON(str);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            //Emulate a Crouch
            str = "{\"a\":0,\"b\":0,\"c\":0,\"d\":0}";
            setWiiDataJSON(str);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Emulate a Jump
            str = "{\"a\":0,\"b\":3,\"c\":0,\"d\":1}";
            setWiiDataJSON(str);
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            //Emulate a Jump
            str = "{\"a\":0,\"b\":3,\"c\":0,\"d\":0}";
            setWiiDataJSON(str);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            //Emulate a step detector call
            stepDetected("1");
        }
#endif
    }
    /**Call the Java Plugin Function to finish the Plugin Activity*/
    private void ExitGame()
    {
#if !UNITY_EDITOR
        pluginTutorialActivityJavaClass.CallStatic("exitPlugin");
#endif
        Application.Quit();
    }
    public static void vibrateForMs(int durationMs)
    {
#if !UNITY_EDITOR
        instance.pluginTutorialActivityJavaClass.CallStatic("vibrateForMs",durationMs);
#endif

    }
    /**Function called by the Android plugin to update a quaternion of a body tracker with id*/
    void setBodyQuaternionJSON(string quaternionJSON)
    {
        IdQuaternion quaternion = new IdQuaternion(quaternionJSON);
        if (quaternion.id == 0)// master node
        {
            Quaternion rawQ = new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            GameObject.Find("GameBone").SendMessage("setRawQuaternion", rawQ);
        }
    }
    /**Function called by the Android plugin to set the new glove data*/
    void setGloveDataJSON(string gloveJSON)
    {
    }
    /**Function called by the Android plugin to set the new glove data*/
    void setWiiDataJSON(string wiiJSON)
    {
    }
    /**Function called by the Android plugin when cardboard trigger button is pushed*/
    void cardboardTrigger(string triggerType)
    {
    }
    /**Function called by the Android plugin when barometer value changes*/
    void barometerValue(string sensorValue)
    {
        //Debug.Log("barometerValue"+float.Parse(sensorValue));
    }
    /**Function called by the Android plugin when step detector detect new step*/
    void stepDetected(string newstep)
    {
    }
}
