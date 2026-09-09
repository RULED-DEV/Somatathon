using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maurice_fx : MonoBehaviour
{

    public int face_amount;

    void Start()
    {
        for(int i = 1; i <= face_amount; i++){
            GameObject sav = Instantiate(transform.GetChild(1).gameObject,transform.position,transform.rotation);
            sav.transform.parent = transform;
            sav.transform.localPosition = new Vector3(0,-0.06f*i,-0.06f*i);
            float scaler = (1.0f+(0.2f*i)-0.4f);
            sav.transform.localScale += new Vector3(scaler,scaler,scaler);
        }      
        gameObject.name = "face";  
    }

    void Update(){

    }

}
