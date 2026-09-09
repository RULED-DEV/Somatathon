using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class totem_handle : MonoBehaviour
{
    public bool done;
    public float pos;
    public Vector3 localsave;

    void Awake(){
        GameObject hall = null;
        for(int i = 0; i < 3; i++){
            GameObject hold = GameObject.Find("hall " + (i+1));
            if(hall == null || Vector3.Distance(transform.position,hold.transform.position) < Vector3.Distance(transform.position,hall.transform.position)){
                hall = hold; 
            }
        }
        transform.parent = hall.transform;
        localsave = transform.localPosition;
        transform.parent = null;

        pos = hall.GetComponent<hallscr>().pos;

        gameObject.name = "totem";
        gameObject.GetComponent<act_obj>().door_pos = GameObject.Find("map").GetComponent<mapguy>().playerpos;
    }

    public void activate(){
        gameObject.GetComponent<Animation>().Play("totem activate");
    }

    public void setdone(){
        done = true;
    }

    public void castdowner(GameObject camera){
        if(Vector3.Angle(camera.transform.forward,camera.transform.position-transform.position) < 15){
            Debug.Log("dont do that");
        }
    }
}
