using UnityEngine;

public sealed class PerformanceManager : MonoBehaviour {
    public int targetFrameRate=60;
    public bool lowMemoryMode;
    void Awake(){
        Application.targetFrameRate=targetFrameRate;
        QualitySettings.vSyncCount=0;
        lowMemoryMode=SystemInfo.systemMemorySize<3000;
        if(lowMemoryMode) QualitySettings.SetQualityLevel(0,true);
        DontDestroyOnLoad(gameObject);
    }
    void OnApplicationPause(bool paused){if(!paused)Application.targetFrameRate=targetFrameRate;}
}
