﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class imageRollover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // Get colour info from the colour settings in the button component and swap colours on the press and hover 
    // setting and reset origanal colour.
    Button btnObject;
    public Image subImage;
    Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        btnObject = GetComponent<Button>();
        if (subImage == null) subImage = GetComponentInChildren<Image>();
        startColor = subImage.color;
    }

    public void OnPointerDown(PointerEventData data)
    {
        subImage.color = btnObject.colors.pressedColor;
    }
    public void OnPointerUp(PointerEventData data)
    {
        subImage.color = startColor;
    }
    public void OnPointerExit(PointerEventData data)
    {
        subImage.color = startColor;
    }
        public void OnPointerEnter(PointerEventData data)
    {
        subImage.color = btnObject.colors.highlightedColor;
    }
}
