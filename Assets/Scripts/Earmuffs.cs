using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Earmuffs : MonoBehaviour {
    public bool EarmuffsOn;
    [SerializeField]
    GameObject earmuffModel;

    private FMOD.DSP DSPfilter;
    private FMOD.DSP_DESCRIPTION description;
    public int sampleRate;
    private int sampleNumber = 0;
    int inchannels;

    //lowpass filter settings
    bool coefficientsInitialized = false;
    float[] xn, yn, xn1, xn2, yn1, yn2;
    float s, c, alpha, r, a0, a1, a2, b1, b2;
    float QValue = 0.5f;
    public float CutOffFrequency = 350.0f;

    // Use this for initialization
    void Start () {
        EarmuffsOn = false;
        earmuffModel.SetActive(false);
        coefficientsInitialized = false;
        description = new FMOD.DSP_DESCRIPTION();
        description.read = DSPCallback;
        FMODUnity.RuntimeManager.LowlevelSystem.createDSP(ref description, out DSPfilter);
        DSPfilter.setBypass(true);
        FMOD.ChannelGroup channelgroup;
        FMODUnity.RuntimeManager.LowlevelSystem.getMasterChannelGroup(out channelgroup);

        FMOD.RESULT result = channelgroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, DSPfilter);
        Debug.Log("Adding earmuffs DSP: " + result);

        FMOD.SPEAKERMODE speakerMode;
        int numrawspeakers;
        FMODUnity.RuntimeManager.LowlevelSystem.getSoftwareFormat(out sampleRate, out speakerMode, out numrawspeakers);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FlipEarmuffStatus()
    {
        // set 'bypass' not 'active'! = false - DSP working, true - DSP off
        EarmuffsOn = !EarmuffsOn;
        DSPfilter.setBypass(!EarmuffsOn);
        coefficientsInitialized = false;
        earmuffModel.SetActive(EarmuffsOn);
    }

    private FMOD.RESULT DSPCallback(ref FMOD.DSP_STATE dsp_state, IntPtr inbuffer, IntPtr outbuffer, uint length, int inchannels, ref int outchannels)
    {
        float[] buffer = new float[length * inchannels];
        int i = 0;
        IntPtr currentPtr;
        this.inchannels = inchannels;

        for (i = 0; i < length * inchannels; i++)
        {
            currentPtr = new IntPtr(inbuffer.ToInt32() + (i * sizeof(float)));
            float sample = (float)System.Runtime.InteropServices.Marshal.PtrToStructure(currentPtr, typeof(float));
            buffer[i] = LowPassFilter(sample, i % inchannels);
            sampleNumber++;

            if (sampleNumber >= sampleRate) sampleNumber = 0;
        }

        Marshal.Copy(buffer, 0, outbuffer, (int)length * inchannels);

        return FMOD.RESULT.OK;
    }

    private float LowPassFilter(float sample, int channelIndex)
    {
        if (!coefficientsInitialized)
        {
            initCoefficients();
        }

        //przesuwamy poprzednie probki o 1 do tylu
        xn2[channelIndex] = xn1[channelIndex];
        xn1[channelIndex] = xn[channelIndex];

        yn2[channelIndex] = yn1[channelIndex];
        yn1[channelIndex] = yn[channelIndex];

        //obecne probki
        xn[channelIndex] = sample;

        s = Mathf.Sin((2.0f * Mathf.PI * CutOffFrequency) / (sampleRate));
        c = Mathf.Cos((2.0f * Mathf.PI * CutOffFrequency) / (sampleRate));
        alpha = s / (2.0f * QValue);
        r = (1.0f / (1.0f + alpha));

        a0 = 0.5f * (1.0f - c) * r;
        a1 = (1.0f - c) * r;
        a2 = a0;
        b1 = -2.0f * c * r;
        b2 = (1.0f - alpha) * r;

        yn[channelIndex] = (a0 * xn[channelIndex]) + (a1 * xn1[channelIndex]) + (a2 * xn2[channelIndex])
            - (b1 * yn1[channelIndex]) - (b2 * yn2[channelIndex]);

        return yn[channelIndex];
    }
    private void initCoefficients()
    {
        //inicjalizujemy tablice
        xn = new float[inchannels];
        yn = new float[inchannels];
        xn1 = new float[inchannels];
        xn2 = new float[inchannels];
        yn1 = new float[inchannels];
        yn2 = new float[inchannels];
        coefficientsInitialized = true;
    }
}
