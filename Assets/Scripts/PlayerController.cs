using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float walkSpeed;
    public float rotateSpeed;
    Rigidbody rb;
    [SerializeField]
    GameObject phone;
    [SerializeField]
    Earmuffs earmuffs;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        float mov = Input.GetAxis("Vertical") * walkSpeed;
        float rot = Input.GetAxis("Horizontal") * rotateSpeed;

        rb.MovePosition(rb.position + rb.transform.forward * mov);
        rb.MoveRotation(Quaternion.Euler(0.0f, rb.rotation.eulerAngles.y + rot, 0.0f));

        if (Input.GetButtonDown("Fire2") && !phone.activeSelf)
        {
            UsePhone();
        }
        if (Input.GetButtonDown("Fire3"))
        {
            earmuffs.FlipEarmuffStatus();
        }
	}

    void UsePhone()
    {
        phone.SetActive(true);
    }
}
