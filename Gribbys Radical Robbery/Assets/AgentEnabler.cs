using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentEnabler : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public GameObject enemy;
    bool enemyJump;

    // Start is called before the first frame update
    public void Awake()
    {
        agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        enemy = GameObject.Find("Enemy");
        enemyJump = enemy.GetComponent<EnemyAiTutorial>().enemyJump;
    }

    // Update is called once per frame
    public void Update()
    {
        if (enemyJump)
        {
            agent.enabled = false;
        }

        if (!agent.enabled)
        {
            Debug.Log("Deactivate!");
        }
    }
}
