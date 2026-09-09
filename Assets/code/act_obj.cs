using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class act_obj : MonoBehaviour
{
    public act_obj o;
    public int door_pos;

    public string name;

    public bool stable;

    public bool hex;

    void Start(){
        Invoke("greg",0.5f);
    }

    void greg(){FindObjectOfType<mapguy>().spell_objs.Add(o);}
}
