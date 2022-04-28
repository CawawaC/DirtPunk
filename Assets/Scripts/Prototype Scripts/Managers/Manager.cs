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

    //variables related to visual effects
    public GameObject mainRootModel;
    public GameObject hintTriggerModel;
    public Material glow;

    //UI elements
    public GameObject blackBox;
    private CanvasGroup blackBoxCG;

    //fungi spawning
    //public GameObject fungiPrefab;
    //private Vector3 fungiSpawnLoc;

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
    private List<Vector3> linePoints;
    private float drawTimer;
    private float drawTimerDelay;
    private GameObject newLine;
    private LineRenderer drawLine;
    private float lineWidth;

    //variables related to pollutant removal 
    public List<GameObject> pollutantList;
    private int initiallyVisiblePollutants;
    private int visiblePollutants;
    //set to 10 after testing
    private int pollutantsToRemove = 1;

    //variables related to sound puzzle
    public GameObject audioPuzzleObject;
    private AudioPuzzle audioPuzzle;
    private bool nodesGrown;

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
        //fungiSpawnLoc = new Vector3(-20,15,100);
        
        //initialize camera & UI variables
        mainCamera = Camera.main;
        camLoc = mainCamera.transform.position;
        blackBoxCG = blackBox.GetComponent<CanvasGroup>();
        blackBoxCG.alpha = 1;

        //initialize drawing variables
        linePoints = new List<Vector3>();
        drawTimer = 0.0f;
        drawTimer = drawTimerDelay;
        lineWidth = 0.5f;

        //initialize gameplay variables
        pollutantList = new List<GameObject>();
        initiallyVisiblePollutants = 0;
        nodesGrown = false;

        //call coroutine that fades scene in
        StartCoroutine(FadeIn());

        // Find the audio puzzle manager script
        audioPuzzle = audioPuzzleObject.GetComponent<AudioPuzzle>();
    }

    private void OnEnable()
    {   //hook up mouse actions with correct function
        leftClick.Enable();
        leftClick.performed += leftClicked;
        rightClick.Enable();
        rightClick.performed += rightClicked;
    }
    
    private void OnDisable()
    {
        //unhook functions from mouse actions
        leftClick.performed -= leftClicked;
        leftClick.Disable();
        rightClick.performed -= rightClicked;
        rightClick.Disable();

        //empty pollutant list just in case
        pollutantList.Clear();
    }

    // try to minimize code here as it is called once per frame
    void Update()
    {
        //spawn more fungi if m key is pressed
        //if (Input.GetKeyDown(KeyCode.M)) {Instantiate(fungiPrefab, fungiSpawnLoc, Quaternion.identity); }

        //drawing code
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

        //when player initially removes enough pollutants
        if (initiallyVisiblePollutants - visiblePollutants > pollutantsToRemove && !nodesGrown)
        {
            mainRootModel.GetComponent<GrowRoots>().Grow();
            nodesGrown = true;
            //also set shader of hint trigger from lit to glowing
            hintTriggerModel.GetComponent<Renderer>().material = glow;
        }
    }

    //function called at left click
    //in charge of dragging and dropping anything in draggable layer
    //and playing music at correct volume depending on pollutants around mother tree
    private void leftClicked(InputAction.CallbackContext context)
    {
        //check number of removed pollutants
        visiblePollutants =VisiblePollutantCounter();
        if (initiallyVisiblePollutants == 0) {
            initiallyVisiblePollutants = visiblePollutants;
        }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider != null && hit.collider.gameObject.layer == 6 && !Input.GetKey(KeyCode.P))
            {
                    StartCoroutine(DragUpdate(hit.collider.gameObject));
            }

            else if (hit.collider != null && hit.collider.gameObject.CompareTag("HintTrigger") && !Input.GetKey(KeyCode.P))
            {
                //check if player has removed enough pollutants to enable hint
                if (initiallyVisiblePollutants - visiblePollutants > pollutantsToRemove) {
                    audioPuzzle.PlayHint();
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

    //helper function for path drawing
    Vector3 GetMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 110;
    }

    //function that returns how many pollutants that are visible in the scene
    private int VisiblePollutantCounter() {
        int counter = 0;
        for (int i = 0; i < pollutantList.Count; i++)
        {
            if (pollutantList[i].transform.GetChild(0).GetComponent<Renderer>().isVisible)
            {
                counter += 1;
            }
        }
        return counter;
    }

    //coroutine that fades scene in
    private IEnumerator FadeIn()
    {
        while (blackBoxCG.alpha>=0)
        {
            yield return waitForFixedUpdate;
            blackBoxCG.alpha -= 0.3f*Time.deltaTime;
        }
    }
   
    //coroutine in charge of dragging and dropping called in LeftClicked function
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

    //coroutine in charge of dragging and dropping called in Rightclicked function
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
