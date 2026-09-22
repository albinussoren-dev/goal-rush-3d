using UnityEngine;
public class FinishTrigger:MonoBehaviour{void OnTriggerEnter(Collider c){var p=c.GetComponent<PlayerController>();if(p!=null)p.Finish();}}
