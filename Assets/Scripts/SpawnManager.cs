using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject point;
    public Mapdata testmap;
    public Transform center;
    public void Spawn_Map(Mapdata mapdata,Transform target)
    {
       // Instantiate();
       for(int i =0; i<mapdata.Map_data.Length;i++)
       {
            for(int j =0;j<mapdata.Map_data.Length;j++)
            {
             GameObject Spawned =  Instantiate(point,new Vector3(i,mapdata.Map_data[i].block[j].height,j),Quaternion.identity,target);
                
                foreach(Map_Material MT in mapdata.Map_material)
                {
                    if(mapdata.Map_data[i].block[j].type==MT.type)
                    {
                        Spawned.GetComponent<Block>().Point.GetComponent<MeshRenderer>().material = MT.material;
                        Spawned.GetComponent<Block>().type = MT.type;
                    }
                }   
            }
       }
       
  
    }
#if UNITY_EDITOR_WIN
    private void Awake()
    {
        Spawn_Map(testmap, center);
    }
#endif

}
