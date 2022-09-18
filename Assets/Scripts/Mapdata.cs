using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Mapdata", menuName = "ScriptableObjects/MapData", order = 1)]
public class Mapdata : ScriptableObject
{
    [Header("좌표 시작은 0,0 부터임")]
    public Map_Array[] Map_data;
    // Start is called before the first frame update
    public Map_Material[] Map_material;
    public Quest mainquest;
}
[System.Serializable]
public class Map_Array
{
    public Block_Data[] block;
}
[System.Serializable]
public class Block_Data
{
    public Block.BLOCK_TYPE type;
    public float height;
    public GameObject Obstacle;
    public bool not_stable = false;
}
[System.Serializable]
public class Map_Material
{
    public Material material;
    public Block.BLOCK_TYPE type;
}

