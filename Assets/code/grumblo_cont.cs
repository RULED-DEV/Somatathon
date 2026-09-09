using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class grumblo_cont : MonoBehaviour
{ 
    public float sensitivity;
    float rot_x;
    float rot_Y;

    public GameObject map;
    public bool death;
    public bool active_ch;

    public bool active;
    public float max_velocity;
    public float acceleration_speed;
    public float cam_speed;
    public bool warn = true;

    public AudioSource aud;
    public AudioSource aud_music;
    public GameObject cam;
    public GameObject cam_analogue;
    public GameObject hand_cam;
    public coverer1 cam_cover;
    public GameObject cont_point;
    public GameObject[] arms;
    public Sprite[] arms_spr;
    Rigidbody RB;
    Vector2 wobble_vect;
    Vector3 cam_origin;
    
    int figor_pos_L;
    int figor_pos_R;
    public SpriteRenderer[] finger_spr;
    public Sprite[] finger_states;

    public AudioClip[] finger_snap;
    public AudioClip[] footsteps;
    public AudioClip[] fx_noise;
    public AudioClip clap_fx;
    public AudioClip figor_melt_fx;
    public AudioClip cast;

    public GameObject[] screens;
    public GameObject vignette;
    Camera Main_C;

    public bool move;
    bool step;
    public bool play = true;
    float drag_save;

    public Animation clap_Anim;
    
    public bool book;
    public bool check_anim;
    int ch_anim;

    public bool totem;
    public bool pull_totem;
    totem_handle totem_obj;
    public GameObject totem_prefab;

    public mapguy guy;

    public float shake;

    public float fogconst = 0.085f;

    book_handler menu;
    bool end;
    public bool TE = true;

    public float[] bob_timings;
    public float[] bob_height;
    public int bober;

    bool book_first = true;
    public bool first = true;
    
    void Awake(){
        gameObject.name = "sam";
        RB = gameObject.GetComponent<Rigidbody>();
        drag_save = RB.drag;
        cam_origin = cam.transform.localPosition;
        Main_C = GameObject.Find("Main Camera").GetComponent<Camera>();
        menu = gameObject.GetComponent<book_handler>();
        clap_Anim.Play("pull hands");
        GameObject.Find("metater").GetComponent<metater>().time_fixed = Time.time;
        GameObject.Find("metater").GetComponent<metater>().spells_cast = 0;
        GameObject.Find("metater").GetComponent<metater>().curr_time = 0;
        if(GameObject.Find("metater").GetComponent<metater>().first){
            FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().beginning_text[0]);
            FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().beginning_text[1]);
            FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().beginning_text[2]);
        }
        else{
            FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().beginning_text[3]);
            FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().beginning_text[4]);
        }
        GameObject mp = GameObject.Find("sam");
        if(mp != null && mp != gameObject){
            Destroy(mp);
        }
    }

    void Update(){
        if(active_ch){
            if(guy != null && guy.getobj("sam") != null && guy.getobj("sam").door_pos > 990 || guy.getobj("sam").door_pos < 0){
                fogconst = 0.13f;
                if(guy.getobj("sam").door_pos > 990){
                    if(guy.getobj("maurice").door_pos < 990){
                        guy.getobj("maurice").door_pos = 1080;
                    }
                }
                if(guy.getobj("sam").door_pos < 0){
                    if(guy.getobj("maurice").door_pos > 0){
                        guy.getobj("maurice").door_pos = -80;
                    }
                }
                
            }
            else if((transform.position.x < -7.5f || transform.position.x > 7.5f) && GameObject.Find("map") != null){
                if(!end){
                    fogconst = 5f;
                    guy.Invoke("deathers",0.8f);
                    end = true;
                }
            }
            else{
                fogconst = 0.085f;
            }
            if(!end){
                if(GameObject.Find("end")!= null){
                    Destroy(GameObject.Find("end"));
                }
            }
            player_cont();
            cam_control();
            update_val();
            if(Input.GetKeyDown(KeyCode.E)){dorime();}
            if(!book){
                if(!check_anim){
                    if(Input.GetKeyDown(KeyCode.Tab)){check_anim = true; book_draw_anim(); ch_anim = 0; menu.active = true;}
                    if(Input.GetKeyDown(KeyCode.Space)){check_anim = true; clap_anim();ch_anim = 0;}
                }
                fingers();
            }
            else{
                if(!check_anim){ 
                    if(Input.GetKeyDown(KeyCode.Tab)){check_anim = true; book_stow_anim();ch_anim = 0; menu.active = false;}
                    if(Input.GetKeyDown(KeyCode.Space)){check_anim = true; menu_clap_anim();ch_anim = 0;}
                }
            }
            if(!check_anim && !end){ // totem
                if(!totem){
                    if(Input.GetKeyDown(KeyCode.R)){check_anim = true; totem_anim_place(); ch_anim = 0;}
                }
                else{
                    if(Input.GetKeyDown(KeyCode.R)){check_anim = true; totem_anim_grab(); ch_anim = 0;}
                }
            }
            else{
                if(totem && !end){
                    if((totem_obj == null || totem_obj.done) && pull_totem){
                        pull_totem_anim();
                    }
                }
            }
            UI_shit();
        }
        if(death && GameObject.Find("death screen(Clone)") == null){
            active_ch = false;
            Main_C.gameObject.SetActive(false);
            hand_cam.SetActive(false);
            cam_cover.gameObject.SetActive(true);
            cam_cover.death(gameObject.GetComponent<grumblo_cont>(),screens[1]);
        }
    }
    
    void UI_shit(){ // vignette kinda syucks
        float x_perc =  0;
        x_perc += rot_x/16;
        float y_perc = 0;
        y_perc = rot_Y/16;

        Vector3 cam_r = cam.transform.eulerAngles;
        cam.transform.eulerAngles = new Vector3(0,cam.transform.eulerAngles.y,0);
        Vector3 Sav_r = cam.transform.forward;

        // float num = 0;
        // float V = RB.velocity.x;
        // if(V < 0){V = -V;}
        // if(RB.velocity.z < 0){V += -RB.velocity.z;}
        // else{V += RB.velocity.z;}

        // if(cam.transform.eulerAngles.y > 315 || cam.transform.eulerAngles.y < 45){
        //     cam.transform.eulerAngles = new Vector3(0,0,0);
        //     num = -((Vector3.Angle(Sav_r,cam.transform.forward) / 45) -1);
        //     y_perc += num*(V/40);
        // }
        // if(cam.transform.eulerAngles.y > 45 && cam.transform.eulerAngles.y < 135){
        //     cam.transform.eulerAngles = new Vector3(0,90,0);
        //     num = -((Vector3.Angle(Sav_r,cam.transform.forward) / 45) -1);
        //     x_perc += num*(V/40);
        // }
        // if(cam.transform.eulerAngles.y > 135 && cam.transform.eulerAngles.y < 225){
        //     cam.transform.eulerAngles = new Vector3(0,180,0);
        //     num = -((Vector3.Angle(Sav_r,cam.transform.forward) / 45) -1);
        //     num = -num;
        //     y_perc += num*(V/40);
        // }
        // if(cam.transform.eulerAngles.y > 225 && cam.transform.eulerAngles.y < 315){
        //     cam.transform.eulerAngles = new Vector3(0,270,0);
        //     num = -((Vector3.Angle(Sav_r,cam.transform.forward) / 45) -1);
        //     num = -num;
        //     x_perc += num*(V/40);
        // }

        cam.transform.eulerAngles = cam_r;

        if(x_perc > 1){x_perc = 1;}
        if(x_perc < -1){x_perc = -1;}
        if(y_perc > 1){y_perc = 1;}
        if(y_perc < -1){y_perc = -1;}
        
        Vector3 sav = new Vector3(0,0,Mathf.LerpAngle(arms[0].transform.localEulerAngles.z,(20*x_perc),Time.deltaTime));
        arms[0].transform.localEulerAngles = sav;
        sav = new Vector3(0,0,Mathf.LerpAngle(arms[1].transform.localEulerAngles.z,(20*x_perc),Time.deltaTime));
        arms[1].transform.localEulerAngles = sav;

        sav = new Vector3(Mathf.Lerp(arms[0].transform.localPosition.x,-1.4f+(0.8f*x_perc),Time.deltaTime),Mathf.Lerp(arms[0].transform.localPosition.y,0.5f*y_perc,Time.deltaTime),0);
        arms[0].transform.localPosition = sav;
        sav = new Vector3(Mathf.Lerp(arms[1].transform.localPosition.x,1.3f+(0.8f*x_perc),Time.deltaTime),Mathf.Lerp(arms[1].transform.localPosition.y,0.5f*y_perc,Time.deltaTime),0);
        arms[1].transform.localPosition = sav;
        
        cam.transform.eulerAngles = cam.transform.eulerAngles + new Vector3(Random.Range(-shake*10,shake*10)/100,Random.Range(-shake*10,shake*10)/100,Random.Range(-shake*10,shake*10)/100);
        shake = Mathf.Lerp(shake,0,Time.deltaTime);
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity,fogconst,Time.deltaTime*2);
    }

    void clap_anim(){
        clap_Anim.Play("clap anim fore");
    }

    void menu_clap_anim(){
        clap_Anim.Play("book clap");
    }

    void book_draw_anim(){
        clap_Anim.Play("draw book");
        if(book_first){
            book_first = false;
            FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().book_text[0]);
        }
        else{
            if(Random.Range(0,10) == 7){
                FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().book_text[Random.Range(1,GameObject.Find("language").GetComponent<lang_pack>().book_text.Length)]);
            }
        }
        FindObjectOfType<texter>().idle_timer = Time.time + Random.Range(30,60);
    }

    void book_stow_anim(){
        clap_Anim.Play("stow book");
        foreach(TMP_Text t in transform.GetComponentsInChildren<TMP_Text>()){
            if(t.gameObject.name != "5"){
                Destroy(t);
            }
        }
    }

    void totem_anim_place(){
        clap_Anim.Play("totem place");
    }

    void totem_anim_grab(){
        clap_Anim.Play("totem grab");
        if(totem_obj != null){totem_obj.activate();}
    }

    void pull_totem_anim(){
        clap_Anim.Play("totem pull");
        if(totem_obj != null && guy != null){guy.spell_objs.Remove(totem_obj.GetComponent<act_obj>()); Destroy(totem_obj.gameObject);}
    }

    public void finish_book_clap(){
        arms[0].GetComponent<SpriteRenderer>().sprite = finger_states[finger_states.Length-1];
        arms[0].transform.localEulerAngles = new Vector3(0,0,-5);
        arms[1].transform.localEulerAngles = new Vector3(0,0,5);
        arms[0].transform.localPosition = new Vector3(-1.4f,0,0);
        arms[1].transform.localPosition = new Vector3(1.3f,0,0);
        ch_anim = 0;
        check_anim = false;
    }

    public void finish_totem_pull(){
        ch_anim = 0;
        check_anim = false;
        pull_totem = false;
        totem = false;
    }

    public void totem_anim_finish(){
        check_anim = false;
        ch_anim = 0;
        totem = true;
    }

    public void totem_anim_place_obj(){
        GameObject obj = Instantiate(totem_prefab,transform.position-cam.transform.forward/4,transform.rotation);
        obj.transform.position = new Vector3(transform.position.x, -2.384186e-07f, transform.position.z);
        totem_obj = obj.GetComponent<totem_handle>();
    }

    public void menu_anim_handler_draw(){
        ch_anim = 0;
        check_anim = false;
        book = true;
        figor_pos_L = 0;
        figor_pos_R = 0;
    }

    public void menu_anim_handler_stow(){
        ch_anim = 0;
        check_anim = false;
        figor_pos_L = 0;
        figor_pos_R = 0;
        book = false;
        gameObject.GetComponent<book_handler>().page = 3;
    }

    public void clap_shit(){
        arms[0].transform.localEulerAngles = new Vector3(0,0,-5);
        arms[1].transform.localEulerAngles = new Vector3(0,0,5);
        arms[0].transform.localPosition = new Vector3(-1.4f,0,0);
        arms[1].transform.localPosition = new Vector3(1.3f,0,0);
        ch_anim = 0;
        check_anim = false;
        figor_pos_L = 0;
        figor_pos_R = 0;
    }

    public void call_menu_clap(){
        // this is where settings are edited
        int Rval = 0;
        if(gameObject.GetComponent<book_handler>().page == 6){
            for(int i = 0; i < finger_spr.Length; i++){
                if(i > 2){
                    if(!(finger_spr[i].transform.localScale.z > 1)){ // curled
                        if(i == 3){Rval += 1;}
                        if(i == 4){Rval += 2;}
                        if(i == 5){Rval += 4;}
                    }
                }
            }
            metater met = GameObject.Find("metater").GetComponent<metater>();
            if(Rval == 1){ // master aud
                Debug.Log("mast aud");
                met.master_aud += 0.25f;
                if(met.master_aud > 1){
                    met.master_aud = 0;
                }
            }
            if(Rval == 4){ // ambient aud
                Debug.Log("amb aud");
                met.amb_aud += 0.25f;
                if(met.amb_aud > 1){
                    met.amb_aud = 0;
                }
            }
            if(Rval == 6){ // player aud
                Debug.Log("pl aud");
                met.pl_aud += 0.25f;
                if(met.pl_aud > 1){
                    met.pl_aud = 0;
                }
            }
            if(Rval == 0){ // quit
                Debug.Log("quitted");
                Application.Quit();
            }
            if(Rval == 7){ // reset
                Debug.Log("reset");
                GameObject.Find("metater").GetComponent<DATA>().best_spells = 0;
                GameObject.Find("metater").GetComponent<DATA>().best_time = -1;
                GameObject.Find("metater").GetComponent<DATA>().Save();
                GameObject.Find("metater").GetComponent<DATA>().Save();
                hold_data data = save_data.LOAD();
                GameObject.Find("metater").GetComponent<metater>().best_spells = data.best_spells;
                GameObject.Find("metater").GetComponent<metater>().best_time = data.best_time;
                death = true;
            }
            GameObject.Find("metater").GetComponent<metater>().Update_aud();
        }
    }
    
    public void figorswap(){
        float[] list = {0,0,0,0,0,0};
        for(int i = 0; i < finger_spr.Length;i++){ // swaps fingers
            if(i > 2){
                list[i-3] = finger_spr[i].transform.localScale.z;
            }
            else{
                list[i+3] = finger_spr[i].transform.localScale.z;
            }
        }
        for(int i = 0; i < finger_spr.Length;i++){
            finger_spr[i].transform.localScale = new Vector3(1,1,list[i]);
        }
    }

    public void unflashclaps(){
        arms[0].GetComponent<SpriteRenderer>().sprite = arms_spr[0];
        arms[1].GetComponent<SpriteRenderer>().sprite = arms_spr[1];
        for(int i = 0; i < finger_spr.Length; i++){
            int pos = i;
            if(i > 2){pos -= 3;}
            if(finger_spr[i].transform.localScale.z > 1){ // curled
                pos += 3;
            }
            finger_spr[i].sprite = finger_states[pos];
        }
    }

    public void flashclaps(){
        arms[0].GetComponent<SpriteRenderer>().sprite = arms_spr[2];
        arms[1].GetComponent<SpriteRenderer>().sprite = arms_spr[3];
        for(int i = 0; i < finger_spr.Length; i++){
            int pos = i;
            if(i > 2){pos -= 3;}
            pos += 6;
            if(finger_spr[i].transform.localScale.z > 1){ // curled
                pos += 3;
            }
            finger_spr[i].sprite = finger_states[pos];
        }
        couch_of_casting();
        ch = true;
        Invoke("hate_my_players",10);
    }

    void hate_my_players(){
        if(ch){
            ICANTSEEEEEEEE();
        }
    }

    public void resetfingies(){
        for(int i = 0; i < finger_spr.Length; i++){
            int pos = i;
            if(i > 2){pos -= 3;}
            if(finger_spr[i].transform.localScale.z > 1){ // curled
                pos += 3;
            }
            finger_spr[i].sprite = finger_states[pos];
        }
    }

    public void figor_anim_frame(){
        for(int i = 0; i < finger_spr.Length; i++){
            int pos = i;
            if(i > 2){pos -= 3;}
            pos = pos * 10;
            pos += 12;
            if(finger_spr[i].transform.localScale.z > 1){ // curled
                pos += 5;
            }
            
            if(ch_anim > 4){
                // reform fingies
                pos += 4;
                pos -= (ch_anim-4);
            }
            else{
                pos += ch_anim;
            }
            finger_spr[i].sprite = finger_states[pos];
        }
        ch_anim++;
    }

    void update_val(){
        Cursor.visible = !play;
    }

    void fingers(){
        if(Input.GetMouseButtonDown(0)){ // left hand
            articulate(0);
        }
        if(Input.GetMouseButtonDown(1)){ // right hand
            articulate(3);
        }
    }

    void dorime(){
        RaycastHit[] hit = Physics.RaycastAll(cam.transform.position, cam.transform.forward, 20.0F);
        if(hit.Length > 0 && hit[0].collider != null){
            if(hit[0].collider.gameObject.GetComponent<door>() != null){
                hit[0].collider.gameObject.GetComponent<door>().check();
            }
        }

    }

    public void articulate(int hand_pos){
        int pos = figor_pos_L;
        if(hand_pos > 0){pos = figor_pos_R;figor_pos_R++;if(figor_pos_R > 2){figor_pos_R = 0;}}
        else{figor_pos_L++;if(figor_pos_L > 2){figor_pos_L = 0;}}
        SpriteRenderer spr = finger_spr[hand_pos+pos];
        if(spr.transform.localScale.z > 1){ // curled
            spr.sprite = finger_states[0+pos];
            spr.transform.localScale += new Vector3(0,0,-1); 
        } 
        else{
            spr.sprite = finger_states[3+pos];
            spr.transform.localScale += new Vector3(0,0,1);
        }
        crack();
    }

    public void crack(){
        aud.PlayOneShot(finger_snap[Random.Range(0,finger_snap.Length)]);
    }

    bool ch = true;

    void couch_of_casting(){
        float scr_time = 0.0f;
        int Lval = 0;
        int Rval = 0;

        string Ltype = "";
        string Rtype = "";

        string Lname = "";
        string Rname = "";

        for(int i = 0; i < finger_spr.Length; i++){
            if(i > 2){
                if(!(finger_spr[i].transform.localScale.z > 1)){ // curled
                    if(i == 3){Rval += 1;}
                    if(i == 4){Rval += 2;}
                    if(i == 5){Rval += 4;}
                }
            }
            else{
                if(!(finger_spr[i].transform.localScale.z > 1)){ // curled
                    if(i == 0){Lval += 1;}
                    if(i == 1){Lval += 2;}
                    if(i == 2){Lval += 4;}
                }
            }
        }
        // L hand stuff
            if(Lval == 0){
                Lname = "cast";
                Ltype = "CAST";
            }
            if(Lval == 1){
                Lname = "expulse";
                Ltype = "effect";
            }
            if(Lval == 2){
                Lname = "mend";
                Ltype = "effect";
            }
            if(Lval == 3){ // totem
                Lname = "totem";
                Ltype = "object";
            }
            if(Lval == 4){
                Lname = "loop";
                Ltype = "effect";
            }
            if(Lval == 5){
                Lname = "cleave";
                Ltype = "effect";
            }
            if(Lval == 6){ // 235
                Lname = "low door";
                Ltype = "object";
            }
            if(Lval == 7){
                // yeets player across the map
                Lname = "converge";
                Ltype = "effect";
            }
        // L hand stuff

        // R hand stuff
            if(Rval == 0){ // maurice
                Rname = "maurice";
                Rtype = "object";
            }
            if(Rval == 1){ // 783
                Rname = "jozzo";
                Rtype = "object";
            }
            if(Rval == 2){
                Rname = "decimate";
                Rtype = "effect";
            }
            if(Rval == 3){
                Rname = "blahaj";
                Rtype = "CAST";
            }
            if(Rval == 4){ // sam
                Rname = "sam";
                Rtype = "object";
            }
            if(Rval == 5){
                Rname = "down";
                Rtype = "CAST";
            }
            if(Rval == 6){
                Rname = "reverse";
                Rtype = "effect";
            }
            if(Rval == 7){
                Rname = "impulse";
                Rtype = "effect";
            }
        // R hand stuff

        // timings
            if(Ltype == "CAST" && Rtype == "CAST"){
                scr_time = 1.1f;
            }
            if((Ltype == "object" && Rtype == "effect")||(Rtype == "object" && Ltype == "effect")){
                scr_time = 0.6f;
            }
            if(Ltype == "object" && Rtype == "object"){
                scr_time = 0.5f;
            }
        // timings
        Debug.Log(Lname);
        Debug.Log(Rname);
        GameObject scr = screens[0]; // set to cast by default

        if(scr_time != 0 && guy != null){
            bool save = warn;
            int p = guy.cast(Ltype,Lname,Rtype,Rname);
            if(save == warn){warn = true;}
            else{scr_time = 0;}
            scr = screens[p];
            if(p == 4){scr_time = 2;}
            if(GameObject.Find("jozzo") != null){
                GameObject.Find("jozzo").GetComponent<jozzo>().mercy_cast(Lname,Rname);
            }
            GameObject.Find("metater").GetComponent<metater>().spells_cast ++;
            if(GameObject.Find("face") != null){
                Destroy(GameObject.Find("face"));
            }
        } 
        GameObject.Find("metater").GetComponent<metater>().Update_aud();

        if(GameObject.Find("end") != null){
            if(Lname == "cast" && Rname == "down"){
                // reset
                Debug.Log("end");
                scr = screens[4];
                TE = false;
                scr_time = 2;
                GameObject.Find("metater").GetComponent<metater>().saveshit();
            }
        }
        cover(scr_time,scr);
        ch = false;
    }

    public void cover(float time, GameObject screen){
        float scr_time = time;
        hold = false;
        ch = false;
        if(scr_time != 0){
            hold = true;
            Main_C.gameObject.SetActive(false);
            hand_cam.SetActive(false);
            cam_cover.gameObject.SetActive(true);
            cam_cover.anim(scr_time,screen);
        }
        Invoke("ICANTSEEEEEEEE",scr_time);
    }

    bool hold = false;

    void ICANTSEEEEEEEE(){
        // vignette.transform.localScale = new Vector3(1,1,1);
        // vignette.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        clap_Anim.Play("clap anim aft");
        Main_C.gameObject.SetActive(true);
        hand_cam.SetActive(true);
        aud.PlayOneShot(clap_fx);
        aud.PlayOneShot(figor_melt_fx);
        ch = false;
        if(hold){
            if(Random.Range(0,10) == 7){
                FindObjectOfType<texter>().queue.Add(GameObject.Find("language").GetComponent<lang_pack>().casting_text[Random.Range(0,GameObject.Find("language").GetComponent<lang_pack>().casting_text.Length)]);
            }
            FindObjectOfType<texter>().idle_timer = Time.time + Random.Range(30,60);
        }
        
    }

    void player_cont(){
        if (active){
            if(RB.velocity.z < 0){
                if(RB.velocity.z > 50){
                    RB.drag = 50;
                }
            }
            else{
                if(-RB.velocity.z > 50){
                    RB.drag = 50;
                }
            }
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S)){
                step = true;
            }
            wobble_vect = new Vector2(0,0);
            move = false;
            if(Input.GetKey(KeyCode.A)){ // movement
                RB.AddForce(-cont_point.transform.right * acceleration_speed);
                move = true;
                if(wobble_vect.x <= 0){
                    wobble_vect.x += -9;
                }
                else{
                    wobble_vect.x -= -9;
                }
                if(wobble_vect.y <= 0){
                    wobble_vect.y += -8;
                }
                else{
                    wobble_vect.y -= -8;
                }
            }
            if(Input.GetKey(KeyCode.S)){
                RB.AddForce(-cont_point.transform.forward * acceleration_speed);
                move = true;
                if(wobble_vect.x <= 0){
                    wobble_vect.x += -9;
                }
                else{
                    wobble_vect.x -= -9;
                }
                if(wobble_vect.y <= 0){
                    wobble_vect.y += -5;
                }
                else{
                    wobble_vect.y -= -5;
                }
            }
            if(Input.GetKey(KeyCode.D)){
                RB.AddForce(cont_point.transform.right * acceleration_speed);
                move = true;
                if(wobble_vect.x >= 0){
                    wobble_vect.x += 9;
                }
                else{
                    wobble_vect.x -= 9;
                }
                if(wobble_vect.y >= 0){
                    wobble_vect.y += 8;
                }
                else{
                    wobble_vect.y -= 8;
                }
            }
            if(Input.GetKey(KeyCode.W)){
                RB.AddForce(cont_point.transform.forward * acceleration_speed);
                move = true;
                if(wobble_vect.x >= 0){
                    wobble_vect.x += 9;
                }
                else{
                    wobble_vect.x -= 9;
                }
                if(wobble_vect.y >= 0){
                    wobble_vect.y += 5;
                }
                else{
                    wobble_vect.y -= 5;
                }
            }
            if (move){
                RB.drag = drag_save / 3;
            }
            RB.drag = drag_save;
        }
    }

    void cam_control(){
        rot_x = Input.GetAxis("Mouse X") * sensitivity;
        rot_Y = Input.GetAxis("Mouse Y") * -sensitivity;

        float magnitude = cam.transform.eulerAngles.x + rot_Y;
        if(magnitude > 280 || magnitude < 80){
            
        }
        else{
            rot_Y = 0;
        }
        cam_analogue.transform.Rotate(rot_Y,rot_x,0,Space.Self); 
        cam_analogue.transform.eulerAngles = new Vector3(cam_analogue.transform.eulerAngles.x,cam_analogue.transform.eulerAngles.y,0);
        cam.transform.eulerAngles = new Vector3(Mathf.LerpAngle(cam.transform.eulerAngles.x,cam_analogue.transform.eulerAngles.x,Time.deltaTime*cam_speed),Mathf.LerpAngle(cam.transform.eulerAngles.y,cam_analogue.transform.eulerAngles.y,Time.deltaTime*cam_speed),0);
        cont_point.transform.eulerAngles = new Vector3(cont_point.transform.eulerAngles.x,cam.transform.eulerAngles.y,cont_point.transform.eulerAngles.z);
    
        // head bob
        if(move){
            if(bob_timings[0] < Time.time){
                bob_timings[0] = Time.time + ((Random.Range(100,300))/100);
                bober = 0;
            }
            else if(bob_timings[1] < Time.time){
                bob_timings[1] = Time.time + ((Random.Range(100,300))/100);
                bober = 1;
            }
            else if(bob_timings[2] < Time.time){
                bob_timings[2] = Time.time + ((Random.Range(100,300))/100);
                bober = 2;
            }
            

            float V = Mathf.Sin(Time.time*4) / 10;
            if(dir == 0){
                // going down
                if(sav_bob < V){
                    dir = 1;
                    aud.PlayOneShot(footsteps[Random.Range(0,footsteps.Length)]);
                }
            }
            else{
                if(sav_bob > V){
                    dir = 0;
                    aud.PlayOneShot(footsteps[Random.Range(0,footsteps.Length)]);
                }
            }
            sav_bob = V;
            if(V < 0){V -= bob_height[bober];}
            else{V += bob_height[bober];}
            cam_analogue.transform.localPosition = new Vector3(0,0.3f + V,0);
            
            
        }
        else{
            cam_analogue.transform.localPosition = new Vector3(0,0.3f,0);
        }
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,cam_analogue.transform.localPosition,Time.deltaTime);
    }
    float sav_bob = 0;
    int dir = 0;

}
