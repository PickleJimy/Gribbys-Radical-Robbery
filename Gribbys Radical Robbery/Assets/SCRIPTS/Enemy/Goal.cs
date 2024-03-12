using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public float posRange;
    public bool enemyInPosRange, playerInPosRange;

    public Transform player;
    public Transform enemy;

    public LayerMask whatIsEnemy, whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        enemy = GameObject.Find("Enemy").transform;
    }

    // Update is called once per frame
    public void Update()
    {
        enemyInPosRange = Physics.CheckSphere(transform.position, posRange, whatIsEnemy);
        playerInPosRange = Physics.CheckSphere(transform.position, posRange, whatIsPlayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, posRange);
    }
}
