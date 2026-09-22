using UnityEngine;
public class Coin : MonoBehaviour
{
 public int value=5;
 void Update(){transform.Rotate(0,180f*Time.deltaTime,0); transform.position += Vector3.up*Mathf.Sin(Time.time*4f+transform.position.z)*.0015f;}
 void Collect(PlayerController p){p.CollectCoin(value); if(SaveSystem.Data.missions!=null)SaveSystem.Data.missions.totalCoins+=Mathf.Max(0,value); SaveSystem.Save(); Destroy(gameObject);}
 void OnTriggerEnter(Collider other){var p=other.GetComponent<PlayerController>();if(p!=null)Collect(p);}
 void OnCollisionEnter(Collision c){var p=c.collider.GetComponent<PlayerController>();if(p!=null)Collect(p);}
}