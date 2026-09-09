using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wiggler : MonoBehaviour
{

    Vector3 pos;

    void Start()
    {
        pos = transform.localPosition;
    }


    void Update()
    {
        float a = Random.Range(-10,10);
        float b = Random.Range(-10,10);
        transform.localPosition = pos + new Vector3(a/500,b/500,0);
    }
}
