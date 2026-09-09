using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class page_handler : MonoBehaviour
{

    public SpriteRenderer spr;
    public Animation anim;
    public Sprite[] list;
    int count = 0;
    public string id;

    float Times;

    public AudioClip melt;

        
    public void Awake(){
        // page fade anim
        if(id == "cast"){
            setwords(GameObject.Find("language").GetComponent<lang_pack>().cast_text);
        }
        GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("hand cam").GetComponent<Camera>();
        Times = Time.time + 0.4f;
    }

    public void noise(){FindObjectOfType<grumblo_cont>().aud.PlayOneShot(melt);}

    void Update(){
        if(Times < Time.time){
            SpriteRenderer spre = transform.GetChild(0).GetComponent<SpriteRenderer>();
            Color col = spre.color;
            col.a = Mathf.Lerp(col.a,0.25f,Time.deltaTime*2);
            spre.color = col;

            TMP_Text[] txts = transform.GetComponentsInChildren<TMP_Text>();
            SpriteRenderer[] sprs = transform.GetComponentsInChildren<SpriteRenderer>();
            if(txts.Length > 0){
                foreach(TMP_Text txt in txts){
                    if(txt.transform.parent.gameObject.name == "spec text"){
                        metater m = GameObject.Find("metater").GetComponent<metater>();
                        if(m.best_time > 0){
                            if(txt.gameObject.name != "sam text" && txt.gameObject.name != "sam sig"){
                                col = txt.color;
                                col.a = Mathf.Lerp(col.a,1,Time.deltaTime*2);
                                txt.color = col;
                            }
                            else{
                                if(m.best_time < 60){
                                    col = txt.color;
                                    col.a = Mathf.Lerp(col.a,1,Time.deltaTime*2);
                                    txt.color = col;
                                }
                            }
                        }
                    }
                    else{
                        col = txt.color;
                        col.a = Mathf.Lerp(col.a,1,Time.deltaTime*2);
                        txt.color = col;
                    }
                }

                foreach(SpriteRenderer spr in sprs){
                    if(spr.transform.parent.gameObject.name == "spec text"){
                        metater m = GameObject.Find("metater").GetComponent<metater>();
                        if(m.best_time > 0){
                            if(spr.gameObject.name != "sam"){
                                col = spr.color;
                                col.a = Mathf.Lerp(col.a,0.6f,Time.deltaTime*2);
                                spr.color = col;
                            }
                            else{
                                if(m.best_time < 60){
                                    col = spr.color;
                                    col.a = Mathf.Lerp(col.a,0.6f,Time.deltaTime*2);
                                    spr.color = col;
                                }
                                else{
                                    col = spr.color;
                                    col.a = 0;
                                    spr.color = col;
                                }
                            }
                        }
                        else{
                            col = spr.color;
                            col.a = 0;
                            spr.color = col;
                        }
                    }
                    else{
                        col = spr.color;
                        col.a = Mathf.Lerp(col.a,1f,Time.deltaTime*2);
                        spr.color = col;
                    }
                }
            }
        }
    }

    public void setwords(string[] words){
        TMP_Text[] texts = transform.GetComponentsInChildren<TMP_Text>();
        int pos = 0;
        for(int i = 0; i < texts.Length;i++){
            if(texts[i].gameObject.name != "S" && texts[i].gameObject.name != "T" && texts[i].gameObject.name != "level" && texts[i].gameObject.name != "levellang"){
                texts[i].text = words[i];
            }
            if(texts[i].gameObject.name == "S"){
                texts[i].text = GameObject.Find("metater").GetComponent<metater>().best_spells.ToString();
            }
            if(texts[i].gameObject.name == "T"){
                float t = GameObject.Find("metater").GetComponent<metater>().best_time;
                float tm = t-(t%1);
                string s = "";
                s = s + (((tm/60)-((tm/60)%1)).ToString());
                s = s + "m ";
                s = s + ((tm%60)).ToString();
                s = s + "s ";
                texts[i].text = s;
            }
            if(texts[i].gameObject.name == "level"){
                // consult current level
                metater met = GameObject.Find("metater").GetComponent<metater>();
                lang_pack l = GameObject.Find("language").GetComponent<lang_pack>();
                float dec = 0;
                if(pos == 0){dec = met.master_aud;}// master
                if(pos == 1){dec = met.amb_aud;}// master
                if(pos == 2){dec = met.pl_aud;}// master
                if(pos < 3){
                    texts[i].text = l.options[(int)(dec*4)];
                }
                pos ++ ;
            }
        }
        transform.GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("hand cam").GetComponent<Camera>();
    }

    public void initiate(){ // this needs to initiate an anim
        spr.sortingOrder += 9;
        Times = 99999999999;
        Destroy(transform.GetChild(0).gameObject);
        anim.Play("page_erase");
        TMP_Text[] txts = transform.GetComponentsInChildren<TMP_Text>();
        if(txts.Length > 0){
            foreach(TMP_Text txt in txts){
                if(txt.gameObject.name != "5"){
                   Destroy(txt.gameObject); 
                }
            }
        }
    }

    public void pager(){ // anim should use this
        spr.sprite = list[count];
        count += 1;
    }

    public void erase(){
        Destroy(transform.parent.gameObject);
    }

}
