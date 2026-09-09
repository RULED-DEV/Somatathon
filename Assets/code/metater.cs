using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metater : MonoBehaviour{

    public int spells_cast;
    public float curr_time;
    public float time_fixed;

    public int best_spells;
    public float best_time;

    public bool first = true;

    void Awake(){
        gameObject.name = "metater";
        time_fixed = Time.time;
        hold_data data = save_data.LOAD();
        best_spells = data.best_spells;
        best_time = data.best_time;
    }

    public void Update(){
        grumblo_cont gc = FindObjectOfType<grumblo_cont>();
        if(gc != null){

            float mode = 1;
            if(GameObject.Find("maurice") != null){
                mode = FindObjectOfType<maurice>().noise_fact;
            }

            gc.aud.volume = (pl_aud * master_aud)*mode;
            gc.aud_music.volume = (amb_aud * master_aud)*mode*0;

            if(GameObject.Find("end") != null){
                GameObject.Find("end").GetComponent<AudioSource>().volume = (amb_aud*master_aud)*mode*0.25f;
                GameObject.Find("end").transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume = ((amb_aud*master_aud)*mode)*0.25f;
                gc.aud_music.volume = 0;
            }
            if(GameObject.Find("jozzo") != null){
                GameObject.Find("jozzo").GetComponentInChildren<AudioSource>().volume = (amb_aud*master_aud)*mode;
            }
            if(GameObject.Find("face") != null){
                GameObject.Find("face").GetComponent<AudioSource>().volume = (amb_aud*master_aud)*(-(mode-1)*2);
            }
            if(GameObject.Find("cover cam") != null){
                gc.aud_music.volume = 0;
                GameObject.Find("cover cam").GetComponentInChildren<AudioSource>().volume = (pl_aud*master_aud)*mode;
            }
            if(GameObject.Find("map") != null){
                mapguy guy = FindObjectOfType<mapguy>();
                if(guy.getobj("sam") != null){
                    if(guy.getobj("sam").door_pos < 0 || guy.getobj("sam").door_pos > 999){
                        gc.aud_music.volume = 0;
                    }
                }
            }
            if(GameObject.Find("vaneernt") != null){
                gc.aud_music.volume = 0;
            }
            gc.aud_music.volume = 0;
        }
    }

    public void Update_aud(){}

    public void saveshit(){
        curr_time = Time.time;
        if(curr_time-time_fixed < best_time || best_time == -1){best_spells = spells_cast; best_time = curr_time-time_fixed;}
        time_fixed = Time.time;
        curr_time = Time.time;
        Debug.Log(curr_time);
        Debug.Log(time_fixed);
        spells_cast = 0;
        first = false;
        gameObject.GetComponent<DATA>().best_spells = best_spells;
        gameObject.GetComponent<DATA>().best_time = best_time;
        gameObject.GetComponent<DATA>().Save();
    }
    // options

    public float master_aud = 1;
    public float pl_aud = 1;
    public float amb_aud = 1;
}
