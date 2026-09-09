using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class book_handler : MonoBehaviour
{
    // should handle swap between pages, pages and pulling words for pages

    public GameObject[] pages;

    public int page;

    public bool active = false;
    int ind;

    grumblo_cont pl;

    public bool check;

    void Awake(){
        pl = GameObject.Find("sam").GetComponent<grumblo_cont>();

        ind = 221-7 + 46;
    }

    void Update(){
        if(active){
            check = false;
            if(Input.GetMouseButtonDown(1)){
                if(page >= pages.Length-1){
                    // articulaet figor
                    pl.articulate(3);
                }
                else{
                    check = true;
                    page += 1;
                }
            }
            if(Input.GetMouseButtonDown(0)){
                if(page <= 0){
                    // articulaet figor
                    pl.articulate(3);
                }
                else{
                    check = true;
                    page -= 1;
                }
            }
            if(GameObject.Find("Text (TMP) (1)") != null && GameObject.Find("Text (TMP) (1)").GetComponent<TMP_Text>().text == ""){
                check = true;
            }
            if(check){
                Transform obj = GameObject.Find("Left hand").transform;
                obj.GetChild(1).GetChild(0).GetComponent<page_handler>().initiate();
                obj.GetChild(1).GetChild(1).GetComponent<page_handler>().initiate();
                GameObject gam = Instantiate(pages[page],obj.transform.position,transform.rotation);
                gam.transform.parent = obj;
                gam.transform.localScale = new Vector3(1f,0.9f,0.9f);
                gam.transform.localEulerAngles = Vector3.zero;
                gam.transform.localPosition = new Vector3(0.317f,1.188f,-0.1f);

                lang_pack langer = GameObject.Find("language").GetComponent<lang_pack>();
                if(page == 0){
                    gam.transform.GetChild(0).GetComponent<page_handler>().setwords(langer.lore_text);
                }
                if(page == 1){
                    gam.transform.GetChild(0).GetComponent<page_handler>().setwords(langer.info_text);
                }
                if(page == 2){
                    gam.transform.GetChild(0).GetComponent<page_handler>().setwords(langer.Rcast_text);
                }
                if(page == 3){
                    gam.transform.GetChild(0).GetComponent<page_handler>().setwords(langer.cast_text);
                }
                if(page == 4){
                    gam.transform.GetChild(0).GetComponent<page_handler>().setwords(langer.Lcast_text);
                }
                if(page == 5){
                    gam.transform.GetChild(0).GetComponent<page_handler>().setwords(langer.up_info_text);
                }
                if(page == 6){
                    gam.transform.GetChild(0).GetComponent<page_handler>().setwords(langer.menu_text);
                }
                GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("hand cam").GetComponent<Camera>();
            }
            if(page == 1){
                string txt = "";
                GameObject g = GameObject.Find("Text (TMP) (4)");
                if(g != null){
                    txt = g.GetComponent<TMP_Text>().text;
                }
                mapguy guy = GameObject.Find("map").GetComponent<mapguy>();
                if(guy.getobj("hex") != null && txt != ""){
                    txt = txt.Remove(ind);
                    txt = txt.Insert(ind,guy.getobj("hex").door_pos.ToString());
                    GameObject.Find("Text (TMP) (4)").GetComponent<TMP_Text>().text = txt;
                }
                else{
                    GameObject.Find("Text (TMP) (4)").GetComponent<TMP_Text>().text = "<color=#322E21>i</color> have destroyed <color=#022240>jozzo's</color> <color=#1B0035>hex</color>, <color=#022240>door 783</color> is no longer blocked";
                }
            }
        }
    }
}
