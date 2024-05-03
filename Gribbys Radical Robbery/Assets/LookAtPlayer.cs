using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    private Transform Enemy;
    private Transform MeleeEnemy;

    public bool playerInSightRange;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = GameObject.Find("Enemy").transform;
        MeleeEnemy = GameObject.Find("Melee Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Enemy.GetComponent<EnemyAiTutorial>().playerInSightRange;

        if (playerInSightRange)
        {
            transform.LookAt(player);
        }
    }
}
