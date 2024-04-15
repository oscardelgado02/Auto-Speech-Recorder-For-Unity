# Auto-Speech-Recorder-For-Unity
 An easy to use tool to automatically detect and record speech in Unity

<div align="center">
<img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/Auto-Speech-Recorder-For-Unity-Preview.png" align="center" style="width: 80%" />
</div>
<br>

## How to use:
- Add the [Assets/AutoSpeechRecorder](https://github.com/oscardelgado02/Auto-Speech-Recorder-For-Unity/tree/main/Assets/AutoSpeechRecorder) folder into your "Assets" folder.
- Attach the [Assets/AutoSpeechRecorder/AutoSpeechRecorder.cs](https://github.com/oscardelgado02/Auto-Speech-Recorder-For-Unity/blob/main/Assets/AutoSpeechRecorder/AutoSpeechRecorder.cs) script into a GameObject on your Unity scene.
- Attach the [Assets/AutoSpeechRecorder/TimeUpdate.cs](https://github.com/oscardelgado02/Auto-Speech-Recorder-For-Unity/blob/main/Assets/AutoSpeechRecorder/TimeUpdate.cs) script into the same (or another) GameObject on your Unity scene.
- Modify the "ProcessAudio" method (at the end of [Assets/AutoSpeechRecorder/AutoSpeechRecorder.cs](https://github.com/oscardelgado02/Auto-Speech-Recorder-For-Unity/blob/main/Assets/AutoSpeechRecorder/AutoSpeechRecorder.cs) script) to use the output AudioClip as desired.
- You have a scene example on [Assets/Examples/Example.unity](https://github.com/oscardelgado02/Auto-Speech-Recorder-For-Unity/blob/main/Assets/Examples/Example.unity) to test it.
