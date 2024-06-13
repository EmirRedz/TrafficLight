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
    [TextArea(3,5)]public string description;
    public Color color;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private IntersectionManager currentIntersection;

    [SerializeField] private PlayerButton playerButton;
    [SerializeField] private TMP_Text currentLightStateText;
    
    [SerializeField] private float stopDistanceThreshold;

    [SerializeField] private UIData green;
    [SerializeField] private UIData red;
    [SerializeField] private UIData redYellow;
    [SerializeField] private UIData yellow;
    
    private void Start()
    {
        playerButton.GetButton().onClick.AddListener(OnDriveButtonClicked);
        currentIntersection.OnTrafficLightChanged += UpdateSignalText;

        var pos = transform.position;
        pos.z += 12;
        
        transform.DOMove(pos, 2.5f).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete((() =>
        {
            UpdateSignalText(currentIntersection.GetCurrentState());
        }));
    }

   
    private void OnDestroy()
    {
        currentIntersection.OnTrafficLightChanged -= UpdateSignalText; 
    }


    private void Update()
    {
        float distanceToTrafficLight = Vector3.Distance(transform.position, currentIntersection.transform.position);

        currentLightStateText.transform.parent.gameObject.SetActive(distanceToTrafficLight < stopDistanceThreshold);
        playerButton.gameObject.SetActive(distanceToTrafficLight < stopDistanceThreshold);
    }

    private void OnDriveButtonClicked()
    {
        if (currentIntersection.GetCurrentState() == IntersectionManager.TrafficLightState.Green)
        {
            playerButton.GetImage().DOColor(green.color, 0.15f);
        }
        else if (currentIntersection.GetCurrentState() == IntersectionManager.TrafficLightState.Yellow)
        {
            float distanceToTrafficLight = Vector3.Distance(transform.position, currentIntersection.transform.position);
            if (distanceToTrafficLight < stopDistanceThreshold)
            {
                playerButton.GetImage().DOColor(yellow.color, 0.15f);
            }
            else
            {
                playerButton.GetImage().DOColor(red.color, 0.15f);
            }

        }
        else if (currentIntersection.GetCurrentState() == IntersectionManager.TrafficLightState.RedYellow)
        {
            playerButton.GetImage().DOColor(redYellow.color, 0.15f);

        }
        else
        {
            playerButton.GetImage().DOColor(red.color, 0.15f);
        }
    }


    void UpdateSignalText(IntersectionManager.TrafficLightState state)
    {
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
