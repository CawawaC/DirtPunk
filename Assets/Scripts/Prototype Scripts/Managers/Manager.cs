
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;
using Ink.Runtime;
using TMPro;

/*
This is the main manager for the game
This is where most game mechanics are implemented
 */

public class Manager : MonoBehaviour {
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
    public GameObject DL1;
    public GameObject DL2;
    public GameObject completionEffects;
    public GameObject globalVol;
    public GameObject[] nodeGrowingEffect;
    public GameObject[] nodeClickingEffect;
    private IEnumerator firstCR;
    private bool fadedOut;

    //UI elements
    public GameObject blackBox;
    private CanvasGroup blackBoxCG;

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
    public List<Vector3> linePoints;
    private float drawTimer;
    private float drawTimerDelay;
    private GameObject newLine;
    private LineRenderer drawLine;
    private float lineWidth;

    //variables related to pollutant removal 
    public List<GameObject> pollutantList;
    private int initiallyVisiblePollutants;
    private int visiblePollutants;
    private int pollutantsToRemove = 12;
    private int pollutantsRemoved = 0;

    //variables related to sound puzzle
    public GameObject audioPuzzleObject;
    private AudioPuzzle audioPuzzle;
    private MusicManager musicManager;
    private int noiseIndex;
    private string noiseChoices;
    private bool hintPlayed;
    //variables(bools) related to game state
    private bool nodesGrown;
    private bool puzzleSolved;
    private bool fungiDialogueLoaded;
    private bool fungiFed;
    private bool connectionDialogueLoaded;
    private bool plantConnected;
    private bool completeDialogueLoaded;

    //variables related to post-puzzle microbe action
    //empty object containing microbes that will appear
    public GameObject amContainer;
    //growing microbe list
    private GameObject[] gmList;
    //growing fungi list
    private GameObject[] gfList;
    //fungi food list
    public List<GameObject> ffList;
    //eaten food list
    public List<GameObject> efList;

    //variables related to dialogue UI
    public GameObject dialogueBox;
    public static bool dialogueOpen;
    public GameObject responseButton;
    public TextMeshProUGUI speakerDialogue;

    //Ink dialogue variables
    private Story currentStory;
    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;
    [SerializeField] private TextMeshProUGUI responseText;
    private string convoName;

    private void Awake() {
        //make sure that there is only single instance of manager
        if (instance != null) {
            Debug.LogWarning("Found more than one manager");
        }
        instance = this;

        //GameObject.FindGameObjectsWithTag("MusicManager").ForEach((c) => Debug.Log(c));
        // Destroy old music
        Destroy(GameObject.Find("Intro Music"));
    }

    void Start() {
        //initialize camera & visual & UI variables
        mainCamera = Camera.main;
        camLoc = mainCamera.transform.position;
        blackBoxCG = blackBox.GetComponent<CanvasGroup>();
        blackBoxCG.alpha = 1;
        DL1.SetActive(true);
        DL2.SetActive(false);
        globalVol.SetActive(true);
        completionEffects.SetActive(false);
        foreach (GameObject e in nodeClickingEffect) {
            e.SetActive(false);
        }
        foreach (GameObject e in nodeGrowingEffect)
        {
            e.SetActive(false);
        }
        fadedOut = false;

        //initialize drawing variables
        linePoints = new List<Vector3>();
        drawTimer = 0.0f;
        drawTimer = drawTimerDelay;
        lineWidth = 0.5f;

        //initialize gameplay variables
        pollutantList = new List<GameObject>();
        initiallyVisiblePollutants = 0;
        noiseIndex = -1;
        noiseChoices = "";
        amContainer.SetActive(false);
        gmList = GameObject.FindGameObjectsWithTag("GrowingMicrobe");
        gfList = GameObject.FindGameObjectsWithTag("GrowingFungi");
        ffList = new List<GameObject>();
        efList = new List<GameObject>();

        //initialize game state bools
        //you can change these to true to debug specific parts of game
        //but should always be set to false before pushes
        hintPlayed = false;
        nodesGrown = false;
        puzzleSolved = false;
        fungiFed = false;
        fungiDialogueLoaded = false;
        connectionDialogueLoaded = false;
        plantConnected = false;
        completeDialogueLoaded = false;


        //initialize dialogue variables
        dialogueOpen = true;
        convoName = "PollutionInstructions";
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(convoName);
        ContinueStory();

        //call coroutine that fades scene in
        firstCR = FadeIn();
        StartCoroutine(firstCR);
        // Find the audio puzzle manager script
        audioPuzzle = audioPuzzleObject.GetComponent<AudioPuzzle>();


        GameObject musicManagerObject = GameObject.Find("Music (main scene)");
        if (musicManagerObject != null) {
            musicManager = musicManagerObject.GetComponent<MusicManager>();
        }
    }

