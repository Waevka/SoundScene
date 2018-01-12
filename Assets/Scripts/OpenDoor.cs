using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {
    public bool isOpen = false;
    private Animator animator;

    [FMODUnity.EventRef]
    public string DoorEvent;
    FMOD.Studio.EventInstance Door;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
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
                }
                else
                {
                    PlayDoorAnim("DoorOpen");
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
