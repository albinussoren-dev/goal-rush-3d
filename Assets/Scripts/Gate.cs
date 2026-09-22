using UnityEngine;
public enum GateType{Add,Subtract,Double,Health,Damage,Shield,Slow}
public class Gate:MonoBehaviour
{
 public GateType type; public int amount; private bool used;
 void OnTriggerEnter(Collider other){
   if(used)return; var p=other.GetComponent<PlayerController>(); if(p==null)return;
   used=true; p.ApplyGate(type,amount); GameManager.I?.RegisterGate();
   if(AudioManager.I!=null)AudioManager.I.PlayGate(type==GateType.Add); Destroy(gameObject);
 }
}