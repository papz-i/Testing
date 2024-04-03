using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-16, 16), 5, UnityEngine.Random.Range(-16, 16));
            Debug.Log(spawnPosition);
            GameObject spherePrefab = Resources.Load<GameObject>("Sphere");
            Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
        }
    }
}