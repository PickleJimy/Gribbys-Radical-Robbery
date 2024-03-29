using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseGround : MonoBehaviour
{
    [Header("Finding the closest thing")]
    public GameObject[] AllObjects;
    public GameObject NearestGround;
    float distance;
    public float nearestDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AllObjects = GameObject.FindGameObjectsWithTag("HighGround");

        for (int i = 0; i < AllObjects.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);

            if (distance < nearestDistance)
            {
                NearestGround = AllObjects[i];
                nearestDistance = distance;
            }
        }
    }
}
