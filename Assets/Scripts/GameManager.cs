using System;
using UnityEngine;

public enum GameState { Boot, MainMenu, WorldSelect, LevelSelect, CharacterSelect, Playing, Paused, Complete, GameOver, Shop, Missions, DailyReward, Settings }

public class GameManager:MonoBehaviour {
    public static GameManager I{get;private set;}
    public GameState State{get;private set;}=GameState.Boot;
    public int CurrentLevel{get;private set;}=1;
    public int CurrentWorld=>(CurrentLevel-1)/20+1;
    public event Action<GameState> OnStateChanged;
    void Awake(){if(I!=null&&I!=this){Destroy(gameObject);return;}I=this;DontDestroyOnLoad(gameObject);SaveSystem.Load();CurrentLevel=SaveSystem.Data.level;}
    public void SetState(GameState s){State=s;Time.timeScale=s==GameState.Playing?1f:0f;OnStateChanged?.Invoke(s);}
    public void StartLevel(int level){CurrentLevel=Mathf.Clamp(level,1,100);SetState(GameState.Playing);TrackManager.I?.BuildLevel(CurrentLevel);PlayerController.I?.BeginRun();}
    public void CompleteLevel(int score,int money,int coins){if(State!=GameState.Playing)return;int i=CurrentLevel-1;int stars=score>=800?3:score>=500?2:1;SaveSystem.Data.stars[i]=Mathf.Max(SaveSystem.Data.stars[i],stars);SaveSystem.Data.bestScores[i]=Mathf.Max(SaveSystem.Data.bestScores[i],score);SaveSystem.Data.money+=Mathf.Max(0,money);SaveSystem.Data.coins+=Mathf.Max(0,coins);SaveSystem.Data.missionLevels++;if(CurrentLevel<100)SaveSystem.Data.level=Mathf.Max(SaveSystem.Data.level,CurrentLevel+1);SaveSystem.Save();SetState(GameState.Complete);UIManager.I?.ShowComplete(score,money,coins,stars);}
    public void FailLevel(){if(State!=GameState.Playing)return;SetState(GameState.GameOver);UIManager.I?.ShowGameOver();}
    public void Pause()=>SetState(GameState.Paused);
    public void Resume()=>SetState(GameState.Playing);
    public void Retry()=>StartLevel(CurrentLevel);
    public void NextLevel()=>StartLevel(Mathf.Min(100,CurrentLevel+1));
}
