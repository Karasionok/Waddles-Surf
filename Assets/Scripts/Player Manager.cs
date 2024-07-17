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
    float laneOffset;
    float laneChangeSpeed = 15f;
    Rigidbody rb;
    Vector3 targetVelocity;
    float pointStart;
    float pointFinish;



    void Start()
    {
        laneOffset = MapGenerator.instance.laneOffset;
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
        float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }


    private void FixedUpdate()
    {
        Vector3 power = rb.velocity;
        power.x = targetVelocity.x;
        if ((transform.position.x > pointFinish && targetVelocity.y > 0) ||
        (transform.position.x < pointFinish && targetVelocity.x < 0))
        {
            targetVelocity = Vector3.zero;
            power.x = targetVelocity.x;
            power.x = pointFinish;
        }
        rb.velocity = power;
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
