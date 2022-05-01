using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Only put code related to prefab generation in this script.
Do not put any code related to game mechanics here- put that in manager script.
This script initially spawns pollutants and then later spawns food when puzzle is solved;
 */
public class RockSpawner : MonoBehaviour
{
    public GameObject manager;

    //pollution
    private List<GameObject> prefabList = new List<GameObject>();
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public static int spawnNumber = 35;

    //food
    public GameObject foodPrefab;
    public static int foodSpawnNumber = 10;
    private float foodZPos = 40;

    //variables related to random pos generation
    Vector3 position;
    float x;
    float y;
    float z;
    void Start()
    {
        //Add prefabs to prefab list
        prefabList.Add(prefab1);
        prefabList.Add(prefab2);
        prefabList.Add(prefab3);
        prefabList.Add(prefab4);

        //Spawn pollutants
        for (int i = 0; i < spawnNumber; i++)
        {
            int prefabIndex = UnityEngine.Random.Range(0, 4);
            //randomly generate a position within camera projection
            x = Random.Range(0.05f, 0.95f);
            y = Random.Range(0.05f, 0.95f);
            z = Random.Range(15, 100);
            position = new Vector3(x, y, z);
            position = Camera.main.ViewportToWorldPoint(position);
            //spawn prefabs and add them to list of pollutants in manager
            manager.GetComponent<Manager>().pollutantList.Add(Instantiate(prefabList[prefabIndex], position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
        }
    }

    //Spawn food and add to manager's ff list when asked by manager
    public void SpawnFood(){
        for (int i = 0; i < foodSpawnNumber; i++)
        {
            //randomly generate a position within camera projection
            x = Random.Range(0.05f, 0.95f);
            y = Random.Range(0.05f, 0.95f);
            //z = Random.Range(15, 100);
            position = new Vector3(x, y, foodZPos);
            position = Camera.main.ViewportToWorldPoint(position);
            //spawn prefabs and add them to list of pollutants in manager
            manager.GetComponent<Manager>().ffList.Add(Instantiate(foodPrefab, position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
        }
    }
}