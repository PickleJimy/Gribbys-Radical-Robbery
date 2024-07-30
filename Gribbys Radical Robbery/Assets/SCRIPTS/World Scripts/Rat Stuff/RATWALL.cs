using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RATWALL : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    public GameObject rats;
    public GameObject[] paths;

    private int i = 0;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = rats.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        Vector3 goal = paths[i].transform.position;

        Vector3 targetDirection = goal - rats.transform.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(rats.transform.forward, targetDirection, step, 0);
        rats.transform.rotation = Quaternion.LookRotation(newDirection);

        if (rats.transform.position != goal)
        {
            float dist = Vector3.Distance(startPos, goal);

            rats.transform.position = Vector3.MoveTowards(rats.transform.position, goal, dist / speed);
        }

        if (rats.transform.position == goal)
        {
            i++;
            startPos = rats.transform.position;
        }
    }
}
