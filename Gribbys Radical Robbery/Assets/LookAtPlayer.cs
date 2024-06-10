using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    public bool canSeePlayer;

    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canSeePlayer = Enemy.GetComponent<FieldOfView>().canSeePlayer;

        if (canSeePlayer)
        {
            transform.LookAt(player);
        }
    }
}
