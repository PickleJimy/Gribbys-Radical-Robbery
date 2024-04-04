using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagment: MonoBehaviour
{
    public bool isLevelComplete;

    public int stolenGoodsNeeded;

    public int currentAmountStolen;


    // Start is called before the first frame update
    void Start()
    {
        isLevelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentAmountStolen = PlayerStats.stolenGoods - GameManager.stolenGoods;

        if (currentAmountStolen >= stolenGoodsNeeded)
        {
            isLevelComplete = true;
        }
    }
}
