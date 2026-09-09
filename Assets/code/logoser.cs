using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logoser : MonoBehaviour
{

    public GameObject map;

    void Update(){
        transform.position = new Vector3(0,0,0);
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.E)){
            end();
        }
    }

    public void end(){
        Destroy(transform.GetChild(0).gameObject);
        Instantiate(map,transform.position,transform.rotation);
        Destroy(gameObject);
    }

    public void noise(){
        Debug.Log("aaaaa");
        gameObject.GetComponent<AudioSource>().Play();
    }

}
