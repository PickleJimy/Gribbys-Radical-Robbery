using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Range(0, 100)]
    public int health;
    
    private int damageDealt;

    private bool mouseOver;

    // Start is called before the first frame update
    void Start()
    {
        mouseOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon") && mouseOver && other.GetComponent<PlayerDealDamage>().dealDamage)
        {
            Debug.Log("W");
            damageDealt = other.GetComponent<PlayerDealDamage>().damage;
            health -= damageDealt;
        }
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
