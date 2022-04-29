using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  



    List<GameObject> prefabList = new List<GameObject>();
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;
    public GameObject Prefab4;
    public float XspawndistanceA = 500.0f;
    public float XspawndistanceB = 500f;
    public float YspawndistanceA = 500;
    public float YspawndistanceB = 500;
    public float ZspawndistanceA = 500;
    public float ZspawndistanceB = 500;
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
            Vector3 position = new Vector3(Random.Range(XspawndistanceA, XspawndistanceB), Random.Range(YspawndistanceA, YspawndistanceB), Random.Range(ZspawndistanceA, ZspawndistanceB));
            Instantiate(prefabList[prefabIndex], position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));


        }

    }






}

