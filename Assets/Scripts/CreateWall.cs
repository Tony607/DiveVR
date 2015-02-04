using UnityEngine;
using System.Collections;

public class CreateWall : MonoBehaviour
{

    public GameObject prefab;
    public float gridX = 2f;
    public float gridY = 2f;
    public float spacing = 2f;

    void Start()
    {
        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 pos = new Vector3(x, 0, y) * spacing;
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }
}