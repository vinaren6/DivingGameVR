using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSwim : MonoBehaviour
{
    [SerializeField] private float swimForce;
    [SerializeField] private float resistanceForce;
    [SerializeField] private float deadZone;
    [SerializeField] private float interval;
    [SerializeField] private Transform trackingSpace;

    private float triggerPress = 0.9f;
    private float leftTrigger, rightTrigger;
    [SerializeField] private ControllerVelocity rightVelocity;
    [SerializeField] private ControllerVelocity leftVelocity;

    private float currentWaitTime;
    private Rigidbody rigidbody;
    private Vector3 currentDirection;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        leftTrigger = ControllerManager.Instance.leftTrigger;
        rightTrigger = ControllerManager.Instance.rightTrigger;
    }

    private void FixedUpdate()
    {
        currentWaitTime += Time.deltaTime;
        if (leftTrigger > triggerPress || rightTrigger > triggerPress)
        {
            Vector3 localVelocity = Vector3.zero;
            if (leftTrigger > triggerPress && rightTrigger > triggerPress) //Left AND Right
                localVelocity = rightVelocity.Velocity + leftVelocity.Velocity;
            else if (leftTrigger > triggerPress) //Left only
                localVelocity = leftVelocity.Velocity;
            else if (rightTrigger > triggerPress) //Right only
                localVelocity = rightVelocity.Velocity;

            localVelocity *= -1f;

            if (localVelocity.sqrMagnitude > deadZone * deadZone && currentWaitTime > interval)
            {
                AddSwimming(localVelocity);
                currentWaitTime = 0f;
            }

            if (rigidbody.velocity.sqrMagnitude > 0.01f && currentDirection != Vector3.zero)
            {
                rigidbody.AddForce(-rigidbody.velocity * resistanceForce, ForceMode.Acceleration);
            }
            else
            {
                currentDirection = Vector3.zero;
            }
        }
    }

    private void AddSwimming(Vector3 localVelocity)
    {
        Vector3 worldSpaceVelocity = trackingSpace.TransformDirection(localVelocity);
        rigidbody.AddForce(worldSpaceVelocity * swimForce, ForceMode.Impulse);
        currentDirection = worldSpaceVelocity.normalized;
    }
}