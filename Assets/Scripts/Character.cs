using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character : MonoBehaviour
{
    public string cha_name;
    public int cha_level = 1;
    public Status cha_status = new Status();
    public List<Skill> skills = new List<Skill>();

    public Vector3 location;

    public Weapon weapon;
    public Armor shiled;
    public Armor helmet;
    public Armor armor;
    public Armor pants;
    public Armor shoes;

    public Status current_ingame_status = new Status();
    public Status max_ingame_status = new Status();
    public Growth growth_status = new Growth();


    public List<Buff_Skill> buff = new List<Buff_Skill>();
    public State current = State.DEFAULT;
    public void Euip(Equipment target)
    {
        if(target!=null)
        {
            max_ingame_status.ATK += target.equipstatus.ATK;
            max_ingame_status.MAG += target.equipstatus.MAG;
            max_ingame_status.DEF += target.equipstatus.DEF;
            max_ingame_status.RES += target.equipstatus.RES;
            max_ingame_status.HP += target.equipstatus.HP;
            max_ingame_status.MP += target.equipstatus.MP;
            max_ingame_status.MOVE += target.equipstatus.MOVE;
            max_ingame_status.JUMP += target.equipstatus.JUMP;
            max_ingame_status.fire_res += target.equipstatus.fire_res;
            max_ingame_status.water_res += target.equipstatus.water_res;
            max_ingame_status.lightning_res += target.equipstatus.lightning_res;
            max_ingame_status.wind_res += target.equipstatus.wind_res;
            max_ingame_status.dark_res += target.equipstatus.dark_res;
            max_ingame_status.light_res += target.equipstatus.light_res;
        }
       
    }
    public void Cha_Init()
    {
        max_ingame_status = cha_status;
        Euip(weapon);
        Euip(shiled);
        Euip(helmet);
        Euip(armor);
        Euip(pants);
        Euip(shoes);
        current_ingame_status = max_ingame_status;

    }
    public void Damaged(Attack_Skill skill , Character enemy)
    {
        int damage = 0;
        switch (skill.attack_type)
        {
            case attack_type.MELEE:
                damage = skill.dmg* enemy.current_ingame_status.ATK - current_ingame_status.DEF;
                break;
            case attack_type.MAGIC:
                damage = skill.dmg * enemy.current_ingame_status.MAG - current_ingame_status.RES;
                break;
        }
        switch(skill.attack_attribute)
        {
            case attribute.NONE:
               
                break;
            case attribute.FIRE:
                damage /= (100 - current_ingame_status.fire_res);
                break;
            case attribute.WATER:
                damage /= (100 - current_ingame_status.water_res);
                break;
            case attribute.WIND:
                damage /= (100 - current_ingame_status.wind_res);
                break;
            case attribute.LIGHTNING:
                damage /= (100 - current_ingame_status.lightning_res);
                break;
            case attribute.LIGHT:
                damage /= (100 - current_ingame_status.light_res);
                break;
            case attribute.DARK:
                damage /= (100 - current_ingame_status.dark_res);
                break;
                
        }

    }
    public void Damaged(Buff_Skill debuff_skill , Character enemy)
    {

    }
    public void Damaged(Buff_Skill buff_skill)
    {
        switch (buff_skill.bufftype)
        {
            case Buff.STURN:
                
                break;
            case Buff.POISON:
                break;
            case Buff.SHOCK:
                break;
            case Buff.SLEEP:
                break;
            case Buff.BURNS:
                break;
            case Buff.STATUSDOWN:
                break;
            case Buff.FROSTBITE:
                break;
        }
    }
    public void Buff_Update()
    {
        if(buff!=null)
        {
            int sturndebuff = 0;
            foreach(Buff_Skill c_buff in buff)
            {
                if (c_buff.Turn == 0)
                {
                    switch (c_buff.bufftype)
                    {
                        case Buff.STURN:
                            sturndebuff -= 1;
                            break;
                        case Buff.SHOCK:
                            sturndebuff -= 1;
                            break;
                        case Buff.SLEEP:
                            sturndebuff -= 1;
                            break;
                        case Buff.FROSTBITE:
                            sturndebuff -= 1;
                            break;
                    }
                    buff.Remove(c_buff);
                }
                switch (c_buff.bufftype)
                {
                    case Buff.STURN:
                        sturndebuff += 1;
                        break;
                    case Buff.POISON:
                        current_ingame_status.HP -= (max_ingame_status.HP / 100) * 5;
                        //데미지 출력 구문 추가
                        break;
                    case Buff.SHOCK:
                        sturndebuff += 1;
                        break;
                    case Buff.SLEEP:
                        sturndebuff += 1;
                        break;
                    case Buff.BURNS:
                        current_ingame_status.HP -= (max_ingame_status.HP / 100) * 5;
                        //데미지 출력 구문 추가
                        break;
                    case Buff.STATUSDOWN:
                        break;
                    case Buff.FROSTBITE:
                        sturndebuff += 1;
                        break;
                }
                c_buff.Turn -= 1;    
            }
            if (sturndebuff != 0) { current = State.STURN; }
            else
            {
                current = State.DEFAULT;
            }
        }
    }

    private void Update()
    {
    #if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.M))
        {
            MoverangeTest1();
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            MoverangeTest2();
        }
    #endif

    }
    public Transform[] test;
    public void Move()
    {
       

       // StartCoroutine(Movement(test));
    }
    public void Move(Vector3 target)
    {

    }


    public void MoverangeTest1()
    {
        MapManager.s_instance.MoveCheck(this, true);
    }
    public void MoverangeTest2()
    {
        MapManager.s_instance.MoveCheck(this, false);
    }


    public void Resist()
    {
        Gamemanager.s_instance.in_game_character.Add(this);
    }
    public void Movable_block()
    {
       
    }
    IEnumerator Movement(Transform[] path)
    {
        for(int i = 0; i<path.Length-1;i++)
        {
            if(path[i].position.y+0.5f<=path[i+1].position.y)
            {
                Jump(path[i].position, path[i + 1].position);
            }
            else
            {
                iTween.MoveTo(gameObject, path[i + 1].position, 0.5f);
            }
            yield return new WaitForSeconds(0.45f);
        }
    }
    //이동시 점프 애니메이션 용 
    public void Jump(Vector3 current, Vector3 target) 
    {
        
        Vector3 rvec = new Vector3( target.x-current.x, target.y , target.z - current.z);
        Vector3[] jump_path = { current, rvec, target };
        iTween.MoveTo(gameObject, iTween.Hash("path", jump_path, "time", 1, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "movetopath", false));
        //iTween.MoveTo(gameObject,new Vector3(current.x+rvec.x,rvec.y, current.z + rvec.z), 0.5f);
    }
    private void Start()
    {
        Cha_Init();
        Resist();
    }
}
[System.Serializable]
public class Status
{
    public int HP;
    public int MP;
    public int ATK;
    public int DEF;
    public int MAG;
    public int RES;

