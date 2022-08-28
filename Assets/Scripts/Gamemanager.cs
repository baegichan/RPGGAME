using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager s_instance;
    GameObject COBJ;

    public GameObject in_game_canvas;
    public GameObject turn_ui;
    public Transform turn_start;
    public Transform turn_end;
    public Material area_mtl;
    public GameObject CurrentOBJ
    {
        get {return COBJ; }
        set {COBJ=value; }
    }


    List<List<GameObject>> map = new List<List<GameObject>>();
    public List<Character> in_game_character = new List<Character>();
    public List<Character> max_speed_character = new List<Character>();
    public bool Turn_Play = false;
    public bool Game_Play = false;

    public GameObject Spawnchaimage()
    {
        return Instantiate(turn_ui, in_game_canvas.transform);
    }
    //testcode
    public void ChangeCurrentTarget(bool player)
    {
        int value = player == true ? 1 : 0;
        area_mtl.SetFloat("_Check", value);
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
    //Speed 공식 설정해야됨
    //맵 내에 MAX 스피드 / 3 을 기준으로 설정 (MAX 스피드일시 3초동안 이동)
    //
    //or SPEED 가 100이 될때 설정  S
    //0.2초마다 SPEED/10 만큼 턴진행도 오름
    Character current_turn_character;
    public void TurnEnd()
    {
        current_turn_character.turn_speed -= 100;
        max_speed_character.Remove(current_turn_character);
        current_turn_character = null;
        if(max_speed_character.Count==0)
        {
            Turn_Play = true;
        }
    }
    private void FixedUpdate()
    {
        if(Game_Play)
        {

            if (Turn_Play == true)
            {
                foreach (Character cha in in_game_character)
                {
                    cha.turn_speed += cha.current_ingame_status.SPEED / 100;
                    cha.cha_image.transform.position= new Vector3( Mathf.Lerp(turn_start.position.x, turn_end.position.x, cha.turn_speed / 100), cha.cha_image.transform.position.y);
                    
                    if (cha.turn_speed >= 100)
                    {
                        max_speed_character.Add(cha);
                        Turn_Play = false;
                    }
                }
            }
            if (Turn_Play == false)
            {
                if (current_turn_character == null && max_speed_character.Count!=0)
                {
                    current_turn_character = max_speed_character[0];
                    current_turn_character.MoverangeTest1();
             
                }
            }

        }
      
    }
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

                    if (current_turn_character != null)
                    {
                            current_turn_character.MoverangeTest2();
                            TurnEnd();                       
                    }
                }
            }
        }
    }
}
//이동후 스킬누르면 각 스킬별 사거리 표시 (빨간색..?)
//클릭하면 공격후 