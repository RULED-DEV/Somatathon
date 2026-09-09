using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tendrilser : MonoBehaviour
{

    GameObject tendril;
    public GameObject refe;

    Vector3 pos;

    public float dist;
    public float ang;

    float sav;
    float time;

    int count;

    void Update(){
        while(tendril == null && time < Time.time){
            transform.LookAt(GameObject.Find("sam").transform.position);
            transform.localEulerAngles += new Vector3(Random.Range(-ang,ang),Random.Range(-ang,ang),0);
            RaycastHit[] hit = Physics.RaycastAll(transform.position, transform.forward,dist);
            if(hit.Length > 0 && hit[0].collider.gameObject != null){
                pos = hit[0].point;
                tendril = Instantiate(refe,transform.position,transform.rotation);
                tendril.transform.parent = transform;
                time = Time.time + (Random.Range(200,500)/100);
                sav = Time.time;
            }
            count++;
            if(count > 5){
                time = Time.time + 2;
            }
        }
        if(tendril != null){
            tendril.transform.LookAt(pos);
            float val = (Time.time-sav)/(time-sav);
            if(val <= 0.25f){
                val = val/0.25f;
            }
            else if(val <= 0.75f){
                val = 1;
            }
            else{
                val = ((val-0.75f)/0.25f)-1;
                val = -val;
            }
            tendril.transform.localScale = new Vector3(1,1,Vector3.Distance(transform.position,pos)*val);
        }
        if(time < Time.time || Vector3.Distance(pos,transform.position) < 10){
            count = 0;
            Destroy(tendril);
        }
    }
}
