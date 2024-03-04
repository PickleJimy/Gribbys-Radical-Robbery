using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour
{
    [Range(0, 100)]
    public int damage;

    public bool dealDamage;

    Collider damageZone;

    private void Start()
    {
        damageZone = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!dealDamage) 
        {
            damageZone.enabled = false;
        }

        else
        {
            damageZone.enabled = true;
        }
    }
}
