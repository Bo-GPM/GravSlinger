using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CheckPointController : MonoBehaviour
{
    // public UnityEvent checkPointActivated;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // checkPointActivated.Invoke();
            LevelManager.instance.ActivateCheckPoint(gameObject);
            Debug.Log("CheckPoint Triggered");
        }
    }
    
}