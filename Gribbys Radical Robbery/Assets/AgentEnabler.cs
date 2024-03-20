using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentEnabler : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public GameObject enemy;
    bool playerJump;

    // Start is called before the first frame update
    void Start()
    {
        agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        enemy = GameObject.Find("Enemy");
        playerJump = player.GetComponent<PlayerMovement>().isJumping;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerJump)
        {
            agent.enabled = false;
        }
    }
}
