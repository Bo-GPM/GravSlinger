using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public DoorController door;

    private void OnTriggerEnter2D(Collider2D other)
    {
        door.DoorOpen();
    }
}
