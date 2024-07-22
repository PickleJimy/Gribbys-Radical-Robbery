using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerDirectionPoints : MonoBehaviour
{
    [Header("Finding the closest thing")]
    public GameObject[] AllObjects;
    public GameObject NearestPoint;
    float distance;
    public float nearestDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        AllObjects = GameObject.FindGameObjectsWithTag("MovementPoint");

        for (int i = 0; i < AllObjects.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);

            if (distance < nearestDistance)
            {
                NearestPoint = AllObjects[i];
                nearestDistance = distance;
            }
        }
    }
}
