using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerControll: MonoBehaviour
{
    float laneOffset = 2.5f;
    float laneChangeSpeed = 15f;
    Rigidbody rb;
    Vector3 targetVelocity;
    float pointStart;
    float pointFinish;
    bool isJumping = false;
    float jumpPower = 150;
    float jumpGravity = -150f;
    float realGravity = -9.8f;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && pointFinish > -laneOffset)
        {
            pointStart = pointFinish;
            pointFinish -= laneOffset;
            targetVelocity = new Vector3(-laneChangeSpeed, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && pointFinish < laneOffset)
        {
            pointStart = pointFinish;
            pointFinish += laneOffset;
            targetVelocity = new Vector3(laneChangeSpeed, 0, 0);
        }
        // if (Input.GetKeyDown(KeyCode.W) && isJumping == false)
        // {
        //     isJumping = true;
        //     rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        //     Physics.gravity = new Vector3(0, jumpGravity, 0);
        //     StartCoroutine(StopJumpCororutine());
        // }
        float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }


    // IEnumerator StopJumpCororutine()
    // {
    //     do
    //     {
    //         yield return new WaitForSeconds(0.02f);
    //     } while (rb.velocity.y != 0);
    //     isJumping = false;
    //     Physics.gravity = new UnityEngine.Vector3(0, realGravity, 0);
    // }

    private void FixedUpdate()
    {
        rb.velocity = targetVelocity;
        if ((transform.position.x > pointFinish && targetVelocity.y > 0) ||
        (transform.position.x < pointFinish && targetVelocity.x < 0))
        {
            targetVelocity = Vector3.zero;
            rb.velocity = targetVelocity;
            rb.position = new Vector3(pointFinish, rb.position.y, rb.position.z);
        }
    }

    public void ResetGame()
    {
        pointStart = 0;
        pointFinish = 0;
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "lose")
        {
            ResetGame();
        }
    }
}
