using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class imageRollover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // Get colour info from the colour settings in the button component and swap colours on the press and hover 
    // setting and reset origanal colour.
    Button btnObject;
    public Image childImage;
    Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        if ((btnObject = GetComponent<Button>()) == null) Debug.LogError("Can't find Button on: " + gameObject.name);
        if (childImage == null) childImage = GetComponentInChildren<Image>();
        startColor = childImage.color;
    }

    // change image bast on button states
    public void OnPointerDown(PointerEventData data)
    {
        childImage.color = btnObject.colors.pressedColor;
    }
    public void OnPointerUp(PointerEventData data)
    {
        childImage.color = startColor;
    }
    public void OnPointerExit(PointerEventData data)
    {
        childImage.color = startColor;
    }
    public void OnPointerEnter(PointerEventData data)
    {
        childImage.color = btnObject.colors.highlightedColor;
    }
   void Update()
    {
        //check if attached button is disabled and set colour correctly
        if(!btnObject.enabled)
        {
            childImage.color = btnObject.colors.disabledColor;
        }
        else
        {
            childImage.color = startColor;
        }
    }
}
