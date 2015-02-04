using UnityEngine;
using System.Collections;
/**The Class manages the player rotation in Y axis, 
 * aligning it with the PlayerHead GameObject*/
public class PlayerRotation : MonoBehaviour
{
    void Update()
    {
        Quaternion rot = Cardboard.SDK.HeadRotation;
        Quaternion yRot = new Quaternion(0, rot.y, 0, rot.w);
        transform.rotation = yRot;
        //transform.rotation.Set(0, multi * rot.y, 0, multi* rot.w);
    }
}
