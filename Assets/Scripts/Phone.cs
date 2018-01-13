using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour {

    [FMODUnity.EventRef]
    public string PhoneEvent;
    FMOD.Studio.EventInstance Ringtone;
    bool effectPlaying = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (effectPlaying){
            Ringtone.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        }
    }

    private void OnEnable()
    {
        Ringtone = FMODUnity.RuntimeManager.CreateInstance(PhoneEvent);
        Ringtone.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Ringtone.start();
        effectPlaying = true;
        print("Playing ringtone sound");
        StartCoroutine(DisablePhone());
    }

    private IEnumerator DisablePhone()
    {
        yield return new WaitForSeconds(5.0f);
        effectPlaying = false;
        gameObject.SetActive(false);
    }
}
