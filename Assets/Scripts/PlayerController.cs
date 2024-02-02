using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = System.Numerics.Vector2;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movementForce;
    [SerializeField] private float maxSpeed;
    public float impulsiveForceMagnitude = 40f;
    
    public int abilityAwakenIndex = 0;
    
    // [SerializeField] private 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AblityAwaker();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }
    
    private void AblityAwaker()
    {
        if (abilityAwakenIndex <= 1)
        {
            // TODO: Add gravity here
            // Bullets will find it. No need to implement here :)
        }

        if (abilityAwakenIndex <= 2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(ApplyElasticForce());
            }
        }
    }

    private IEnumerator ApplyElasticForce()
    {
        float originalBounciness = rb.sharedMaterial.bounciness;
        rb.sharedMaterial.bounciness = 1.0f;

        Vector3 forceDirection = rb.velocity.normalized;
        rb.AddForce(forceDirection * impulsiveForceMagnitude, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);

        rb.sharedMaterial.bounciness = originalBounciness;
        LevelManager.instance.UpdateHP(1);
    }
    
    private void PlayerMovement()
    {
        // Raw Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        
        // Calculate angle of 2 vectors
        float angleDifference = Vector3.Angle(movement, rb.velocity);
        
        // if different direction, apply the force no matter what speed is
        if (angleDifference > 90)
        {
            rb.AddForce(movement * movementForce);
        }
        else
        {
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(movement * movementForce);
            }
            else
            {
                // Debug.Log($"Max speed reached, Player is no longer Accelerating, current speed is {rb.velocity}");
            }
        }
    }
    
    
}
