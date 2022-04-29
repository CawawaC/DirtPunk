
using System;
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
    public GameObject spawner;

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
    
    private int noiseIndex;
    private string noiseChoices;

    //variables(bools) related to game state
    private bool nodesGrown;
    private bool puzzleSolved;
    private bool fungiFed;

    //variables related to post-puzzle microbe action
    //appearing microbe list
    //going to have to add these manually to list as the objects start off disabled
    public List<GameObject> amList;
    //growing microbe list
    private GameObject[] gmList;
    //growing fungi list
    private GameObject[] gfList;
    //fungi food list
    public List<GameObject> ffList;
    //eaten food list
    public List<GameObject> efList;

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
        noiseIndex = -1;
        //Debugging
        //noiseChoices = "12345";
        noiseChoices = "";
        gmList = GameObject.FindGameObjectsWithTag("GrowingMicrobe");
        gfList = GameObject.FindGameObjectsWithTag("GrowingFungi");
        ffList = new List<GameObject>();
        efList = new List<GameObject>();

        //initialize game state bools
        nodesGrown = false;
        puzzleSolved = false;
        fungiFed = false;

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

        //empty generated prefab list just in case
        pollutantList.Clear();
        ffList.Clear();
        efList.Clear();
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
            mainRootModel.GetComponent<GrowRoots>().Grow(1);
            nodesGrown = true;
            //also set shader of hint trigger from lit to glowing
            hintTriggerModel.GetComponent<Renderer>().material = glow;
        }
        //if player initially solves puzzle
        if (noiseChoices.Contains("12345") && !puzzleSolved)
        {
            puzzleSolved = true;
            //enable game items with tag "AppearingMicrobe"
            foreach (GameObject am in amList)
            {
                am.SetActive(true);
            }
            //grow game items with tag "GrowingMicrobe"
            foreach (GameObject gm in gmList)
            {
                gm.GetComponent<GrowRoots>().Grow(1);
            }
            //grow baby fungi a little
            foreach (GameObject gf in gfList)
            {
                gf.GetComponent<GrowRoots>().Grow(0.5f);
            }
            //spawn fungi food in random locations
            spawner.GetComponent<RockSpawner>().SpawnFood();
        }

        //puzzle is solved but fungi have not been fed
        if (!fungiFed && puzzleSolved)
        {
            //check for collisions between food and fungi
            for (int i = 0; i < gfList.Length; i++) {
                for (int j = 0; j < ffList.Count; j++) {
                    if (CollisionDetection(gfList[i], ffList[j])) {
                        
                        if (!efList.Contains(ffList[j])) {
                            efList.Add(ffList[j]);
                            ffList[j].SetActive(false);
                        }
                        //and set fungi fed to true if all the food has more or less been eaten
                        if (ffList.Count-efList.Count<3) { fungiFed = true; }
                    }
                }
            }
        }

        //if fungi have been fed then trigger the rest of their growth
        if (fungiFed) {
            //grow fungi fully
            foreach (GameObject gf in gfList)
            {
                gf.GetComponent<GrowRoots>().Grow(1);
            }
        }
 
    }

    //function called at left click
    //in charge of dragging and dropping anything in draggable layer
    //and playing music hint & selecting puzzle noises
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

            else if (hit.collider != null && hit.collider.gameObject.CompareTag("HintTrigger") && !Input.GetKey(KeyCode.P) && !puzzleSolved)
            {
                //check if player has removed enough pollutants to enable hint
                if (initiallyVisiblePollutants - visiblePollutants > pollutantsToRemove ) {
                    audioPuzzle.PlayHint();
                }
            }

            else if (hit.collider != null && hit.collider.gameObject.CompareTag("PuzzleNoise") && !Input.GetKey(KeyCode.P) && nodesGrown && !puzzleSolved)
            {
                int.TryParse(hit.collider.gameObject.name.Substring(7,1),out noiseIndex);
                if (noiseIndex > -1) {
                    noiseChoices=String.Concat(noiseChoices,noiseIndex);
                    Debug.Log(noiseChoices);
                    audioPuzzle.PlayElement(noiseIndex-1); 
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


    //Function that detects "collision" between two game objects
    //Not actual collision, just if  it appears as if they are colliding to the player
    private bool CollisionDetection(GameObject a, GameObject b) {
        Vector3 aScreenPos = mainCamera.WorldToScreenPoint(a.transform.position);
        Vector3 bScreenPos = mainCamera.WorldToScreenPoint(b.transform.position);
        if (Math.Abs(aScreenPos.x - bScreenPos.x) < 50 && Math.Abs(aScreenPos.y - bScreenPos.y) < 50) {
            return true;
        }
        return false;
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
