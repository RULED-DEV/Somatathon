using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end : MonoBehaviour
{

    GameObject player;
    public float dist = 28;

    public Animation anim_main;
    public Animation anim_R;
    public Animation anim_L;
    public string[] Lhand;

    float clap_timer;
    float clap_const = 10;
    int animer = 0;

    float wait;

    void Awake(){
        player = GameObject.Find("cont point (1)");
        gameObject.name = "end";
        transform.position = player.transform.forward*(dist+20);
        transform.position = new Vector3(transform.position.x,0,transform.position.z);
        GameObject.Find("metater").GetComponent<metater>().Update_aud();
    }

    void Update(){
        if(wait < Time.time){
            wait = Time.time + Random.Range(10,20);
            GameObject.Find("sam").GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().end_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().end_text.Length)]);
        }
        if(clap_timer < Time.time){
            animser();
        }
        transform.position = player.transform.position+(player.transform.forward*dist);
        transform.position = new Vector3(transform.position.x,0,transform.position.z);

        transform.LookAt(player.transform.position);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);

        if(Input.GetKey(KeyCode.W)){
            dist = Mathf.Lerp(dist,5,Time.deltaTime/2.5f);
        }
    }

    public void animser(){
        if(animer == 0){
            anim_main.Play("jozz clap");
            clap_timer = 10 + Time.time;
            animer = 3;
        }
        else{
            animer --;
            anim_L.Play(Lhand[Random.Range(0,Lhand.Length)]);
            anim_R.Play(Lhand[Random.Range(0,Lhand.Length)]);
            clap_timer = 1 + Time.time;
        }
    }

    public void noise(){
        gameObject.GetComponent<AudioSource>().Play();
    }
}
