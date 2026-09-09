using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapguy : MonoBehaviour
{

    public hallscr[] corridoors;

    public List<act_obj> spell_objs = new List<act_obj>();

    public GameObject player;
    public GameObject p;
    public int playerpos;
    public int hallconst = 10; // 10 doors per hall
    public int cleave_val = 1;

    public float[] loopser; // 0 is jozz, 1 is sam, 2 is maur

    maurice maurice;

    public bool grace;

    public GameObject the_end;
    public GameObject blahaj;

    void Awake(){
        maurice = GameObject.Find("maurice").GetComponent<maurice>();
        gameObject.name = "map";
        RenderSettings.fogDensity = 5;
        player = Instantiate(p,transform.position,transform.rotation);
        Debug.Log(player.name);
        player.GetComponent<grumblo_cont>().guy = gameObject.GetComponent<mapguy>();
        GameObject mp = GameObject.Find("map");
        if(mp != null && mp != gameObject){
            Destroy(gameObject);
        }
    }

    void Start(){
        int rel = playerpos - hallconst*cleave_val;
        foreach(hallscr corr in corridoors){
            corr.Awake();
            corr.updatenumber(rel);
            rel += hallconst*cleave_val;
        }
    }

    void Update(){
        if(player == null){player = FindObjectOfType<grumblo_cont>().gameObject;}
        foreach(hallscr corr in corridoors){
            if(Vector3.Distance(corr.transform.position,player.transform.position) > 32*1.5f){
                corr.updatehallorder();
            }
        }        
        if(!grace){
            if(playerpos > 550 || playerpos < 450){grace = true;}
        }
        if(player.transform.position.x < -7.5f || player.transform.position.x > 7.5f){

        }
    }

    public bool checkupd(float inp){
        for(int i = 0; i < 3; i++){
            if(loopser[i] > 0 && Vector2.Distance(new Vector2(inp,0), new Vector2(loopser[i],0)) <= 15){
                return true;
            }
        }
        return false;
    }

    bool checkloop(float inp){
        foreach(float fl in loopser){
            if(fl > 0 && Vector2.Distance(new Vector2(inp,0), new Vector2(fl,0)) <= 100){
                foreach(float f in loopser){
                    if(f > 0 && f != fl && Vector2.Distance(new Vector2(inp,0), new Vector2(fl,0)) <= 100){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void updateplayerpos(){
        hallscr h = null;
        foreach(hallscr ha in corridoors){
            if(h == null || Vector3.Distance(ha.transform.position,player.transform.position) < Vector3.Distance(h.transform.position,player.transform.position)){
                h = ha;
            }
        }
        playerpos = (int)h.pos;
        player.GetComponent<act_obj>().door_pos = playerpos;
    }

    public int cast(string Ltype,string Lname,string Rtype,string Rname){
        act_obj AO1 = null;
        act_obj AO2 = null;
        act_obj p = null;

        int posi = 0;
        bool warn = GameObject.Find("sam").GetComponent<grumblo_cont>().warn;

        bool casts = false;
        
        for(int i = 0 ; i < spell_objs.Count; i++){
            act_obj obj = spell_objs[i];
            if(obj.name == Lname){AO1 = obj;}
            if(obj.name == Rname){AO2 = obj;}
            if(obj.name == "sam"){p = obj;}
        }

        Debug.Log(Ltype);
        Debug.Log(Rtype);

        if(Ltype == "CAST" && Rtype == "CAST"){
            Debug.Log("casted");
        }
        else{
            if(Ltype == "object" && Rtype == "object"){
                casts = true;
                if(AO1.hex || AO2.hex){
                    Debug.Log("hex blocked!!");
                }
                else if(AO1.stable && AO2.stable){
                    act_obj pee = getobj("sam");
                    if(pee.door_pos > 500){
                        pee.door_pos = 1010;
                    }
                    else{
                        pee.door_pos = -10;
                    }
                    Debug.Log("exception break");
                }
                else{
                    if(AO1.stable){
                        AO2.door_pos = AO1.door_pos;
                    }
                    else if(AO2.stable){
                        AO1.door_pos = AO2.door_pos;
                    }
                    else{
                        AO2.door_pos = AO1.door_pos;
                    }
                }
            }
            else{ // object / effect
                casts = true;
                if(Lname == "expulse"){
                    // pushes target 200 doors away from user
                    if(Rname == "jozzo" && getobj("jozzo") != null){
                        // shifts every non-static object
                        int f = -1;
                        act_obj door = getobj("jozzo");
                        act_obj pl = getobj("sam");
                        if(door.door_pos < pl.door_pos){
                            f = 1;
                        }
                        pl.door_pos += 200*f;
                        act_obj tot = getobj("totem");
                        if(tot != null){tot.door_pos += 200*f;}
                        if(getobj("maurice") != null){getobj("maurice").door_pos += 200*f;}
                    }
                    if(Rname == "sam"){
                        // true death
                        if(!warn){
                            posi = 4;
                        }
                        else{
                            warn = false;
                            player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().caution_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().caution_text.Length)]);
                        }
                    }
                    if(Rname == "maurice" && getobj("maurice") != null){
                        int f = 1;
                        act_obj maur = getobj("maurice");
                        act_obj pl = getobj("sam");
                        if(maur.door_pos < pl.door_pos){
                            f = -1;
                        }
                        maur.door_pos += 200 * f;
                    }
                }
                if(Lname == "cleave"){
                    // halves space between target and user
                    if(Rname == "jozzo" && getobj("jozzo") != null){
                        act_obj door = getobj("jozzo");
                        act_obj pl = getobj("sam");

                        pl.door_pos += (door.door_pos-pl.door_pos)/2;
                        pl.door_pos = (pl.door_pos-(pl.door_pos%10));
                    }
                    if(Rname == "sam"){
                        if(cleave_val < 8){
                            cleave_val += cleave_val;
                        }
                    }
                    if(Rname == "maurice" && getobj("maurice") != null){
                        act_obj door = getobj("maurice");
                        act_obj pl = getobj("sam");

                        pl.door_pos += (door.door_pos-pl.door_pos)/2;
                        pl.door_pos = (pl.door_pos-(pl.door_pos%10));
                    }
                }
                if(Lname == "mend"){
                    // doubles space between target and user mends space
                    if(Rname == "jozzo" && getobj("jozzo") != null){
                        act_obj door = getobj("jozzo");
                        act_obj pl = getobj("sam");

                        pl.door_pos -= (door.door_pos-pl.door_pos)/2;
                        pl.door_pos = (pl.door_pos-(pl.door_pos%10));
                    }
                    if(Rname == "sam"){
                        if(cleave_val > 1){
                            cleave_val -= cleave_val/2;
                        }
                    }
                    if(Rname == "maurice" && getobj("maurice") != null){
                        act_obj maur = getobj("maurice");
                        act_obj pl = getobj("sam");

                        pl.door_pos -= (maur.door_pos-pl.door_pos)/2;
                        pl.door_pos = (pl.door_pos-(pl.door_pos%10));
                    }
                }
                if(Lname == "loop"){
                    if(Rname == "jozzo" && getobj("jozzo") != null){
                        if(loopser[0] < 0){
                            loopser[0] = getobj("jozzo").door_pos;
                        }
                        else{
                            bool check = true;
                            for(int ii = 0; ii < loopser.Length; ii++){
                                if(loopser[ii] > 0 && Vector2.Distance(new Vector2(playerpos,0), new Vector2(loopser[ii],0)) < 100){
                                    for(int i = 0; i < loopser.Length; i++){
                                        if(loopser[i] > 0 && loopser[i] != loopser[ii] && Vector2.Distance(new Vector2(playerpos,0), new Vector2(loopser[i],0)) < 100){
                                            loopser[i] += 1;
                                            loopser[ii] += 1;
                                            check = true;
                                        }
                                    }
                                }
                            }
                            if(check){
                                loopser[0] = -10;
                            }
                        }
                    }
                    if(Rname == "sam"){
                        if(loopser[1] < 0){
                            loopser[1] = getobj("sam").door_pos;
                        }
                        else{
                            bool check = true;
                            for(int ii = 0; ii < loopser.Length; ii++){
                                if(loopser[ii] > 0 && Vector2.Distance(new Vector2(playerpos,0), new Vector2(loopser[ii],0)) < 100){
                                    for(int i = 0; i < loopser.Length; i++){
                                        if(loopser[i] > 0 && loopser[i] != loopser[ii] && Vector2.Distance(new Vector2(playerpos,0), new Vector2(loopser[i],0)) < 100){
                                            loopser[i] += 1;
                                            loopser[ii] += 1;
                                            check = true;
                                        }
                                    }
                                }
                            }
                            if(check){
                                loopser[1] = -10;
                            }
                        }
                    }
                    if(Rname == "maurice" && getobj("maurice") != null){
                        if(loopser[2] < 0){
                            loopser[2] = getobj("maurice").door_pos;
                        }
                        else{
                            bool check = true;
                            for(int ii = 0; ii < loopser.Length; ii++){
                                if(loopser[ii] > 0 && Vector2.Distance(new Vector2(playerpos,0), new Vector2(loopser[ii],0)) < 100){
                                    for(int i = 0; i < loopser.Length; i++){
                                        if(loopser[i] > 0 && loopser[i] != loopser[ii] && Vector2.Distance(new Vector2(playerpos,0), new Vector2(loopser[i],0)) < 100){
                                            loopser[i] += 1;
                                            loopser[ii] += 1;
                                            check = true;
                                        }
                                    }
                                }
                            }
                            if(check){
                                loopser[2] = -10;
                            }
                        }
                    }
                }
                if(Lname == "converge"){
                    if(Rname == "jozzo" && getobj("jozzo") != null){
                        act_obj pl = getobj("sam");
                        act_obj target = getobj("jozzo");
                        int f = -1;
                        if(pl.door_pos < target.door_pos){
                            f = 1;
                        }

                        float mag = ((Vector2.Distance(new Vector2(target.door_pos,0), new Vector2(pl.door_pos,0))*f)/1)*4;
                        pl.GetComponent<Rigidbody>().velocity += new Vector3(0,0,mag);
                    }
                    if(Rname == "sam"){
                        // true death
                        if(!warn){
                            posi = 4;
                        }
                        else{
                            warn = false;
                            player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().caution_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().caution_text.Length)]);
                        }
                    }
                    if(Rname == "maurice" && getobj("maurice") != null){
                        act_obj pl = getobj("sam");
                        act_obj target = getobj("maurice");
                        int f = -1;
                        if(pl.door_pos < target.door_pos){
                            f = 1;
                        }

                        float mag = ((Vector2.Distance(new Vector2(target.door_pos,0), new Vector2(pl.door_pos,0))*f)/1)*4;
                        pl.GetComponent<Rigidbody>().velocity += new Vector3(0,0,mag);
                    }
                }
                if(Rname == "impulse"){
                    // pulls user 100 doors toward target
                    if(Lname == "low door"){
                        int f = 1;
                        act_obj door = getobj("low door");
                        act_obj pl = getobj("sam");
                        if(door.door_pos < pl.door_pos){
                            f = -1;
                        }

                        pl.door_pos += 100*f;
                    }
                    if(Lname == "totem"){
                        int f = 1;
                        act_obj tot = getobj("totem");
                        if(tot != null){
                            act_obj pl = getobj("sam");
                            if(tot.door_pos < pl.door_pos){
                                f = -1;
                            }
                            if(tot != pl){
                                pl.door_pos += 100*f;
                            }
                        }
                    }
                }
                if(Rname == "reverse"){
                    // flips door order
                    if(Lname == "low door"){
                        if(getobj("maurice") != null){
                            act_obj maur = getobj("maurice");
                            maur.door_pos -= 1000;
                            if(maur.door_pos < 0){
                                maur.door_pos = -maur.door_pos;
                            }
                        }
                    }
                    if(Lname == "totem"){
                        act_obj tot = getobj("totem");
                        if(tot != null){
                            tot.door_pos -= 1000;
                            if(tot.door_pos < 0){
                                tot.door_pos = -tot.door_pos;
                                tot.GetComponent<totem_handle>().pos = tot.door_pos;
                            }
                        }
                    }
                }
                if(Rname == "decimate"){
                    if(Lname == "low door"){
                        if(GameObject.Find("vaneernt") != null){
                            act_obj door = getobj("lowdoor");
                            act_obj maur = getobj("maurice");                        
                            act_obj pl = getobj("sam");                        
                            act_obj tot = getobj("totem");

                            if(maur != null){maur.door_pos -= 300;}
                            pl.door_pos -= 300;

                            if(tot != null){tot.door_pos -= 300;if(tot.door_pos <= 0){tot.door_pos = 0;}}
                            if(pl.door_pos <= 0){pl.door_pos = 0;}
                            

                            if(Vector2.Distance(new Vector2(playerpos,0),new Vector2(door.door_pos,0)) <= 100){
                                pocket_realm(door);
                            }
                        }
                        else{
                            if(!warn){
                                posi = 4;
                            }
                            else{
                                warn = false;
                                player.GetComponentInChildren<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().caution_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().caution_text.Length)]);
                            }
                        }
                    }
                    if(Lname == "totem"){
                        act_obj tot = getobj("totem");
                        if(tot != null){
                            if(Vector2.Distance(new Vector2(playerpos,0),new Vector2(tot.door_pos,0)) <= 100){
                                pocket_realm(tot);
                            }
                        }
                    }
                }
            }
        }
        player.GetComponent<grumblo_cont>().warn = warn;
        map_reload();
        grace = true;
        if(Ltype == "CAST" && Rtype == "CAST"){
            if(Rname == "down"){
                GameObject camera = GameObject.Find("Main Camera");
                foreach(hallscr h in corridoors){
                    foreach(door d in h.doors){
                        bool c = d.castdowner(camera);
                        if(c){posi = 3;}
                    }
                }
                if(getobj("maurice") != null){
                    bool c = getobj("maurice").GetComponent<maurice>().castdowner(camera);
                    if(c){posi = 2;}
                }
            }
            if(Rname == "blahaj"){
                GameObject obj = Instantiate(blahaj,player.transform.position,transform.rotation);
                RaycastHit[] hit = Physics.RaycastAll(GameObject.Find("Main Camera").transform.position,GameObject.Find("cont point (1)").transform.forward,15);
                obj.transform.parent = GameObject.Find("Main Camera").transform;
                obj.transform.localPosition = new Vector3(0,0,2);
                obj.transform.parent = null;
                obj.transform.position = new Vector3(obj.transform.position.x,0,obj.transform.position.z);
                if(hit.Length > 0 && hit[0].distance < 10){
                    obj.transform.position = hit[0].point;
                    obj.transform.LookAt(GameObject.Find("Main Camera").transform.position);
                    obj.transform.position += obj.transform.forward*3;
                    obj.transform.position = new Vector3(obj.transform.position.x,0,obj.transform.position.z);
                }
            }
        }
        return posi;
    }

    public void pocket_realm(act_obj obj){
        act_obj pl = getobj("sam");
        
        if(obj.door_pos < 500){
            if(!obj.stable){
                obj.door_pos = 0;
            }
            pl.door_pos = 0;
        }
        else{
            if(!obj.stable){
                obj.door_pos = 990;
            }
            pl.door_pos = 990;
        }

        foreach(hallscr obje in corridoors){
            obje.transform.GetChild(0).gameObject.SetActive(false);
            obje.transform.GetChild(1).gameObject.SetActive(true);
        }
        player.GetComponent<grumblo_cont>().fogconst = 0.085f*3;
        Invoke("exit",9);
    }

    void exit(){
        foreach(hallscr obj in corridoors){
            obj.transform.GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(1).gameObject.SetActive(false);
        }
        player.GetComponent<grumblo_cont>().cover(0.3f,player.GetComponent<grumblo_cont>().screens[0]);
        player.GetComponent<grumblo_cont>().fogconst = 0.085f;
    }

    public act_obj getobj(string name){
        for(int i = 0 ; i < spell_objs.Count; i++){
            if(spell_objs[i] != null && spell_objs[i].name == name){return spell_objs[i];}
        }
        return null;
    }

    public void map_reload(){
        playerpos = GameObject.Find("sam").GetComponent<act_obj>().door_pos;
        // update door vals
        hallscr[] h = new hallscr[3];
        h[0] = corridoors[0];
        h[1] = corridoors[1];
        h[2] = corridoors[2];
        for(int e = 0; e < h.Length; e++){
            for(int i = 0; i < h.Length-1; i++){
                if(h[i].pos > h[i+1].pos){
                    hallscr hold = h[i];
                    h[i] = h[i+1];
                    h[i+1] = hold;
                }
            }
        }
        int count = (playerpos-(playerpos%10)) - hallconst*cleave_val;
        foreach(hallscr s in h){
            s.updatenumber(count);
            count += hallconst*cleave_val;
        }
        Destroy(GameObject.Find("maurice face"));
        
        act_obj tot = getobj("totem");

        if(tot != null && Vector2.Distance(new Vector2(tot.door_pos,0),new Vector2(playerpos,0)) > 30){
            tot.transform.position += new Vector3(0,20,0);
        }
    }

    public void deathers(){
        Instantiate(the_end,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
