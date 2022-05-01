using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
This is the manager of the first scene of the game
It allows the player to click on the mother plant
Which leads to a zoom in + fade to black
And loads the next scene (cellular level)
 */
public class IntroSceneManager : MonoBehaviour
{
    //singleton pattern
    public static IntroSceneManager instance;

    //UI elements
    public GameObject blackBox;
    private CanvasGroup blackBoxCG;

    private void Awake()
    {
        //make sure that there is only single instance of manager
        if (instance != null)
        {
            Debug.LogWarning("Found more than one intro scene manager");
        }
        instance = this;
    }

    void Start()
    {
        blackBoxCG = blackBox.GetComponent<CanvasGroup>();
        blackBoxCG.alpha = 0;
    }
}
