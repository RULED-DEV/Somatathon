using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lang_pack : MonoBehaviour
{
    void Awake(){gameObject.name = "language";}

    public string name;
    public Sprite flag;

    public string[] lore_text;
    public string[] info_text;
    public string[] cast_text;
    public string[] Lcast_text;
    public string[] Rcast_text;
    public string[] up_info_text;
    public string[] menu_text;

    public string[] options;

    public string[] beginning_text;
    public string[] book_text;
    public string[] caution_text;
    public string[] idle_text;
    public string[] casting_text; 
    public string[] interact_text;
    public string[] end_text;
    public string[] cast_down_text;
    public string[] maurice_text;
}
