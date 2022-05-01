using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 Class for handling the audio puzzle elements playback
 */

public class AudioPuzzle : MonoBehaviour {

    public List<AudioSource> melodicElements;   // The melodic chops, ordered by solution play order
    public AudioSource hint;
    public AudioMixer mixer;

    public void PlayHint() {
        Debug.Log("play hint sound");
        if (hint != null) {
            hint.Play();
        }
    }

    public void StopHint() {
        if (hint != null) {
            hint.Stop();
        }
    }

    public void PlayElement(int i) {
        if (i < melodicElements.Count) {
            melodicElements[i].Play();
        }
    }

    // Set volume of melody
    // vol goes from 0 to 1
    public void SetVolume(float vol) {
        float currentVol = 0;
        mixer.GetFloat("melodicVolume", out currentVol);
        Debug.Log(vol + " " + currentVol);
        if(mixer != null) {
            mixer.SetFloat("melodicVolume", vol);
        }
        return;
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
