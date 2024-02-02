using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private bool onlyActiveInScreen = false;
    [SerializeField] private bool aimingAtPlayer;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxHP;
    private int currentHP;
    [SerializeField] private int damageNumber;
    [SerializeField] private float pushBackForce = 5.0f;

    [Header("Shooting")] 
    [SerializeField] private bool isShooting;
    [SerializeField] private Transform ejectorTransform;
    [SerializeField] private float shootInterval;
    private float remainShootInterval;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletToShoot;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAlive();
        EnemyMove();
        
        // TODO: (Optional) if perf is tanking, use the code below.
        // if (checkOnScreen() && onlyActiveInScreen)
        {
            if (aimingAtPlayer)
            {
                AimAtPlayer();
            }

            if (isShooting)
            {
                ShootBullets();
            }
        }
    }

    private void CheckAlive()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
            LevelManager.instance.objectsToDestoryAfterDeath.Remove(gameObject);
        }
    }

    private void EnemyMove()
    {
        if (movementSpeed != 0)
        {
            transform.position += transform.right * (movementSpeed * Time.deltaTime);
        }
    }
    
    private void ShootBullets()
    {
        if (remainShootInterval < 0)
        {
            remainShootInterval = shootInterval;
            GameObject bullet = Instantiate(bulletToShoot, ejectorTransform.position, ejectorTransform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(ejectorTransform.right * bulletSpeed, ForceMode2D.Impulse);
            
            LevelManager.instance.objectsToDestoryAfterDeath.Add(bullet);
        }
        else
        {
            remainShootInterval -= Time.deltaTime;
        }
    }
    private void AimAtPlayer()
    {
        Vector2 direction = LevelManager.instance.player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion tempQ = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = tempQ;
        // Debug.Log("Rotation is working");
    }
    private bool checkOnScreen()
    {
        // TODO: fix this issue;
        return true;
        // Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        // if (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1 &&
        //     viewportPosition.z > 0)
        // {
        //     Debug.Log("It's on screen");
        // }
        // else
        // {
        //     Debug.Log("It's offscreen");
        // }
    }

    private void Initialize()
    {
        currentHP = maxHP;
        remainShootInterval = shootInterval;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Push Back
            Rigidbody2D playerRB = LevelManager.instance.player.GetComponent<Rigidbody2D>();
            Vector3 pushBackDirection = other.transform.position - transform.position;
            pushBackDirection.Normalize();
            
            playerRB.AddForce(pushBackDirection * pushBackForce, ForceMode2D.Impulse);
            
            // Damage Taken
            LevelManager.instance.UpdateHP(-damageNumber);
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            currentHP -= damageNumber;
        }
    }
}
