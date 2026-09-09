using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ender : MonoBehaviour
{

    void Update()
    {
        if(transform.parent != null){transform.parent.gameObject.name = "true death"; Invoke("d",1.8f);}
    }

    void d(){
        transform.parent = null;
        if(GameObject.Find("end") != null){
            Destroy(GameObject.Find("end"));
        }
        if(GameObject.Find("sam").GetComponent<grumblo_cont>().TE){
            Application.Quit();
            Debug.Log("true end");
        }
        else{
            GameObject.Find("sam").GetComponent<grumblo_cont>().cam_cover.kill2();
            Destroy(gameObject);
            Debug.Log("restart");
        }
    }
}
