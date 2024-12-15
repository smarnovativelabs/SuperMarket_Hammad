﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
public class FloatingJoystick : Joystick
{
    float targetAlphaUP = 0.2f;
    float targetAlphaDown = 1.0f;
    Vector2 fixedPosition;
    protected override void Start()
    {
        base.Start();
        // background.gameObject.SetActive(false);

        Color currentColor = background.gameObject.GetComponent<Image>().color;


        currentColor.a = Mathf.Clamp01(targetAlphaUP);


        background.gameObject.GetComponent<Image>().color = currentColor;
        background.transform.GetChild(0).gameObject.GetComponent<Image>().color = currentColor;
        fixedPosition = background.anchoredPosition;

        #if UNITY_EDITOR
                background.gameObject.SetActive(false);
        #endif
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        //  background.gameObject.SetActive(true);

        Color currentColor = background.gameObject.GetComponent<Image>().color;


        currentColor.a = Mathf.Clamp01(targetAlphaDown);

    
        background.gameObject.GetComponent<Image>().color = currentColor;
        background.transform.GetChild(0).gameObject.GetComponent<Image>().color = currentColor;
    

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // background.gameObject.SetActive(false);

        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        //  background.gameObject.SetActive(true);

        Color currentColor = background.gameObject.GetComponent<Image>().color;


        currentColor.a = Mathf.Clamp01(targetAlphaUP);

       
        background.gameObject.GetComponent<Image>().color = currentColor;
        background.transform.GetChild(0).gameObject.GetComponent<Image>().color = currentColor;
  
        background.anchoredPosition = fixedPosition;

        base.OnPointerUp(eventData);
    }

    public void SimulatePointerUp()
    { // Ensure the EventSystem is available
        if (EventSystem.current == null)
        {
            Debug.LogError("EventSystem is missing in the scene.");
            return;
        }

        // Create a simulated PointerEventData
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = background.anchoredPosition // Set the position or any relevant data
        };

        // Call the OnPointerUp method and pass the simulated PointerEventData
        OnPointerUp(pointerEventData);
    }
}