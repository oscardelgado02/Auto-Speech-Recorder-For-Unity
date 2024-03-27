/**
 * A tool that can be used on Unity to automatically detect and record speech throught the microphone.
 * 
 * How to use:
 *  - Add the "AutoSpeechRecorder" folder into your "Assets" folder.
 *  - Attach this script into a GameObject on your Unity scene.
 *  - Attach the "TimeUpdate.cs" script into the same (or another) GameObject on your Unity scene.
 *  - Modify the "ProcessAudio" method (at the end of the script) to use the output AudioClip as you wish.
 *  - You have a scene example on "Examples/Example".
 *
 * Developed by https://github.com/oscardelgado02
 */

using UnityEngine;

public class AutoSpeechRecorder : MonoBehaviour
{
    [StringInList(typeof(PropertyDrawersHelper), "MicrophoneOptions")] public string microphone;

    [SerializeField][Tooltip("Output audio sample window")] private int sampleWindow = 64;
    [SerializeField][Tooltip("Adjust the sensibility of the microphone")] private float loudnessSensibility = 10f;
    [SerializeField][Tooltip("Threshold of the microphone to detect sound")] private float threshold = 0.35f;

    private AudioClip microphoneClip;
    private bool speechClipRecording = false;

    [SerializeField][Tooltip("Maximum seconds to record a speech")] private int maxSecondsTalking = 5;
    [SerializeField][Tooltip("Maximum seconds without detecting a sound to stop recording a speech")] private int maxSecondsBeingSilent = 1;

    private int timerTalking;
    private int timerBeingSilent;
    private int startPositionClip = 0;

    [SerializeField][Tooltip("AudioSource to reproduce the speech recorded as an example")] private AudioSource audioSource;

    private void Start()
    {
        //  Init the microphone
        if (!string.IsNullOrEmpty(microphone))
        {
            Debug.Log("Selected option: " + microphone);

            //  Init the timers
            timerTalking = Timers.Instance.CreateTimer(true);
            timerBeingSilent = Timers.Instance.CreateTimer(true);

            //  Start recording
            RecordLoudness();
        }
        else
        {
            Debug.Log("No microphone found!");
        }
    }

    private void Update()
    {
        //  In case the system stops recording, start again
        if (Microphone.GetPosition(microphone) >= microphoneClip.samples)
        {
            RecordLoudness();
        }

        //  In case it detects some sound, save the position of the clip when the sound started
        if (!speechClipRecording && GetIfLoudnessGreaterThanThreshold())
        {
            speechClipRecording = true;
            startPositionClip = Microphone.GetPosition(microphone);
        }

        //  In case a sound has been detected
        if (speechClipRecording)
        {
            //  If it has recorded the maximum seconds set in "maxSecondsTalking", it processes the audio and starts again recording
            if (Timers.Instance.WaitTime(timerTalking, maxSecondsTalking))
                ProcessAudioAndDetectAgain();

            //  Else, if the user keeps doing sound, keep recording
            else if (GetIfLoudnessGreaterThanThreshold())
                Timers.Instance.ResetTimer(timerBeingSilent);

            //  Else, if the user stops doing sound for "maxSecondsBeingSilent", it processes the audio and starts again recording
            else if (Timers.Instance.WaitTime(timerBeingSilent, maxSecondsBeingSilent))
                ProcessAudioAndDetectAgain();
        }
    }

    private void ProcessAudioAndDetectAgain()
    {
        //  The system enables the variable to wait and detect sound again
        speechClipRecording = false;

        //  Both timers are reseted
        Timers.Instance.ResetTimer(timerTalking);
        Timers.Instance.ResetTimer(timerBeingSilent);

        //  The audio is processed
        ProcessAudio();

        //  The system waits to detect sound again
        RecordLoudness();
    }

    //-------------Loudness Detection---------------

    //  The system records audio for 60 seconds each time "RecordLoudness" is called
    private void RecordLoudness()
    {
        Microphone.End(microphone);
        microphoneClip = Microphone.Start(microphone, false, 60, AudioSettings.outputSampleRate);
    }

    //  TRUE: The system has detected sound above the threshold; FALSE: Otherwise
    public bool GetIfLoudnessGreaterThanThreshold()
    {
        float loudness = GetLoudnessFromMicrophone() * loudnessSensibility;
        return loudness > threshold;
    }

    //  RETURN: Gets the value of the loudness detected from the system
    private float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(microphone), microphoneClip);
    }

    private float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition > 0)
        {
            float[] waveData = new float[sampleWindow];
            clip.GetData(waveData, startPosition);

            float totalLoudness = 0f;
            for (int i = 0; i < sampleWindow; i++)
            {
                totalLoudness += Mathf.Abs(waveData[i]);
            }

            return totalLoudness / sampleWindow;
        }

        return 0f;
    }

    //-------------Process Speech---------------

    private float[] GetSampleDataFromMicrophone()
    {
        float[] samples = new float[(microphoneClip.samples - Microphone.GetPosition(microphone)) * microphoneClip.channels];
        microphoneClip.GetData(samples, startPositionClip);
        return samples;
    }

    //  Method that process the audio recorded. You can modify it to send the audio to other scripts.
    private void ProcessAudio()
    {
        //  Extract sub-audio clip starting from the beginning of the recorded sound
        AudioClip subClip = AudioClip.Create("SubClip", microphoneClip.samples - Microphone.GetPosition(microphone), microphoneClip.channels, microphoneClip.frequency, false);
        float[] samples = GetSampleDataFromMicrophone();
        subClip.SetData(samples, 0);

        //  Example: Reproduce the audio
        audioSource.clip = subClip;
        audioSource.Play();
    }
}