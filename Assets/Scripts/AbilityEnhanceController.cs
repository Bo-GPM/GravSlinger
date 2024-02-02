using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEnhanceController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.instance.playerController.abilityAwakenIndex++;
            gameObject.SetActive(false);
        }
        
    }
}
