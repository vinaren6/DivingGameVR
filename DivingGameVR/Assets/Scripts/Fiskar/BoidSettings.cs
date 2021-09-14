using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{
    public float minSpeed = 2;
    public float maxSpeed = 6;
    public float perceptionRadius = 2.5f;
    public float avoidanceRadius = 1;
    public float rotationSpeed = 1.4f;

    public float alignWeight = 1;
    public float cohesionWeight = 1;
    public float seperateWeight = 1;

    public float targetWeight = 1;

    public float boundsRadius = 1.5f;
    public float collisionAvoidDist = 4;
    public LayerMask obstacleMask;
    public float avoidCollisionWeight = 10;
}
