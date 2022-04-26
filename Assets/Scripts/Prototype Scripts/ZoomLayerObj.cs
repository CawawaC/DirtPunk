using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This is a script you should attach to any object in the zoomable layer
 */
public class ZoomLayerObj : MonoBehaviour
{
    public int zoomDir;
    // Start is called before the first frame update
    void Start()
    {
        zoomDir = 1;
    }
}
