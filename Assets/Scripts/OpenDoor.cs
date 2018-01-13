using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {
    public bool isOpen = false;
    private Animator animator;

    [FMODUnity.EventRef]
    public string DoorEvent;
    FMOD.Studio.EventInstance Door;
    [FMODUnity.EventRef]
    public string MuffledSpeechEvent;
    FMOD.Studio.EventInstance Speech;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        Speech = FMODUnity.RuntimeManager.CreateInstance(MuffledSpeechEvent);
        Speech.start();
        print("Muffling sound");
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (isOpen)
                {
                    PlayDoorAnim("DoorClose");
                    Speech.start();
                    print("Muffling sound");
                }
                else
                {
                    PlayDoorAnim("DoorOpen");
                    Speech.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    print("Stopping muffled sound");
                }
                isOpen = !isOpen;
            }
        }
    }
    private void PlayDoorAnim(string name)
    {
        //Fire FMOD event
        animator.Play(name);
        Door = FMODUnity.RuntimeManager.CreateInstance(DoorEvent);
        Door.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Door.start();
        print("Playing door sound");
    }

}
