using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeScript : MonoBehaviour
{
    private GameObject parentbone;
    private Vector3 lastPos;
    private Vector3 curVel;
    private bool releaseSignal = false;
    private bool attachSignal = false;

    private Vector3 lastPosition;
    private Vector3 speedVector;
    /**The roation axis caluclated over time of the attached item*/
    private Vector3 rotationAxis;
    /**The roation angle caluclated over time of the attached item*/
    private float rotationAngle = 0.0f;
    //those variable belongs to the class, same for each instances
    //the que size to hold the history transforms of parentbone
    const int queSize = 10;
    static Queue<Vector3> positionQueue;
    static Queue<Quaternion> rotationQueue;
    // Use this for initialization
    void Start()
    {
        parentbone = GameObject.Find("AttachPoint");
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        positionQueue = new Queue<Vector3>(queSize);
        rotationQueue = new Queue<Quaternion>(queSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            transform.rotation = parentbone.transform.rotation;
            transform.position = parentbone.transform.position;
        }
        if (gameObject.tag == "AttachedItem")
        {
            while (positionQueue.Count < queSize)
            {
                positionQueue.Enqueue(transform.position);
                rotationQueue.Enqueue(transform.rotation);
            }
            positionQueue.Dequeue();
            rotationQueue.Dequeue();
        }
    }
    void FixedUpdate()
    {
        if (releaseSignal)
        {
            releaseSignal = false;
            ReleaseMe();
        }
        if (attachSignal)
        {
            attachSignal = false;
            AttachMe();
        }
    }



    private void ReleaseMe()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            speedVector = transform.position - positionQueue.Peek();
            //calculate the spin
            Quaternion rotationQuaternion = rotationQueue.Peek()*Quaternion.Inverse(transform.rotation);
            rotationQuaternion.ToAngleAxis(out rotationAngle, out rotationAxis);
            //add the velocity
            rigidbody.AddForce(speedVector * Time.smoothDeltaTime *180f, ForceMode.VelocityChange);
            //add the spin
            rigidbody.AddTorque(-rotationAxis * rotationAngle * Time.smoothDeltaTime * 10f, ForceMode.VelocityChange);
            if (positionQueue.Count > 0)
            {//clear the queue
                positionQueue.Clear();
                rotationQueue.Clear();
            }
        }

    }

    private void AttachMe()
    {
        //the gray color is set by the hand collider when when it touch an item
        if (gameObject.tag == "FocusedItem")
        {
            transform.parent = parentbone.transform;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            //Attached object looks green
            renderer.material.color = Color.green;
            gameObject.tag = "AttachedItem";
        }

    }
    /**Set the release flag, it will be clear automaticly in the FixedUpdate
     function once physics release logic is executed*/
    public void ReleaseSelf()
    {
        releaseSignal = true;
    }
    /**Set the attach flag, it will be clear automaticly in the FixedUpdate
     function once physics attach logic is executed*/
    public void AttachSelf()
    {
        attachSignal = true;
    }
}
