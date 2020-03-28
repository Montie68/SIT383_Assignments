using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AvatarActions : MonoBehaviour
{
    [Tooltip("The prefab for objects created when the drop event is triggered.")]
    public GameObject blockTemplate;
    [Tooltip("Turn speed in degrees per second")]
    public float turnSpeed = 100.0f;

    [Tooltip("Movement speed in meters per second (assumes 1 unit = 1 meter)")]
    public float moveSpeed = 10.0f;

    private float turn = 0.0f;

    private float move = 0.0f;

    public void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(turn* turnSpeed * Time.deltaTime, Vector3.up);
        transform.position += move* moveSpeed * transform.forward * Time.deltaTime;
    }
    private void dropObject()
    {
        // Create the new object up and to the front, so it is away from the avatar.
        GameObject o = Instantiate(blockTemplate, transform.position + transform.forward +
                        transform.up, Quaternion.identity);
    }
    public void Left()
    {
        turn = -1.0f;
    }
    public void Right()
    {
        turn = 1.0f;
    }
    public void Forward()
    {
        move = 1.0f;
    }
    public void Drop()
    {
        dropObject();
    }
    public void Stop()
    {
        turn = 0.0f;
        move = 0.0f;
    }
}