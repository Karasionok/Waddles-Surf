using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSlide : MonoBehaviour
{
    bool isJumping = false;
    public float jumpPower = 7;
    public float jumpGravity = -18f;
    public float realGravity = -9.8f;
    public Rigidbody rb;
    public Collider bc;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isJumping == false)
        {
            isJumping = true;
            Physics.gravity = new Vector3(0, jumpGravity, 0);
            rb.velocity += Vector3.up * jumpPower;
            StartCoroutine(StopJumpCororutine());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            bc.transform.localScale = new Vector3(1, 0.5f, 1);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            bc.transform.localScale = new Vector3(1, 1, 1);
        }
        
    }

    IEnumerator StopJumpCororutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
    }
}
