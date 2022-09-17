using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject block;
    public Mapdata testmap;
    public Transform up;
    public Transform down;

    public static MapManager s_instance;


    public GameObject testCha;

    public Block[,] current_map_data;

    public bool[,] obstacle_map;




    public void Obstacle(int x, int z, bool value)
    {
        obstacle_map[x, z] = value;
    }
    public void Spawn_Cha(GameObject default_cha, Vector3 point)
    {

        GameObject ins = Instantiate(default_cha, new Vector3(point.x, current_map_data[(int)point.x, (int)point.z].Block_obj.transform.position.y + 0.5f, point.z), Quaternion.identity);
        ins.GetComponent<Character>().location = point;
        Obstacle((int)point.x, (int)point.z, true);
    }
    IEnumerator Spawn_Routine(float time)
    {
        yield return new WaitForSeconds(time);
        Spawn_Cha(testCha, new Vector3(UnityEngine.Random.Range(0, 10), 0, UnityEngine.Random.Range(0, 10)));
    }
    public void Spawn_Map(Mapdata mapdata, Transform upblock, Transform underblock)
    {
        current_map_data = null;
        current_map_data = new Block[mapdata.Map_data.Length, mapdata.Map_data.Length];
        obstacle_map = new bool[mapdata.Map_data.Length, mapdata.Map_data.Length];
        // Instantiate();
        for (int i = 0; i < mapdata.Map_data.Length; i++)
        {
            for (int j = 0; j < mapdata.Map_data.Length; j++)
            {
                GameObject Spawned = Instantiate(block, new Vector3(i, mapdata.Map_data[i].block[j].height, j), Quaternion.identity, upblock);
                current_map_data[i, j] = Spawned.GetComponent<Block>();
                List<GameObject> instances = new List<GameObject>();
                Spawned.GetComponent<Block>().set_location(i, mapdata.Map_data[i].block[j].height, j);
                if (mapdata.Map_data[i].block[j].not_stable == true)
                {
                    Spawned.GetComponent<Block>().Set_stable(false);
                }
                if (mapdata.Map_data[i].block[j].Obstacle == null)
                {
                    obstacle_map[i, j] = false;
                }
                #region ������ ����
                /*
                float maximum_dis = 0.0f;
                if (i+1< mapdata.Map_data.Length)
                {
                    maximum_dis = maximum_dis < mapdata.Map_data[i].block[j].height-mapdata.Map_data[i + 1].block[j].height ? mapdata.Map_data[i].block[j].height-mapdata.Map_data[i + 1].block[j].height : maximum_dis; 
                }
                if(j+1<mapdata.Map_data[i].block.Length)
                {
                    maximum_dis = maximum_dis < mapdata.Map_data[i].block[j].height-mapdata.Map_data[i ].block[j+1].height ? mapdata.Map_data[i].block[j].height-mapdata.Map_data[i].block[j+1].height : maximum_dis;
                }
                if (i -1>0)
                {
                    maximum_dis = maximum_dis < mapdata.Map_data[i].block[j].height-mapdata.Map_data[i -1].block[j].height ? mapdata.Map_data[i].block[j].height-mapdata.Map_data[i - 1].block[j].height : maximum_dis;
                }
                if (j -1 >0)
                {
                    maximum_dis = maximum_dis < mapdata.Map_data[i].block[j].height-mapdata.Map_data[i ].block[j-1].height ? mapdata.Map_data[i].block[j].height-mapdata.Map_data[i].block[j - 1].height : maximum_dis;
                }
              
                for(float dis = maximum_dis; dis>0.5f;dis-=0.5f)
                {
                    instances .Add( Instantiate(block, new Vector3(i, mapdata.Map_data[i].block[j].height-dis, j), Quaternion.identity, down));
                     
                }*/
                #endregion
                #region 0���� ���̱��� ����
                for (float dis = mapdata.Map_data[i].block[j].height; dis > 0.5f; dis -= 0.5f)
                {
                    instances.Add(Instantiate(block, new Vector3(i, mapdata.Map_data[i].block[j].height - dis, j), Quaternion.identity, down));

                }

                #endregion
                foreach (Map_Material MT in mapdata.Map_material)
                {
                    if (mapdata.Map_data[i].block[j].type == MT.type)
                    {
                        Spawned.GetComponent<Block>().Block_obj.GetComponent<MeshRenderer>().material = MT.material;
                        Spawned.GetComponent<Block>().type = MT.type;

                        foreach (GameObject obj in instances)
                        {
                            obj.GetComponent<Block>().Block_obj.GetComponent<MeshRenderer>().material = MT.material;
                            obj.GetComponent<Block>().type = MT.type;
                        }
                    }
                }
            }
        }

    }
    //������ �����ؼ� �̵������� ���� �����ϰ� ���� ���������ؼ� ���� 
    public void MoveCheck(Character cha, bool On)
    {
        int size = (int)Get_Size();
        Block[,] blocks = new Block[size, size];


        if (On)
        {
            //current_map_data[(int)(cha.location.x + x), 0];
            //  blocks[(int)(cha.location.x + x), (int)(cha.location.z + z)] = current_map_data[(int)(cha.location.x + x), (int)(cha.location.z + z)];
            // blocks.Add(current_map_data[(int)(cha.location.x + x), (int)(cha.location.z + z)]);
            //current_map_data[(int)(cha.location.x + x), (int)(cha.location.z + z)].Area.SetActive(true);
            MoveRangeCheck(current_map_data[(int)(cha.location.x), (int)(cha.location.z)], cha.max_ingame_status.MOVE, cha.max_ingame_status.JUMP);
        }
        else
        {
            for (int x = -cha.current_ingame_status.MOVE; x <= cha.current_ingame_status.MOVE; x++)
            {
                for (int z = -(cha.current_ingame_status.MOVE - Mathf.Abs(x)); z <= (cha.current_ingame_status.MOVE - Mathf.Abs(x)); z++)
                {
                    if (x < 0)
                    {
                        if (cha.location.x + x < 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (cha.location.x + x > size - 1)
                        {
                            continue;
                        }
                    }
                    if (z < 0)
                    {
                        if (cha.location.z + z < 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (cha.location.z + z > size - 1)
                        {
                            continue;
                        }
                    }

                    current_map_data[(int)(cha.location.x + x), (int)(cha.location.z + z)].Area.SetActive(false);

                }
            }
        }

    }
    public void AreaOff()
    {
        for (int x = 0; x < Get_Size(); x++)
        {
            for (int z = 0; z < Get_Size(); z++)
            {
                current_map_data[x, z].Area.SetActive(false);
            }
        }
    }
    public float Get_Size()
    {
        return Mathf.Sqrt(current_map_data.Length); ;
    }
    public Block[,] Get_Active_Area()
    {

        int size = (int)Get_Size();
        Block[,] test = new Block[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (current_map_data[i, j].Area.activeSelf)
                {
                    test[i, j] = current_map_data[i, j];
                }
            }
        }
        return test;
    }
    public List<Transform> Get_Path(Block start, Block target, Block[,] map)
    {
        int size = (int)Get_Size();
        NODE[,] nodes = new NODE[size, size];
        List<Transform> Path = new List<Transform>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (map[i, j] == null)
                {
                    continue;
                }
                else
                {
                    if (obstacle_map[i, j] == false || ((int)start.x_location == i && (int)start.z_location == j))
                    {
                        nodes[i, j] = new NODE(map[i, j], Mathf.Abs((int)start.x_location - i) + Mathf.Abs((int)start.z_location - j), Mathf.Abs((int)target.x_location - i) + Mathf.Abs((int)target.z_location - j));
                    }
                }
            }
        }
        NODE start_node = nodes[Mathf.Abs((int)start.x_location), Mathf.Abs((int)start.z_location)];
        NODE target_node = nodes[Mathf.Abs((int)target.x_location), Mathf.Abs((int)target.z_location)];
        NODE current = null;
        List<NODE> Movable = new List<NODE>() { start_node };
        List<NODE> Not_Movable = new List<NODE>();
        while (Movable.Count > 0)
        {
            current = Movable[0];
            for (int i = 0; i < Movable.Count; i++)
            {
                if (Movable[i].f <= current.f && Movable[i].h < current.h) { current = Movable[i]; }
            }
            Debug.Log(1);
            Movable.Remove(current);
            Not_Movable.Add(current);
            if (current == target_node)
            {
                Path.Add(target.Area.transform);
                NODE TargetCurNode = target_node;
                while (TargetCurNode != start_node)
                {
                    Path.Add(TargetCurNode.block_location);
                    TargetCurNode = TargetCurNode.before_node;
                }
                Path.Add(start_node.block_location);
                Path.Reverse();
                break;

            }
            else
            {
                if (current.x + 1 < size)
                {
                    if (nodes[current.x + 1, current.z] != null)
                    {
                        if (Not_Movable.Contains(nodes[current.x + 1, current.z]) == false)
                        {
                            Movable.Add(nodes[current.x + 1, current.z]);
                            nodes[current.x + 1, current.z].Set_before_node(current);

                        }

                    }
                }
                if (current.x - 1 > 0)
                {
                    if (nodes[current.x - 1, current.z] != null)
                    {
                        if (Not_Movable.Contains(nodes[current.x - 1, current.z]) == false)
                        {
                            Movable.Add(nodes[current.x - 1, current.z]);
                            nodes[current.x - 1, current.z].Set_before_node(current);

                        }


                    }
                }
                if (current.z + 1 < size)
                {
                    if (nodes[current.x, current.z + 1] != null)
                    {
                        if (Not_Movable.Contains(nodes[current.x, current.z + 1]) == false)
                        {
                            Movable.Add(nodes[current.x, current.z + 1]);
                            nodes[current.x, current.z + 1].Set_before_node(current);
                        }
                    }

                }
                if (current.z - 1 > 0)
                {
                    if (nodes[current.x, current.z - 1] != null)
                    {
                        if (Not_Movable.Contains(nodes[current.x, current.z - 1]) == false)
                        {
                            Movable.Add(nodes[current.x, current.z - 1]);
                            nodes[current.x, current.z - 1].Set_before_node(current);
                        }

                    }
                }
            }
        }
        return Path;
    }
    //4��üũ
    public void MoveRangeCheck(Block start, int range, int height)
    {
        int current = range;
        if (current != -1)
        {
            if (start.stable)
            {
                start.Area.SetActive(true);
                for (int i = 1; i <= 4; i++)
                {
                    switch (i)
                    {
                        case 1:
                            if (start.x_location + 1 < (int)Mathf.Sqrt(current_map_data.Length))
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location + 1, (int)start.z_location].height - start.height) <= height && obstacle_map[(int)start.x_location + 1, (int)start.z_location] == false)
                                {
                                    MoveRangeCheck(current_map_data[(int)start.x_location + 1, (int)start.z_location], current - 1, height);
                                }
                            }
                            break;
                        case 2:
                            if (start.z_location + 1 < (int)Mathf.Sqrt(current_map_data.Length))
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location + 1].height - start.height) <= height && obstacle_map[(int)start.x_location, (int)start.z_location + 1] == false)
                                {
                                    MoveRangeCheck(current_map_data[(int)start.x_location, (int)start.z_location + 1], current - 1, height);
                                }
                            }
                            break;
                        case 3:
                            if (start.x_location - 1 >= 0)
                            {

                                if (Mathf.Abs(current_map_data[(int)start.x_location - 1, (int)start.z_location].height - start.height) <= height && obstacle_map[(int)start.x_location - 1, (int)start.z_location] == false)
                                {
                                    MoveRangeCheck(current_map_data[(int)start.x_location - 1, (int)start.z_location], current - 1, height);
                                }
                            }
                            break;
                        case 4:
                            if (start.z_location - 1 >= 0)
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location - 1].height - start.height) <= height && obstacle_map[(int)start.x_location, (int)start.z_location - 1] == false)
                                {
                                    MoveRangeCheck(current_map_data[(int)start.x_location, (int)start.z_location - 1], current - 1, height);
                                }
                            }
                            break;
                    }
                }
            }
        }

    }
    public void AttackRangeCheck_Straight(Block start, int range, int height,bool on)
    {
        int current = range;
        if (current != -1)
        {
            if (start.stable)
            {
             
                bool x_minus=true, x_plus=true, z_minus=true, z_plus = true;
                for (int i = 1; i < range+1; i++)
                {


                    if (start.x_location + i < (int)Mathf.Sqrt(current_map_data.Length)&& x_plus)
                    {
                        if (Mathf.Abs(current_map_data[(int)start.x_location + i, (int)start.z_location].height - start.height) <= height)
                        {
                            current_map_data[(int)start.x_location + i, (int)start.z_location].Area.SetActive(on);
                        }
                        else
                        {
                            x_plus = false;
                        }
                    }

                    if (start.z_location + i < (int)Mathf.Sqrt(current_map_data.Length)&& z_plus)
                    {
                        if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location + i].height - start.height) <= height)
                        {
                            current_map_data[(int)start.x_location , (int)start.z_location+i].Area.SetActive(on);
                        }
                        else
                        {
                            z_plus = false;
                        }
                    }

                    if (start.x_location - i >= 0&&x_minus)
                    {

                        if (Mathf.Abs(current_map_data[(int)start.x_location - i, (int)start.z_location].height - start.height) <= height)
                        {
                            current_map_data[(int)start.x_location - i, (int)start.z_location].Area.SetActive(on);
                        }
                        else
                        {
                            x_minus = false;
                        }
                    }

                    if (start.z_location - i >= 0 && z_minus)
                    {
                        if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location - i].height - start.height) <= height)
                        {
                            current_map_data[(int)start.x_location , (int)start.z_location - i].Area.SetActive(on);
                        }
                        else
                        {
                            z_minus = false;
                        }
                    }
                }
            }
        }
    }
    public void AttackRangeCheck_Circle(Block start, int range, int height ,bool on)
    {
        int current = range;
        if (current != -1)
        {
            if (start.stable)
            {
                if(on)
                {
                    start.Area.SetActive(true);
                }
                else
                {
                    start.Area.SetActive(false);
                }
                for (int i = 1; i <= 4; i++)
                {
                    switch (i)
                    {
                        case 1:
                            if (start.x_location + 1 < (int)Mathf.Sqrt(current_map_data.Length))
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location + 1, (int)start.z_location].height - start.height) <= height)
                                {
                                    AttackRangeCheck_Circle(current_map_data[(int)start.x_location + 1, (int)start.z_location], current - 1, height, on);
                                }
                            }
                            break;
                        case 2:
                            if (start.z_location + 1 < (int)Mathf.Sqrt(current_map_data.Length))
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location + 1].height - start.height) <= height)
                                {
                                    AttackRangeCheck_Circle(current_map_data[(int)start.x_location, (int)start.z_location + 1], current - 1, height, on);
                                }
                            }
                            break;
                        case 3:
                            if (start.x_location - 1 >= 0)
                            {

                                if (Mathf.Abs(current_map_data[(int)start.x_location - 1, (int)start.z_location].height - start.height) <= height)
                                {
                                    AttackRangeCheck_Circle(current_map_data[(int)start.x_location - 1, (int)start.z_location], current - 1, height, on);
                                }
                            }
                            break;
                        case 4:
                            if (start.z_location - 1 >= 0)
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location - 1].height - start.height) <= height)
                                {
                                    AttackRangeCheck_Circle(current_map_data[(int)start.x_location, (int)start.z_location - 1], current - 1, height, on);
                                }
                            }
                            break;
                    }
                }
            }
        }

    }
    public void Clear_Map()
    {
        List<Character> instance = Gamemanager.s_instance.Get_character();
        for (int i = 0; i < instance.Count; i++)
        {
            instance[i].transform.SetParent(current_map_data[(int)instance[i].location.x, (int)instance[i].location.z].Block_obj.transform);
        }
        Gamemanager.s_instance.Clear_cha_list();

    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.P))
        {
            Spawn_Map(testmap, up, down);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Clear_Map();
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            for (int i = 1; i < 5; i++)
            {
                StartCoroutine(Spawn_Routine(i / 5.0f));
            }

        }
#endif
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }



}
public class NODE
{
    public int h;
    public int g;
    public int f;
    public int x;
    public int z;

    float height;
    public Transform block_location;
    public Block block;
    public NODE before_node;
    public NODE(Block blk, int in_g, int in_h)
    {
        h = in_h;
        g = in_g;
        f = h + g;
        block = blk;
        block_location = blk.Area.transform;
        x = (int)blk.x_location;
        z = (int)blk.z_location;
        height = blk.height;
    }
    public void Set_before_node(NODE before)
    {
        before_node = before;
    }
}