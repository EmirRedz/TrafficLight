using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TrafficLightContainer
{
    [SerializeField] private TrafficLight trafficLightA;
    [SerializeField] private TrafficLight trafficLightB;

    public TrafficLight GetTrafficLightA()
    {
        return trafficLightA;
    }
    
    
    public TrafficLight GetTrafficLightB()
    {
        return trafficLightB;
    }
}

[System.Serializable]
public class LightMaterial
{
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    public Material GetActiveMaterial()
    {
        return activeMaterial;
    }

    public Material GetInActiveMaterial()
    {
        return inactiveMaterial;
    }
}
public class IntersectionManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TrafficLightSO trafficLightConfig;
    
    [Space(10)]
    
    [Header("Light Visuals")]
    [SerializeField] private List<TrafficLightContainer> trafficLightGroup;
    [SerializeField] private LightMaterial redLightMaterial;
    [SerializeField] private LightMaterial yellowLightMaterial;
    [SerializeField] private LightMaterial greenLightMaterial;
    
    private float timer;
    private TrafficLightState currentState;

    public event Action<TrafficLightState> OnTrafficLightChanged;

    private void Start()
    {
        currentState = TrafficLightState.Red;
        timer = trafficLightConfig.redLightDuration;
        UpdateTrafficLight();
    }

   

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SwitchLight();
        }
    }

    private void SwitchLight()
    {
        switch (currentState)
        {
            case TrafficLightState.Red:
                currentState = TrafficLightState.RedYellow;
                timer = trafficLightConfig.redYellowDuration;
                break;
            case TrafficLightState.RedYellow:
                currentState = TrafficLightState.Green;
                timer = trafficLightConfig.greenLightDuration;
                break;
            case TrafficLightState.Yellow:
                currentState = TrafficLightState.Red;
                timer = trafficLightConfig.redLightDuration;
                break;
            case TrafficLightState.Green:
                currentState = TrafficLightState.Yellow;
                timer = trafficLightConfig.yellowLightDuration;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        UpdateTrafficLight();
    }
    
    
    void SetLightState(TrafficLight light, TrafficLightState state)
    {
        bool isRed = state == TrafficLightState.Red;
        bool isGreen = state == TrafficLightState.Green;
        bool isYellow = state == TrafficLightState.Yellow;
        bool isRedYellow = state == TrafficLightState.RedYellow;
        
        light.GetRedLight().sharedMaterial = isRed || isRedYellow ? redLightMaterial.GetActiveMaterial() : redLightMaterial.GetInActiveMaterial();
        light.GetYellowLight().sharedMaterial = isYellow || isRedYellow ? yellowLightMaterial.GetActiveMaterial() : yellowLightMaterial.GetInActiveMaterial();
        light.GetGreenLight().sharedMaterial = isGreen ? greenLightMaterial.GetActiveMaterial() : greenLightMaterial.GetInActiveMaterial();
    }

    private void UpdateTrafficLight()
    {
        foreach (TrafficLightContainer group in trafficLightGroup)
        {
            SetLightState(group.GetTrafficLightA(), currentState);
            SetLightState(group.GetTrafficLightB(), GetOppositeState(currentState));
        }
        
        OnTrafficLightChanged?.Invoke(currentState);

    }
    TrafficLightState GetOppositeState(TrafficLightState state)
    {
        switch (state)
        {
            case TrafficLightState.Red:
                return TrafficLightState.Green;
                break;
            case TrafficLightState.RedYellow:
                return TrafficLightState.Yellow;
                break;
            case TrafficLightState.Yellow:
                return TrafficLightState.RedYellow;
                break;
            case TrafficLightState.Green:
                return TrafficLightState.Red;
                break;
            default:
                return TrafficLightState.Red;
        }
    }
    
    public TrafficLightState GetCurrentState()
    {
        return currentState;
    }
    
    public enum TrafficLightState
    {
        Red,
        RedYellow,
        Yellow,
        Green
    }
}
