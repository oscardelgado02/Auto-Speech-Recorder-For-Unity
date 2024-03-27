using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    [SerializeField] private AutoSpeechRecorder amr;

    enum microUI { off, on }
    [SerializeField] private List<GameObject> m_;

    private void Update() { MicroRecordingUI(amr.GetIfLoudnessGreaterThanThreshold()); }

    private void MicroRecordingUI(bool status)
    {
        m_[(int)microUI.off].SetActive(!status);
        m_[(int)microUI.on].SetActive(status);
    }
}
