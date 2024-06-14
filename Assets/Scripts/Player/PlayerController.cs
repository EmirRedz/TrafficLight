using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIData
{
    [TextArea(3,5)]public string description; // Description text for UI
    public Color color; // Color associated with this UI data
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TrafficLight currentTrafficLight;
    
    [Space(10)]
    
    [SerializeField] private PlayerButton playerButton;
    [SerializeField] private TMP_Text currentLightStateText;
    
    [Space(10)]
    
    [SerializeField] private float stopDistanceThreshold;

    [Space(10)]
    
    [SerializeField] private UIData green;
    [SerializeField] private UIData red;
    [SerializeField] private UIData redYellow;
    [SerializeField] private UIData yellow;

    
    private void Start()
    {
        playerButton.GetButton().onClick.AddListener(OnDriveButtonClicked);
        currentTrafficLight.GetMyIntersection().OnTrafficLightChanged += UpdateSignalText;

        #region Move Player To Position
        
        var pos = transform.position;
        pos.z += 12;
        
        transform.DOMove(pos, 2.5f).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete((UpdateSignalText));

        #endregion
    }

   
    private void OnDestroy()
    {
        currentTrafficLight.GetMyIntersection().OnTrafficLightChanged -= UpdateSignalText; 
    }


    private void Update()
    {
        UpdaterPlayerPosition();
    }

    // Update player's position interaction with traffic light
    private void UpdaterPlayerPosition()
    {
        if (currentTrafficLight != null)
        {
            float distanceToTrafficLight = Vector3.Distance(transform.position, currentTrafficLight.transform.position);
            currentLightStateText.transform.parent.gameObject.SetActive(distanceToTrafficLight < stopDistanceThreshold);
            playerButton.gameObject.SetActive(distanceToTrafficLight < stopDistanceThreshold);
        }
    }

    

    // Handle drive button click event
    private void OnDriveButtonClicked()
    {
        var state = currentTrafficLight.currentState;
        
        // Change button color based on current traffic light state
        if (state == IntersectionManager.TrafficLightState.Green)
        {
            playerButton.GetImage().DOColor(green.color, 0.15f);
        }
        else if (state == IntersectionManager.TrafficLightState.Yellow)
        {
            float distanceToTrafficLight = Vector3.Distance(transform.position, currentTrafficLight.transform.position);
            if (distanceToTrafficLight < stopDistanceThreshold)
            {
                playerButton.GetImage().DOColor(yellow.color, 0.15f);
            }
            else
            {
                playerButton.GetImage().DOColor(red.color, 0.15f);
            }

        }
        else if (state == IntersectionManager.TrafficLightState.RedYellow)
        {
            playerButton.GetImage().DOColor(redYellow.color, 0.15f);

        }
        else
        {
            playerButton.GetImage().DOColor(red.color, 0.15f);
        }
    }


    // Update signal text based on current traffic light state
    void UpdateSignalText()
    {
        var state = currentTrafficLight.currentState;
        switch (state)
        {
            case IntersectionManager.TrafficLightState.Red:
                currentLightStateText.SetText(red.description);
                break;
            case IntersectionManager.TrafficLightState.RedYellow:
                currentLightStateText.SetText(redYellow.description); 
                break;
            case IntersectionManager.TrafficLightState.Green:
                currentLightStateText.SetText(green.description); 
                break;
            case IntersectionManager.TrafficLightState.Yellow:
                currentLightStateText.SetText(yellow.description); 
                break;
        }
    }
    
    
}
