using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class hold_data { // organises data
    
    public float best_time;
    public int best_spells;

    public hold_data(DATA data){ // pulls data values from the DATA script
        best_spells = data.best_spells;
        best_time = data.best_time;
    }


}
