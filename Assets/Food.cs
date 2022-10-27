using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public bool isOnHand;
    void Start()
    {
        float terrainHeight = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position);
        if (terrainHeight > 10f) Destroy(this);
        transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant" && !isOnHand)
        {
            isOnHand = true;
            other.GetComponent<Ant>().getFood(this.gameObject);
        }      
    }
}
