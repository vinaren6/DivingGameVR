using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSwim : MonoBehaviour
{
    [SerializeField] private float swimForce;
    [SerializeField] private float resistanceForce;
    [SerializeField] private float stopOffset;
    [SerializeField] private float deadZone;
    [SerializeField] private float turningDeadZone;
    [SerializeField] private float interval;
    [SerializeField] private Transform trackingSpace;

    private float triggerPress = 0.9f;
    public float leftTrigger, rightTrigger;
    [SerializeField] private ControllerVelocity rightVelocity;
    [SerializeField] private ControllerVelocity leftVelocity;

    [SerializeField] private Vector3 LEFT_DEBUG;
    [SerializeField] private float RIGHT_DEBUG;

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

        LEFT_DEBUG = leftVelocity.Velocity;
        RIGHT_DEBUG = rightVelocity.Velocity.sqrMagnitude;
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
                AddSwimmingForce(localVelocity.normalized);

            //rigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            if (rightTrigger > triggerPress)
            {
                //rightHandDirection *= -1;
                //Debug.Log(Mathf.Abs(rightVelocity.Velocity.sqrMagnitude) > turningDeadZone);
                if (Mathf.Abs(rightVelocity.Velocity.sqrMagnitude) > turningDeadZone)
                {
                    Debug.LogWarning("RETARD ALERT!");
                    AddRotateForce(rightVelocity.Velocity.normalized);
                }
            }
            else if (leftTrigger > triggerPress)
            {
                return;
                Vector3 leftHandDirection = leftVelocity.Velocity;
                leftHandDirection *= -1;

                if (leftHandDirection.sqrMagnitude > deadZone * deadZone)
                    AddRotateForce(leftHandDirection.normalized);
            }
            else
            {
                rigidbody.angularVelocity = Vector3.zero;//Stannar spelaren efter man släpper trigger knappen. Ta bort
            }
        }

        ApplyReststanceForce();
    }

    private void ApplyReststanceForce()
    {
        if (rigidbody.velocity.sqrMagnitude > stopOffset)
            rigidbody.AddForce(-rigidbody.velocity * resistanceForce, ForceMode.Impulse);//Acc instead?
        else
            rigidbody.velocity = Vector3.zero;
    }

    private void AddSwimmingForce(Vector3 localVelocity)
    {
        Vector3 worldSpaceVelocity = trackingSpace.TransformDirection(localVelocity);
        rigidbody.AddForce(worldSpaceVelocity * swimForce, ForceMode.Acceleration);
        currentDirection = worldSpaceVelocity.normalized;
    }

    private void AddRotateForce(Vector3 localVelocity)
    {
        Debug.Log("What the fuck");
        Vector3 worldSpaceVelocity = trackingSpace.TransformDirection(localVelocity);
        rigidbody.AddTorque(worldSpaceVelocity * swimForce);
    }
}