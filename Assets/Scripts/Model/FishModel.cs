using System;
using UnityEngine;

[Serializable]
public class FishModel
{
    public FishState currentState;
    public float idleTimer;
    public Vector3 targetPosition; // This is now UnityEngine.Vector3
}
