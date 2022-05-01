using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    Vector3 position;
    float x;
    float y;
    private float foodZPos = 40;
    void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy)
        {
            //move back onto screen

            //randomly generate a position within camera projection
            x = Random.Range(0.05f, 0.95f);
            y = Random.Range(0.05f, 0.95f);
            position = new Vector3(x, y, foodZPos);
            position = Camera.main.ViewportToWorldPoint(position);
            gameObject.transform.position = position;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
