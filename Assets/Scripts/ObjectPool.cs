using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPool : MonoBehaviour {
    public static ObjectPool I { get; private set; }
    readonly Dictionary<string,Queue<GameObject>> pools=new();
    void Awake(){if(I!=null&&I!=this){Destroy(gameObject);return;}I=this;DontDestroyOnLoad(gameObject);}
    public GameObject Get(GameObject prefab,Vector3 position,Quaternion rotation) {
        if(prefab==null) return null;
        string key=prefab.name;
        if(!pools.TryGetValue(key,out var q)){q=new Queue<GameObject>();pools[key]=q;}
        while(q.Count>0) {
            var item=q.Dequeue();
            if(item!=null){item.transform.SetPositionAndRotation(position,rotation);item.SetActive(true);return item;}
        }
        var created=Instantiate(prefab,position,rotation);created.name=key;return created;
    }
    public void Release(GameObject item) {
        if(item==null)return;
        item.SetActive(false);
        string key=item.name.Replace("(Clone)","");
        if(!pools.TryGetValue(key,out var q)){q=new Queue<GameObject>();pools[key]=q;}
        q.Enqueue(item);
    }
    public void Clear(){foreach(var pair in pools)while(pair.Value.Count>0){var item=pair.Value.Dequeue();if(item!=null)Destroy(item);}pools.Clear();}
}
