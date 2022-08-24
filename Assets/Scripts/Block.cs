using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject Point;
    public Vector3[] path = new Vector3[3];
    
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
        Point.SetActive(true);
    }
    public void Block_Move()
    {   
        iTween.MoveTo(Point, iTween.Hash("path", path, "time", 1, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "movetopath", false));
    }
    private void Awake()
    {
        //TESTCODE
        if(Point==null)
        {
            Point = transform.GetChild(0).gameObject;
        }
        path[0] = (Point.transform.position);
        path[1] = (this.transform.position + new Vector3(0, 1 * Random.Range(0.8f, 1.0f), 0));
        path[2] = (this.transform.position );
        StartCoroutine(SpawnBlock());
    }
    // Start is called before the first frame update
    void Start()
    {



        Block_Move();
    }
}
