using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarValue : MonoBehaviour
{
    private Slider healthSlider;
    public GameObject player;
    private GameObject playerUI;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
        health = player.GetComponent<PlayerStats>().health;
        SetHealthSlider();
        playerUI = GameObject.Find("Grab Text");
        playerUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        health = player.GetComponent<PlayerStats>().health;
        SetHealthSlider();
    }
    
    public void SetHealthSlider()
    {
        healthSlider.value = health;
    }
}
