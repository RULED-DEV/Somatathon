using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lang_button : MonoBehaviour
{

    public GameObject highlight;
    public GameObject lang;
    public GameObject logo;

    bool mouse_over;

    void Awake(){
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 60;
    }

    void Update(){
        if(mouse_over){
            highlight.SetActive(true);
            if(Input.GetMouseButtonDown(0)){
                Instantiate(lang,transform.position,transform.rotation);
                Instantiate(logo,transform.position,transform.rotation);
                Destroy(transform.parent.gameObject);
            }
        }
        else{
            highlight.SetActive(false);
        }
    }

    void OnMouseEnter(){
        mouse_over = true;
    }

    void OnMouseExit(){
        mouse_over= false;
    }

}
