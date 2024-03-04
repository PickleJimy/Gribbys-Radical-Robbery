using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public Transform player; 

    public LayerMask whatIsGround, whatIsPlayer;

    //States
    public float jumpRange;
    public bool playerInJumpRange;

    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    public void Update()
    {
        playerInJumpRange = Physics.CheckSphere(transform.position, jumpRange, whatIsPlayer);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, jumpRange);
    }
}
