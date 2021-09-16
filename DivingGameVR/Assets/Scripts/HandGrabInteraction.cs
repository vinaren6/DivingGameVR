using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandGrabInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private InputDevice targetDevice;
    private InputDeviceCharacteristics controllerType;

    private bool lockedInHand;
    private Transform pickupObject;
    private float pickupDistance = 0.08f;
    private SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        controllerType = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right |
                         InputDeviceCharacteristics.TrackedDevice;
    }

    private void FixedUpdate()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.8f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, pickupDistance, pickupLayer);
            if (colliders.Length > 0)
            {
                pickupObject = colliders[0].transform;
            }

            lockedInHand = !lockedInHand;
            Debug.Log(colliders.Length);
        }
    }

    private void Update()
    {
        if (!targetDevice.isValid)
        {
            Init();
            return;
        }

        if (lockedInHand)
        {
            pickupObject.position = transform.position;
        }
    }

    private void Init()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerType, devices);

        if (devices.Count == 1)
        {
            targetDevice = devices[0];
            Debug.Log($"Assigned {gameObject.name} with {targetDevice.name}");
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}