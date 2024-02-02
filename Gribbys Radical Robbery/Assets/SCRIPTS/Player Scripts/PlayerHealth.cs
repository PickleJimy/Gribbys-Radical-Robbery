using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(0,100)]
    public int health;

    public int damageDealt;

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
        if (other.tag == "EnemyDamager" && other.GetComponent<EnemyDealDamage>().dealDamage)
        {
            Debug.Log("YEEOWCH");
            damageDealt = other.GetComponent<EnemyDealDamage>().damage;
            health -= damageDealt;
        }
    }
}
