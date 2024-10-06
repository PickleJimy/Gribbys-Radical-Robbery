using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatPidDamage : MonoBehaviour
{
    float elapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 0.5f)
        {
            elapsed = elapsed % 0.5f;
            gameObject.GetComponent<EnemyDealDamage>().dealDamage = !gameObject.GetComponent<EnemyDealDamage>().dealDamage;
        }
    }
}
