using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class getZoom : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.value = (float)RetrieveMap.instance.zoom;
    }
    public void Zoom()
    {
        RetrieveMap.instance.SetZoom((int)slider.value);
    }
}
