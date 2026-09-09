using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coverer1 : MonoBehaviour
{
    GameObject screen;
    grumblo_cont pl;

    public void anim(float time, GameObject scr){
        transform.localPosition = new Vector3(0,160,0);
        screen = Instantiate(scr,transform.position,transform.rotation);
        screen.transform.parent = transform;
        screen.transform.localPosition = new Vector3(0,0,0.3f);
        Invoke("kill",time);
    }

    void kill(){
        Destroy(screen);
        gameObject.SetActive(false);
    }

    public void kill2(){
        if(GameObject.Find("totem") != null){
            Destroy(GameObject.Find("totem"));
        }
        GameObject mp = GameObject.Find("sam").GetComponent<grumblo_cont>().map;
        Destroy(GameObject.Find("map"));
        Destroy(GameObject.Find("sam"));
        Instantiate(mp);
        Destroy(transform.parent.gameObject);
    }

    public void death(grumblo_cont ple,GameObject scr){
        pl = ple;
        transform.localPosition = new Vector3(0,160,0);
        screen = Instantiate(scr,transform.position,transform.rotation);
        screen.transform.parent = transform;
        screen.transform.localPosition = new Vector3(0,0,0.3f);
        Invoke("kill2",1f);
    }
}
