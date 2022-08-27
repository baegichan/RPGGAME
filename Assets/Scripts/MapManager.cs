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

    public void Spawn_Cha(GameObject default_cha,Vector3  point)
    {
       
        GameObject ins= Instantiate(default_cha, new Vector3(point.x, current_map_data[(int)point.x, (int)point.z].Block_obj.transform.position.y+0.5f, point.z), Quaternion.identity);
        ins.GetComponent<Character>().location = point;
    }
    IEnumerator Spawn_Routine(float time)
    {
        yield return new WaitForSeconds(time);
        Spawn_Cha(testCha, new Vector3(UnityEngine.Random.Range(0, 10), 0, UnityEngine.Random.Range(0, 10)));
    }
    public void Spawn_Map(Mapdata mapdata,Transform upblock,Transform underblock)
    {
        current_map_data = null;
        current_map_data = new Block[mapdata.Map_data.Length, mapdata.Map_data.Length];
        // Instantiate();
        for (int i =0; i<mapdata.Map_data.Length;i++)
       {
            for(int j =0;j<mapdata.Map_data.Length;j++)
            {
                GameObject Spawned =  Instantiate(block, new Vector3(i,mapdata.Map_data[i].block[j].height,j),Quaternion.identity, upblock);
                current_map_data[i, j] = Spawned.GetComponent<Block>();
                List<GameObject> instances = new List<GameObject>();
                Spawned.GetComponent<Block>().set_location(i, mapdata.Map_data[i].block[j].height, j);
                if(mapdata.Map_data[i].block[j].not_stable == true)
                {
                    Spawned.GetComponent<Block>().Set_stable(false);
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
                    if(mapdata.Map_data[i].block[j].type==MT.type)
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
    public void MoveCheck(Character cha ,bool On)
    {
        int size =(int)Mathf.Sqrt(current_map_data.Length);
        Block[,] blocks = new Block[size, size];


        if (On)
        {
            //current_map_data[(int)(cha.location.x + x), 0];
            //  blocks[(int)(cha.location.x + x), (int)(cha.location.z + z)] = current_map_data[(int)(cha.location.x + x), (int)(cha.location.z + z)];
            // blocks.Add(current_map_data[(int)(cha.location.x + x), (int)(cha.location.z + z)]);
            //current_map_data[(int)(cha.location.x + x), (int)(cha.location.z + z)].Area.SetActive(true);
            MoveCheck(current_map_data[(int)(cha.location.x), (int)(cha.location.z)], cha.max_ingame_status.MOVE, cha.max_ingame_status.JUMP);
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

    //4��üũ
    public void MoveCheck(Block start,int range,int height)
    {
        int current = range;
        if(current !=-1)
        {
            if (start.stable)
            {
                start.Area.SetActive(true);
                for (int i = 1; i <= 4; i++)
                {
                    switch (i)
                    {
                        case 1:
                            if (start.x_location+1  < (int)Mathf.Sqrt(current_map_data.Length))
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location + 1, (int)start.z_location].height - start.height) <= height)
                                {
                                    MoveCheck(current_map_data[(int)start.x_location + 1, (int)start.z_location], current - 1, height);
                                }
                            }
                            break;
                        case 2:
                            if (start.z_location+1 < (int)Mathf.Sqrt(current_map_data.Length))
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location + 1].height - start.height) <= height)
                                {
                                    MoveCheck(current_map_data[(int)start.x_location, (int)start.z_location + 1], current - 1, height);
                                }
                            }
                            break;
                        case 3:
                            if (start.x_location - 1 >= 0)
                            {

                                if (Mathf.Abs(current_map_data[(int)start.x_location - 1, (int)start.z_location].height - start.height) <= height)
                                {
                                    MoveCheck(current_map_data[(int)start.x_location - 1, (int)start.z_location], current - 1, height);
                                }
                            }
                            break;
                        case 4:
                            if (start.z_location - 1 >= 0)
                            {
                                if (Mathf.Abs(current_map_data[(int)start.x_location, (int)start.z_location - 1].height - start.height) <= height)
                                {
                                    MoveCheck(current_map_data[(int)start.x_location, (int)start.z_location - 1], current - 1, height);
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
        for(int i=0; i<instance.Count;i++)
        {
            instance[i].transform.SetParent(current_map_data[(int)instance[i].location.x, (int)instance[i].location.z].Block_obj.transform);
        }
        Gamemanager.s_instance.Clear_cha_list();
        
    }
    private void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.P))
        {
            Spawn_Map(testmap, up, down);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Clear_Map();
        }
        if(Input.GetKeyUp(KeyCode.O))
        {
            for(int i=1; i<5;i++)
            {
                StartCoroutine(Spawn_Routine(i / 5.0f));
            }
           
        }
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
    public NODE(Block block)
    {
        this.block = block;
    }
    Block block;

}
