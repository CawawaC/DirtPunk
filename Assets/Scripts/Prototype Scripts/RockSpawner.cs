using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    List<GameObject> prefabList = new List<GameObject>();
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    private int spawnNumber = 50;
    
    //variables related to random pos generation
    Vector3 position;
    float x;
    float y;
    float z;
    void Start()
    {
        prefabList.Add(prefab1);
        prefabList.Add(prefab2);
        prefabList.Add(prefab3);
        prefabList.Add(prefab4);

        for (int i = 0; i < spawnNumber; i++)
        {
            int prefabIndex = UnityEngine.Random.Range(0, 4);
            //randomly generate a position within camera projection
            x = Random.Range(0.05f, 0.95f);
            y = Random.Range(0.05f, 0.95f);
            z = Random.Range(15, 100);
            Vector3 position = new Vector3(x, y, z);
            position = Camera.main.ViewportToWorldPoint(position);
            Instantiate(prefabList[prefabIndex], position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        }
    }
}