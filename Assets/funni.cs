using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class funni : MonoBehaviour
{
    void Start()
    {
        Invoke("death",4.5f);
    }

    void Update(){
        SpriteRenderer[] sprs = transform.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spr in sprs){
            Color col = spr.color;
            col.a = Mathf.Lerp(col.a,1,Time.deltaTime);
            spr.color = col;
        }
    }

    void death(){
        Application.Quit();
    }


}
