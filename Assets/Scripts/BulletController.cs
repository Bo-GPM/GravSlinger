using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletRB;
    [SerializeField] private float ignoreDistance = 100f;
    [SerializeField] private float attractionForce = 5.0f;
    [SerializeField] private int damageNumber = 1;
    [SerializeField] private float distancePowerFactor = 1.3f;
    [SerializeField] private float maxSurviveTime = 8.0f;
    private float currentSurviveTime = 0;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        CheckSurviveTime();
    }

    private void CheckSurviveTime()
    {
        if (currentSurviveTime > maxSurviveTime)
        {
            Destroy(gameObject);
            LevelManager.instance.objectsToDestoryAfterDeath.Remove(gameObject);
        }
        else
        {
            currentSurviveTime += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        // Check awakenIndex first
        if (LevelManager.instance.playerController.abilityAwakenIndex >= 1)
        {
            // Then see if the bullet is close to the player enough
            if (IsCloseEnough())
            {
                bulletRB.AddForce(CalculateForce(), ForceMode2D.Impulse); 
            }
        }
    }

    private bool IsCloseEnough()
    {
        // Debug.Log(LevelManager.instance.transform.position.x);
        Vector3 mag = LevelManager.instance.player.transform.position - transform.position;
        bool tempBool = mag.magnitude < ignoreDistance;
        // Debug.Log(tempBool);
        return tempBool;
    }

    private Vector3 CalculateForce()
    {
        Vector3 difference = LevelManager.instance.player.transform.position - transform.position;
        float distance = difference.magnitude;
        float forceScale = attractionForce / (Mathf.Pow(distance, distancePowerFactor));
        Vector3 nomForceVector3 = difference.normalized;
        return nomForceVector3 * forceScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            LevelManager.instance.objectsToDestoryAfterDeath.Remove(gameObject);
        }

        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            LevelManager.instance.objectsToDestoryAfterDeath.Remove(gameObject);
            LevelManager.instance.UpdateHP(-damageNumber);
        }
        // else
        // {
        //     Destroy(gameObject);
        //     LevelManager.instance.objectsToDestoryAfterDeath.Remove(gameObject);
        // }
    }
}
