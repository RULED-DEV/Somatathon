using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class texter : MonoBehaviour
{
    public List<string> queue = new List<string>();

    public string curr_text;
    int t_pos = 0;
    public TMP_Text txt;

    public GameObject[] symbols;
    public List<GameObject> selected = new List<GameObject>();

    public float wait_const;

    float wait_timer;
    float act_timer;

    float main_fade_const = 0;
    float sub_fade_const = 0;

    float remove_time;
    float add_time;

    public float idle_timer;

    public AudioClip[] noise;

    // y = 1.1-0.8
    // x = -5.8 - 6.5

    void Awake(){
        wait_timer = Time.time + 3;
        idle_timer = Time.time + Random.Range(30,60);
    }

    void Update(){
        if(txt == null){txt = gameObject.GetComponentInChildren<TMP_Text>();}
        if(idle_timer < Time.time){
            idle_timer = Time.time + Random.Range(60*3,60*5);
            gameObject.GetComponent<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().idle_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().idle_text.Length)]);
        }

        if(wait_timer < Time.time){ // this pauses the code
            if(act_timer < Time.time){ // updates text + symbols
                if(curr_text == ""){
                    if(queue.Count > 0){
                        // updates
                        for(int i = 0; i < selected.Count; i++){
                            GameObject g = selected[i];
                            Destroy(g);
                            selected.RemoveAt(0);
                        }

                        txt.text = "";
                        main_fade_const = 1;
                        curr_text = queue[0];
                        queue.Remove(curr_text);
                        int conste = curr_text.Length;
                        act_timer = Time.time + wait_const + conste*0.1f;
                        t_pos = 0;

                        float x = -10f;
                        float y = 0.5f;
                        while(x < 10.4f){
                            GameObject g = Instantiate(symbols[Random.Range(0,symbols.Length)],transform.position,transform.rotation);
                            g.transform.parent = txt.transform;
                            float f = (Random.Range(120,180));
                            f = f/100;
                            g.transform.localScale = new Vector3(f,f,f);
                            g.transform.localPosition = new Vector3(x,y,0);
                            selected.Add(g);
                            x += (Random.Range(200,400))/100;
                            y = (Random.Range(-50,50));
                            y = y / 100;
                        }
                    }
                    else{
                        // doesnt
                        main_fade_const = 0;
                    }
                }
                else{
                    wait_timer = Time.time + wait_const + (txt.text.Length*0.1f);
                    curr_text = "";
                }
            }
            else{
                if(add_time < Time.time && t_pos < curr_text.Length){
                    add_time = Time.time + 0.05f;
                    FindObjectOfType<grumblo_cont>().GetComponent<AudioSource>().PlayOneShot(noise[Random.Range(0,noise.Length)]);
                    string add = "";
                    add += curr_text[t_pos];
                    t_pos ++;
                    if(add == "<"){ 
                        bool check = true;
                        while(check){
                            add += curr_text[t_pos];
                            if(curr_text[t_pos] == '>'){check = false;}
                            t_pos ++;
                            if(t_pos >= curr_text.Length){check = false;}
                        }
                    }
                    txt.text += add;
                          
                }
                bool cont = true;
                for(int i = 0; i < selected.Count; i++){
                    SpriteRenderer spre = selected[i].GetComponent<SpriteRenderer>();
                    Color cole = spre.color;
                    if(cont){
                        cole.a = Mathf.Lerp(cole.a,0.7f,Time.deltaTime*3);
                    }
                    spre.color = cole;
                    if(cole.a < 0.3f){
                        cont = false;
                    }
                }     
            }


            // sets backgrounds transparency
            SpriteRenderer spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            Color col = spr.color;
            col.a = Mathf.Lerp(col.a,main_fade_const,Time.deltaTime*2);
            spr.color = col;
            spr = spr.transform.GetChild(0).GetComponent<SpriteRenderer>();
            spr.color = col;
            txt.color = col;
        }
        else{
            // removes text + symbols
            if(remove_time < Time.time && txt.text != ""){
                remove_time = Time.time + 0.05f;
                FindObjectOfType<grumblo_cont>().GetComponent<AudioSource>().PlayOneShot(noise[Random.Range(0,noise.Length)]);
                t_pos --;
                if(selected.Count > 0){Destroy(selected[0]);selected.RemoveAt(0);}
                if(txt.text[t_pos] == '>'){
                    bool check = true;
                    while(check){
                        if(txt.text[t_pos] == '<'){check = false;}
                        t_pos --;
                        if(t_pos <= 0){check = false;}
                    }
                }
                txt.text = txt.text.Remove(t_pos);
            }
            if(txt.text.Length <= 0){
                wait_timer -= 0.05f;
            }
        }
    }
}