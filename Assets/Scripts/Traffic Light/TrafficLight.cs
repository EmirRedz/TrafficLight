using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] private IntersectionManager myIntersection;
    [SerializeField] private Renderer redLight;
    [SerializeField] private Renderer yellowLight;
    [SerializeField] private Renderer greenLight;
    public IntersectionManager.TrafficLightState currentState;
    
    // Sets the visual state of the traffic light based on the provided state and materials
    public void SetLightState(IntersectionManager.TrafficLightState state, LightMaterial redLightMaterial,
        LightMaterial yellowLightMaterial, LightMaterial greenLightMaterial)
    {
        bool isRed = state == IntersectionManager.TrafficLightState.Red;
        bool isGreen = state == IntersectionManager.TrafficLightState.Green;
        bool isYellow = state == IntersectionManager.TrafficLightState.Yellow;
        bool isRedYellow = state == IntersectionManager.TrafficLightState.RedYellow;
    
        // Set the material of each light renderer based on the traffic light state
        redLight.sharedMaterial = isRed || isRedYellow ? redLightMaterial.GetActiveMaterial() : redLightMaterial.GetInActiveMaterial();
        yellowLight.sharedMaterial = isYellow || isRedYellow ? yellowLightMaterial.GetActiveMaterial() : yellowLightMaterial.GetInActiveMaterial();
        greenLight.sharedMaterial = isGreen ? greenLightMaterial.GetActiveMaterial() : greenLightMaterial.GetInActiveMaterial();
    }

    // Returns the IntersectionManager associated with this traffic light
    public IntersectionManager GetMyIntersection()
    {
        return myIntersection;
    }
}