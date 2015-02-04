using UnityEngine;
using System.Collections;

public class ColorControl : MonoBehaviour
{
    private bool toggleLight = true;
    // Use this for initialization
    void Start()
    {

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
        Debug.Log("GC keycode" + keycode);
        if (keycode == GloveKeyCode.MIDDLE)
        {
            if (toggleLight)
            {
                light.color = Color.cyan;
            }
            else
            {
                light.color = Color.yellow;
            }
            toggleLight = !toggleLight;
        }
    }
}
