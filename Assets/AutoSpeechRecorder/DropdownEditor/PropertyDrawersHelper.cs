using UnityEngine;

public static class PropertyDrawersHelper
{
#if UNITY_EDITOR
    public static string[] MicrophoneOptions()
    {
        return Microphone.devices;
    }

#endif
}