using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Gamemanager : MonoBehaviour
{
    public static Gamemanager s_instance;
    GameObject COBJ;
    public Character[] user_character_list;


    public GameObject in_game_canvas;
    public GameObject turn_ui;
    public Transform turn_start;
    public Transform turn_end;
    public Material area_mtl;

    public Transform root_campos;
    public List<Transform> cam_poses;
    public Transform current_campos;
    //List<List<GameObject>> map = new List<List<GameObject>>();
    public List<Character> in_game_character = new List<Character>();
    public List<Character> max_speed_character = new List<Character>();
    public bool Turn_Play = false;
     bool Game_Play = false;
    public bool INGAME
    {
        get {return Game_Play; }
        set {
            if (value) { in_game_canvas.SetActive(true); }
            else { in_game_canvas.SetActive(false); }
            Game_Play = value; 
            }
    }
    public Turn_Type Turn_Type = Turn_Type.NONE;
    public GameObject[] skill_buttons;
    public GameObject damage_prefab;
    private Attack_Skill current_skill;
    public GameObject Spawnchaimage()
    {
        return Instantiate(turn_ui, turn_start.transform.position,Quaternion.identity,in_game_canvas.transform);
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
    public IEnumerator Delay_Turn(Turn_Type next_turn,float time)
    {
        yield return new WaitForSeconds(time);
        Turn_Type = next_turn;
#if UNITY_EDITOR
        if(next_turn == Turn_Type.ATTACK)
        {
            foreach (GameObject obj in skill_buttons)
            {
                obj.SetActive(true);
            }
        }
#endif
    }

    public IEnumerator Delay_Turn2(Turn_Type next_turn, float time)
    {
        yield return new WaitForSeconds(time);
        Turn(next_turn);

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
    //Speed ���� �����ؾߵ�
    //�� ���� MAX ���ǵ� / 3 �� �������� ���� (MAX ���ǵ��Ͻ� 3�ʵ��� �̵�)
    //
    //or SPEED �� 100�� �ɶ� ����  S
    //0.2�ʸ��� SPEED/10 ��ŭ �����൵ ����
    public Character current_turn_character;
    public void TurnEnd()
    {
        switch(Turn_Type)
        {
            case Turn_Type.NONE:
                Turn(Turn_Type.MOVE);
                break;
            case Turn_Type.MOVE:
                Turn(Turn_Type.ATTACK);

                break;
            case Turn_Type.ATTACK:
                 Turn(Turn_Type.NONE);
                break;
        }
    }
    public void Turn(Turn_Type targetturn)
    {
        switch (targetturn)
        {
            case Turn_Type.NONE:
                StartCoroutine(Delay_Turn(Turn_Type.NONE, 0));
                current_turn_character.turn_speed -= 100;
                max_speed_character.Remove(current_turn_character);
                current_turn_character = null;
                MapManager.s_instance.AreaOff();
                if (max_speed_character.Count == 0)
                {
                    Turn_Play = true;
                }
                foreach (GameObject obj in skill_buttons)
                {
                    obj.SetActive(false);
                }
                break;
            case Turn_Type.MOVE:
                StartCoroutine(Delay_Turn(Turn_Type.MOVE, 0));
                foreach (GameObject obj in skill_buttons)
                {
                    obj.SetActive(false);
                }
          
                break;
            case Turn_Type.ATTACK:
                MapManager.s_instance.AreaOff();
                StartCoroutine(Delay_Turn(Turn_Type.ATTACK, 0));
                foreach (GameObject obj in skill_buttons)
                {
                    obj.SetActive(true);
                }
                
       
                break;
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
    Skill currnet_skill;
    public void Set_Skill_Ui()
    {
        
    }
    public void Click_Skill_UI(int num)
    {
        if(Turn_Type==Turn_Type.ATTACK)
        {
            MapManager.s_instance.AreaOff();
            ChangeCurrentTarget(false);
            switch (num)
            {
                case 0:
                    if(current_turn_character.default_attack.attack_shape == attack_shape.STRAIGHT)
                    {
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.default_attack.distance, current_turn_character.default_attack.height,true);
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.default_attack.m_distance, current_turn_character.default_attack.height, false);

                    }
                    else
                    {
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.default_attack.distance, current_turn_character.default_attack.height,true);
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.default_attack.m_distance, current_turn_character.default_attack.height, false);
                    }
                    current_skill = current_turn_character.default_attack;
                    break;
                case 1:
                    if (current_turn_character.cha_skill.attack_shape == attack_shape.STRAIGHT)
                    {
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.cha_skill.distance, current_turn_character.cha_skill.height,true);
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.cha_skill.m_distance, current_turn_character.cha_skill.height, false);
                    }
                    else
                    {
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.cha_skill.distance, current_turn_character.cha_skill.height,true);
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.cha_skill.m_distance, current_turn_character.cha_skill.height, false);
                    }
                    current_skill = current_turn_character.cha_skill;
                    break;
                case 2:
                    if (current_turn_character.skill_1.attack_shape == attack_shape.STRAIGHT)
                    {
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_1.distance, current_turn_character.skill_1.height,true);
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_1.m_distance, current_turn_character.skill_1.height, false);
                    }
                    else
                    {
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_1.distance, current_turn_character.skill_1.height,true);
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_1.m_distance, current_turn_character.skill_1.height, false);

                    }
                    current_skill = current_turn_character.skill_1;
                    break;
                case 3:
                    if (current_turn_character.skill_2.attack_shape == attack_shape.STRAIGHT)
                    {
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_2.distance, current_turn_character.skill_2.height,true);
                        MapManager.s_instance.AttackRangeCheck_Straight(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_2.m_distance, current_turn_character.skill_2.height, false);

                    }
                    else
                    {
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_2.distance, current_turn_character.skill_2.height,true);
                        MapManager.s_instance.AttackRangeCheck_Circle(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], current_turn_character.skill_2.m_distance, current_turn_character.skill_2.height, false);

                    }
                    current_skill = current_turn_character.skill_2;
                    break;
            }
        }     
    }
    public void Spawn_Damage_IMG(GameObject target,int damage)
    {
        //test
        GameObject ins_dmg = Instantiate(damage_prefab);
        ins_dmg.GetComponentInChildren<TextMeshProUGUI>().text = System.Convert.ToString(damage);
    }
    private void Update()
    {
        if (INGAME)
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
        if(Input.GetKeyDown(KeyCode.G))
        {
           
           INGAME=INGAME?false:true;
        }
        if(INGAME)
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
                              
                            }
                        }
                    }
                    if (Turn_Play == false)
                    {
                        if (current_turn_character == null && max_speed_character.Count != 0)
                        {
                            current_turn_character = max_speed_character[0];
                            
                           
                            if(current_turn_character.current== State.DEFAULT)
                            {
                                StartCoroutine(Delay_Turn(Turn_Type.MOVE, 0));
                                current_turn_character.MoverangeTest1();
                            }
                            else
                            {
                                
                                TurnEnd();
                            }
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
                                    MapManager.s_instance.Obstacle((int)current_turn_character.location.x, (int)current_turn_character.location.z, false);
                                    MapManager.s_instance.Cha_Move((int)current_turn_character.location.x, (int)current_turn_character.location.z, null);
                                    current_turn_character.Move(Move);
                                    MapManager.s_instance.Obstacle((int)target.x_location, (int)target.z_location, true);
                                    MapManager.s_instance.Cha_Move((int)target.x_location, (int)target.z_location, current_turn_character);
                                    MapManager.s_instance.AreaOff();
                                    
                                    StartCoroutine(Delay_Turn(Turn_Type.ATTACK, (Move.Count-1)*0.5f));
                                    // TurnEnd();                       
                                }
                            }
                        }
                    }
                    break;
                case Turn_Type.ATTACK:

                    //ChangeCurrentTarget(false);
                   // MapManager.s_instance.AttackRangeCheck(MapManager.s_instance.current_map_data[(int)current_turn_character.location.x, (int)current_turn_character.location.z], 2, 1);
                    if(Input.GetKeyUp(KeyCode.I))
                    {
                        StartCoroutine(Delay_Turn(Turn_Type.ATTACK,0));
                        MapManager.s_instance.AreaOff();
                        TurnEnd();
                    }
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
                                    
                                   Character enermy = MapManager.s_instance.Get_cha_from_map((int)target.x_location, (int)target.z_location);
                                   if(current_skill!=null&&enermy!=null)
                                   {
                                        MapManager.s_instance.AreaOff();
                                        enermy.Damaged(current_skill, current_turn_character);
                                        StartCoroutine(Delay_Turn2(Turn_Type.NONE, 1));
                                       
                                    }
                                }
                            }
                        }
                    }
                    break;
               
            }

            

           



        }

    }


    public void Save_data()
    {


    }
    public void Load_data()
    {
    
    }
    
    

}
public enum Turn_Type
{
    NONE,
    MOVE,
    ATTACK
}


