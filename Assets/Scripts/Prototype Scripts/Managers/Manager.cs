using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;

/*
This is the main manager for the game
This is where most game mechanics are implemented
 */

public class Manager : MonoBehaviour
{
    //singleton pattern
    public static Manager instance;

    //general variables
    private Camera mainCamera;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    //UI elements
    public GameObject blackBox;
    private CanvasGroup blackBoxCG;

    //fungi spawning
    public GameObject fungiPrefab;
    private Vector3 fungiSpawnLoc;

    //variables related to drag and drop with left mouse
    [SerializeField]
    private InputAction leftClick;
    private float mouseDragPhysicsSpeed = 10.0f;

    //variables related to zoom in and out with right mouse
    [SerializeField]
    private InputAction rightClick;
    private Vector3 camLoc;
    private Vector3 zoomLoc;
    private int zoomDir;
    private float zoomTimer;
    private float zoomTime = 2.0f;

    //variables related to path drawing
    List<Vector3> linePoints;
    float drawTimer;
    public float drawTimerDelay;
    GameObject newLine;
    LineRenderer drawLine;
    float lineWidth;

    private void Awake()
    {
        //make sure that there is only single instance of manager
        if (instance != null)
        {
            Debug.LogWarning("Found more than one manager");
        }
        instance = this;
    }

    void Start()
    {
        //initialize variables
        mainCamera = Camera.main;
        camLoc = mainCamera.transform.position;
        blackBoxCG = blackBox.GetComponent<CanvasGroup>();
        blackBoxCG.alpha = 1;
        fungiSpawnLoc = new Vector3(-20,15,100);

        //initialize drawing variables
        linePoints = new List<Vector3>();
        drawTimer = 0.0f;
        drawTimer = drawTimerDelay;
        lineWidth = 0.5f;

        //call coroutine that fades scene in
        StartCoroutine(FadeIn());
    }

    //hook up mouse actions with correct function
    private void OnEnable()
    {
        leftClick.Enable();
        leftClick.performed += leftClicked;
        rightClick.Enable();
        rightClick.performed += rightClicked;
    }
    private void OnDisable()
    {
        leftClick.performed -= leftClicked;
        leftClick.Disable();
        rightClick.performed -= rightClicked;
        rightClick.Disable();
    }

    // Try to minimize code here as it is called once per frame
    void Update()
    {
        //Spawn more fungi if m key is pressed
        //if (Input.GetKeyDown(KeyCode.M)) {Instantiate(fungiPrefab, fungiSpawnLoc, Quaternion.identity); }

        //Drawing code
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.P))
        {
            newLine = new GameObject();
            drawLine = newLine.AddComponent<LineRenderer>();
            drawLine.material = new Material(Shader.Find("Sprites/Default"));
            drawLine.startWidth = lineWidth;
            drawLine.endWidth = lineWidth;
        }

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.P))
        {
            drawTimer -= Time.deltaTime;
            if (drawTimer <= 0)
            {
                linePoints.Add(GetMousePosition());
                drawLine.positionCount = linePoints.Count;
                drawLine.SetPositions(linePoints.ToArray());
                drawTimer = drawTimerDelay;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            linePoints.Clear();
        }
    }

    //function called at left click in charge of dragging and dropping
    //It moves anything in "draggable" layer
    private void leftClicked(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider != null && hit.collider.gameObject.layer == 6 && !Input.GetKey(KeyCode.P))
            {
                if (hit.collider != null)
                {
                    StartCoroutine(DragUpdate(hit.collider.gameObject));
                }
            }
        }
    }

    //function called at right click in charge of zooming in and out
    private void rightClicked(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.layer == 3)
            {
                zoomDir = hit.collider.gameObject.GetComponent<ZoomLayerObj>().zoomDir;
                hit.collider.gameObject.GetComponent<ZoomLayerObj>().zoomDir *= -1;
                StartCoroutine(ZoomIn(hit.collider.gameObject));
            }
        }
    }

    //Helper function for path drawing
    Vector3 GetMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 110;
    }

    //Coroutine that fades scene in
    private IEnumerator FadeIn()
    {
        while (blackBoxCG.alpha>=0)
        {
            yield return waitForFixedUpdate;
            blackBoxCG.alpha -= 0.3f*Time.deltaTime;
        }
    }
   
    //Coroutine in charge of dragging and dropping called in LeftClicked function
    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        while (leftClick.ReadValue<float>() != 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (rb != null)
            {
                Vector2 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicsSpeed;
                yield return waitForFixedUpdate;
            }
        }
    }

    //Coroutine in charge of dragging and dropping called in Rightclicked function
    private IEnumerator ZoomIn(GameObject clickedObject)
    {
        zoomLoc = clickedObject.transform.position;
        zoomLoc.z -= 15;
        zoomTimer = 0.0f;
        while (zoomTimer < zoomTime)
        {
            yield return new WaitForEndOfFrame();
            zoomTimer += Time.deltaTime;
            if (zoomDir == 1) { mainCamera.transform.position = Vector3.Lerp(camLoc, zoomLoc, zoomTimer / zoomTime); }
            else { mainCamera.transform.position = Vector3.Lerp(zoomLoc, camLoc, zoomTimer / zoomTime); }
        }
    }
}
