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
    //those variable belongs to the class, same for each instances
    //the que size to hold the history transforms of parentbone
    const int queSize = 10;
    static Queue<Vector3> tranformQueue;
    // Use this for initialization
    void Start()
    {
        parentbone = GameObject.Find("AttachPoint");
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        tranformQueue = new Queue<Vector3>(queSize);
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
            while (tranformQueue.Count < queSize)
            {
                tranformQueue.Enqueue(transform.position);
            }
            tranformQueue.Dequeue();
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
            speedVector = transform.position - tranformQueue.Peek();
            rigidbody.AddForce(speedVector * 4, ForceMode.VelocityChange);
            if (tranformQueue.Count > 0)
            {//clear the queue
                tranformQueue.Clear();
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
