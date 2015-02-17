using UnityEngine;
using System.Collections;
using System;

public class StepControl : MonoBehaviour
{
    public CharacterController controller;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    private float moveSpeed = 10.0f;
    private static float stepDistance = 2.0f;
    private static float chaseDistance = 0;
    //JoyStick control
    /**a vector 2 varialbe to store the joy stick data*/
    private Vector2 joyVector = Vector2.zero;
    /**the actual moving vector of the character controller aligned to its forward direction*/
    private Vector3 moveVector = Vector3.zero;
    private float moveSpeedJoy = 2f;
    public float jumpSpeed = 12.0f;
    private bool pendingJump = false;
    //private bool grounded = false;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //comment out the step detector step, use joy stick to move
        //chaseStep ();
        if (Time.timeScale == 0) return;//return if paused
        joyMove();
    }
    void OnEnable()
    {
        WiiManager.JoyChanged += joyChangedCallBack;
        WiiManager.WiiButtonChanged += wiiButtonChangedCallBack;
    }
    void OnDisable()
    {
        WiiManager.JoyChanged -= joyChangedCallBack;
        WiiManager.WiiButtonChanged -= wiiButtonChangedCallBack;
    }
    void joyChangedCallBack(WiiAxisEnum joyAxis, int joyValue)
    {
        if (joyAxis == WiiAxisEnum.AXIS_X)
        {
            joyVector.x = joyValue;
        }
        else if (joyAxis == WiiAxisEnum.AXIS_Y)
        {
            joyVector.y = joyValue;
        }
    }
    void wiiButtonChangedCallBack(WiiButtonCodeEnum code, Boolean buttonValue)
    {
        if (code == WiiButtonCodeEnum.BUTTON_Z)
        {
            //if (buttonValue && grounded)
            if (controller.isGrounded)
            { //push--> jump
                //moveVector.y = jumpSpeed;
                pendingJump = true;
            }
        }
    }
    public static void stepDetected()
    {
        Debug.Log("stepDetected");
        chaseDistance += stepDistance;
        //Target.position.
    }
    /**Function called in the update function to move the Character controller using joy stick data*/
    void joyMove()
    {
        if (controller.isGrounded)
        {
            moveVector = new Vector3(joyVector.x, 0, joyVector.y);
            moveVector = transform.TransformDirection(moveVector);
            moveDirection *= moveSpeedJoy;
            if (pendingJump)
            {
                moveVector.y = jumpSpeed;
                pendingJump = false;
            }
        }
        moveVector.y -= gravity * Time.deltaTime;
        //since the controller.Move call is computational expensive, we only call it
        //when needed(either, the joystick is non zero/ is jumping/ not on the ground)
        if (joyVector != new Vector2(0, 0) || moveVector.y > 0 || !controller.isGrounded)
        {
            controller.Move(moveVector * Time.deltaTime);
        }
    }
    /**Function called in the update function to move the Character controller using step detector data*/
    void chaseStep()
    {
        if (chaseDistance > 0)
        {
            chaseDistance -= moveSpeed * Time.deltaTime;
            if (chaseDistance <= 0)
            {
                chaseDistance = 0;
            }
            moveDirection = transform.forward;
            moveDirection *= moveSpeed;

            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
