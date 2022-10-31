using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ant : MonoBehaviour
{
    public int teamNum;
    public int health;
    public int healthMax;
    public int attackBonus;


    public RectTransform UI;
    public float baseSpeed; 
    public float recoverSpeed = 0;
    public float fleeTime;

    public enum AntState
    {
       AIthinking, SeekingFood, SendFood, Attack, SeekingEnemy, Flee
    }
    public AntState currentState;


    public GameObject foodInNear;
    public GameObject foodInHand;
    public GameObject antQueen;
    public GameObject enemy;
    public GameObject deadBody;

    public BarController barController;

    private List<GameObject> foodsInNear;
    private List<GameObject> enemiesInNear;


    private NavMeshAgent agent;

    private bool isDead = false;


    void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>().antGetIntoList(teamNum, this);

        health = healthMax;
        barController.SetValue_Initial((float)healthMax);
        barController.SetValue((float)health, (float)healthMax);

        
        isDead = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = baseSpeed + 2.5f;
        float terrainHeight = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position);
        if (terrainHeight > 10f) Destroy(this);
        transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);

        //currentState = AntState.SeekingFood;
    }

    public void changeAIState(string command)
    { 
        if(command == "attack") currentState = AntState.SeekingEnemy;
        if (command == "defend") enterFlee();
        if (command == "farm") currentState = AntState.SeekingFood;
    }

    public void enterFlee()
    {
        fleeTime = 10f;
        currentState = AntState.Flee;
    }

   
    void FixedUpdate()
    {
        fleeTime -= Time.fixedDeltaTime;

        if (health <= 0 && isDead == false)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>().antDelete(teamNum, this); 

            isDead = true;
            if(Random.Range(0,2) > 0) Instantiate(deadBody, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject, .15f);
        }
        if (health <= 0) return;
        if (health <= 10)
        {
            enterFlee();
        }

        if (enemy != null)
            if (enemy.GetComponent<Ant>().teamNum == teamNum) enemy = null;
        if (enemy != null)
            if(enemy.GetComponent<Ant>().health < 0) enemy = null;
           
      
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

            case AntState.SeekingEnemy:
                checkFood();
                On_The_Way_To_Fight();
                break;

            case AntState.Attack:
                checkFood();
                Attack();
                break;

            case AntState.Flee:
                checkFood();
                Flee();
                break;
        }
        UI.rotation = Quaternion.Euler(90, 0, -this.gameObject.transform.rotation.eulerAngles.z);

        recoverSpeed += 10f * Time.fixedDeltaTime;
        if (recoverSpeed > 2.5f)
        {
            recoverSpeed = 0;
            health += 3;
        }
        if (health > healthMax) health = healthMax;
        barController.SetValue((float)health, (float)healthMax);
    }

    private void Flee()
    {
        agent.speed = baseSpeed + 3.5f;
        agent.SetDestination(antQueen.transform.position);
        health += 2;
        if(fleeTime < 0) currentState = AntState.AIthinking;
    }

    private void Attack()
    {
        agent.speed = 0.25f;
        if (enemy == null)
        {
            currentState = AntState.AIthinking;
            return;
        }
        float dist = Vector3.Distance(enemy.transform.position, this.transform.position);
        if (dist > 2f) currentState = AntState.SeekingEnemy;

  
        enemy.GetComponent<Ant>().GetDamage(Random.Range(0, 2) + attackBonus, this.gameObject);
        agent.SetDestination(enemy.transform.position);
    }

    public void GetDamage(int num, GameObject attacker)
    {
        health -= num;
      
        if (currentState == AntState.SeekingEnemy) enemy = attacker;
        if (currentState == AntState.SeekingFood)
        {
            currentState = AntState.SeekingEnemy;
            enemy = attacker; 
        }
    }

    private void On_The_Way_To_Fight()
    {
        agent.speed = baseSpeed + 2.5f;
        if (enemy == null) Fight_Choice();

        if (enemy != null)
        {
            agent.SetDestination(enemy.transform.position);

            float dist = Vector3.Distance(enemy.transform.position, this.transform.position);

            if (dist < 2f)
            {
               
                currentState = AntState.Attack;
            }
        }
    }

    private void Fight_Choice()
    {
        if (enemiesInNear != null)enemiesInNear.Clear();

        enemiesInNear = FindNearAntsWithTag("Ant");

        if (enemiesInNear == null) 
        {
            currentState = AntState.AIthinking;
            return;
        }
        enemy = enemiesInNear[Random.Range(0, enemiesInNear.Count)];
    }

    private void DecideWhatToDoNext()
    {
        agent.speed = baseSpeed;
        int choice = Random.Range(0, 3);
        if(choice == 0 || choice == 1) currentState = AntState.SeekingFood;
        if(choice == 2) currentState = AntState.SeekingEnemy;
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

        if (foodInNear != null)
        {
            if (foodInNear.GetComponent<Food>().isOnHand == true) choose_A_Food_ToMove();
            if (foodInNear != null)
                agent.SetDestination(foodInNear.transform.position);

        }
    }

    public void getFood(GameObject food)
    {
        if (foodInHand == null)
        {
            agent.speed = baseSpeed + -0.25f;

            foodInHand = food;
            foodInHand.transform.SetParent(null);
            foodInHand.transform.SetParent(gameObject.transform);
            foodInHand.transform.localPosition = new Vector3(0, 1, 1);

            currentState = AntState.SendFood;
        }
    }
    private void checkFood()
    {
        if (foodInHand == true)
        {
            if (foodInHand != null)
            {
                foodInHand.transform.SetParent(null);
                foodInHand = null;
            }
        }
    }
    private void SendFoodToQueen()
    {
        agent.SetDestination(antQueen.transform.position);
    }

    private void choose_A_Food_ToMove()
    {
        foodsInNear = FindNearObjectsWithTag("Food");
        if (foodsInNear != null) foodInNear = foodsInNear[Random.Range(0, foodsInNear.Count)];
        else currentState = AntState.SeekingEnemy;  
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


    private List<GameObject> FindNearAntsWithTag(string tag)
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

                if (distanceToCurrentObject < distanceToClosestObject)
                    if (currentObject.GetComponent<Ant>() != null)
                        if (currentObject.GetComponent<Ant>().teamNum != teamNum)
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
                    queen.birthCD += foodInHand.GetComponent<Food>().foodAmout;
                    Destroy(foodInHand);
                    agent.speed = baseSpeed + 2.5f;
                    foodInNear = null;
                    //queen.birth();
                }
            }


        if (other.tag == "Ant")
        {
            if (other.GetComponent<Ant>() != null)
            {
                Ant ant = other.GetComponent<Ant>();
                if (ant.teamNum != this.teamNum)
                {
                    enemy = other.gameObject;
                    currentState = AntState.Attack;
                }
            }
         
        }
    }
}

  