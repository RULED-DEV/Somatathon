using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public float number;
    public bool hexed;

    public Sprite[] nums;
    public Sprite[] symbols;

    SpriteRenderer[] nums_rend;
    SpriteRenderer[] sym_rend;

    public GameObject hex;
    public Sprite brok_door;
    public Sprite norm;
    public GameObject brok;
    float sav_val;

    mapguy guy;

    public AudioClip[] rattle;

    void Awake(){
        guy = GameObject.Find("map").GetComponent<mapguy>();
        sym_rend = transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        nums_rend = transform.GetChild(1).GetComponentsInChildren<SpriteRenderer>();
        gameObject.GetComponent<AudioSource>().volume = GameObject.Find("metater").GetComponent<metater>().amb_aud * GameObject.Find("metater").GetComponent<metater>().master_aud;
    }

    public void setval(float val){
        number = val;
        if(guy.getobj("hex") != null && guy.getobj("hex").door_pos == number){
            hexed = true;
            hex.SetActive(true);
            nums_rend[0].transform.parent.gameObject.SetActive(false);
            sym_rend[0].transform.parent.gameObject.SetActive(false);
        }
        else{
            hexed = false;
            // convert val into strings and shit
            if(number >= 0 && number <= 999){
                hex.SetActive(false);
                nums_rend[0].transform.parent.gameObject.SetActive(true);
                sym_rend[0].transform.parent.gameObject.SetActive(true);
                int[] list = new int[3];
                string hold = val.ToString();
                for(int i = 0; i < list.Length; i++){
                    list[2-i] = (int)(val % 10);
                    val = val / 10;
                }
                for(int i = 0; i < nums_rend.Length; i++){
                    nums_rend[i].sprite = nums[list[i]];
                }
                foreach(SpriteRenderer spr in sym_rend){
                    spr.sprite = symbols[Random.Range(0,symbols.Length)];
                } 
            }
            else{
                // oob
                nums_rend[0].sprite = null;
                nums_rend[1].sprite = null;
                nums_rend[2].sprite = null;
                sym_rend[0].sprite = null;
                sym_rend[1].sprite = null;
            }
        }
        if(sav_val != number){
            gameObject.GetComponent<SpriteRenderer>().sprite = norm;
            brok.SetActive(false);
        }
        sav_val = number;
    }

    public bool castdowner(GameObject camera){
        if(Vector3.Angle(camera.transform.forward,transform.position-camera.transform.position) < 30 || Vector3.Distance(camera.transform.position,transform.position) < 11){
            Debug.Log("door destroyed");
            if(hex.activeInHierarchy){
                guy.getobj("jozzo").hex = false;
                guy.getobj("hex").door_pos += 200;
                Destroy(guy.getobj("hex"));
                hexed = false;
                setval(number);
                return true;
            }
            else if(number == 783){
                gameObject.GetComponent<BoxCollider>().enabled =false;
                gameObject.GetComponent<SpriteRenderer>().sprite = brok_door;
                nums_rend[0].transform.parent.gameObject.SetActive(false);
                sym_rend[0].transform.parent.gameObject.SetActive(false);
            }
            else{
                Debug.Log("aaaa");
                gameObject.GetComponent<SpriteRenderer>().sprite = null;
                nums_rend[0].transform.parent.gameObject.SetActive(false);
                sym_rend[0].transform.parent.gameObject.SetActive(false);
                brok.SetActive(true);
            }
        }
        return false;
    }

    public void check(){
        if(number == 783){
            guy.player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().cast_down_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().cast_down_text.Length)]);
        }
        else if(hexed){
            guy.player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().cast_down_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().cast_down_text.Length)]);
        }
        else{
            guy.player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().interact_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().interact_text.Length)]);
        }
        gameObject.GetComponent<AudioSource>().PlayOneShot(rattle[Random.Range(0,rattle.Length)]);
    }
}
