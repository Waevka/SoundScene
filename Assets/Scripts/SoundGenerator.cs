using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SoundGenerator : MonoBehaviour {

    private FMOD.System lowLevelSystem;
    private FMOD.Channel channel;
    private FMOD.ChannelGroup channelGroup;
    private FMOD.CREATESOUNDEXINFO soundInfo;
    private FMOD.Sound generatedSound;
    private int sampleRate = 44100;
    private int numberOfChannels = 1;
    public float volume;
    public float frequency;
    public float minDistance;
    public float maxDistance;
    FMOD.VECTOR alt_pan_pos;
    private int sampleNumber = 0;

    // Use this for initialization
    void Start () {
        lowLevelSystem = RuntimeManager.LowlevelSystem;
        channel = new FMOD.Channel();
        lowLevelSystem.getMasterChannelGroup(out channelGroup);

        soundInfo = new FMOD.CREATESOUNDEXINFO();
        soundInfo.cbsize = Marshal.SizeOf(soundInfo);
        soundInfo.decodebuffersize = (uint)sampleRate / 10;
        soundInfo.length = (uint)(sampleRate * numberOfChannels * sizeof(short));
        soundInfo.numchannels = numberOfChannels;
        soundInfo.defaultfrequency = sampleRate;
        soundInfo.format = FMOD.SOUND_FORMAT.PCM16;

        soundInfo.pcmreadcallback = PCMReadCallback;
        soundInfo.pcmsetposcallback = PCMSetPositionCallback;

        lowLevelSystem.setStreamBufferSize(65536, FMOD.TIMEUNIT.RAWBYTES);
        lowLevelSystem.createStream("SoundGeneratorStream", FMOD.MODE.OPENUSER, ref soundInfo, out generatedSound);

        generatedSound.setMode(FMOD.MODE.OPENUSER | FMOD.MODE._3D | FMOD.MODE._3D_LINEARSQUAREROLLOFF);

        lowLevelSystem.playSound(generatedSound, channelGroup, true, out channel);
        channel.setLoopCount(-1);
        channel.setMode(FMOD.MODE.LOOP_NORMAL);
        channel.setPosition(0, FMOD.TIMEUNIT.MS);
        channel.set3DMinMaxDistance(minDistance, maxDistance);
        Update();
        channel.setPaused(false);
    }
	
	// Update is called once per frame
	void Update () {
        FMOD.ATTRIBUTES_3D attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);
        alt_pan_pos = Vector3.zero.ToFMODVector();
        channel.set3DAttributes(ref attributes.position, ref attributes.velocity, ref alt_pan_pos);
	}


    private FMOD.RESULT PCMSetPositionCallback(IntPtr raw, int subsound, uint position, FMOD.TIMEUNIT postype)
    {
        return FMOD.RESULT.OK;
    }

    private FMOD.RESULT PCMReadCallback(IntPtr raw, IntPtr data, uint shortLength)
    {
        int length = ((int)shortLength) / sizeof(short);
        short[] buffer = new short[length];
        int i = 0;

        for(i = 0; i < length; i++)
        {
            short sample = (short)Mathf.RoundToInt(GenerateSample() * 32767.0f * volume);
            buffer[i] = sample;
            sampleNumber++;

            if (sampleNumber >= sampleRate) sampleNumber = 0;
        }

        Marshal.Copy(buffer, 0, data, length);

        return FMOD.RESULT.OK;
    }

    private void OnDestroy()
    {
        channel.stop();
    }

    private float GenerateSample()
    {
        return Mathf.Sin((Mathf.PI * 2 * sampleNumber * frequency) / sampleRate);
    }
}
