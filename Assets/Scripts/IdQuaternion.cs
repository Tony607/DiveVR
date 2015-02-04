using System;
using SimpleJSON;

public class IdQuaternion
{
    public int id;
    public float x;
    public float y;
    public float z;
    public float w;

    public IdQuaternion()
    {
        id = 0;
        x = 0; y = 0; z = 0;
        w = 1.0f;
    }
    public IdQuaternion(string quaternionJSON)
    {

        var parsedQuaternionJSON = JSONNode.Parse(quaternionJSON);
        id = int.Parse(parsedQuaternionJSON["id"]);
		float qx = float.Parse(parsedQuaternionJSON["x"]);
		float qy = float.Parse(parsedQuaternionJSON["y"]);
		float qz = float.Parse(parsedQuaternionJSON["z"]);
		float qw = float.Parse(parsedQuaternionJSON["w"]);
        //axis change
        //IMU X-> Unity Z
        //IMU Y-> Unity X
        //IMU Z-> Unity Y
        x = -qy;
        y= -qz;
        z = qx;
        w = qw;
    }
}

