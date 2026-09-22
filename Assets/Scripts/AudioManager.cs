using UnityEngine;
public class AudioManager:MonoBehaviour
{
 public static AudioManager I{get;private set;} void Awake(){I=this;DontDestroyOnLoad(gameObject);}
 public void PlayCoin(){} public void PlayGate(bool positive){} public void PlayUI(){}
}
