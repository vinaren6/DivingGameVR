using System;
using UnityEngine;
using UnityEngine.XR;

public class ControllerInput : MonoBehaviour
{
    private InputDevice targetDevice;
    private float triggerGrip = 0.8f;
    [SerializeField] private bool rightController;
    [SerializeField] private Animator handAnimator;

    private readonly string gripAnim = "Grip";
    private readonly string triggerAnim = "Trigger";

    private void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!targetDevice.isValid)
        {
            targetDevice = rightController
                ? ControllerManager.Instance.rightController
                : ControllerManager.Instance.leftController;
            return;
        }

        UpdateHandAnimation();
    }

    private void UpdateHandAnimation()
    {
        //Trigger
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat(triggerAnim, triggerValue);
        }
        else
        {
            handAnimator.SetFloat(triggerAnim, 0);
        }

        //Grip
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat(gripAnim, gripValue);
        }
        else
        {
            handAnimator.SetFloat(gripAnim, 0);
        }
    }
}