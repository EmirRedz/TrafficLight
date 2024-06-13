using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Traffic Light Config", menuName = "Scriptable Objects/Traffic Light Config")]
public class TrafficLightSO : ScriptableObject
{
    public float redLightDuration;
    public float redYellowDuration;
    public float yellowLightDuration;
    public float greenLightDuration;
}
