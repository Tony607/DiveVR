using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//The class manage the transform of the Body nodes
public class BodyManager : MonoBehaviour
{
    /**Body nodes objects stored in a Dictionary*/
    private static Dictionary<BodyIDEnum, BodyNode> bodyNodes;
    //public GameObject bodyModelGameObject;
    public GameObject playerGraphicGameObject;
    private static Transform playerGraphicTrans;
    static public BodyManager instance; //the instance of our class that will do the work

    static bool isWaitingForCalibration = false;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        bodyNodes = new Dictionary<BodyIDEnum, BodyNode>();
        playerGraphicTrans = playerGraphicGameObject.transform;
        Transform armatureR = playerGraphicTrans.transform.Find("RightArm/Armature");
        Transform bigArmTransR = armatureR.GetChild(0);
        Quaternion flipQ = new Quaternion(0, 0, -1, 0);
        bodyNodes.Add(BodyIDEnum.BIG_ARM_R, new BodyNode(bigArmTransR, flipQ));
        Transform smallArmTransR = bigArmTransR.GetChild(0);
        bodyNodes.Add(BodyIDEnum.SMALL_ARM_R, new BodyNode(smallArmTransR, flipQ));
        Transform handTransR = smallArmTransR.GetChild(0);
        bodyNodes.Add(BodyIDEnum.HAND_R, new BodyNode(handTransR, flipQ));
        //left arm
        flipQ.Set(0, 0, 0, 1);
        Transform armatureL = playerGraphicTrans.transform.Find("LeftArm/Armature");
        Transform bigArmTransL = armatureL.GetChild(0);
        bodyNodes.Add(BodyIDEnum.BIG_ARM_L, new BodyNode(bigArmTransL, flipQ));
        Transform smallArmTransL = bigArmTransL.GetChild(0);
        bodyNodes.Add(BodyIDEnum.SMALL_ARM_L, new BodyNode(smallArmTransL, flipQ));
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
        bodyNodes[(BodyIDEnum)quaternion.id].setRawQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
    /**The calibration routine for body tracking node*/
    public static void calibrateTpose()
    {
        if (!isWaitingForCalibration)
        {
            PluginManager.vibrateForMs(100);
            isWaitingForCalibration = true;
            instance.StartCoroutine(instance.WaitAndCalibrate(2.0F));
        }
    }
    IEnumerator WaitAndCalibrate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("WaitAndCalibrate " + Time.time);
        foreach (var bone in bodyNodes)
        {
            bone.Value.setRawAsInitial(playerGraphicTrans.rotation);
        }
        isWaitingForCalibration = false;
        //vibrate phone a little bit to nodify the user the calibration is done
        PluginManager.vibrateForMs(100);
    }
}
