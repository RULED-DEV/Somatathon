using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hajer : MonoBehaviour
{
    public AudioClip uper;
    public AudioClip downer;
    public void kill(){Destroy(gameObject);}

    public void up(){gameObject.GetComponent<AudioSource>().PlayOneShot(uper);}
    public void down(){gameObject.GetComponent<AudioSource>().Stop();gameObject.GetComponent<AudioSource>().PlayOneShot(downer);}
}
