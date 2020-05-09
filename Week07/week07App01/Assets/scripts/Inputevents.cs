using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputevents : MonoBehaviour
{
    public enum Direction
    {
        Forward,
        Back, 
        Left,
        Right
    };
    public Transform avatar;
    public Direction direction;
    int directionMod;
    bool isMoving = false;
    bool isTurning = false;

    // Update is called once per frame
    void Start()
    {
        switch (direction)
        {
            case (Direction.Left):
                directionMod = -1;
                break;
            case (Direction.Back):
                directionMod = -1;
                break;
            default:
                directionMod = 1;
                break;
        }
    }

    public void InputDown()
    {
         isMoving = true;
         isTurning = true;
    }
    public void InputUp()
    {
         isMoving = false;
         isTurning = false;
    }

    private void Update()
    {
        if ((direction == Direction.Back || direction == Direction.Forward) && isMoving)
        {
            avatar.position += directionMod * avatar.forward * Time.deltaTime * 10.0f;
        }
        if ((direction == Direction.Left || direction == Direction.Right) && isTurning)
        {
            avatar.rotation *= Quaternion.AngleAxis(directionMod * 250.0f * Time.deltaTime, avatar.up);
        }
    }
}
