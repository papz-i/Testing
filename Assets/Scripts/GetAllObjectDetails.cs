using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllObjectDetails : MonoBehaviour
{
    void Start()
    {
        GameObject[] objectsInScene = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objectsInScene)
        {
            Debug.Log("Name: " + obj.name + 
                " | Position: " + obj.transform.position + 
                " | Scale: " + obj.transform.localScale + 
                " | Rotation: " + obj.transform.rotation.eulerAngles);
        }
    }
}
