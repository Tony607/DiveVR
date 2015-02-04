using UnityEngine;
using System.Collections;
using SimpleJSON;
/**The class manages the glove object, right now there is only right hand glove*/
public class GloveManager : MonoBehaviour
{

    private static GloveNode gloveNode;
    public GameObject handObject;
    private static GloveContactsManager gcManager;
    // Use this for initialization
    void Start()
    {

        gloveNode = new GloveNode(handObject);
        gloveNode.updateObjRoation();
        gcManager = new GloveContactsManager();
    }
    // Update is called once per frame
    void Update()
    {
        gloveNode.updateObjRoation();
    }

    public static void setGloveData(string gloveJSON)
    {
        var parsedGloveJSON = JSONNode.Parse(gloveJSON);
        int flexSensorValue = int.Parse(parsedGloveJSON["a"]);
        int fingerContactsSum = int.Parse(parsedGloveJSON["b"]);
        gloveNode.setValue(flexSensorValue);
        gcManager.setContactsValue(fingerContactsSum);
    }

}
