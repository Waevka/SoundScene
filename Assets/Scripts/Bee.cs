﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour {

    public float beeSpeed;
    [SerializeField]
    private GameObject beeModel;
    [FMODUnity.EventRef]
    public string BeeEvent;
    FMOD.Studio.EventInstance BeeLoop;
    // Use this for initialization
    void Start () {
        BeeLoop = FMODUnity.RuntimeManager.CreateInstance(BeeEvent);
        BeeLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(beeModel));
        BeeLoop.start();
        print("Playing bee sound");
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0.0f, beeSpeed, 0.0f);
        BeeLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(beeModel));
    }
}