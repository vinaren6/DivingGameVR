using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSwim : MonoBehaviour
{
    [SerializeField] private float swimForce;
    [SerializeField] private float resistanceForce;
    [SerializeField] private float resistanceForceAngular;
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
        if (rightTrigger > triggerPress && leftTrigger > triggerPress)
        {
            Vector3 rightHandDirection = leftVelocity.Velocity;
            Vector3 leftHandDirection = rightVelocity.Velocity;
            Vector3 localVelocity = leftHandDirection + rightHandDirection;
            localVelocity *= -1f;

            if (localVelocity.sqrMagnitude > deadZone * deadZone)
                AddSwimmingForce(localVelocity);
        }

        ApplyReststanceForce();
    }

    private void ApplyReststanceForce()
    {
        if (rigidbody.velocity.sqrMagnitude > 0.01f && currentDirection != Vector3.zero)
            rigidbody.AddForce(-rigidbody.velocity * resistanceForce, ForceMode.Acceleration);
        else
            currentDirection = Vector3.zero;

        if (rigidbody.angularVelocity.sqrMagnitude > 0.01f)
            rigidbody.AddTorque(new Vector3(0,rigidbody.angularVelocity.y, 0) * -resistanceForceAngular, ForceMode.Acceleration);
        else
            currentDirection = Vector3.zero;
    }

    private void AddSwimmingForce(Vector3 localVelocity)
    {
        Vector3 worldSpaceVelocity = trackingSpace.TransformDirection(localVelocity);
        rigidbody.angularVelocity = new Vector3(
            rigidbody.angularVelocity.x,
            rigidbody.angularVelocity.y + worldSpaceVelocity.x,
            rigidbody.angularVelocity.z);
        rigidbody.AddForce(worldSpaceVelocity * swimForce, ForceMode.Acceleration);
        currentDirection = worldSpaceVelocity.normalized;

        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
}