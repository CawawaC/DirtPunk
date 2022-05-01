using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using TMPro;
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

    //General variables
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    //Fade variables
    public GameObject blackBox;
    private CanvasGroup blackBoxCG;
    private IEnumerator firstCR;
    private bool fadedOut;
    //Dialogue UI
    public GameObject dialogueBox;
    public TextMeshProUGUI speakerDialogue;
    //Dialogue variables
    private Story currentStory;
    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;
    [SerializeField] private TextMeshProUGUI responseText;
    private string convoName;
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
        blackBoxCG.alpha = 1;
        firstCR = FadeIn();
        StartCoroutine(firstCR);
        fadedOut = false;
        convoName = "MotherTreeIntro";
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(convoName);
        ContinueStory();
    }

    private void Update()
    {
        if (speakerDialogue.text == "")
        {
            dialogueBox.SetActive(false);
            if (!fadedOut)
            {
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
            DisplayChoice();
        }
    }
    //Only shows response button if it's an option to click it
    private void DisplayChoice()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > 0)
        {
            Choice choice = currentStory.currentChoices[0];
            responseText.text = choice.text;
        }
    }

    //function that loads story when given story name
    public void LoadNewStory(string newConvo)
    {
        convoName = newConvo;
        currentStory.ChoosePathString(newConvo);
        ContinueStory();
    }

    //coroutine that fades scene in
    private IEnumerator FadeIn()
    {
        while (blackBoxCG.alpha >= 0)
        {
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
            if (blackBoxCG.alpha >= 1) {
                SceneManager.LoadScene(1);
            }
        }

    }
}
