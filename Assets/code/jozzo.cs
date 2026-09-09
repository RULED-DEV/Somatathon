using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jozzo : MonoBehaviour
{

    public mapguy guy;

    public string options_debug;

    public float player_vel;
    public float player_pos;
    float update_timer;
    float decision_timer;

    public GameObject maur_temp;

    public int mercy_cast_count;
    public bool final_mercy;

    void Start(){
        guy = GameObject.Find("map").GetComponent<mapguy>();
        
        Invoke("set",0.00001f);
    }

    void set(){
        player_pos = guy.getobj("sam").door_pos;
        update_timer = Time.time + 2f;
        decision_timer = Time.time + 25;
    }

    void Update(){
        if(guy.grace){
            if(update_timer < Time.time){
                update_timer = Time.time + 5f;
                player_vel += guy.getobj("sam").door_pos - player_pos;
                player_pos = guy.getobj("sam").door_pos;
                if(player_vel > 40){
                    decision_timer -= (player_vel % 10)/2;
                }
                if(distance_J(guy.getobj("sam")) < 30 && final_mercy && (decision_timer-Time.time) < 5.1f){
                    final_mercy = false;
                    decision_timer += 5;
                }
            }
            if(decision_timer < Time.time){
                if(GameObject.Find("maurice") == null){
                    // remake maurice
                    Instantiate(maur_temp,transform.position,transform.rotation);
                }
                else{
                    if(distance_J(guy.getobj("maurice")) < 100){
                        options_debug = "teleport maurice away";
                        if(guy.getobj("maurice").door_pos > guy.getobj("jozzo").door_pos){
                            reverse(guy.getobj("maurice"));
                        }
                        else{
                            push(guy.getobj("maurice"));
                        }
                    }
                    else{
                        int f = 1;
                        if(guy.getobj("sam").door_pos > guy.getobj("jozzo").door_pos){
                            f = -1;
                        }
                        player_vel = player_vel*f;
                        if((distance_J(guy.getobj("sam")) < 500 && player_vel >= 20 )|| distance_J(guy.getobj("sam")) < 150){
                            if(guy.getobj("sam").door_pos > 600){
                                reverse(guy.getobj("sam"));
                            }
                            else{
                                push(guy.getobj("sam"));
                                flip();
                            }
                            options_debug = "teleport player away";
                        }
                        else{
                            if(distance_P(guy.getobj("hex")) < 100){
                                while(distance_P(guy.getobj("hex")) < 200 || distance_J(guy.getobj("hex")) < 200){
                                    guy.getobj("hex").door_pos = Random.Range(0,999);
                                }
                                options_debug = "teleport hex away";
                            }
                            else{
                                if(distance_P(guy.getobj("maurice")) > 100){
                                    pull(guy.getobj("maurice"));
                                    options_debug = "teleport maurice";
                                }
                                else{
                                    while(distance_P(guy.getobj("hex")) < 200 || distance_J(guy.getobj("hex")) < 200){
                                        guy.getobj("hex").door_pos = Random.Range(0,999);
                                    }
                                    options_debug = "teleport hex away";
                                }
                            }
                        }
                    }
                }
                if(GameObject.Find("maurice face") != null){
                    Destroy(GameObject.Find("maurice face"));
                }
                player_vel = 0;
                Debug.Log(options_debug);
                decision_timer = Time.time + 25;
                final_mercy = true;
                mercy_cast_count = 0;
                // reload map
                guy.map_reload();
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    void reverse(act_obj obj){ // flips map pos
        obj.door_pos -= 1000;
        if(obj.door_pos < 0){
            obj.door_pos = -obj.door_pos;
        }
    }

    void pull(act_obj obj){ // pulls closer to player
        int f = 1;
        if(obj.door_pos > guy.getobj("sam").door_pos){
            f = -1;
        }
        Debug.Log(obj.door_pos);
        obj.door_pos += 100*f;
        Debug.Log(obj.door_pos);
    }

    void push(act_obj obj){ // pushes away from jozzo
        int f = 1;
        if(obj.door_pos > guy.getobj("jozzo").door_pos){
            f = -1;
        }
        obj.door_pos -= 200*f;
    }

    void flip(){ // turns player around
        GameObject.Find("sam").transform.GetChild(0).transform.localEulerAngles += new Vector3(0,180,0);
    }

    public void maur_perish(){
        decision_timer = Time.time + 45f;
    }

    public float distance_J(act_obj obj){
        return Vector2.Distance(new Vector2(guy.getobj("jozzo").door_pos,0),new Vector2(obj.door_pos,0));
    }

    public float distance_P(act_obj obj){
        return Vector2.Distance(new Vector2(guy.getobj("sam").door_pos,0),new Vector2(obj.door_pos,0));
    }

    public void mercy_cast(string Lname,string Rname){
        if(Lname == "Loop"){
            if(mercy_cast_count == 0){
                decision_timer += 8;
            }
            if(mercy_cast_count == 1){
                decision_timer += 5;
            }
            if(mercy_cast_count == 2){
                decision_timer += 3;
            }
            if(mercy_cast_count > 2){
                decision_timer -= mercy_cast_count;
            }
            mercy_cast_count ++ ;
        }
        else if(Lname == "converge"){
            // adds no time
        }
        else if(Rname == "decimate"){
            if(distance_J(guy.getobj("sam")) < 300){
                decision_timer = 10f;
            }
        }
        else{
            if(mercy_cast_count == 0){
                decision_timer += 5;
            }
            if(mercy_cast_count == 1){
                decision_timer += 3;
            }
            if(mercy_cast_count == 2){
                decision_timer += 1;
            }
            if(mercy_cast_count > 2){
                decision_timer -= mercy_cast_count;
            }
            mercy_cast_count ++;
        }
        player_vel = 40;
    }
}
