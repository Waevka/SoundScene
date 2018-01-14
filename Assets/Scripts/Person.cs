using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {
    [FMODUnity.EventRef]
    public string SpeechEvent;
    FMOD.Studio.EventInstance SpeechLoop;
    // Use this for initialization
    void Start () {
        SpeechLoop = FMODUnity.RuntimeManager.CreateInstance(SpeechEvent);
        SpeechLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        SpeechLoop.start();
        Debug.Log("Playing speech loop sound", gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
