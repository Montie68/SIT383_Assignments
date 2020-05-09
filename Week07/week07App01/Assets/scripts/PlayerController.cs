using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public GameObject VirtualSurface;
   public GameObject PhysicalSurface;

    bool isPhysical = true;
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += v * transform.forward * Time.deltaTime * 10.0f;
        transform.rotation *= Quaternion.AngleAxis(h * 250.0f * Time.deltaTime, transform.up);
        VirtualSurface.SetActive(isPhysical);
        PhysicalSurface.SetActive(isPhysical);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            isPhysical = isPhysical ? false : true;

        }
    }
}
