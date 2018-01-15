using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioStand : MonoBehaviour {
    [SerializeField]
    GameObject radioPlayerPrefab;
    [SerializeField]
    GameObject currentRadioPlayerReference;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(currentRadioPlayerReference == null)
        {
            currentRadioPlayerReference = Instantiate(radioPlayerPrefab, transform, false);
        }
	}
}
