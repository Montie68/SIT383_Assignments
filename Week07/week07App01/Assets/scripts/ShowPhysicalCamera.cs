using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPhysicalCamera : MonoBehaviour
{
    public Camera camera;
    public Material PhysicalMaterial;
    private WebCamTexture webCamTexture;
    // Start is called before the first frame update
    void Start()
    {
        if (camera == null) camera = Camera.main;
        webCamTexture = new WebCamTexture();
        PhysicalMaterial.mainTexture = webCamTexture;
        webCamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float pos = (camera.nearClipPlane+0.5f);
        transform.position = camera.transform.position + camera.transform.forward * pos;
        //transform.rotation = camera.transform.rotation;
        float hieght = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f ) * pos * 2.0f;
        transform.localScale = new Vector3(hieght * camera.aspect, hieght, 1.0f);

    }
}
