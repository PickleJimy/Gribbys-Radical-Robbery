using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompeltionZone : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.stolenGoods = PlayerStats.stolenGoods;
            PlayerStats.health = 100;
            SceneManager.LoadScene(nextScene);
        }
    }
}
