using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    public InputDeviceCharacteristics deviceType;
    void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceType, devices);
        //InputDevices.GetDevices(devices);

        foreach (var items in devices)
        {
            Debug.LogWarning(items.characteristics);
        }
    }
}
