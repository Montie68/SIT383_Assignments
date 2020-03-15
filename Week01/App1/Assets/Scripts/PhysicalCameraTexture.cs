using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PhysicalCameraTexture : MonoBehaviour
{

    public Material camTextMaterial;
    public TextMeshProUGUI message;
    private WebCamTexture camTexture;

    private int currentCammera = 0;
    // Start is called before the first frame update
    void Start()
    {
        camTexture = new WebCamTexture();
        camTextMaterial.mainTexture = camTexture;
        camTexture.Play();
        ShowCameras();
    }

    void Update()
    {
        // message.text = "Update: " + Time.time;
    }

    private void ShowCameras()
    {
        message.text = "";
        foreach (WebCamDevice d in WebCamTexture.devices)
        {
            message.text += d.name + (d.name == camTexture.deviceName ? " *" : "") + "\n";
        }
    }

    public void NextCamera()
    {
        currentCammera = (currentCammera + 1) % WebCamTexture.devices.Length;
        // Change camera only works after stopping the current cameras feed

        camTexture.Stop();
        camTexture.deviceName = WebCamTexture.devices[currentCammera].name;
        camTexture.Play();
        ShowCameras();
    }
}
