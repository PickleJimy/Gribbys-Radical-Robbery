using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    // It will move during attack
    Rigidbody enemyRig;
    public Transform player;
    public float magnitude;
    public void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = transform.position;
        Vector3 b = player.position;

        enemyRig = GetComponent<Rigidbody>();
        enemyRig.AddForce(Vector3.MoveTowards(a, b, magnitude));
        transform.position = a;
    }
}
