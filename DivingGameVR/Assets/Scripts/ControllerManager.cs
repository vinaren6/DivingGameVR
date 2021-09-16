using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance { get; private set; }
    public InputDevice rightController { get; private set; }
    public InputDevice leftController { get; private set; }
    [SerializeField] private InputDeviceCharacteristics rightCharacteristics;
    [SerializeField] private InputDeviceCharacteristics leftCharacteristics;

    private void Awake()
    {
        rightCharacteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand |
                               InputDeviceCharacteristics.Right;
        leftCharacteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand |
                              InputDeviceCharacteristics.Left;

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Init()
    {
        if (!rightController.isValid)
        {
            List<InputDevice> rightControllerDevice = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(rightCharacteristics, rightControllerDevice);
            
            if (rightControllerDevice.Count == 1)
            {
                rightController = rightControllerDevice[0];
                Debug.Log($"Assigned {gameObject.name} with {rightController.name}");
            }
        }

        if (!leftController.isValid)
        {
            List<InputDevice> leftControllerDevice = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(leftCharacteristics, leftControllerDevice);
            
            if (leftControllerDevice.Count == 1)
            {
                leftController = leftControllerDevice[0];
                Debug.Log($"Assigned {gameObject.name} with {leftController.name}");
            }
        }
    }

    private void Update()
    {
        if (!rightController.isValid || !leftController.isValid)
        {
            Init();
            return;
        }
    }
}