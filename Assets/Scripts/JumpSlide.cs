using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSlide : MonoBehaviour
{
    bool isJumping = false;
    float jumpPower = 150;
    float jumpGravity = -150f;
    float realGravity = -9.8f;
    public Rigidbody rb;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isJumping == false)
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            Physics.gravity = new Vector3(0, jumpGravity, 0);
            StartCoroutine(StopJumpCororutine());
        }
        
    }

    IEnumerator StopJumpCororutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new UnityEngine.Vector3(0, realGravity, 0);
    }
}
