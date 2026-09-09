using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DATA : MonoBehaviour{ // grabs data

    public int best_spells;
    public float best_time;

    public void Save(){
        save_data.SAVE(this);
    }

    void Start(){
        Load();
        Save();
    }

    public void Load(){
        hold_data data = save_data.LOAD();
        best_spells = data.best_spells;
        best_time = data.best_time;
    }
}