    private void OnEnable() {   //hook up mouse actions with correct function
        leftClick.Enable();
        leftClick.performed += leftClicked;
        rightClick.Enable();
        rightClick.performed += rightClicked;
    }

    private void OnDisable() {
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
    void Update() {
        //open dialogue box depending on bool
        dialogueBox.SetActive(dialogueOpen);

        //update pollutant stats
        if (!nodesGrown) {
        visiblePollutants = VisiblePollutantCounter();
        if (initiallyVisiblePollutants == 0)
        {
            initiallyVisiblePollutants = visiblePollutants;
        }
        pollutantsRemoved = initiallyVisiblePollutants - visiblePollutants;
        Debug.Log("initially visible:"+initiallyVisiblePollutants+" visible now:"+visiblePollutants +" removed:"+ pollutantsRemoved);
         }
        //update dialogue with each pollutant removal
        if (pollutantsRemoved < pollutantsToRemove) {
            if (pollutantsRemoved>10) {
               LoadNewStory("PollutionRemoval6"); 
            }
            else if (pollutantsRemoved > 9)
            {
                LoadNewStory("PollutionRemoval6");
            }
            else if (pollutantsRemoved > 7)
            {
                LoadNewStory("PollutionRemoval4");
            }
            else if (pollutantsRemoved > 5)
            {
                LoadNewStory("PollutionRemoval3");
            }
            else if (pollutantsRemoved > 3)
            {
                LoadNewStory("PollutionRemoval2");
            }
            else if (pollutantsRemoved > 1)
            {
                LoadNewStory("PollutionRemoval1");
            }
        }

        //when player initially removes enough pollutants
        if (pollutantsRemoved >= pollutantsToRemove && !nodesGrown) {
            dialogueOpen = false;
            //wait until nodes start forming to open dialogue related to node growth
            StartCoroutine(LoadAfterSec("PlantNodulesGrow", 8));
            //later use this to make sure player can't click on nodes until fully formed
            mainRootModel.GetComponent<GrowRoots>().Grow(1);
            nodesGrown = true;
            //also set shader of hint trigger from lit to glowing
            hintTriggerModel.GetComponent<Renderer>().material = glow;

            // Trigger growth sound
            GameObject.Find("nodules growth sound").GetComponent<AudioSource>().Play();
            
            // trigger audio puzzle music part
            if (musicManager != null) {
                musicManager.PlayPart("puzzle");
            }
        }

        //if player initially solves puzzle
        if (noiseChoices.Contains("12345") && !puzzleSolved) {
            puzzleSolved = true;
            //load correct dialogue
            LoadNewStory("MicrobesSpawn"); 
            //make appearing microbes visible & adjust lighting
            amContainer.SetActive(true);
            DL1.SetActive(false);
            DL2.SetActive(true);

            //grow game items with tag "GrowingMicrobe"
            foreach (GameObject gm in gmList) {
                gm.GetComponent<GrowRoots>().Grow(1);
            }
            //grow baby fungi a little
            foreach (GameObject gf in gfList) {
                gf.GetComponent<GrowRoots>().Grow(0.5f);
            }
            //spawn fungi food in random locations
            spawner.GetComponent<RockSpawner>().SpawnFood();

            if (musicManager != null) {
                musicManager.PlayPart("growth");
            }
        }

        //puzzle is solved but fungi have not been fed
        if (!fungiFed && puzzleSolved) {
            //load correct dialogue
            if (!fungiDialogueLoaded) { 
                LoadNewStory("MicrobesSpawn");
                fungiDialogueLoaded = true;
            }
            //check for collisions between food and fungi
            for (int i = 0; i < gfList.Length; i++) {
                for (int j = 0; j < ffList.Count; j++) {
                    if (CollisionDetection(gfList[i], ffList[j])) {

                        if (!efList.Contains(ffList[j])) {
                            efList.Add(ffList[j]);
                            ffList[j].SetActive(false);
                            
                        }

                        AudioSource[] feedSounds = GameObject.Find("fungi feed").GetComponents<AudioSource>();
                        feedSounds[(int)UnityEngine.Random.Range(0, feedSounds.Length)].Play();

                        //series of dialogue at fungi being fed
                        if (efList.Count == 3 && fungiDialogueLoaded) { LoadNewStory("FungiFed1"); }
                        if (efList.Count == 4) { LoadNewStory("FungiFed2"); }
                        //and set fungi fed to true if all the food has more or less been eaten
                        if (ffList.Count - efList.Count < 3) { 
                            fungiFed = true;

                            // Triggerfungi growth sound
                            GameObject.Find("fungi growth sound").GetComponent<AudioSource>().Play();

                            LoadNewStory("FungiFed3");
                        }
                    }
                }
            }
        }

        //if fungi have been fed then trigger the rest of their growth
        //and enable line drawing as an interaction
        if (fungiFed && !plantConnected) {
            //load correct dialogue
            if (!connectionDialogueLoaded)
            {
                LoadNewStory("ConnectAllThings");
                connectionDialogueLoaded = true;
            }

            //grow fungi fully
            foreach (GameObject gf in gfList) {
                gf.GetComponent<GrowRoots>().Grow(1);
            }

            //drawing interaction
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.P)) {
                newLine = new GameObject();
                drawLine = newLine.AddComponent<LineRenderer>();
                drawLine.material = new Material(Shader.Find("Sprites/Default"));
                drawLine.startWidth = lineWidth;
                drawLine.endWidth = lineWidth;
            }

            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.P)) {
                drawTimer -= Time.deltaTime;
                if (drawTimer <= 0) {
                    linePoints.Add(GetMousePosition());
                    drawLine.positionCount = linePoints.Count;
                    drawLine.SetPositions(linePoints.ToArray());
                    drawTimer = drawTimerDelay;
                }
            }

            if (Input.GetMouseButtonUp(0) && linePoints.Count >= 1) {
                plantConnected = LineConnected();
                linePoints.Clear();
            }

            // Trigger sequence start sound
            GameObject.Find("fungi growth sound").GetComponent<AudioSource>().Play();
        }

        if (plantConnected) {
            if (!completeDialogueLoaded) { 
                LoadNewStory("PlantConnected");
                completeDialogueLoaded = true;

                // Trigger sequence start sound
                GameObject.Find("swell sound").GetComponent<AudioSource>().Play();
            }
            globalVol.SetActive(false);
            completionEffects.SetActive(true);
            if ((int)currentStory.variablesState["fadeToBlack"] == 1 && !fadedOut) {
                StopCoroutine(firstCR);
                StartCoroutine(FadeOut());
                fadedOut = true;
            }
        }
    }

    /*Functions related to dialogue*/
   
    //function hooked to response button
    public void MakeChoice()
    {
        currentStory.ChooseChoiceIndex(0);
        ContinueStory();
    }
    //function that loads next dialogue from story as long as there are more lines to read
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            speakerDialogue.text = currentStory.Continue();
            if (speakerDialogue.text == "") { dialogueOpen = false; }
            else { DisplayChoice();  }
           
        }
    }
    //Only shows response button if it's an option to click it
    private void DisplayChoice()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > 0)
        {
            Choice choice = currentStory.currentChoices[0];
            if (choice.text == "padding")
            {
                responseButton.SetActive(false);
            }
            else
            {
                responseButton.SetActive(true);
                responseText.text = choice.text;
            }
        }
    }

    //function that loads story when given story name
    public void LoadNewStory(string newConvo)
    {
        convoName = newConvo;
        currentStory.ChoosePathString(newConvo);
        dialogueOpen = true;
        ContinueStory();
    }

    /*Functions in related to game mechanics*/
    //function called at left click
    //in charge of dragging and dropping anything in draggable layer
    //and playing music hint & selecting puzzle noises
    private void leftClicked(InputAction.CallbackContext context) {
        //make sure that interactions can only happen when user is incapable of clicking through dialogue
        if (!responseButton.activeInHierarchy) { 
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {

                if (hit.collider != null && hit.collider.gameObject.layer == 6 && !Input.GetKey(KeyCode.P)) {
                    StartCoroutine(DragUpdate(hit.collider.gameObject));
                } else if (hit.collider != null && hit.collider.gameObject.CompareTag("HintTrigger") && !Input.GetKey(KeyCode.P) && !puzzleSolved && (int)currentStory.variablesState["hintEnabled"] == 1) {
                    //check if player has removed enough pollutants to enable hint
                    if (pollutantsRemoved >= pollutantsToRemove) {
                        audioPuzzle.PlayHint();
                        Debug.Log("audioPuzzle puzzle play hint");
                        if (!hintPlayed) { LoadNewStory("AudioPuzzle1"); }
                    }
                    hintPlayed = true;
                } else if (hit.collider != null && hit.collider.gameObject.CompareTag("PuzzleNoise") && !Input.GetKey(KeyCode.P) && nodesGrown && !puzzleSolved && (int)currentStory.variablesState["puzzleEnabled"] == 1) {
                    int.TryParse(hit.collider.gameObject.name.Substring(7, 1), out noiseIndex);
                    if (noiseIndex > 0) {
                        //call coroutine that turns related visual effect on and off
                        StartCoroutine(NodeClicked(noiseIndex-1));
                        noiseChoices = String.Concat(noiseChoices, noiseIndex);
                        Debug.Log(noiseChoices);
                        audioPuzzle.PlayElement(noiseIndex - 1);
                        audioPuzzle.StopHint();
                        LoadNewStory("AudioPuzzle2");
                    }
                }
            }
        }
    }

    //function called at right click in charge of zooming in and out
    private void rightClicked(InputAction.CallbackContext context) {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider != null && hit.collider.gameObject.layer == 3) {
                zoomDir = hit.collider.gameObject.GetComponent<ZoomLayerObj>().zoomDir;
                hit.collider.gameObject.GetComponent<ZoomLayerObj>().zoomDir *= -1;
                StartCoroutine(ZoomIn(hit.collider.gameObject));
            }
        }
    }

    //helper function for path drawing
    Vector3 GetMousePosition() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 110;
    }

    //function that detects "collision" between two game objects
    //not actual collision, just if  it appears as if they are colliding to the player
    private bool CollisionDetection(GameObject a, GameObject b) {
        Vector3 aScreenPos = mainCamera.WorldToScreenPoint(a.transform.position);
        Vector3 bScreenPos = mainCamera.WorldToScreenPoint(b.transform.position);
        if (Math.Abs(aScreenPos.x - bScreenPos.x) < 50 && Math.Abs(aScreenPos.y - bScreenPos.y) < 50) {
            return true;
        }
        return false;
    }

    //functions that checks if path connects plant with fungus 
    private bool LineConnected() {
        //check if line was connected from plant...
        if (31 > Math.Abs(linePoints[0].x) && 31 > Math.Abs(linePoints[0].y)) {
            //to bottom right fungus
            if (linePoints[linePoints.Count - 1].x < 80 && linePoints[linePoints.Count - 1].x > 50) {
                if (linePoints[linePoints.Count - 1].y < -10 && linePoints[linePoints.Count - 1].y > -40) { return true; } 
                
                //to top right fungus
                if (linePoints[linePoints.Count - 1].x < 65 && linePoints[linePoints.Count - 1].x > 45)
                {
                    if (linePoints[linePoints.Count - 1].y < 45 && linePoints[linePoints.Count - 1].y > 20) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            //to bottom left fungus
            if (linePoints[linePoints.Count - 1].x < -40 && linePoints[linePoints.Count - 1].x > -90)
            {
                if (linePoints[linePoints.Count - 1].y < 10 && linePoints[linePoints.Count - 1].y > -50) { return true; }
                else { return false; }
            }
            else { return false; }
        }

        //check if line was connected from to plant from...
        else if(31 > Math.Abs(linePoints[linePoints.Count - 1].x) && 31 > Math.Abs(linePoints[linePoints.Count - 1].y)) {
            //bottom right fungus
            if (linePoints[0].x < 80 && linePoints[0].x > 50)
            {
                if (linePoints[0].y < -10 && linePoints[0].y > -50) { return true; }
                //to top right fungus
                if (linePoints[0].x < 65 && linePoints[0].x > 45)
                {
                    if (linePoints[0].y < 45 && linePoints[0].y > 20) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            //bottom left fungus
            if (linePoints[0].x < -40 && linePoints[0].x > -90)
            {
                if (linePoints[01].y < 10 && linePoints[0].y > -50) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    //function that returns how many pollutants that are visible in the scene
    private int VisiblePollutantCounter() {
        int counter = 0;
        for (int i = 0; i < pollutantList.Count; i++) {
            if (pollutantList[i].transform.GetChild(0).GetComponent<Renderer>().isVisible) {
                counter += 1;
            }
        }
        return counter;
    }

    //coroutine that loads dialogue after certain amount of seconds
    //going to use to turn on some effects as well as it is only called when nodules grow
    IEnumerator LoadAfterSec(string path, int sec)
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(sec);
        foreach (GameObject e in nodeGrowingEffect)
        {
            e.SetActive(true);
        }
        dialogueOpen = true;
        LoadNewStory(path);
    }

    //coroutine in charge of dragging and dropping called in Rightclicked function
    private IEnumerator NodeClicked(int nodeInd)
    {
        nodeClickingEffect[nodeInd].SetActive(true);
        yield return new WaitForSeconds(20);
        nodeClickingEffect[nodeInd].SetActive(false);
    }

    //coroutine that fades scene in
    private IEnumerator FadeIn() {
        while (blackBoxCG.alpha >= 0) {
            yield return waitForFixedUpdate;
            blackBoxCG.alpha -= 0.3f * Time.deltaTime;
        }
    }

    //coroutine that fades scene out
    private IEnumerator FadeOut()
    {
        while (blackBoxCG.alpha <= 1)
        {
            yield return waitForFixedUpdate;
            blackBoxCG.alpha += 0.3f * Time.deltaTime;
        }

    }
    //coroutine in charge of dragging and dropping called in LeftClicked function
    private IEnumerator DragUpdate(GameObject clickedObject) {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        while (leftClick.ReadValue<float>() != 0) {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (rb != null) {
                Vector2 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicsSpeed;
                yield return waitForFixedUpdate;
            }
        }
    }

    //coroutine in charge of dragging and dropping called in Rightclicked function
    private IEnumerator ZoomIn(GameObject clickedObject) {
        zoomLoc = clickedObject.transform.position;
        zoomLoc.z -= 15;
        zoomTimer = 0.0f;
        while (zoomTimer < zoomTime) {
            yield return new WaitForEndOfFrame();
            zoomTimer += Time.deltaTime;
            if (zoomDir == 1) { mainCamera.transform.position = Vector3.Lerp(camLoc, zoomLoc, zoomTimer / zoomTime); } else { mainCamera.transform.position = Vector3.Lerp(zoomLoc, camLoc, zoomTimer / zoomTime); }
        }
    }
}
