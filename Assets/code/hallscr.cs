using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hallscr : MonoBehaviour
{
    mapguy guy;
    public door[] doors;

    public float pos;
    public Vector2 next_pos;

    public bool upd = true;

    public void Awake(){
        guy = transform.parent.gameObject.GetComponent<mapguy>();
        doors = transform.GetComponentsInChildren<door>();
    }

    public void updatehallorder(){
        // updates the halls position in the world
        if(upd || (pos > 0 && pos < 1000)){
            int mod = 0;
            if(guy.player.transform.position.z > transform.position.z){
                // player in front
                mod = 1;
            }
            else{
                mod = -1; // player is behind
            }
            transform.position += new Vector3(0,0,(32*3)*mod); // jumps to end
            hallscr h = null;
            foreach(hallscr ha in guy.corridoors){
                if((ha.gameObject != gameObject && h == null) || (ha.gameObject != gameObject && Vector3.Distance(ha.transform.position,transform.position) < Vector3.Distance(h.transform.position,transform.position))){
                    h = ha;
                }
            }

            if(!guy.checkupd(pos)){
                if(mod < 0){
                    updatenumber(h.next_pos[0]);
                }
                else{
                    updatenumber(h.next_pos[1]);
                }
            }
            
            bridger();
        }
        guy.updateplayerpos();
    }

    public void bridger(){
        if(guy.checkupd(pos)){
            List<float> hold = new List<float> {};
            foreach(float fl in guy.loopser){
                if(fl % 10 == 1){
                    hold.Add(fl);
                }
            }
            foreach(float fl in hold){
                if(fl > 0 && Vector2.Distance(new Vector2(pos,0), new Vector2(fl,0)) <= 15){
                    for(int i = 0; i < guy.loopser.Length; i++){
                        if(Vector2.Distance(new Vector2(fl,0),new Vector2(guy.loopser[i],0)) <= 200){
                            if(fl < guy.loopser[i]){
                                next_pos[1] = guy.loopser[i] - 11;
                            }
                            else{
                                next_pos[0] = guy.loopser[i] + 9;
                                
                            }
                            guy.loopser[i] = -10;
                        }
                    }
                }
                
            }
        }
    }

    public void updatenumber(float num){
        pos = num;
        if((pos < -20 || pos >= 1020)){
            if(GameObject.Find("maurice") == null){
                upd = false;
            }
        }
        else{
            upd = true;
            next_pos[0] = pos - guy.hallconst * guy.cleave_val;
            next_pos[1] = pos + guy.hallconst * guy.cleave_val;
        }
        if(guy != null){
            for(int i = 0; i < guy.hallconst;i++){
                doors[i].setval(pos+(i*guy.cleave_val));
                if(guy.getobj("hex") != null){
                    if(guy.getobj("hex").door_pos == pos+(i*guy.cleave_val)){
                        doors[i].hexed = true;
                    }
                }
                else{
                    doors[i].hexed = false;
                }
            }
        }
        GameObject tot = GameObject.Find("totem");
        if(tot != null){
            if(tot.GetComponent<act_obj>().door_pos == pos){
                tot.transform.parent = transform;
                tot.transform.localPosition = tot.GetComponent<totem_handle>().localsave;
                tot.transform.parent = null;
            }
        }
    }
}
