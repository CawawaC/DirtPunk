using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
/*
This is the manager of the first scene of the game
It allows the player to click on the mother plant
Which leads to a zoom in + fade to black
And loads the next scene (cellular level)
 */
public class ZoomSceneManager : MonoBehaviour
{
    //singleton pattern
    public static ZoomSceneManager instance;
    
    //camera variables
    private Camera mainCamera;
    private Vector3 camLoc;

    //variables related to zoom and fade
    public GameObject zoomLocObj;
    private Vector3 zoomLoc;
    private float zoomTime = 5.0f;
    private float timer;
    //second timer for fading to black halfway through zoom
    private float timer2;

    //UI elements
    public GameObject blackBox;
    private CanvasGroup blackBoxCG;

    //General variables
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        //make sure that there is only single instance of manager
        if (instance != null)
        {
            Debug.LogWarning("Found more than one zoom scene manager");
        }
        instance = this;
    }

    void Start()
    {
        //initialize variables
        mainCamera = Camera.main;
        zoomLoc = mainCamera.transform.position;
        zoomLoc = zoomLocObj.transform.position;
        blackBoxCG = blackBox.GetComponent<CanvasGroup>();
        blackBoxCG.alpha = 0;
    }

    //function called when click area button is pressed in plant zoom scene
    public void PlantClicked() {
        StartCoroutine(ZoomIn());
    }

    private IEnumerator ZoomIn()
    {
        timer = 0.0f;
        timer2 = 0.0f;
        while (timer < zoomTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(camLoc, zoomLoc, timer / zoomTime);
            //if half of zoom time elapsed, start fading to black
            if (timer>=2.5) {
                timer2 += Time.deltaTime;
                blackBoxCG.alpha= Mathf.Lerp(0, 1, timer2/2.5f);
            }   
        }
        //call next scene when zoom and fade complete
        SceneManager.LoadScene(1);
    }
}
