using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Range(0, 100)]
    public int health;
    
    private int damageDealt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon") && other.GetComponent<PlayerDealDamage>().dealDamage)
        {
            damageDealt = other.GetComponent<PlayerDealDamage>().damage;
            health -= damageDealt;
        }
    }
}
