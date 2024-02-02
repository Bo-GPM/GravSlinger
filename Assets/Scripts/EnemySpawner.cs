using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float timeToRespawn = 5.0f;
    private float currentTime;
    private GameObject theEnemy;
    private bool theEnemyIsDestroyed;
    
    
    
    // void Start()
    // {
    //     GameObject tempObj = Instantiate(prefabToSpawn, transform.position, transform.rotation);
    //     LevelManager.instance.objectsToDestoryAfterDeath.Add(tempObj);
    // }

    private void Update()
    {
        if (theEnemyIsDestroyed)
        {
            currentTime += Time.deltaTime;
        }

        if (currentTime > timeToRespawn)
        {
            currentTime = 0;
            respawnObject();
        }
    }

    private void CheckEnemyIsDestroyed()
    {
        if (theEnemy == null)
        {
            theEnemyIsDestroyed = true;
        }
    }

    public void respawnObject()
    {
        theEnemy = Instantiate(prefabToSpawn, transform.position, transform.rotation);
        LevelManager.instance.objectsToDestoryAfterDeath.Add(theEnemy);
    }
}
