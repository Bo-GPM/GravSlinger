using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSmoothSpeed = 0.01f;
    [SerializeField] private Vector3 offset;
    
    private void LateUpdate()
    {
        Vector3 desiredPosition = LevelManager.instance.player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSmoothSpeed);
        transform.position = smoothedPosition;
    }
}
