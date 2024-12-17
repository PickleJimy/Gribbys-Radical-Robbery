using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    // It will move during attack
    Rigidbody enemyRig;

    public float magnitude;
    public void Awake()
    {

    }
    void Start()
    {
        enemyRig = GetComponent<Rigidbody>();
        enemyRig.AddForce(Vector3.forward*magnitude,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
