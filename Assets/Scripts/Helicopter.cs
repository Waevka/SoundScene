using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour {

    [FMODUnity.EventRef]
    public string HelicopterEvent;
    FMOD.Studio.EventInstance HelicopterSound;
    [SerializeField]
    Transform helicopterModel;
    // Use this for initialization
    void Start () {
        HelicopterSound = FMODUnity.RuntimeManager.CreateInstance(HelicopterEvent);
        HelicopterSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(helicopterModel));
        HelicopterSound.start();
        print("Playing helicopter sound");
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0.0f, 0.3f, 0.0f);
        HelicopterSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(helicopterModel));
    }
}
