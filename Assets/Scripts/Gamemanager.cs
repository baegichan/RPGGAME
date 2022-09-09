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

    public Transform root_campos;
    public List<Transform> cam_poses;
    public Transform current_campos;
    List<List<GameObject>> map = new List<List<GameObject>>();
    public List<Character> in_game_character = new List<Character>();
    public List<Character> max_speed_character = new List<Character>();
    public bool Turn_Play = false;
    public bool Game_Play = false;
    public Turn_Type Turn_Type = Turn_Type.NONE;
    public GameObject Spawnchaimage()
    {
        return Instantiate(turn_ui, in_game_canvas.transform);
    }
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
    public IEnumerator Delay_Turn(Turn_Type next_turn_type,float time)
    {
        yield return new WaitForSeconds(time);
        Turn_Type = next_turn_type;

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
    public Character current_turn_character;
    public void TurnEnd()
    {
        current_turn_character.turn_speed -= 100;
        max_speed_character.Remove(current_turn_character);
        
        current_turn_character = null;
        if(max_speed_character.Count==0)
        {
            Turn_Play = true;
            Turn_Type = Turn_Type.NONE;
        }
    }
    public int IndexReturner()
    {
        int count = 0;
        foreach (Transform i in cam_poses)
        {
            if (i == current_campos)
            {
                return count;
            }
            else
            {
                count++;
                continue;
            }
        }
        return -1;
    }


    private void Update()
    {
        if (Game_Play)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                int Index = IndexReturner();
                if (Index != -1)
                {
                    if (Index == 0)
                    {
                        current_campos = cam_poses[cam_poses.Count - 1];

                    }
                    else
                    {
                        current_campos = cam_poses[Index - 1];
                    }

                }


            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                int Index = IndexReturner();
                if (Index != -1)
                {
                    if (Index == cam_poses.Count - 1)
                    {
                        current_campos = cam_poses[0];

                    }
                    else
                    {
                        current_campos = cam_poses[Index + 1];
                    }


                }
            }
        }
    }
    private void FixedUpdate()
    {
        if(Game_Play)
        {
            
            Camera.main.transform.position = current_campos.position;
            Camera.main.transform.rotation = current_campos.rotation;
                switch (Turn_Type)
                {
                case Turn_Type.NONE:
                    if (Turn_Play == true)
                    {
                        foreach (Character cha in in_game_character)
                        {
                            cha.turn_speed += cha.current_ingame_status.SPEED / 100;
                            cha.cha_image.transform.position = new Vector3(Mathf.Lerp(turn_start.position.x, turn_end.position.x, cha.turn_speed / 100), cha.cha_image.transform.position.y);

                            if (cha.turn_speed >= 100)
                            {
                                max_speed_character.Add(cha);
                                Turn_Play = false;
                                StartCoroutine(Delay_Turn(Turn_Type.MOVE, 0));
                            }
                        }
                    }
                    if (Turn_Play == false)
                    {
                        if (current_turn_character == null && max_speed_character.Count != 0)
                        {
                            current_turn_character = max_speed_character[0];
                            current_turn_character.MoverangeTest1();
                            StartCoroutine(Delay_Turn(Turn_Type.MOVE, 0));
                        }
                    }
                    break;
                case Turn_Type.MOVE:
                    ChangeCurrentTarget(true);
                    root_campos.position = current_turn_character.transform.position;
                    if (Input.GetMouseButtonDown(0))
                    {
                        
                        
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.tag == "Area")
                            {
                                Block target = hit.transform.GetComponentInParent<Block>();

                                if (current_turn_character != null)
                                {
                                    List<Transform> Move = MapManager.s_instance.Get_Path(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], target, MapManager.s_instance.Get_Active_Area());
                                    current_turn_character.Move(Move);
                                    MapManager.s_instance.AreaOff();
                                    StartCoroutine(Delay_Turn(Turn_Type.ATTACK, (Move.Count-1)*0.5f));
                                    // TurnEnd();                       
                                }
                            }
                        }
                    }
                    break;
                case Turn_Type.ATTACK:
#if UNITY_EDITOR
                    ChangeCurrentTarget(false);
                    MapManager.s_instance.RangeCheck(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], 2, 1);
                    if(Input.GetKeyDown(KeyCode.I))
                    {
                        StartCoroutine(Delay_Turn(Turn_Type.NONE,0));
                        TurnEnd();
                    }
                    #endif
                    break;
               
            }

            

           



        }

    }
}
public enum Turn_Type
{
    NONE,
    MOVE,
    ATTACK
}


//이동후 스킬누르면 각 스킬별 사거리 표시 (빨간색..?)
//클릭하면 공격후 