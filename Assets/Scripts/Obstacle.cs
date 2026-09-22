using UnityEngine;
public enum ObstacleType{RotatingBar,Spike,MovingBlock}
public class Obstacle:MonoBehaviour
{
 public ObstacleType type;
 void Update(){if(type==ObstacleType.RotatingBar)transform.Rotate(0,100f*Time.deltaTime,0);if(type==ObstacleType.MovingBlock)transform.position+=Vector3.right*Mathf.Sin(Time.time*2f+transform.position.z)*.03f;}
 void OnCollisionEnter(Collision c){var p=c.collider.GetComponent<PlayerController>();if(p!=null)p.Damage(false);}
}
