using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSwim : MonoBehaviour
{
    [SerializeField] private float swimForce;
    [SerializeField] private float turnForce;
    [SerializeField] private float resistanceForce;
    [SerializeField] private float resistanceForceTurning;
    [SerializeField] private float stopOffset;
    [SerializeField] private float stopOffsetTurning;
    [SerializeField] private float deadZone;
    [SerializeField] private float turningDeadZone;
    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform leftHandPosition;
    [SerializeField] private Transform trackingSpace;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform startDirection;
    [SerializeField] private float turnDirection = 1f;
    [SerializeField] private bool rightGrabbing = false;
    [SerializeField] private bool leftGrabbing = false;

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
        if (rightTrigger > triggerPress && leftTrigger > triggerPress && rightGrabbing == false && leftGrabbing == false)
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
            if (rightTrigger > triggerPress && (leftTrigger < triggerPress || leftGrabbing == true) && rightGrabbing == false)
            {
                //Sets left point
                startDirection.parent = rightHandPosition;
                startDirection.localPosition = Vector3.left;

                //Last and current direction
                Vector3 lastDirection = direction;
                direction = rightHandPosition.localPosition - startDirection.localPosition;

                Vector3 vTemp = rightVelocity.Velocity.normalized;
                float fTemp = rightVelocity.Velocity.x * rightVelocity.Velocity.x +
                              rightVelocity.Velocity.z * rightVelocity.Velocity.z;
                fTemp = Mathf.Sqrt(fTemp);

                //Turn direction, + or -
                if (direction.magnitude < lastDirection.magnitude)
                    turnDirection = -1;
                else
                    turnDirection = 1;
                
                //Add turn force
                if (fTemp > turningDeadZone)
                {
                    AddRotateForce(new Vector3(0, fTemp, 0));
                }
            }
            else if (leftTrigger > triggerPress && (rightTrigger < triggerPress || rightGrabbing == true) && leftGrabbing == false)
            {
                //Sets Right point
                startDirection.parent = leftHandPosition;
                startDirection.localPosition = Vector3.left;

                //Last and current direction
                Vector3 lastDirection = direction;
                direction = leftHandPosition.localPosition - startDirection.localPosition;

                Vector3 vTemp = leftVelocity.Velocity.normalized;
                float fTemp = leftVelocity.Velocity.x * leftVelocity.Velocity.x +
                              leftVelocity.Velocity.z * leftVelocity.Velocity.z;
                fTemp = Mathf.Sqrt(fTemp);

                //Turn direction, + or -
                if (direction.magnitude < lastDirection.magnitude)
                    turnDirection = -1;
                else
                    turnDirection = 1;
                
                //Add turn force
                if (fTemp > turningDeadZone)
                {
                    AddRotateForce(new Vector3(0, fTemp, 0));
                }
            }
        }

        ApplyReststanceForce();
        ApplyReststanceTurning();
    }

    #region DebugGizmos

    private void OnDrawGizmos()
    {
        //Right
        var temp = rightHandPosition;
        temp.eulerAngles = new Vector3(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z * 0);

        Gizmos.color = Color.blue;
        Gizmos.matrix = temp.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, Vector3.left);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, Vector3.right);

        //Left
        temp = leftHandPosition;
        temp.eulerAngles = new Vector3(temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z * 0);

        Gizmos.color = Color.blue;
        Gizmos.matrix = temp.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, Vector3.left);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, Vector3.right);
    }

    #endregion

    public void RightGrabbing()
    {
        rightGrabbing = true;
    }

    public void RightNotGrabbing()
    {
        rightGrabbing = false;
    }
    
    public void LeftGrabbing()
    {
        leftGrabbing = true;
    }

    public void LeftNotGrabbing()
    {
        leftGrabbing = false;
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
        rigidbody.AddRelativeTorque(worldSpaceVelocity * turnForce * turnDirection);
    }
}