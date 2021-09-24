using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSwim : MonoBehaviour
{
    [SerializeField] private float swimForce;
    [SerializeField] private float resistanceForce;
    [SerializeField] private float resistanceForceTurning;
    [SerializeField] private float stopOffset;
    [SerializeField] private float stopOffsetTurning;
    [SerializeField] private float deadZone;
    [SerializeField] private float turningDeadZone;
    [SerializeField] private float interval;
    [SerializeField] private Transform trackingSpace;

    private float triggerPress = 0.9f;
    public float leftTrigger, rightTrigger;
    [SerializeField] private ControllerVelocity rightVelocity;
    [SerializeField] private ControllerVelocity leftVelocity;

    [SerializeField] private Vector3 LEFT_DEBUG;
    [SerializeField] private Vector3 RIGHT_DEBUG;

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
        RIGHT_DEBUG = rightVelocity.Velocity;
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
                Debug.Log(rightVelocity.Velocity.normalized);

                Vector3 vTemp = rightVelocity.Velocity.normalized;
                float fTemp = rightVelocity.Velocity.x * rightVelocity.Velocity.x +
                              rightVelocity.Velocity.z * rightVelocity.Velocity.z;
                fTemp = Mathf.Sqrt(fTemp);

                if (fTemp > turningDeadZone)
                {
                    AddRotateForce(new Vector3(0, fTemp, 0));
                }
            }
            else if (leftTrigger > triggerPress)
            {
                if (Mathf.Abs(leftVelocity.Velocity.sqrMagnitude) > turningDeadZone)
                {
                    AddRotateForce(leftVelocity.Velocity.normalized);
                }
            }
        }

        ApplyReststanceForce();
        ApplyReststanceTurning();
    }

    private void ApplyReststanceTurning()
    {
        if (rigidbody.angularVelocity.sqrMagnitude > stopOffsetTurning)
            rigidbody.AddTorque(-rigidbody.angularVelocity * resistanceForceTurning, ForceMode.Impulse); //Acc instead?
        else
            rigidbody.angularVelocity = Vector3.zero;
    }

    private void ApplyReststanceForce()
    {
        if (rigidbody.velocity.sqrMagnitude > stopOffset)
            rigidbody.AddForce(-rigidbody.velocity * resistanceForce, ForceMode.Impulse); //Acc instead?
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
        Vector3 worldSpaceVelocity = trackingSpace.TransformDirection(localVelocity);
        rigidbody.AddRelativeTorque(worldSpaceVelocity * swimForce);
    }
}