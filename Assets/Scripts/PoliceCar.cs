using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour {
    public Transform[] controlPoints;
    public int currentControlPoint;
    public float dopplerPitch;
    NavMeshAgent agent;
    [SerializeField]
    GameObject Player;
    [FMODUnity.EventRef]
    public string PoliceEvent;
    FMOD.Studio.EventInstance PoliceAlarm;

    const float SPEED_OF_SOUND = 340.0f;
    // Use this for initialization
    void Start () {
        currentControlPoint = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        TravelToNextPoint();
        PoliceAlarm = FMODUnity.RuntimeManager.CreateInstance(PoliceEvent);
        PoliceAlarm.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        PoliceAlarm.start();
        print("Playing police car sound");
    }
	
	// Update is called once per frame
	void Update () {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            TravelToNextPoint();
        }
        PoliceAlarm.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        //calculate Doppler Effect
        Vector3 directionOfListener = (transform.position - Player.transform.position).normalized;
        float speedRelativeToListener = agent.speed * Vector3.Dot(transform.forward, directionOfListener);
        dopplerPitch = (SPEED_OF_SOUND - speedRelativeToListener) / SPEED_OF_SOUND;
        PoliceAlarm.setPitch(dopplerPitch);
    }

    private void TravelToNextPoint()
    {
        if (controlPoints.Length == 0)
        {
            return;
        }
        
        agent.destination = controlPoints[currentControlPoint].position;
        currentControlPoint = (currentControlPoint + 1) % controlPoints.Length;
    }
}
