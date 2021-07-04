using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManger : MonoBehaviour
{
    public Text coinUI;
    public int coinCount;
    public Movement movement;


    // Start is called before the first frame update
    void Start()
    {
        coinCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EventHandler updateCoins;

    public void handleCoinCollected()
    {
        Debug.Log(coinCount);
        coinCount++;
        coinUI.text = "Coins: " + Convert.ToInt32(coinCount);
        if (coinCount >= 3)
        {
            movement.canUseJetPack = true;
            
        }
        
    }

    public void display()
    {
        coinUI.text = "Coins: " + Convert.ToInt32(coinCount);
    }
}
