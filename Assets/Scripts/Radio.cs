using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Radio : MonoBehaviour {
    public bool isReadyAndPlaying = false;
    public float volume;
    FMOD.System lowLevelSystem;
    FMOD.ChannelGroup channelGroup;
    FMOD.CREATESOUNDEXINFO soundInfo;
    FMOD.Channel channel;
    FMOD.Sound radioStream;
    public float minDistance;
    public float maxDistance;

    FMOD.OPENSTATE state;
    FMOD.RESULT r;
    uint percentbuffered;
    bool starving;
    bool diskbusy;
    // Use this for initialization
    void Start () {
        channel = new FMOD.Channel();

        soundInfo = new FMOD.CREATESOUNDEXINFO();
        soundInfo.cbsize = Marshal.SizeOf(soundInfo);
        soundInfo.suggestedsoundtype = FMOD.SOUND_TYPE.MPEG;

        lowLevelSystem = FMODUnity.RuntimeManager.LowlevelSystem;
        FMODUnity.RuntimeManager.LowlevelSystem.getMasterChannelGroup(out channelGroup);
        lowLevelSystem.setStreamBufferSize(64000, FMOD.TIMEUNIT.RAWBYTES);
        channel.setMode(FMOD.MODE.IGNORETAGS | FMOD.MODE.MPEGSEARCH);
        lowLevelSystem.createStream("http://stream4.nadaje.com:15274/live", //"http://stream.gensokyoradio.net:8000/stream/1/" "http://stream4.nadaje.com:12818/test"
            FMOD.MODE.DEFAULT | FMOD.MODE.NONBLOCKING | FMOD.MODE.CREATESTREAM | FMOD.MODE.MPEGSEARCH,
            ref soundInfo,
            out radioStream);
        
        StartCoroutine(WaitForStreamBuffer());
    }
	
	// Update is called once per frame
	void Update () {
        r = radioStream.getOpenState(out state, out percentbuffered, out starving, out diskbusy);
        Debug.Log("State: " + state + ", %buffered: " + percentbuffered + ", starving?: " + starving + ", diskbusy?: " + diskbusy + " " + r);
        if( r == FMOD.RESULT.ERR_FILE_COULDNOTSEEK)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForStreamBuffer()
    {
        do
        {
            
            yield return null;
        } while (state == FMOD.OPENSTATE.CONNECTING || state == FMOD.OPENSTATE.BUFFERING);

        if (state == FMOD.OPENSTATE.READY)
        {
            Debug.Log("Internet stream loaded");
            radioStream.setMode(FMOD.MODE.OPENUSER | FMOD.MODE._3D | FMOD.MODE._3D_LINEARROLLOFF);
            lowLevelSystem.playSound(radioStream, channelGroup, true, out channel);
            FMOD.ATTRIBUTES_3D attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);
            FMOD.VECTOR alt_pan_pos = Vector3.zero.ToFMODVector();
            channel.set3DMinMaxDistance(minDistance, maxDistance);
            channel.setLoopCount(-1);
            channel.setMode(FMOD.MODE.LOOP_NORMAL);
            channel.set3DAttributes(ref attributes.position, ref attributes.velocity, ref alt_pan_pos);
            channel.setPaused(false);
            channel.setVolume(volume);

            isReadyAndPlaying = true;
        }
        else if (state == FMOD.OPENSTATE.ERROR)
        {
            Debug.Log("Error while loading online , check the address or connection");
        }
    }

    private void OnDestroy()
    {
        channel.setPaused(true);
    }
}
