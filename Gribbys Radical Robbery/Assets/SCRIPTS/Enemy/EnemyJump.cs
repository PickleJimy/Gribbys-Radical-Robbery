using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public Transform player;

    public Transform enemy;

    public LayerMask whatIsPlayer;

    //States

    public float jumpHeightDetect;
    public bool playerInJumpRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    public void Update()
    {
        playerInJumpRange = Physics.CheckSphere(transform.position, jumpHeightDetect, whatIsPlayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, jumpHeightDetect);
    }
}
