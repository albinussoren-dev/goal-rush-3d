using System;
using UnityEngine;

[Serializable] public struct LevelDefinition {
    public int id, world, difficulty, targetScore, coinGoal;
    public float speed, length;
    public LevelDefinition(int id) {
        this.id=id; world=(id-1)/20+1; difficulty=1+(id-1)/20;
        targetScore=400+id*20; coinGoal=10+(id%10);
        speed=7f+world*0.7f+id*0.015f; length=90f+id*3f;
    }
}

public static class LevelCatalog {
    public const int TotalLevels=100, LevelsPerWorld=20, WorldCount=5;
    public static LevelDefinition Get(int id)=>new LevelDefinition(Mathf.Clamp(id,1,TotalLevels));
    public static bool IsUnlocked(int id)=>id<=Mathf.Clamp(SaveSystem.Data.level,1,TotalLevels);
    public static int WorldFor(int id)=>(Mathf.Clamp(id,1,TotalLevels)-1)/LevelsPerWorld+1;
}
