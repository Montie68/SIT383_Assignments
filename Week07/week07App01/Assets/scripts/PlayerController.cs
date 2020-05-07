using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += v * transform.forward * Time.deltaTime * 10.0f;
        transform.rotation *= Quaternion.AngleAxis(h * 250.0f * Time.deltaTime, transform.up);

    }
}
