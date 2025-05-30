using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseEnemies : MonoBehaviour
{
    [Header("Finding the closest attacking enemy")]
    public GameObject[] AllObjects;
    public GameObject NearestEnemy;
    float distance;
    public float nearestDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < AllObjects.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);

            if (distance < nearestDistance)
            {
                NearestEnemy = AllObjects[i];
                nearestDistance = distance;
            }
        }
    }
}
