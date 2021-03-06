//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System;

/**The class is expecting to the Hand object has children strcuted and named 
like this
Hand/IndexMeta/IndexProxPha/IndexIntePha
Hand/ThumbProxPha/ThumbIntePha/ThumbDistPha
 * */
public class GloveNode
{
    /** Value if the flex sensor value mapped between 0~255 */
    private int flexSensorValue = 0;
    //This value give the initial calibrated value of the flex sensor
    //In the sensor node the flex value 0~254 is mapping
    //the flex sensor analog read forward flexing range.
    //Write down the open hand mapped value, it should be close to 0
    //then replace the number below
    private int initialFlexSensorValue = 0;
    //degree between 0~90
    private float flexSensorBendDegree = 0;
    private bool handIsOpen = true;
    private float handOpenDegree = 15;
    private float handCloseDegree = 30;

    //The hand object
    private GameObject handObject;

    private Transform thumbIntePhaTransform;
    private Transform thumbDistPhaTransform;

    private Transform indexProxPhaTransform;
    private Transform indexIntePhaTransform;


    public GloveNode(GameObject gameobj)
    {
        handObject = gameobj;
        //The code is expecting two object attached to the hand object
        //IndexProxPha and IndexProxPha/IndexIntePha
        indexProxPhaTransform = handObject.transform.Find("IndexMeta/IndexProxPha");
        indexIntePhaTransform = indexProxPhaTransform.Find("IndexIntePha");

        thumbIntePhaTransform = handObject.transform.Find("ThumbProxPha/ThumbIntePha");
        thumbDistPhaTransform = thumbIntePhaTransform.Find("ThumbDistPha");
    }
    /**
     * Function to set the value of the glove node flexSensor is a value between
     * 0~254
     * */
    public void setValue(int flexSensor)
    {
        flexSensorValue = (int)(flexSensor & 0xFF);
        updateMappedDegree();

    }
    /**Set the current flex sensor value to represent the hand is opened*/
    public void setRawAsInitial()
    {
        initialFlexSensorValue = flexSensorValue;
    }
    /**Function called by the implementing class to update the roation 
     * of the attached GameObject*/
    public void updateObjRoation()
    {
        indexProxPhaTransform.localEulerAngles = new Vector3(
            indexProxPhaTransform.localEulerAngles.x,
            flexSensorBendDegree,
            indexProxPhaTransform.localEulerAngles.z
        );
        indexIntePhaTransform.localEulerAngles = new Vector3(
            indexIntePhaTransform.localEulerAngles.x,
            flexSensorBendDegree,
            indexIntePhaTransform.localEulerAngles.z
        );

        thumbIntePhaTransform.localEulerAngles = new Vector3(
            thumbIntePhaTransform.localEulerAngles.x,
            flexSensorBendDegree,
            thumbIntePhaTransform.localEulerAngles.z
            );
        thumbDistPhaTransform.localEulerAngles = new Vector3(
            thumbDistPhaTransform.localEulerAngles.x,
            flexSensorBendDegree - 180,
            thumbDistPhaTransform.localEulerAngles.z
            );

    }
    /**Calculate the flexSensorBendDegree*/
    private void updateMappedDegree()
    {
        flexSensorBendDegree = (flexSensorValue - initialFlexSensorValue) * 40f / (16f - initialFlexSensorValue);
        //Debug.Log("flexSensorBendDegree" + flexSensorBendDegree);
        //hand close/open logic
        if (handIsOpen && flexSensorBendDegree >= handCloseDegree)
        {
            handIsOpen = false;
            if (GameObject.FindWithTag("FocusedItem") != null)
            {
                GameObject.FindWithTag("FocusedItem").SendMessage("AttachSelf");
                handObject.transform.Find("AttachPoint").GetComponent<HandColliderLogic>().OnAttachChild();
            }
        }
        else if (!handIsOpen && flexSensorBendDegree <= handOpenDegree)
        {
            handIsOpen = true;
            Debug.Log("sending message to RealCube : ReleaseSelf");
            //TODO: uncomment this after the laggy finger issue is resolve
            if (GameObject.FindWithTag("AttachedItem") != null)
            {
                GameObject.FindWithTag("AttachedItem").SendMessage("ReleaseSelf");
                handObject.transform.Find("AttachPoint").GetComponent<HandColliderLogic>().OnReleaseChild();
            }
        }
        Debug.Log("flexSensorBendDegree" + flexSensorBendDegree);
    }
}


