using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotator = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("The Roatrion class has been Called");
    }
        // Update is called once per frame
        void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(.5f, rotator);
    }
}
