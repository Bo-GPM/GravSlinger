using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoorController : MonoBehaviour
{
    public Animator animator;

    // private void Start()
    // {
    //     animator.GetComponent<Animator>();
    // }

    public void OpenDoor()
    {
        animator.SetBool("TriggerPressed", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("TriggerPressed", false);
    }
}
