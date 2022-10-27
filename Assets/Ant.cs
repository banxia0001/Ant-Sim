using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ant : MonoBehaviour
{
    public enum AntState
    {
       AIthinking, SeekingFood, SendFood, Attack
    }
    public AntState currentState;
    public int teamNum;

    public GameObject foodInNear;
    public GameObject foodInHand;
    public GameObject antQueen;

    public List<GameObject> foodsInNear;

    public int health = 5;
    public float speed = 5f;
    private NavMeshAgent agent;

   



    void Start()
    {
        //GameObject Queen[] = GameObject.FindGameObjectsWithTag("AntQueen");
        //foreach (GameObject queen in Queen)
        //{
        //    if (queen.GetComponent<Queen>().teamNum == this.teamNum) antQueen = queen;
        //}

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        float terrainHeight = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position);
        if (terrainHeight > 10f) Destroy(this);
        transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);

        currentState = AntState.SeekingFood;
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case AntState.AIthinking:
                DecideWhatToDoNext();
                break;

            case AntState.SeekingFood:
                SeekingFood();
                break;

            case AntState.SendFood:
                SendFoodToQueen();
                break;

            case AntState.Attack:
                //keepMoving();
                break;
        }

    }


    private void DecideWhatToDoNext()
    {
        int choice = Random.Range(0, 2);
        if(choice == 0) currentState = AntState.SeekingFood;
        //if(choice == 1) currentState = AntState.SeekingFood;
    }


    private void SeekingFood()
    {
        if (foodsInNear != null) foodsInNear.Clear();

        if (foodInHand != null)
        {
            currentState = AntState.SendFood;
            return;
        }

        if (foodInNear == null)
        {
            choose_A_Food_ToMove();
        }
        if (foodInNear.GetComponent<Food>().isOnHand == true)
        {
            choose_A_Food_ToMove();
        }

        agent.SetDestination(foodInNear.transform.position);
    }

    public void getFood(GameObject food)
    {
        if (foodInHand == null)
        {
            foodInHand = food;
            foodInHand.transform.SetParent(null);
            foodInHand.transform.SetParent(gameObject.transform);
            foodInHand.transform.localPosition = new Vector3(0, 1, 1);

            currentState = AntState.SendFood;
        }
    }

    private void SendFoodToQueen()
    {
        agent.SetDestination(antQueen.transform.position);
    }

    private void choose_A_Food_ToMove()
    {
        foodsInNear = FindNearObjectsWithTag("Food");
        foodInNear = foodsInNear[Random.Range(0, foodsInNear.Count)];
    }

    private List<GameObject> FindNearObjectsWithTag(string tag)
    {
        GameObject[] objectWithTag = GameObject.FindGameObjectsWithTag(tag);
        if (objectWithTag.Length == 0)
            return null;

        List<GameObject> check = new List<GameObject>();

        for (int i2 = 0; i2 < 3; i2++)
        {
            GameObject closestObject = objectWithTag[0];

            float distanceToClosestObject = 1e6f,
                  distanceToCurrentObject;

            for (int i = 0; i < objectWithTag.Length; i++)
            {
                Vector3 vectorToCurrent;
                GameObject currentObject;
                currentObject = objectWithTag[i];
                vectorToCurrent = currentObject.transform.position - transform.position;
                distanceToCurrentObject = vectorToCurrent.magnitude;


                if (distanceToCurrentObject < distanceToClosestObject && currentObject.GetComponent<Food>().isOnHand == false)
                {
                    bool isInList = false;
                    if (check != null)
                        if (check.Count > 0)
                            for (int i1 = 0; i1 < check.Count; i1++)
                            {
                                if (check[i1] == objectWithTag[i]) isInList = true;
                            }

                    if (isInList == false)
                    {
                        closestObject = objectWithTag[i];
                        distanceToClosestObject = distanceToCurrentObject;
                    }
                }
            }
            check.Add(closestObject);
        }
        return check;
    }


    public void OnTriggerEnter(Collider other)
    {
        if(currentState == AntState.SendFood)
            if (other.tag == "AntQueen")
            {
                Queen queen = other.GetComponent<Queen>();

                if (foodInHand != null && teamNum == queen.teamNum)
                {
                    currentState = AntState.AIthinking;
                    Destroy(foodInHand);
                    foodInNear = null;
                    queen.birth();
                }
            }
    }
}

  