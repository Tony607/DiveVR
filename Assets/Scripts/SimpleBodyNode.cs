using UnityEngine;
using System.Collections;

public class SimpleBodyNode : MonoBehaviour {

    /** quaternion read from body tracking node */
    private Quaternion rawQuaternion;
    /** quaternion aligned with user body */
    private Quaternion alignedQuaternion;
    /** quaternion initial aligned with user body */
    private Quaternion initialQuaternion;
    /** Reversed quaternion of initial aligned with user body */
    private Quaternion initialQuaternionReversed;
    private Quaternion camQuaternion;
    //constant quaternion used to rotate the left arm 180 degree around z axis to become right arm
    //ie. right arm need the quaternion to be (0, 0, -1, 0)
    //left arm need it to be (0,0,0,1)
    private Quaternion flipQuaternion = new Quaternion(0, 0, -1, 0);
    //slerp speed of the bone
    private float speed = 18;

    void Awake()
    {
        rawQuaternion = new Quaternion(0, 0, 0, 1);
        alignedQuaternion = new Quaternion(0, 0, 0, 1);
        initialQuaternion = new Quaternion(0, 0, 0, 1);
        initialQuaternionReversed = new Quaternion(0, 0, 0, 1);
        camQuaternion = new Quaternion(0, 0, 0, 1);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Cardboard.SDK.CardboardTriggered)
        {
            setRawAsInitial(Cardboard.SDK.HeadRotation);
        }
        updateObjRoation();
    }

    public void setRawQuaternion(Quaternion rot)
    {
        rawQuaternion = rot;
        calculateAlignedQuaternion();
    }
    /** set current body tracking node quaternion as the initial quaternion. */
    public void setRawAsInitial(Quaternion cameraRotation)
    {
        initialQuaternion = rawQuaternion;
        camQuaternion = cameraRotation;
        initialQuaternionReversed = Quaternion.Inverse(initialQuaternion);
        calculateAlignedQuaternion();
    }
    /**
 * calculate and update the aligned quaternion by multiplying it by the
 * inverse of the initial quaternion. <br/>
 * ie, Aligned = TnitialReversed x Raw
 */
    public void calculateAlignedQuaternion()
    {
        //alignedQuaternion = initialQuaternionReversed * rawQuaternion;
        //This is correct as long as the cordinate of the bone and the tracker is the same.(they are using the same world cordinate)
        //We rotate camQuaternion finally because we want the bone to rotate what the camera rotates(especially in yaw axis) so 
        //the bone is aligned from our eye perspective
        alignedQuaternion = rawQuaternion * initialQuaternionReversed * flipQuaternion * camQuaternion;
    }

    /**Function called by the implementing class to update the roation 
     * of the attached Transform*/
    public void updateObjRoation()
    {
        //attachedObjectTrans.transform.rotation = alignedQuaternion;
        float donePercentage = Mathf.Min(1F, Time.deltaTime * speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, alignedQuaternion, donePercentage);
    }
}
