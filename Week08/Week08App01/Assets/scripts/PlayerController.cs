using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        //add the player animator
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v > 0) anim.SetBool("isMovingForward", true);
        else if (v < 0) anim.SetBool("isMovingBack", true);
        else if (h > 0) anim.SetBool("isMovingRight", true);
        else if (h < 0) anim.SetBool("isMovingLeft", true);

        if (v == 0)
        {
            anim.SetBool("isMovingForward", false);
            anim.SetBool("isMovingBack", false);
        }
        if (h == 0) {
            anim.SetBool("isMovingRight", false);
            anim.SetBool("isMovingLeft", false);
        }



        transform.position += v * transform.forward * Time.deltaTime * 10.0f;
        transform.rotation *= Quaternion.AngleAxis(h * 250.0f * Time.deltaTime, transform.up);
    }
    void OnTriggerExit(Collider other)
    {

    }
}
