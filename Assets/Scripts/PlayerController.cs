using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController:MonoBehaviour {
    public static PlayerController I{get;private set;}
    public int Health{get;private set;}=3;
    public int CoinsCollected{get;private set;}
    public int GateBonus{get;private set;}
    public int Score{get;private set;}
    public float Speed{get;private set;}=7.5f;
    public bool ShieldActive{get;private set;}
    private CharacterController cc;
    private float lane,verticalVelocity,slideTimer;
    private Vector2 touchStart;
    private bool running;
    void Awake(){if(I!=null&&I!=this){Destroy(gameObject);return;}I=this;cc=GetComponent<CharacterController>();cc.height=2f;cc.radius=.45f;cc.center=Vector3.up;BuildVisual();}
    void BuildVisual(){var body=GameObject.CreatePrimitive(PrimitiveType.Capsule);body.name="FictionalStriker";body.transform.SetParent(transform,false);body.transform.localPosition=Vector3.up;body.transform.localScale=new Vector3(.55f,1f,.55f);Destroy(body.GetComponent<Collider>());var m=new Material(Shader.Find("Standard"));m.color=new Color(.85f,.05f,.06f);body.GetComponent<Renderer>().material=m;var head=GameObject.CreatePrimitive(PrimitiveType.Sphere);head.transform.SetParent(transform,false);head.transform.localPosition=new Vector3(0,2.15f,0);head.transform.localScale=Vector3.one*.45f;Destroy(head.GetComponent<Collider>());}
    public void BeginRun(){Health=3;CoinsCollected=0;GateBonus=0;Score=0;Speed=7.5f+(GameManager.I.CurrentLevel-1)*.04f;lane=0;verticalVelocity=0;running=true;slideTimer=0;transform.position=new Vector3(0,1,0);cc.enabled=true;}
    void Update(){if(!running||GameManager.I==null||GameManager.I.State!=GameState.Playing)return;ReadInput();float targetX=lane*2.1f;float x=Mathf.Lerp(transform.position.x,targetX,14f*Time.deltaTime);if(cc.isGrounded&&verticalVelocity<0)verticalVelocity=-1;verticalVelocity+=Physics.gravity.y*Time.deltaTime;cc.Move(new Vector3(x-transform.position.x,verticalVelocity*Time.deltaTime,Speed*Time.deltaTime));if(slideTimer>0){slideTimer-=Time.deltaTime;if(slideTimer<=0)cc.height=2f;}if(transform.position.y<-5)Damage(true);Score+=Mathf.RoundToInt(Speed*Time.deltaTime*10);}
    void ReadInput(){if(Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A))ChangeLane(-1);if(Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.D))ChangeLane(1);if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.UpArrow))Jump();if(Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))Slide();if(Input.touchCount==1){var t=Input.GetTouch(0);if(t.phase==TouchPhase.Began)touchStart=t.position;if(t.phase==TouchPhase.Ended){Vector2 d=t.position-touchStart;if(d.magnitude>50){if(Mathf.Abs(d.x)>Mathf.Abs(d.y))ChangeLane(d.x>0?1:-1);else if(d.y>0)Jump();else Slide();}}}}
    void ChangeLane(int d){lane=Mathf.Clamp(lane+d,-1,1);}
    void Jump(){if(cc.isGrounded)verticalVelocity=9f;}
    void Slide(){slideTimer=.8f;cc.height=1f;}
    public void CollectCoin(int v){CoinsCollected+=v;Score+=v*10;AudioManager.I?.PlayCoin();}
    public void ApplyGate(GateType type,int amount){switch(type){case GateType.Add:GateBonus+=amount;Score+=amount;break;case GateType.Subtract:GateBonus-=amount;Score=Mathf.Max(0,Score-amount);break;case GateType.Double:Score*=2;break;case GateType.Health:Health=Mathf.Min(3,Health+1);break;case GateType.Damage:Damage(false);break;case GateType.Shield:ShieldActive=true;break;case GateType.Slow:Speed=Mathf.Max(5,Speed-2);break;}}
    public void Damage(bool fall){if(ShieldActive){ShieldActive=false;return;}Health--;if(Health<=0)GameManager.I.FailLevel();else if(fall)TrackManager.I?.RespawnCheckpoint();}
    public void Finish(){if(!running)return;running=false;GameManager.I.CompleteLevel(Score,Mathf.Max(0,GateBonus),CoinsCollected);}
}
