using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandGrabInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayer;
    private InputDeviceCharacteristics controllerType;

    [SerializeField] private bool rightHand;
    private bool lockedInHand;
    private float triggerPress = 0.8f;
    private float pickupDistance = 0.08f;
    private SphereCollider collider;

    private Transform pickupObject;
    private Rigidbody pickupRigidbody;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        //If the correct hand is pressing
        if (!lockedInHand && GetCorrectHandTriggerValue() > triggerPress)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, pickupDistance, pickupLayer);
            if (colliders.Length > 0)
            {
                pickupObject = colliders[0].transform;
                pickupRigidbody = pickupObject.GetComponent<Rigidbody>();

                pickupRigidbody.isKinematic = true;
                pickupObject.position = transform.position;
                pickupObject.SetParent(transform);
                lockedInHand = true;
            }
        }
        else if (lockedInHand && GetCorrectHandTriggerValue() < triggerPress)
        {
            pickupObject.parent = null;
            pickupRigidbody.isKinematic = false;
            lockedInHand = false;
        }
    }

    private float GetCorrectHandTriggerValue()
    {
        if (rightHand)
            return ControllerManager.Instance.rightTrigger;
        else
            return ControllerManager.Instance.leftTrigger;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}