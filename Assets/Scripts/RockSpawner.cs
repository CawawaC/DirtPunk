using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{



    List<GameObject> prefabList = new List<GameObject>();
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;
    public float spawndistance = 500.0f;
    public float SpawnNumber = 2500;


    void Start()
    {



        prefabList.Add(Prefab1);
        prefabList.Add(Prefab2);
        prefabList.Add(Prefab3);
        prefabList.Add(Prefab4);

        for (int i = 0; i < SpawnNumber; i++)
        {

            int prefabIndex = UnityEngine.Random.Range(0, 4);
            Vector3 position = new Vector3(Random.Range(-spawndistance, spawndistance), Random.Range(-spawndistance, spawndistance), Random.Range(-spawndistance, spawndistance));
            Instantiate(prefabList[prefabIndex], position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));


        }

    }






}