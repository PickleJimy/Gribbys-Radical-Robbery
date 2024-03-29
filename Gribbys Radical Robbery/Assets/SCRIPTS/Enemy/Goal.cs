using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public float posRange;
    public bool enemyInPosRange, playerInPosRange;

    public Transform player;
    public Transform enemy;
    public GameObject goal;

    public bool playerOnGround;

    public LayerMask whatIsEnemy, whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        enemy = GameObject.Find("Enemy").transform;
        goal = GameObject.Find("Goal").gameObject;
    }

    // Update is called once per frame
    public void Update()
    {
        enemyInPosRange = Physics.CheckSphere(transform.position, posRange, whatIsEnemy);
        playerInPosRange = Physics.CheckSphere(transform.position, posRange, whatIsPlayer);
        playerOnGround = player.GetComponent<PlayerMovement>().grounded;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, posRange);
    }
}
