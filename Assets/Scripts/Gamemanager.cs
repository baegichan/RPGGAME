using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager s_instance;
    public int test = 1;
    GameObject COBJ;
    public Material area_mtl;
    public GameObject CurrentOBJ
    {
        get {return COBJ; }
        set {COBJ=value; }
    }


    List<List<GameObject>> map = new List<List<GameObject>>();
    public List<Character> in_game_character = new List<Character>();
    //testcode
    public void ChangeCurrentTarget()
    {
        area_mtl.SetFloat("_Check",test);
    }
    public List<Character> Get_character()
    {
       
        return in_game_character;
    }
    public void Clear_cha_list()
    {
        in_game_character.Clear();
    }
    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this; 
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


    Transform currnet_turn = null;
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //int쪽으로 변경해야됨
                if(hit.transform.tag=="Area")
                {
                


                }
                if(hit.transform!= currnet_turn)
                {
                    if(currnet_turn != null)
                    {
                        if (currnet_turn.transform.gameObject.GetComponent<Character>() != null)
                        {
                            currnet_turn.transform.gameObject.GetComponent<Character>().MoverangeTest2();
                        }
                    }
                    currnet_turn = hit.transform;
                    if (currnet_turn.transform.gameObject.GetComponent<Character>() != null)
                    {
                        currnet_turn.transform.gameObject.GetComponent<Character>().MoverangeTest1();
                    }
                }
            }

        }
       
    }
}
