using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject Block_obj;
    public GameObject Area;
    public Vector3[] path = new Vector3[3];
    
    public bool stable;
    public float height;
    public float x_location;
    public float z_location;
    
    

    public void set_location(float x, float y, float z)
    {
        x_location = x;
        height = y;
        z_location = z;
    }
    public void set_location(Vector3 location)
    {
        x_location = location.x;
        height = location.y;
        z_location = location.z;
    }
    public enum BLOCK_TYPE
    {
     TEST,
     FOREST,
     RIVER,
     ROCK,
     GRASS
    }
    public BLOCK_TYPE type = BLOCK_TYPE.TEST;
    IEnumerator SpawnBlock()
    {
        yield return new WaitForSeconds(Random.Range(0.0f,1.5f));
        Block_obj.SetActive(true);
    }
    IEnumerator DelBlock()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 1.5f));
        Block_Down();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    public void Block_Move()
    {   
        iTween.MoveTo(Block_obj, iTween.Hash("path", path, "time", 1, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "movetopath", false));
    }
    public void Block_Down()
    {
        iTween.MoveTo(Block_obj, Block_obj.transform.position-new Vector3(0,30,0),9);
        
    }
    public void Set_stable(bool value)
    {
        stable = value;
    }
    private void Awake()
    {
        //TESTCODE
        if(Block_obj == null)
        {
            Block_obj = transform.GetChild(0).gameObject;
        }
        path[0] = (Block_obj.transform.position);
        path[1] = (this.transform.position + new Vector3(0, 1 * Random.Range(0.8f, 1.0f), 0));
        path[2] = (this.transform.position );
        StartCoroutine(SpawnBlock());
    }
    // Start is called before the first frame update
    void Start()
    {
        Block_Move();
    }
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(DelBlock());
        }
    }
}
