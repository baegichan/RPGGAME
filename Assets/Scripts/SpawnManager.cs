using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject point;
    public Mapdata testmap;
    public Transform up;
    public Transform down;
    public void Spawn_Map(Mapdata mapdata,Transform upblock,Transform underblock)
    {
       // Instantiate();
       for(int i =0; i<mapdata.Map_data.Length;i++)
       {
            for(int j =0;j<mapdata.Map_data.Length;j++)
            {
                GameObject Spawned =  Instantiate(point,new Vector3(i,mapdata.Map_data[i].block[j].height,j),Quaternion.identity, upblock);
                
                List<GameObject> instances = new List<GameObject>();

                #region 높이차 스폰
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
                    instances .Add( Instantiate(point, new Vector3(i, mapdata.Map_data[i].block[j].height-dis, j), Quaternion.identity, down));
                     
                }*/
                #endregion
                #region 0부터 높이까지 스폰
                for (float dis = mapdata.Map_data[i].block[j].height; dis > 0.5f; dis -= 0.5f)
                {
                    instances.Add(Instantiate(point, new Vector3(i, mapdata.Map_data[i].block[j].height - dis, j), Quaternion.identity, down));

                }
                
                #endregion
                foreach (Map_Material MT in mapdata.Map_material)
                {
                    if(mapdata.Map_data[i].block[j].type==MT.type)
                    {
                        Spawned.GetComponent<Block>().Point.GetComponent<MeshRenderer>().material = MT.material;
                        Spawned.GetComponent<Block>().type = MT.type;
                        foreach(GameObject obj in instances)
                        {
                            obj.GetComponent<Block>().Point.GetComponent<MeshRenderer>().material = MT.material;
                            obj.GetComponent<Block>().type = MT.type;
                        }
                    }
                }   
            }
       }
    }
    public void Clear_Map()
    {
       
      

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            Spawn_Map(testmap, up, down);
        }
    }
#if UNITY_EDITOR_WIN
    private void Awake()
    {
        
    }
 
#endif

}
