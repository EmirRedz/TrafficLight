using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TrafficLightContainer
{
    [SerializeField] private List<TrafficLight> trafficLights;

    public List<TrafficLight> GetTrafficLights()
    {
        return trafficLights;
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
    [SerializeField] private List<TrafficLightContainer> trafficLightGroup;

   
    [Space(10)]
    
    [Header("Light Visuals")]
    [SerializeField] private LightMaterial redLightMaterial;
    [SerializeField] private LightMaterial yellowLightMaterial;
    [SerializeField] private LightMaterial greenLightMaterial;
    
    private float timer;

    public event Action OnTrafficLightChanged;

    private void Start()
    {
        timer = trafficLightConfig.redLightDuration;
        InitLights();
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
        for (var i = 0; i < trafficLightGroup.Count; i++)
        {
            var group = trafficLightGroup[i];
            var trafficLights = group.GetTrafficLights();
            for (var j = 0; j < trafficLights.Count; j++)
            {
                var trafficLight = trafficLights[j];
                var newState = GetNextState(trafficLight.currentState);
                trafficLight.currentState = newState;
                trafficLight.SetLightState(newState, redLightMaterial, yellowLightMaterial, greenLightMaterial);
            }
        }

        SetTimer();
        OnTrafficLightChanged?.Invoke();
    }
    
    private void InitLights()
    {
        for (int i = 0; i < trafficLightGroup.Count; i++)
        {
            var group = trafficLightGroup[i];
            for (int j = 0; j < group.GetTrafficLights().Count; j++)
            {
                var trafficLight = group.GetTrafficLights()[j];
                var state = j % 2 == 0 ? TrafficLightState.Red : TrafficLightState.Green;
                
                trafficLight.currentState = state;
                trafficLight.SetLightState(state, redLightMaterial, yellowLightMaterial,greenLightMaterial);
            }
        }
    }

    private void SetTimer()
    {
        foreach (var group in trafficLightGroup)
        {
            foreach (var trafficLight in group.GetTrafficLights())
            {
                switch (trafficLight.currentState)
                {
                    case TrafficLightState.Red:
                        timer = trafficLightConfig.redLightDuration;
                        break;
                    case TrafficLightState.RedYellow:
                        timer = trafficLightConfig.redYellowDuration;
                        break;
                    case TrafficLightState.Green:
                        timer = trafficLightConfig.greenLightDuration;
                        break;
                    case TrafficLightState.Yellow:
                        timer = trafficLightConfig.yellowLightDuration;
                        break;
                }
            }
        }
    }

    private TrafficLightState GetNextState(TrafficLightState currentState)
    {
        switch (currentState)
        {
            case TrafficLightState.Red:
                return TrafficLightState.RedYellow;
            case TrafficLightState.RedYellow:
                return TrafficLightState.Green;
            case TrafficLightState.Yellow:
                return TrafficLightState.Red;
            case TrafficLightState.Green:
                return TrafficLightState.Yellow;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
        }
    }
    
    
    TrafficLightState GetOppositeState(TrafficLightState state)
    {
        switch (state)
        {
            case TrafficLightState.Red:
                return TrafficLightState.Green;
            case TrafficLightState.RedYellow:
                return TrafficLightState.Yellow;
            case TrafficLightState.Yellow:
                return TrafficLightState.RedYellow;
            case TrafficLightState.Green:
                return TrafficLightState.Red;
            default:
                return TrafficLightState.Red;
        }
    }

    public enum TrafficLightState
    {
        Red,
        RedYellow,
        Yellow,
        Green
    }
    
    public List<TrafficLightContainer> GetGroup()
    {
        return trafficLightGroup;
    }

}
