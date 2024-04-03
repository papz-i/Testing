using UnityEngine;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.Animations;
using Unity.VisualScripting;

public class RotateObject: MonoBehaviour
{
    public class PrefabRotate
    {
        public string action { get; set; }
        public string prefab  { get; set; }
        public string axis { get; set; }
        public string object_to_replace { get; set; }
        public float value { get; set; }
        public string direction { get; set; }
    }

    IEnumerator GetRequestRotate(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(String.Format("Something went wrong: {0}", webRequest.error));
                    break;
                case UnityWebRequest.Result.Success:
                    PrefabRotate prefab = JsonConvert.DeserializeObject<PrefabRotate>(webRequest.downloadHandler.text);
                   
                    if (prefab.action == "rotate")
                    {
                        GameObject objectPrefab = GameObject.Find(prefab.prefab);
                        switch (prefab.axis.ToLower())
                        {
                            case "x":
                                objectPrefab.transform.Rotate(prefab.value, 0.0f, 0.0f, Space.World); 
                                break;
                            case "y":
                                objectPrefab.transform.Rotate(0.0f, prefab.value, 0.0f, Space.World);
                                break;
                            case "z":
                                objectPrefab.transform.Rotate(0.0f, 0.0f, prefab.value, Space.World);
                                break;
                            default:
                                objectPrefab.transform.Rotate(prefab.value, 0.0f, 0.0f, Space.World);
                                break;
                        }
                    }else if (prefab.action == "scale")
                    {
                        GameObject objectPrefab = GameObject.Find(prefab.prefab);
                        switch (prefab.axis.ToLower())
                        {
                            case "x":
                                objectPrefab.transform.localScale += new Vector3(prefab.value, 0, 0);
                                break;
                            case "y":
                                objectPrefab.transform.localScale += new Vector3(0, prefab.value, 0);
                                break;
                            case "z":
                                objectPrefab.transform.localScale += new Vector3(0, 0, prefab.value);
                                break;
                            case "multiply":
                                objectPrefab.transform.localScale += new Vector3(prefab.value, prefab.value, prefab.value);
                                break;
                            case "increase":
                                objectPrefab.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                                break;
                            case "decrease":
                                objectPrefab.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                                break;
                            default:
                                objectPrefab.transform.localScale = new Vector3(1, 1, 1);
                                break;
                        }
                    }else if(prefab.action == "remove")
                    {
                        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(prefab.prefab);
                        if(objectsToDestroy.Length > 0)
                        {
                            if(prefab.value > objectsToDestroy.Length)
                            {
                                for (int i = 0; i < objectsToDestroy.Length; i++)
                                {
                                    Destroy(objectsToDestroy[i]);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < prefab.value; i++)
                                {
                                    Destroy(objectsToDestroy[i]);
                                }
                            }
                        }
                    }else if (prefab.action == "replace")
                    {
                        GameObject[] objectsInScene = UnityEngine.Object.FindObjectsOfType<GameObject>();

                        bool objectFound = false;
                        GameObject objectPrefab = null;

                        foreach (GameObject obj in objectsInScene)
                        {
                            if (obj.name == prefab.object_to_replace)
                            {
                                objectFound = true;
                                objectPrefab = obj;
                                break;
                            }
                        }

                        if (objectFound)
                        {
                            Vector3 position = objectPrefab.transform.position;
                            Destroy(objectPrefab); 
                            GameObject loadPrefab = Resources.Load<GameObject>(prefab.prefab);
                            GameObject prefabInstance = Instantiate(loadPrefab, position, Quaternion.identity);
                        }
                        else
                        {
                            Debug.LogError("Object to replace is not found in the scene.");
                        }
                    }else if(prefab.action == "move")
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
                            
                            switch (prefab.direction)
                            {
                                case "left":
                                    objectPrefab.transform.Translate(Vector3.left * prefab.value);
                                    break;
                                case "right":
                                    objectPrefab.transform.Translate(Vector3.right * prefab.value);
                                    break;
                                case "top":
                                    objectPrefab.transform.Translate(Vector3.up * prefab.value);
                                    break;
                                case "bottom":
                                    objectPrefab.transform.Translate(Vector3.down * prefab.value);
                                    break;
                                case "back":
                                    objectPrefab.transform.Translate(Vector3.back * prefab.value);
                                    break;
                                case "front":
                                    objectPrefab.transform.Translate(Vector3.forward * prefab.value);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Debug.LogError("Renderer component not found on the prefab.");
                        }
                    }
                    Debug.Log("Success | Object Name: " + prefab.prefab);
                break;
                   
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetRequestRotate("http://127.0.0.1:5000/rotate"));
        }
    }
}
