using System;
using System.IO;
using UnityEngine;

[Serializable] public class SaveData {
    public int saveVersion=2;
    public int coins=0, gems=100, money=0, level=1;
    public int[] stars=new int[100];
    public int[] bestScores=new int[100];
    public int missionLevels=0;
    public int shieldCharges=0, magnetCharges=0;
    public PlayerUpgradeData upgrades=new PlayerUpgradeData();
    public DailyRewardData daily=new DailyRewardData();
    public MissionData missions=new MissionData();
    public SettingsData settings=new SettingsData();
}

public static class SaveSystem {
    public const int CurrentVersion=2;
    public static SaveData Data=new SaveData();
    static string PathName=>Path.Combine(Application.persistentDataPath,"goalrush_save.json");
    static string BackupName=>Path.Combine(Application.persistentDataPath,"goalrush_save.backup.json");
    static string TempName=>Path.Combine(Application.persistentDataPath,"goalrush_save.tmp");
    public static void Load(){
        try {
            if(File.Exists(PathName)) {
                Data=JsonUtility.FromJson<SaveData>(File.ReadAllText(PathName));
            }
        } catch {
            TryRestoreBackup();
        }
        Normalize();
    }
    public static void Save(){
        Normalize();
        try {
            string json=JsonUtility.ToJson(Data,true);
            if(File.Exists(PathName)) File.Copy(PathName,BackupName,true);
            File.WriteAllText(TempName,json);
            if(File.Exists(PathName)) File.Delete(PathName);
            File.Move(TempName,PathName);
        } catch(Exception e) { Debug.LogWarning("Goal Rush save failed: "+e.Message); }
    }
    static void TryRestoreBackup(){
        try {
            if(File.Exists(BackupName)) Data=JsonUtility.FromJson<SaveData>(File.ReadAllText(BackupName));
            else Data=new SaveData();
        } catch { Data=new SaveData(); }
    }
    static void Normalize(){
        if(Data==null)Data=new SaveData();
        Data.saveVersion=CurrentVersion;
        if(Data.stars==null||Data.stars.Length!=100)Data.stars=new int[100];
        if(Data.bestScores==null||Data.bestScores.Length!=100)Data.bestScores=new int[100];
        if(Data.upgrades==null)Data.upgrades=new PlayerUpgradeData();
        if(Data.upgrades.unlockedCharacters==null||Data.upgrades.unlockedCharacters.Length!=5)Data.upgrades.unlockedCharacters=new bool[5];
        if(Data.upgrades.unlockedCharacters.Length>0)Data.upgrades.unlockedCharacters[0]=true;
        if(Data.daily==null)Data.daily=new DailyRewardData();
        if(Data.missions==null)Data.missions=new MissionData();
        if(Data.settings==null)Data.settings=new SettingsData();
        Data.level=Mathf.Clamp(Data.level,1,100);
        Data.coins=Mathf.Max(0,Data.coins); Data.gems=Mathf.Max(0,Data.gems); Data.money=Mathf.Max(0,Data.money);
    }
}
