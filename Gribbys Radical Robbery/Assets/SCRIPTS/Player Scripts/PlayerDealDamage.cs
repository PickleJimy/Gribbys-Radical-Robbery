using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDealDamage : MonoBehaviour
{
    public GameObject player;
    
    public int damage;

    public bool dealDamage;

    Collider damageZone;

    // Start is called before the first frame update
    void Start()
    {
        damageZone = GetComponent<Collider>();
        damage = player.GetComponent<GrabAndStab>().damage;
    }

    // Update is called once per frame
    void Update()
    {
        dealDamage = player.GetComponent<GrabAndStab>().dealDamage;

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
