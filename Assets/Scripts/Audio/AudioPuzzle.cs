using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 Class for handling the audio puzzle elements playback
 */

public class AudioPuzzle : MonoBehaviour {

    public List<AudioSource> melodicElements;   // The melodic chops, ordered by solution play order
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void PlayHint() {
        PlayElement(0);
        // TODO: play the expected sequence of elements
    }

    public void PlayElement(int i) {
        if (i < melodicElements.Count) {
            melodicElements[i].Play();
        }
    }

    // Set volume of melody
    // vol goes from 0 to 1
    public void SetVolume(float vol) {
        return;

        float currentVol = 0;
        mixer.GetFloat("melodicVolume", out currentVol);
        Debug.Log(vol + " " + currentVol);
        if(mixer != null) {
            mixer.SetFloat("melodicVolume", vol);
        }
    }

    void OnEnable() {
        //enable sounds
        foreach(AudioSource source in melodicElements) {
            source.enabled = true;
        }
    }

    void OnDisable() {
        //disable sounds
        foreach (AudioSource source in melodicElements) {
            source.enabled = false;
        }
    }
}
