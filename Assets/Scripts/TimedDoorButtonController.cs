using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TimedDoorButtonController : MonoBehaviour
{
    public TimedDoorController timedDoorController;

    private void OnTriggerStay2D(Collider2D other)
    {
        timedDoorController.OpenDoor();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        timedDoorController.CloseDoor();
    }
}
