using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class Quest : ScriptableObject
{
    public Quest_type quest_type;
    public List<Character> Monster = new List<Character>();
    public bool random_spawn;
    public int monster_num;
    
}
public enum Quest_type
{
    DEFFENSE,
    ALLKILL,
    MOVE
}
[System.Serializable]
public class Reward
{

}