using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour {
    public Transform[] controlPoints;
    public int currentControlPoint;
    NavMeshAgent agent;
    [FMODUnity.EventRef]
    public string PoliceEvent;
    FMOD.Studio.EventInstance PoliceAlarm;
    // Use this for initialization
    void Start () {
        currentControlPoint = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        TravelToNextPoint();
        PoliceAlarm = FMODUnity.RuntimeManager.CreateInstance(PoliceEvent);
        PoliceAlarm.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        PoliceAlarm.start();
        print("Playing bee sound");
    }
	
	// Update is called once per frame
	void Update () {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            TravelToNextPoint();
        }
        PoliceAlarm.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
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
