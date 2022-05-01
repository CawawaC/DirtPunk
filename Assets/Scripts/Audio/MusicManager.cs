using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    GameObject currentPart;
    public string initialPart = "pollution";

    // Start is called before the first frame update
    void Start() {
        currentPart = GetPart(initialPart);
        PlayPart(initialPart);
    }

    // Update is called once per frame
    void Update() {

    }

    public void PlayPart(string partname) {
        Debug.Log("music manager plays part: " + partname);
        StopPart(currentPart);

        GameObject part = GetPart(partname);
        if (part != null) {
            foreach (AudioSource source in part.GetComponents<AudioSource>()) {
                source.Play();
            }
        } else {
            Debug.LogError("Could not find music part: " + partname, this);
        }
        currentPart = part;
    }

    void StopPart(GameObject part) {
        if (part != null) {
            foreach (AudioSource source in part.GetComponents<AudioSource>()) {
                source.Stop();
            }
        } else {
            Debug.LogError("Referenced current part is null", this);
        }
    }

    GameObject GetPart(string partname) {
        return transform.Find(partname).gameObject;
    }
}
