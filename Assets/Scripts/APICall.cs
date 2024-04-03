using UnityEngine;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.UIElements;

public class GetMethod : MonoBehaviour
{
    public class Prefab
    {
        public string prefab { get; set; }
        public int xScale { get; set; }
        public int yScale { get; set; }
        public int zScale { get; set; }
        public int xRotation { get; set; }
        public int yRotation { get; set; }
        public int zRotation { get; set; }
    }

    IEnumerator GetRequest(string uri)
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
                    Prefab prefab = JsonConvert.DeserializeObject<Prefab>(webRequest.downloadHandler.text);
                    
                    Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-16, 16), 1, UnityEngine.Random.Range(-16, 16));
                    Vector3 prefabScale = new Vector3(prefab.xScale, prefab.zScale, prefab.yScale);
                    Quaternion rotation = Quaternion.Euler(prefab.xRotation, prefab.zRotation, prefab.yRotation);

                    GameObject objectPrefab = Resources.Load<GameObject>(prefab.prefab);
                    GameObject objectInstance = Instantiate(objectPrefab, spawnPosition, rotation);
                    objectInstance.transform.localScale = prefabScale;

                    Debug.Log("Success | Object Name: " + prefab.prefab + " | Spawn Position: " + spawnPosition + " | Scale: " + prefabScale + " | Rotation: " + rotation);
                    break;
            }
        }
    }

    void Update()
    {
        // Check for space bar input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call GetRequest when space bar is pressed
            StartCoroutine(GetRequest("http://localhost:3000"));
        }
    }
}
