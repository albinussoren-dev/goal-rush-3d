using UnityEngine;
public class Coin : MonoBehaviour
{
 public int value=5;
 void Update(){transform.Rotate(0,180f*Time.deltaTime,0); transform.position += Vector3.up*Mathf.Sin(Time.time*4f+transform.position.z)*.0015f;}
 void OnTriggerEnter(Collider other){if(other.GetComponent<PlayerController>()!=null){other.GetComponent<PlayerController>().CollectCoin(value);Destroy(gameObject);}}
 void OnCollisionEnter(Collision c){var p=c.collider.GetComponent<PlayerController>();if(p!=null){p.CollectCoin(value);Destroy(gameObject);}}
}
