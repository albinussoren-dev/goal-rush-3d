using System;
using System.IO;
using UnityEngine;

[Serializable] public class SaveData {
    public int coins=0, gems=100, money=0, level=1;
    public int[] stars=new int[100];
    public int[] bestScores=new int[100];
    public int missionLevels=0;
}
public static class SaveSystem {
    public static SaveData Data=new SaveData();
    static string PathName=>Path.Combine(Application.persistentDataPath,"goalrush_save.json");
    public static void Load(){try{if(File.Exists(PathName))Data=JsonUtility.FromJson<SaveData>(File.ReadAllText(PathName));}catch{Data=new SaveData();} Normalize();}
    public static void Save(){Normalize();File.WriteAllText(PathName,JsonUtility.ToJson(Data,true));}
    static void Normalize(){if(Data==null)Data=new SaveData();if(Data.stars==null||Data.stars.Length!=100)Data.stars=new int[100];if(Data.bestScores==null||Data.bestScores.Length!=100)Data.bestScores=new int[100];Data.level=Mathf.Clamp(Data.level,1,100);}
}
