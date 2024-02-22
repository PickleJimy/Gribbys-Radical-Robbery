using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public Transform player; 
    
    public UnityEngine.AI.NavMeshAgent agent;

    public LayerMask whatIsGround, whatIsPlayer;

    public Rigidbody enemyRb;

    public float jumpForce;

    //States
    public float sightRange, jumpRange;
    public bool playerInSightRange, playerInJumpRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInJumpRange = Physics.CheckSphere(transform.position, jumpRange, whatIsPlayer);
        if (playerInJumpRange && playerInSightRange) JumpAtPlayer();
    }

    private void JumpAtPlayer()
    {
        enemyRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, jumpRange);
    }
}
