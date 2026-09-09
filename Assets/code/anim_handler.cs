using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_handler : MonoBehaviour
{
    public grumblo_cont cont;

    public void unfinger(){transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);}
    public void refinger(){transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);}
    public void unfingerR(){transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);}
    public void refingerR(){transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);}
    public void book_swap_for(){transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = cont.finger_states[cont.finger_states.Length-3];}
    public void book_swap_side(){transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = cont.finger_states[cont.finger_states.Length-2];}
    public void book_swap_open(){transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = cont.finger_states[cont.finger_states.Length-1];}
    public void book_swap_hand(){transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = cont.finger_states[cont.finger_states.Length-4];}
    public void totem_swap(){transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = cont.finger_states[cont.finger_states.Length-5];}
    public void totem_swap_hand(){transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = cont.finger_states[cont.finger_states.Length-6];}

    public void add_pages(){
        book_handler p = GameObject.Find("sam").GetComponent<book_handler>();
        GameObject gam = Instantiate(p.pages[p.page],transform.position,transform.rotation);
        gam.transform.parent = transform.GetChild(1);
        gam.transform.localScale = new Vector3(1f,0.9f,0.9f);
        gam.transform.localEulerAngles = Vector3.zero;
        gam.transform.localPosition = new Vector3(0.317f,1.188f,-0.1f);
    }

    public void play_open(){cont.aud.PlayOneShot(cont.fx_noise[2]);}
    public void play_close(){cont.aud.PlayOneShot(cont.fx_noise[1]);}
    public void play_rustle(){cont.aud.PlayOneShot(cont.fx_noise[0]);}
    public void play_place(){cont.aud.PlayOneShot(cont.fx_noise[3]);}
    public void play_pickup(){cont.aud.PlayOneShot(cont.fx_noise[4]);}
    public void play_page(){cont.aud.PlayOneShot(cont.fx_noise[5]);}
    
    public void remove_pages(){
        Destroy(transform.GetChild(1).GetChild(1).gameObject);
    }

    public void menu_anim_handler_draw(){
        cont.menu_anim_handler_draw();
    }

    public void totem_anim_finish(){
        cont.totem_anim_finish();
    }

    public void totem_anim_place_obj(){
        cont.totem_anim_place_obj();
    }

    public void totem_anim_check(){
        cont.pull_totem = true;
    }

    public void finish_totem_pull(){
        cont.finish_totem_pull();
    }

    public void finish_book_clap(){
        cont.finish_book_clap();
    }

    public void call_menu_clap(){
        cont.call_menu_clap();
    }

    public void menu_anim_handler_stow(){
        cont.menu_anim_handler_stow();
    }

    public void flashclaps(){
        cont.flashclaps();
    }

    public void unflashclaps(){
        cont.unflashclaps();
    }

    public void figor_anim_frame(){
        cont.figor_anim_frame();
    }

    public void resetfingies(){
        cont.resetfingies();
    }

    public void figorswap(){
        cont.figorswap();
    }

    public void clap_shit(){
        cont.clap_shit();
    }
}
