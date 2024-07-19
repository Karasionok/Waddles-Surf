using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpSlide : MonoBehaviour
{
    bool isJumping = false;
    public float jumpPower = 10;
    public float jumpGravity = -18f;
    public float realGravity = -9.8f;
    public Rigidbody rb;
    public GameObject go;
    BoxCollider bc;

    private void Update()
    {
        bc = go.GetComponent<BoxCollider>();
        if (Input.GetKeyDown(KeyCode.W) && isJumping == false)
        {
            isJumping = true;
            Physics.gravity = new Vector3(0, jumpGravity, 0);
            rb.velocity += Vector3.up * jumpPower;
            StartCoroutine(StopJumpCororutine());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            go.transform.localScale = new Vector3(1, 0.5f, 1);
            bc.size -= new Vector3(0, 0.5f, 0);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            go.transform.localScale = new Vector3(1, 1, 1);
            bc.size = new Vector3(1, 1, 1);
            go.transform.position = new Vector3(go.transform.position.x, 1.2f, go.transform.position.z);
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
