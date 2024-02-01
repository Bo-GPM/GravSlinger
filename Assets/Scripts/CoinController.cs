using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CoinController : MonoBehaviour
{
    public UnityEvent pickupCoin;

    [SerializeField] private int coinWorth;
    private void OnTriggerEnter2D(Collider2D other)
    {
        pickupCoin.Invoke();
        LevelManager.instance.AddCoins(coinWorth);
        // Debug.Log("Collision Detected");
        // LevelManager.instance.AddCoins(1000);
    }
}
