using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwipeManager: MonoBehaviour
{
    public static SwipeManager instance;
    Vector2 startTouch;
    Vector2 swipeDelta;
    bool touchMoved;
    const float SWIPE_THRESHOLD = 50;
    public enum Direction {Left, Right, Up, Down};
    bool[] swipe = new bool[4]; 

    Vector2 TouchPosition() { return (Vector2)Input.mousePosition;}
    bool TouchBegan() { return Input.GetMouseButtonDown(0);}
    bool TouchEnded() { return Input.GetMouseButtonUp(0);}
    bool GetTouch() { return Input.GetMouseButton(0);}

    // Start is called before the first frame update
    void Awake() {instance = this;}

    // Update is called once per frame
    void Update()
    {
        if (TouchBegan())
        {
            startTouch = TouchPosition();
            touchMoved = true;
        }
        else if(TouchEnded() && touchMoved == true)
        {
            SendSwipe();
            touchMoved = false;
        }

        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }

        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (MathF.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                swipe[(int)Direction.Left] = swipeDelta.x < 0;
                swipe[(int)Direction.Right] = swipeDelta.x > 0;

            }
            else
            {
                swipe[(int)Direction.Down] = swipeDelta.x < 0;
                swipe[(int)Direction.Up] = swipeDelta.x > 0;
            }
            SendSwipe();
        }
    }

    void SendSwipe()
    {
        if (swipe[0] || swipe[1] || swipe[2] || swipe[3])
        {
            Debug.Log(swipe[0] + "|" + swipe[1] + "|" + swipe[2] + "|" + swipe[3]);
        }
    }
}
