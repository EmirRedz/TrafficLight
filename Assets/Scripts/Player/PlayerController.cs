using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private IntersectionManager currentIntersection; 

    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text btnText;
    [SerializeField] private TMP_Text currentLightStateText;
    [SerializeField] private float stopDistanceThreshold;

    private void Start()
    {
        btn.onClick.AddListener(OnDriveButtonClicked);
        currentIntersection.OnTrafficLightChanged += UpdateSignalText; 
    }

   
    private void OnDestroy()
    {
        currentIntersection.OnTrafficLightChanged -= UpdateSignalText; 
    }


    private void Update()
    {
        float distanceToTrafficLight = Vector3.Distance(transform.position, currentIntersection.transform.position);

        currentLightStateText.transform.parent.gameObject.SetActive(distanceToTrafficLight < stopDistanceThreshold);
        btn.gameObject.SetActive(distanceToTrafficLight < stopDistanceThreshold);
    }

    private void OnDriveButtonClicked()
    {
        if (currentIntersection.GetCurrentState() == IntersectionManager.TrafficLightState.Green)
        {
            btnText.SetText("Can Drive!");
        }
        else if (currentIntersection.GetCurrentState() == IntersectionManager.TrafficLightState.Yellow)
        {
            float distanceToTrafficLight = Vector3.Distance(transform.position, currentIntersection.transform.position);
            if (distanceToTrafficLight < stopDistanceThreshold)
            {
                btnText.SetText("Cannot Drive!");
            }
            else
            {
                btnText.SetText("Can Drive!");
            }
        }
        else if (currentIntersection.GetCurrentState() == IntersectionManager.TrafficLightState.RedYellow)
        {
            btnText.SetText("Get Ready!");
        }
        else
        {
            btnText.SetText("Cannot Drive!");
        }
    }


    void UpdateSignalText(IntersectionManager.TrafficLightState state)
    {
        switch (state)
        {
            case IntersectionManager.TrafficLightState.Red:
                currentLightStateText.SetText("Red Light"); 
                break;
            case IntersectionManager.TrafficLightState.RedYellow:
                currentLightStateText.SetText("Red/Yellow Light"); 
                break;
            case IntersectionManager.TrafficLightState.Green:
                currentLightStateText.SetText("Green Light"); 
                break;
            case IntersectionManager.TrafficLightState.Yellow:
                currentLightStateText.SetText("Yellow Light"); 
                break;
        }
    }
}
