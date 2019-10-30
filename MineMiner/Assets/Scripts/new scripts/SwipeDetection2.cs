using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection2 : MonoBehaviour
{

    [SerializeField] private float minSwipeDistance = 20;
    [SerializeField] private bool onlySwipeAfterRelaease = true;
    [SerializeField] private float speed = 1f;

    public Player2 player;

    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;

    bool iswalking;

    private void Update() {
        if (!iswalking) {
            StartCoroutine(walkForward());
            iswalking = true;
        }

        if (Input.touchCount > 0) {
            Touch touch = Input.touches[0];
            switch (touch.phase) {
                case TouchPhase.Began:
                    swipeStartPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    if (!onlySwipeAfterRelaease) {
                        swipeEndPos = touch.position;
                        CheckForSwipe();
                    }
                    break;
                case TouchPhase.Ended:
                    swipeEndPos = touch.position;
                    CheckForSwipe();
                    break;
            }
        }
        debugControls();
    }

    IEnumerator walkForward() {
        while (true) {
            player.Move(2);
            yield return new WaitForSeconds(1/ speed);
        }
    }

    private void CheckForSwipe() {
        if (Vector2.Distance(swipeStartPos, swipeEndPos) > minSwipeDistance) {
            Vector2 dir = swipeEndPos - swipeStartPos;
            Vector2 dirAbs = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            swipeStartPos = Input.touches[0].position;

            if (dirAbs.x > dirAbs.y)  {
                if (dir.x > 0) {
                    //move right
                    //Debug.Log("swipe right");

                    player.Move(1);
                }
                else {
                    //move left
                    //Debug.Log("swipe left");
                    
                    player.Move(0);
                }
            }
            else {
                if (dir.y > 0) {
                    //move up
                    //Debug.Log("swipe up");

                    //player.Move(2);
                }
                else {
                    //move down
                    //Debug.Log("swipe down");

                    //player.Move(3);
                }
            }
        }
    }

    private void debugControls() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            player.Move(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            player.Move(0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            //player.Move(2);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //player.Move(3);
        }
    }
}