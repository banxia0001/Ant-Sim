using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Queen : MonoBehaviour
{
    public int teamNum;
  
    public GameObject Ass;
    public GameObject AntPrefab;
    private NavMeshAgent agent;

    void Start()
    {
     

        float terrainHeight = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position);
        if (terrainHeight > 10f) Destroy(this);
        transform.position = new Vector3(transform.position.x, terrainHeight - .3f, transform.position.z);

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();    
    }


    private float moveTime;
    public void FixedUpdate()
    {
        //moveTime += Time.fixedDeltaTime;
        //if (moveTime > 3f)
        //{
        //    moveTime = 0f;
        //    Vector3 randomNearbyPosition;
        //    randomNearbyPosition = transform.position + 3f * Random.insideUnitSphere;
        //    agent.SetDestination(randomNearbyPosition);
        //}

    }

    public void birth()
    {
        GameObject antBirthed = Instantiate(AntPrefab, new Vector3(Ass.transform.position.x, 0.5f, Ass.transform.position.z), Quaternion.identity) as GameObject;
        antBirthed.GetComponent<Ant>().teamNum = this.teamNum;
        antBirthed.GetComponent<Ant>().antQueen = this.gameObject;

    }


}
