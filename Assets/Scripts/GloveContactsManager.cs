using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GloveContactsManager{

    private Boolean index_finger_contact;
    private Boolean middle_finger_contact;
    private Boolean ring_finger_contact;
    private Boolean small_finger_contact;

    public delegate void ClickAction(GloveKeyCode keycode);//any subscriber must take one para (GloveKeyCode) and return void
    public static event ClickAction OnClicked;// static, no need to instantiate the class


    public Boolean Index_Finger_Contact
    {
        get
        {
            return index_finger_contact;
        }
        set
        {
            if (index_finger_contact != value)
            {
                index_finger_contact = value;
                if (OnClicked != null && value)
                {
                    OnClicked(GloveKeyCode.INDEX);
                }
            }
        }
    }


    public Boolean Middle_Finger_Contact
    {
        get
        {
            return middle_finger_contact;
        }
        set
        {
            if (middle_finger_contact != value)
            {
                middle_finger_contact = value;
                if (OnClicked != null && value)
                {
                    OnClicked(GloveKeyCode.MIDDLE);
                }
            }
        }
    }

    public Boolean Ring_Finger_Contact
    {
        get
        {
            return ring_finger_contact;
        }
        set
        {
            if (ring_finger_contact != value)
            {
                ring_finger_contact = value;
                if (OnClicked != null && value)
                {
                    OnClicked(GloveKeyCode.RING);
                }
            }
        }
    }

    public Boolean Small_Finger_Contact
    {
        get
        {
            return small_finger_contact;
        }
        set
        {
            if (small_finger_contact != value)
            {
                small_finger_contact = value;
                if (OnClicked != null && value)
                {
                    OnClicked(GloveKeyCode.SMALL);
                }
            }
        }
    }
    
    public GloveContactsManager() {
       
    }
    public void setContactsValue(int fingerContactsSum){

        /**fingerContactsSum
         * This value = 1* (Index finger contacted)+ 2* (Middle finger contacted)+
         * 4* (Ring finger contacted)+ 8* (Small finger contacted) The value range
         * from 0~15
         * */
        fingerContactsSum = (fingerContactsSum & 0xFF);
        Index_Finger_Contact = (fingerContactsSum & 0x01) != 0;
        Middle_Finger_Contact = (fingerContactsSum & 0x02) != 0;
        Ring_Finger_Contact = (fingerContactsSum & 0x04) != 0;
        Small_Finger_Contact = (fingerContactsSum & 0x08) != 0;
    }
}
