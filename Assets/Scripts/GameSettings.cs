using System;
using UnityEngine;

[Serializable] public class SettingsData {
    public float masterVolume=1f, musicVolume=0.8f, sfxVolume=1f;
    public bool vibration=true, batterySaver=false;
}

public static class GameSettings {
    public static SettingsData Data => SaveSystem.Data.settings;
    public static void SetMasterVolume(float value){Data.masterVolume=Mathf.Clamp01(value);Apply();}
    public static void SetMusicVolume(float value){Data.musicVolume=Mathf.Clamp01(value);Apply();}
    public static void SetSfxVolume(float value){Data.sfxVolume=Mathf.Clamp01(value);Apply();}
    public static void SetVibration(bool value){Data.vibration=value;SaveSystem.Save();}
    public static void SetBatterySaver(bool value){Data.batterySaver=value;QualitySettings.vSyncCount=0;Application.targetFrameRate=value?45:60;SaveSystem.Save();}
    public static void Apply(){AudioListener.volume=Data.masterVolume;SaveSystem.Save();}
}
