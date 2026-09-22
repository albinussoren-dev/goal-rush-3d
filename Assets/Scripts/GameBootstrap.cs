using UnityEngine;

public class GameBootstrap:MonoBehaviour {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AutoBoot(){if(FindFirstObjectByType<GameBootstrap>()==null)new GameObject("GameBootstrap").AddComponent<GameBootstrap>();}
    void Awake(){
        if(GameManager.I!=null)return;
        new GameObject("GameManager").AddComponent<GameManager>();
        new GameObject("TrackManager").AddComponent<TrackManager>();
        new GameObject("UIManager").AddComponent<UIManager>();
        new GameObject("AudioManager").AddComponent<AudioManager>();
        new GameObject("ObjectPool").AddComponent<ObjectPool>();
        new GameObject("PerformanceManager").AddComponent<PerformanceManager>();
        var p=new GameObject("Player"); p.AddComponent<CharacterController>(); p.AddComponent<PlayerController>();
        var cam=new GameObject("Main Camera"); cam.AddComponent<Camera>(); var cc=cam.AddComponent<CameraController>(); cc.target=p.transform; cam.tag="MainCamera"; DontDestroyOnLoad(cam);
        GameSettings.Apply();
    }
    void Start(){GameManager.I.SetState(GameState.MainMenu);}
}
