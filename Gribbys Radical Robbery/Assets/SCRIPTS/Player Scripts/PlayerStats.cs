using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    public static int health = Mathf.Clamp(100, 0, 100);

    public int damageDealt;

    public static int stolenGoods;

    public TextMeshProUGUI stolenGoodsCounter;
    public GameObject grabText;
    public static string stealableNameText;
    public TextMeshProUGUI stealableName;
    public static GameObject player;

    public Animator GribbyTakesDamage;


    void Start()
    {
        player = gameObject;
        Invoke(nameof(DisableGrabText), 0.01f);
    }

    void DisableGrabText()
    {
        grabText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSGC();
        UpdateSNT();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyDamager") && other.GetComponent<EnemyDealDamage>().dealDamage)
        {
            Debug.Log("YEEOWCH");
            GribbyTakesDamage.SetTrigger("isDamaged");
            damageDealt = other.GetComponent<EnemyDealDamage>().damage;
            health -= damageDealt;
        }
    }

    public void UpdateSGC()
    {
        stolenGoodsCounter.text = "Goods Stolen: " + stolenGoods;
    }

    public void UpdateSNT()
    {
        if (grabText.activeSelf)
        {
            stealableName.text = stealableNameText;
        }
    }
}
