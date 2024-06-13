using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [SerializeField] private Renderer redLight;
    [SerializeField] private Renderer yellowLight;
    [SerializeField] private Renderer greenLight;

    public Renderer GetRedLight()
    {
        return redLight;
    } 
    
    public Renderer GetYellowLight()
    {
        return yellowLight;
    }
    
    public Renderer GetGreenLight()
    {
        return greenLight;
    }
}