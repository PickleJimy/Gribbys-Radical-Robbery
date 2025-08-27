using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RATWALL : MonoBehaviour
{
    [Header("Speed!!!")]
    public float speed;
    private float startSpeed;
    //private float acceleration = 0.001f;

    private float rotationTime;
    private float dist;
    
    [Header("object references")]
    public GameObject rats;
    public GameObject player;
    public GameObject[] paths;

    private int i = 0;
    private Vector3 startPos;
    private Quaternion startRot;

    // Start is called before the first frame update
    void Start()
    {
        startPos = rats.transform.position;
        startRot = rats.transform.rotation;
        startSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        Transform goal = paths[i].transform;

        rats.transform.rotation = Quaternion.Slerp(startRot, goal.rotation, rotationTime);

        //speed += acceleration;

        if(Vector3.Distance(player.transform.position, rats.transform.position) < 3)
        {
            speed = 0;
        }

        if(Vector3.Distance(player.transform.position, rats.transform.position) >= 3)
        {
            speed = startSpeed;
        }

        if (rats.transform.position != goal.position)
        {
            rats.transform.position = Vector3.MoveTowards(rats.transform.position, goal.position, 10 * (speed/100));

            dist = Vector3.Distance(startPos, goal.position);

            rotationTime = Vector3.Distance(startPos, rats.transform.position) / dist;
        }

        if (rats.transform.position == goal.position)
        {
            i++;
            startPos = rats.transform.position;
            startRot = rats.transform.rotation;
            
        }
    }
}
