using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndentifyCoordinates : MonoBehaviour
{
   
    void Start()
    {

        GameObject objectPrefab = GameObject.Find("Cube");

        Renderer rend = objectPrefab.GetComponent<Renderer>();

        if (rend != null)
        {
            Bounds bounds = rend.bounds;

            float left = bounds.min.x;
            float right = bounds.max.x;
            float top = bounds.max.y;
            float bottom = bounds.min.y;

            Debug.Log("Left: " + left + ", Right: " + right + ", Top: " + top + ", Bottom: " + bottom);
            objectPrefab.transform.Translate(Vector3.right * 10f);
        }
        else
        {
            Debug.LogError("Renderer component not found on the prefab.");
        }

        
    }
}
