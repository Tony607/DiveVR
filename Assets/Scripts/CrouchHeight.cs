using UnityEngine;
using System.Collections;
using System;

public class CrouchHeight : MonoBehaviour
{
    /**This varible will be multiplied to the Standing Height when crouched 0~1*/
    public float CrouchHeightMultiply = 0.4f;
    public float smoothing = 7f;
    private CharacterController characterController;
    private Transform theTransform;
    private float orginalCharacterHeight;
    private float characterHeight;

    public float CharacterHeight
    {
        get { return characterHeight; }
        set
        {
            characterHeight = value;
            StopCoroutine("Movement");
            StartCoroutine("Movement", characterHeight);
        }
    }

    void OnEnable()
    {
        WiiManager.WiiButtonChanged += wiiButtonChangedCallBack;
    }
    void OnDisable()
    {
        WiiManager.WiiButtonChanged -= wiiButtonChangedCallBack;
    }
    // Use this for initialization
    void Start()
    {
        theTransform = transform;
        characterController = gameObject.GetComponent<CharacterController>();
        characterHeight = characterController.height;
        orginalCharacterHeight = characterController.height;
    }
    void wiiButtonChangedCallBack(WiiButtonCodeEnum code, Boolean buttonValue)
    {
        if (code == WiiButtonCodeEnum.BUTTON_C)
        {
            if (buttonValue)
            { //push--> crouch
                CharacterHeight = CrouchHeightMultiply * orginalCharacterHeight;
            }
            else
            { //release --> stand up
                CharacterHeight = orginalCharacterHeight;

            }
        }
    }

    IEnumerator Movement(float target)
    {
        while (Math.Abs(characterController.height - target) > 0.05f)
        {
            float lastHeight = characterController.height; //Stand up/crouch smoothly
            characterController.height = Mathf.Lerp(characterController.height, target, smoothing * Time.deltaTime);
            //transform
            theTransform.position = new Vector3(theTransform.position.x, theTransform.position.y + (characterController.height - lastHeight) / 2, theTransform.position.z);
            yield return null;
        }
    }

}
