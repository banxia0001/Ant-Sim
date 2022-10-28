using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGene : MonoBehaviour
{
    public GameObject[] pool;
    private float birthCD;
    public GameObject food;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        birthCD += Time.fixedDeltaTime;
        if (birthCD > 5f)
        {
            birthCD = 0f;
            Instantiate(food, pool[Random.Range(0,pool.Length)].transform.position, Quaternion.identity);
        }
    }
}
