using UnityEngine;

public class WaterLock : MonoBehaviour
{
    [SerializeField] private Vector3 startRotation;
    private void Start()
    {
        startRotation = transform.eulerAngles;
    }

    private void Update()
    {
        transform.eulerAngles = startRotation;
    }
}
