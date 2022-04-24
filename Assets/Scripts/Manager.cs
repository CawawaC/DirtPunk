using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject fungiPrefab;
    private Vector3 fungiSpawnLoc;
    // Start is called before the first frame update
    void Start()
    {
        fungiSpawnLoc = new Vector3(-20,15,100);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {Instantiate(fungiPrefab, fungiSpawnLoc, Quaternion.identity); }
        
    }
}
