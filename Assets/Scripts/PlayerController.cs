using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movementForce;
    [SerializeField] private float maxSpeed;
    
    private int ablityAwakenIndex = 0;
    
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
                Debug.Log($"Max speed reached, Player is no longer Accelerating, current speed is {rb.velocity}");
            }
        }
    }

    private void AblityAwaker()
    {
        if (ablityAwakenIndex <= 1)
        {
            // TODO: Add gravity here
        }

        if (ablityAwakenIndex <= 2)
        {
            // TODO: Add Bouncy here
        }
    }

    private void PlayerMovement()
    {
        
    }
    
    
}
