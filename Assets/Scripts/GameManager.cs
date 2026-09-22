using System;
using UnityEngine;

public enum GameState { Boot, MainMenu, WorldSelect, LevelSelect, CharacterSelect, Playing, Paused, Complete, GameOver, Shop, Missions, DailyReward, Settings }

public class GameManager:MonoBehaviour {
    public static GameManager I{get;private set;}
    public GameState State{get;private set;}=GameState.Boot;
    public int CurrentLevel{get;private set;}=1;
    public int CurrentWorld=>LevelCatalog.WorldFor(CurrentLevel);
    public event Action<GameState> OnStateChanged;

    void Awake(){
        if(I!=null&&I!=this){Destroy(gameObject);return;}
        I=this; DontDestroyOnLoad(gameObject); SaveSystem.Load(); CurrentLevel=SaveSystem.Data.level;
    }
    public void SetState(GameState s){
        State=s; Time.timeScale=s==GameState.Playing?1f:0f; OnStateChanged?.Invoke(s);
    }
    public void StartLevel(int level){
        level=Mathf.Clamp(level,1,LevelCatalog.TotalLevels);
        if(!LevelCatalog.IsUnlocked(level)) return;
        CurrentLevel=level; SetState(GameState.Playing);
        TrackManager.I?.BuildLevel(CurrentLevel); PlayerController.I?.BeginRun();
    }
    public void CompleteLevel(int score,int money,int coins){
        if(State!=GameState.Playing)return;
        int i=CurrentLevel-1;
        int stars=score>=800?3:score>=500?2:1;
        SaveSystem.Data.stars[i]=Mathf.Max(SaveSystem.Data.stars[i],stars);
        SaveSystem.Data.bestScores[i]=Mathf.Max(SaveSystem.Data.bestScores[i],score);
        SaveSystem.Data.money+=Mathf.Max(0,money);
        SaveSystem.Data.coins+=Mathf.Max(0,coins);
        SaveSystem.Data.missionLevels++;
        SaveSystem.Data.missions.totalLevels++;
        SaveSystem.Data.missions.totalCoins+=Mathf.Max(0,coins);
        SaveSystem.Data.missions.bestScore=Mathf.Max(SaveSystem.Data.missions.bestScore,score);
        if(CurrentLevel<LevelCatalog.TotalLevels)SaveSystem.Data.level=Mathf.Max(SaveSystem.Data.level,CurrentLevel+1);
        SaveSystem.Save(); SetState(GameState.Complete);
        UIManager.I?.ShowComplete(score,money,coins,stars);
    }
    public void RegisterGate(){SaveSystem.Data.missions.totalGates++;}
    public bool ClaimDailyReward(out int reward)=>DailyRewardSystem.Claim(out reward);
    public void FailLevel(){if(State!=GameState.Playing)return;SetState(GameState.GameOver);UIManager.I?.ShowGameOver();}
    public void Pause()=>SetState(GameState.Paused);
    public void Resume()=>SetState(GameState.Playing);
    public void Retry()=>StartLevel(CurrentLevel);
    public void NextLevel()=>StartLevel(Mathf.Min(LevelCatalog.TotalLevels,CurrentLevel+1));
    void OnApplicationQuit(){Time.timeScale=1f;SaveSystem.Save();}
}