    public float SPEED;

    public int MOVE = 3;
    public int JUMP = 1;

    public int fire_res;
    public int water_res;
    public int wind_res;
    public int lightning_res;
    public int dark_res;
    public int light_res;

    public int poison_res;
    public int sleep_res;
    public int sturn_res;
    public int burns_res;
    public int frostbite_res;
    public int shock_res;

}

public abstract class Skill
{
    public Image skill_img;
    public string skill_name;
    public int distance;
    public int range;
}
[System.Serializable]
public class Attack_Skill : Skill
{
    public int dmg;
    public attack_type attack_type;
    public attribute attack_attribute;
}
[System.Serializable]
public class Buff_Skill : Skill
{
    public Buff bufftype;
    public int Turn;
}
public class Attack_Range
{


}
public enum attack_type
{
    MELEE,
    MAGIC
}

public enum attribute
{
NONE,
FIRE,
WATER,
WIND,
LIGHTNING,
LIGHT,
DARK
}

public abstract class Equipment
{
    public string equip_name;
    public Image equip_image;
    public Status equipstatus;
   

}
[System.Serializable]
public class Armor : Equipment
{
    public EQUIP_TYPE Armor_type; 
}
[System.Serializable]
public class Weapon : Equipment
{
    public WEAPON_TYPE type;
    public bool two_hand = false;
}
public enum EQUIP_TYPE
{
   
    SHIELD,
    HELMET,
    ARMOR,
    PANTS,
    SHOES
}
public enum WEAPON_TYPE
{
    STAFF,
    SWORD,
    SPEAR,
}

public enum State
{
    DEFAULT,
    STURN,
    DEAD
}

public enum Buff
{
    NONE,
    HEAL,
    STURN,
    POISON,
    SLEEP,
    BURNS,
    FROSTBITE,
    SHOCK,
    STATUSDOWN,
 
}

//성장률 정의 해야됨
//S
//A
//B
//C
//D
//E
//F
//각 상태별로 하면좋을듯?
public class Growth
{
    public string HP;
    public string MP;
    public string ATK;
    public string DEF;
    public string MAG;
    public string RES;

    public string poison_res;
    public string sleep_res;
    public string sturn_res;
    public string burns_res;
    public string frostbite_res;
    public string shock_res;
}
