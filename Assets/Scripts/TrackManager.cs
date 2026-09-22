using System.Collections.Generic;
using UnityEngine;

public class TrackManager:MonoBehaviour {
    public static TrackManager I{get;private set;}
    readonly List<GameObject> spawned=new();
    Transform playerRoot;
    void Awake(){I=this;DontDestroyOnLoad(gameObject);}
    public void BuildLevel(int level){Clear();int length=80+level*2;CreateTrack(length);for(int z=12;z<length-10;z+=8){CreateCoinLine(z,(z/8)%3-1);if(z%16==0)CreateGatePair(z+3);else if(z%24==0)CreateSpikes(z+3);else CreateRotator(z+3);}CreateFinish(length);if(PlayerController.I!=null)playerRoot=PlayerController.I.transform;}
    void CreateTrack(int length){var r=GameObject.CreatePrimitive(PrimitiveType.Cube);r.name="Track";r.transform.position=new Vector3(0,-.2f,length/2f);r.transform.localScale=new Vector3(7,.3f,length);spawned.Add(r);}
    void CreateCoinLine(int z,int l){for(int i=0;i<4;i++){var c=GameObject.CreatePrimitive(PrimitiveType.Sphere);c.name="Coin";c.transform.position=new Vector3(l*2.1f,.8f,z+i*1.4f);c.transform.localScale=Vector3.one*.35f;c.GetComponent<Renderer>().material.color=Color.yellow;c.AddComponent<Coin>();spawned.Add(c);}}
    void CreateRotator(int z){var b=GameObject.CreatePrimitive(PrimitiveType.Cube);b.name="RotatingBar";b.transform.position=new Vector3(0,1.1f,z);b.transform.localScale=new Vector3(6,.35f,.45f);b.GetComponent<Renderer>().material.color=Color.red;b.AddComponent<Obstacle>().type=ObstacleType.RotatingBar;spawned.Add(b);}
    void CreateSpikes(int z){for(int i=-1;i<=1;i++){var s=GameObject.CreatePrimitive(PrimitiveType.Cylinder);s.transform.position=new Vector3(i*2.1f,.55f,z);s.transform.localScale=new Vector3(.6f,1.1f,.6f);s.GetComponent<Renderer>().material.color=Color.red;s.AddComponent<Obstacle>().type=ObstacleType.Spike;spawned.Add(s);}}
    void CreateGatePair(int z){CreateGate(z,-1,GateType.Add,100,Color.green);CreateGate(z,1,GateType.Subtract,50,Color.red);}
    void CreateGate(int z,int l,GateType t,int amount,Color color){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.transform.position=new Vector3(l*2.1f,1.3f,z);g.transform.localScale=new Vector3(1.7f,2.6f,.4f);g.GetComponent<Renderer>().material.color=color;g.GetComponent<BoxCollider>().isTrigger=true;var gate=g.AddComponent<Gate>();gate.type=t;gate.amount=amount;spawned.Add(g);}
    void CreateFinish(int z){var f=GameObject.CreatePrimitive(PrimitiveType.Cube);f.transform.position=new Vector3(0,1.5f,z);f.transform.localScale=new Vector3(7,3,.5f);f.GetComponent<Renderer>().material.color=Color.yellow;f.GetComponent<BoxCollider>().isTrigger=true;f.AddComponent<FinishTrigger>();spawned.Add(f);}
    public void RespawnCheckpoint(){if(playerRoot!=null)playerRoot.position=new Vector3(playerRoot.position.x,.9f,Mathf.Max(1,playerRoot.position.z-5));}
    public void Clear(){foreach(var g in spawned)if(g)Destroy(g);spawned.Clear();}
}
