using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public int test = 1;
    GameObject COBJ;
    public Material area__mtl;
    public GameObject CurrentOBJ
    {
        get {return COBJ; }
        set {COBJ=value; }
    }


    List<List<GameObject>> map = new List<List<GameObject>>();


    //testcode
    public void ChangeCurrentTarget()
    {
        area__mtl.SetFloat("_Check",test);
    }
    private void Awake()
    {
        if(instance==null)
        {
            instance = this; 
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        ChangeCurrentTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
