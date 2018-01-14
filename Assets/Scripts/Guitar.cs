using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : MonoBehaviour {
    FMOD.System lowLevelSystem;
    FMOD.ChannelGroup channelGroup;
    FMOD.Channel channel;
    FMOD.Sound guitarSong;
    public float minDistance;
    public float maxDistance;
	// Use this for initialization
	void Start () {
        channel = new FMOD.Channel();
        lowLevelSystem = FMODUnity.RuntimeManager.LowlevelSystem;
        FMODUnity.RuntimeManager.LowlevelSystem.getMasterChannelGroup(out channelGroup);

        lowLevelSystem.createSound("Assets\\Sounds\\guitar.mp3", FMOD.MODE.DEFAULT, out guitarSong);
        string songName = "";
        guitarSong.getName(out songName, 20);
        Debug.Log("Playing song: " + songName, gameObject);
        guitarSong.setMode(FMOD.MODE.OPENUSER | FMOD.MODE._3D | FMOD.MODE._3D_LINEARSQUAREROLLOFF); 

        lowLevelSystem.playSound(guitarSong, channelGroup, true, out channel);
        channel.set3DMinMaxDistance(minDistance, maxDistance);
        channel.setLoopCount(-1);
        channel.setMode(FMOD.MODE.LOOP_NORMAL);
        FMOD.ATTRIBUTES_3D attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);
        FMOD.VECTOR alt_pan_pos = Vector3.zero.ToFMODVector();
        channel.set3DAttributes(ref attributes.position, ref attributes.velocity, ref alt_pan_pos);

        channel.setPaused(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
