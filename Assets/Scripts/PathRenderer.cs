using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timerDelay;

    GameObject newLine;
    LineRenderer drawLine;
    float lineWidth;
    // Start is called before the first frame update
    void Start()
    {
        lineWidth = 0.5f;
        linePoints = new List<Vector3>();
        timer = timerDelay;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.P))
        {
            newLine = new GameObject();
            drawLine = newLine.AddComponent<LineRenderer>();
            drawLine.material = new Material(Shader.Find("Sprites/Default"));
            drawLine.startWidth = lineWidth;
            drawLine.endWidth = lineWidth;
        }

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.P)) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                linePoints.Add(GetMousePosition());
                drawLine.positionCount = linePoints.Count;
                drawLine.SetPositions(linePoints.ToArray());
                timer = timerDelay;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            linePoints.Clear();
        }
    }

    Vector3 GetMousePosition() {      
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 110;
    }
}
