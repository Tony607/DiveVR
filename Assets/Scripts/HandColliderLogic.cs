using UnityEngine;
using System.Collections;

public class HandColliderLogic : MonoBehaviour
{
    private bool hasItemFocused = false;
    private bool isReleasingObject = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /**The */
    public void OnAttachChild(){
        gameObject.GetComponent<Collider>().isTrigger = true;
    }
    public void OnReleaseChild() {
        isReleasingObject = true;
    }
    void OnTriggerEnter(Collider theCollider)
    {
        if (theCollider.tag == "Items" && !hasItemFocused)
        {
            //Foucused item looks gray
            theCollider.gameObject.renderer.material.color = Color.gray;
            theCollider.gameObject.tag = "FocusedItem";
            hasItemFocused = true;
        }
    }
    void OnTriggerExit(Collider theCollider)
    {
        if (   (theCollider.tag == "AttachedItem" && theCollider.transform.parent == null) || (theCollider.tag == "FocusedItem" && hasItemFocused)    )
        {
            hasItemFocused = false;
            //Free Object is white
            theCollider.gameObject.renderer.material.color = Color.white;
            theCollider.gameObject.tag = "Items";
            if (isReleasingObject)
            {
                gameObject.GetComponent<Collider>().isTrigger = false;
                isReleasingObject = false;
            }
        }
    }
    void OnTriggerStay(Collider theCollider)
    {

        if ((theCollider.tag == "Items" && !hasItemFocused) || (theCollider.tag == "AttachedItem" && theCollider.transform.parent == null))
        {
            //Foucused item looks gray
            theCollider.gameObject.renderer.material.color = Color.gray;
            theCollider.gameObject.tag = "FocusedItem";
            hasItemFocused = true;
        }
    }
}
