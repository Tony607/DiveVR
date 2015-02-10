using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//The class manage the transform of the Body nodes
public class BodyManager : MonoBehaviour
{
    /**Body nodes objects stored in a Dictionary*/
    private static Dictionary<BodyIDEnum, BodyNode> bodyNodes;
    private static Transform playerGraphicTrans;
    static public BodyManager instance; //the instance of our class that will do the work
    //wait to calibrate delay in sec
    public static float calibrateDelay = 2.0F;
    static bool isWaitingForCalibration = false;
    private static bool mainBodyNodeIsPresent = false;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        bodyNodes = new Dictionary<BodyIDEnum, BodyNode>();
        //main body
        Quaternion flipQ = new Quaternion(0, 0, 0, 1);
        bodyNodes.Add(BodyIDEnum.MAIN_BODY, new BodyNode(transform, flipQ, true));
        //Arms
        playerGraphicTrans = transform.FindChild("Graphics");
        //right arm
        Transform armatureR = playerGraphicTrans.transform.Find("RightArm/Armature");
        Transform bigArmTransR = armatureR.GetChild(0);
        flipQ = new Quaternion(0, 0, -1, 0);
        bodyNodes.Add(BodyIDEnum.BIG_ARM_R, new BodyNode(bigArmTransR, flipQ, false));
        Transform smallArmTransR = bigArmTransR.GetChild(0);
        bodyNodes.Add(BodyIDEnum.SMALL_ARM_R, new BodyNode(smallArmTransR, flipQ, false));
        Transform handTransR = smallArmTransR.GetChild(0);
        bodyNodes.Add(BodyIDEnum.HAND_R, new BodyNode(handTransR, flipQ, false));
        //left arm
        flipQ.Set(0, 0, 0, 1);
        Transform armatureL = playerGraphicTrans.transform.Find("LeftArm/Armature");
        Transform bigArmTransL = armatureL.GetChild(0);
        bodyNodes.Add(BodyIDEnum.BIG_ARM_L, new BodyNode(bigArmTransL, flipQ, false));
        Transform smallArmTransL = bigArmTransL.GetChild(0);
        bodyNodes.Add(BodyIDEnum.SMALL_ARM_L, new BodyNode(smallArmTransL, flipQ, false));
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        if (Cardboard.SDK.CardboardTriggered)
        {
            calibrateTpose();
        }
        foreach (var bone in bodyNodes)
        {
            bone.Value.updateObjRoation();
        }

#endif
    }
    public static void setNodeQuaternion(IdQuaternion quaternion)
    {
        //disable the PlayerRotation default roation if we received data from the main body tracker
        if (!mainBodyNodeIsPresent && (BodyIDEnum)quaternion.id == BodyIDEnum.MAIN_BODY)
        {
            mainBodyNodeIsPresent = true;
            instance.disableDefaultMainBodyRotation();
        }
        bodyNodes[(BodyIDEnum)quaternion.id].setRawQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
    public void disableDefaultMainBodyRotation()
    {
        gameObject.GetComponent<PlayerRotation>().enabled = false;
    }
    /**The calibration routine for body tracking node*/
    public static void calibrateTpose()
    {
        if (!isWaitingForCalibration)
        {
            PluginManager.vibrateForMs(100);
            isWaitingForCalibration = true;
            instance.StartCoroutine(instance.WaitAndCalibrate(calibrateDelay));
        }
    }
    IEnumerator WaitAndCalibrate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("WaitAndCalibrate " + Time.time);
        foreach (var bone in bodyNodes)
        {
            bone.Value.setRawAsInitial(Cardboard.SDK.HeadRotation);
        }
        isWaitingForCalibration = false;
        //vibrate phone a little bit to nodify the user the calibration is done
        PluginManager.vibrateForMs(100);
    }
}
