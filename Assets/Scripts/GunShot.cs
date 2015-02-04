using UnityEngine;
using System.Collections;

public class GunShot : MonoBehaviour
{

    // Use this for initialization
    public GameObject Effect;
    public int ShottingRange =  50;
    public int hitForce = 10;
    Transform pistolSlideTransform;
    void Start()
    {
        pistolSlideTransform = transform.Find("slide");
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnEnable()
    {
        GloveContactsManager.OnClicked += fingerClickCallBack;
    }
    void OnDisable()
    {
        GloveContactsManager.OnClicked -= fingerClickCallBack;
    }
    void fingerClickCallBack(GloveKeyCode keycode)
    {
        if (keycode == GloveKeyCode.INDEX)
        {
            if (gameObject.tag == "AttachedItem")
            {
                ShotTheGun();
            }
        }
    }
    void ShotTheGun() {
        RaycastHit hit;
        Vector3 fwd = pistolSlideTransform.TransformDirection(Vector3.left);
        Ray ray = new Ray(pistolSlideTransform.position, fwd);
        animation.Play("USP45Recoil");
        if (Physics.Raycast(ray, out hit, ShottingRange))
        {
            Debug.Log("Hit distance=" + hit.distance);
            Object particleClone = Instantiate(Effect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(particleClone,2);
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(ray.direction * hitForce, ForceMode.Impulse);
            }
        }
    }
}
