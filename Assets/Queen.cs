using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Queen : MonoBehaviour
{
    public int teamNum;
    public RectTransform UI;
    public GameObject Ass;
    public GameObject AntPrefab;
    public GameObject GiantAntPrefab;

    public NavMeshAgent agent;
    public GameObject FatherFolder;
    public BarController barController;
    public float birthCD;

    public bool isAttacking;

    void Start()
    {
        barController.SetValue_Initial(0);
        barController.SetValue((float)birthCD, 15f);
        float terrainHeight = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position);
        if (terrainHeight > 10f) Destroy(this);
        transform.position = new Vector3(transform.position.x, terrainHeight - .3f, transform.position.z);

        //agent = GetComponent<UnityEngine.AI.NavMeshAgent>();    
    }


    private float moveTime;
    public void FixedUpdate()
    {
        birthCD += Time.fixedDeltaTime;
        if (birthCD > 15f)
        {
            birthCD -= 15f;
            birth();
        }

        moveTime += Time.fixedDeltaTime;
        if (moveTime > 3f)
        {
            moveTime = 0f;
            Vector3 randomNearbyPosition;
            randomNearbyPosition = transform.position + 3f * Random.insideUnitSphere;
            agent.SetDestination(randomNearbyPosition);
        }


        //Collider[] overlappingAnt;
        //overlappingItems = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Ant"));
        //if (overlappingItems.Length > 0)
        //{

        //}
        UI.rotation = Quaternion.Euler(90, 0, -FatherFolder.transform.rotation.eulerAngles.z);
        barController.SetValue((float)birthCD, 15f);
    }


    //public GameObject enemy;

    public void birth()
    {
        GameObject pref = AntPrefab;
        if (Random.Range(0, 5) == 1) pref = GiantAntPrefab;
         GameObject antBirthed = Instantiate(pref, new Vector3(Ass.transform.position.x, 0.5f, Ass.transform.position.z), Quaternion.identity) as GameObject;
        antBirthed.GetComponent<Ant>().teamNum = this.teamNum;
        antBirthed.GetComponent<Ant>().antQueen = this.gameObject;
    }


    public void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Ant")
        {
            Ant ant = other.GetComponent<Ant>();
            if (ant.teamNum != this.teamNum)
            {
                ant.health -= 10;
                ant.enterFlee();
            }
        }
    }

}
