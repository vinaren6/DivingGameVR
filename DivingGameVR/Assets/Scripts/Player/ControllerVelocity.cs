using UnityEngine.InputSystem;
using UnityEngine;

public class ControllerVelocity : MonoBehaviour
{
    public InputActionProperty velocityProperty;

    public Vector3 Velocity { get; private set; } = Vector3.zero;

    private void Update()
    {
        Velocity = velocityProperty.action.ReadValue<Vector3>();
    }
}
