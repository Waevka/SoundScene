using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchOrgans : MonoBehaviour {

    [FMODUnity.EventRef]
    public string ChurchEvent;
    FMOD.Studio.EventInstance OrganLoop;

    // Use this for initialization
    void Start () {
        OrganLoop = FMODUnity.RuntimeManager.CreateInstance(ChurchEvent);
        OrganLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        OrganLoop.start();
        print("Playing church organ sound");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
