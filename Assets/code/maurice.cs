using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maurice : MonoBehaviour
{

    public act_obj doors;
    public mapguy guy;

    public int door_speed;
    public float dsintv;
    public float speed;
    float dstime;

    GameObject front;
    Rigidbody rb;
    public GameObject front_obj;

    public float noise_fact = 1;

    void Awake(){
        gameObject.name = "maurice";
        guy = GameObject.Find("map").GetComponent<mapguy>();
        doors = gameObject.GetComponent<act_obj>();
    }

    void Update(){
        if(guy == null){guy = GameObject.Find("map").GetComponent<mapguy>();}
        if(guy.grace){
            if(front == null){
                noise_fact = 1;
                if(dstime < Time.time){
                    dstime = Time.time + dsintv;
                    int f = 1;
                    if(guy.playerpos < doors.door_pos){f = -1;}
                    doors.door_pos += (door_speed*guy.cleave_val) * f;
                    if(Vector2.Distance(new Vector2(guy.playerpos,0),new Vector2(doors.door_pos,0)) <= 80 && GameObject.Find("vaneernt") == null){
                        front = Instantiate(front_obj,guy.player.transform.position + new Vector3(0,2.2f,(32*8) * -f),transform.rotation);
                        front.name = "maurice face";
                        rb = front.GetComponent<Rigidbody>();
                        if(f < 0){front.transform.eulerAngles = new Vector3(-45,180,0);}
                        else{front.transform.eulerAngles = new Vector3(-45,0,0);}
                        GameObject.Find("metater").GetComponent<metater>().Update_aud();
                        if(guy.player.GetComponent<grumblo_cont>().first){
                            guy.player.GetComponent<grumblo_cont>().first = false;
                            guy.player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().maurice_text[0]);
                        }
                        else{
                            if(Random.Range(0,15) == 7){
                                guy.player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().maurice_text[Random.Range(1,GameObject.Find("language").GetComponent<lang_pack>().maurice_text.Length)]);
                            }
                        }
                    }
                }
            }
            else{
                float V = speed*guy.cleave_val;
                int f = 1;
                if(guy.player.transform.position.z < front.transform.position.z){f = -1;}
                if(guy.player.GetComponent<Rigidbody>().velocity.z < 0 && f == -1 || guy.player.GetComponent<Rigidbody>().velocity.z > 0 && f == 1 ){
                    if(guy.player.GetComponent<Rigidbody>().velocity.z < 0){
                        V -= guy.player.GetComponent<Rigidbody>().velocity.z;
                    }
                    else{
                        V += guy.player.GetComponent<Rigidbody>().velocity.z;
                    }
                }
                V = V * f;
                if(guy.player.GetComponent<grumblo_cont>().move){
                    V = V*2;
                }
                rb.velocity = new Vector3(0,0,V);

                // shake calc
                grumblo_cont pl = guy.player.GetComponent<grumblo_cont>();
                float ang = -((Vector3.Angle((front.transform.position-pl.transform.position),pl.cam.transform.forward)/180)-1);
                float dist = -((Vector3.Distance(front.transform.position,pl.transform.position)/(32*8))-1);
                pl.shake = 18*((dist*0.2f)+(dist*ang));

                RenderSettings.fogDensity = guy.player.GetComponent<grumblo_cont>().fogconst+(dist * 0.2f);
                if(RenderSettings.fogDensity < guy.player.GetComponent<grumblo_cont>().fogconst){RenderSettings.fogDensity = guy.player.GetComponent<grumblo_cont>().fogconst;}
                if(Vector2.Distance(new Vector2(front.transform.position.z,0),new Vector2(guy.player.transform.position.z,0)) < 1 && !guy.player.GetComponent<grumblo_cont>().death){
                    guy.player.GetComponent<grumblo_cont>().death = true;
                    Destroy(front);
                    doors.door_pos = 100;
                }

                float diste = Vector2.Distance(new Vector2(front.transform.position.z,0),new Vector2(guy.player.transform.position.z,0));
                diste = diste/(32*8);
                noise_fact = diste;
            }
        }
    }

    public bool castdowner(GameObject camera){
        if(front != null){
            if(Vector3.Angle(camera.transform.forward,front.transform.position-camera.transform.position) < 45){
                Debug.Log("maurice destroyed");
                GameObject.Find("jozzo").GetComponent<jozzo>().maur_perish();
                Invoke("death",0.01f);
                return true;
            }
        }
        return false;
    }

    void death(){Destroy(gameObject);}
}