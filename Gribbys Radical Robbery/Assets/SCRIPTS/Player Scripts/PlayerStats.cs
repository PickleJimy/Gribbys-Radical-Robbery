using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Range(0,100)]
    public int health;

    public int damageDealt;

    public int stolenGoods;

    public TextMeshProUGUI stolenGoodsCounter;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSGC();
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

    public void UpdateSGC()
    {
        stolenGoodsCounter.text = "Goods Stolen: " + stolenGoods;
    }
}
