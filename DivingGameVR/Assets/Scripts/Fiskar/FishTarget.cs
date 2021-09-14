using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTarget : MonoBehaviour
{
    public GameObject target;
    public Vector3 swimLimits;
    public int amountOfTargets = 5;

    public Transform[] targets;
    public float targetsSwapTime;
    float nextSwap;

    private void Awake()
    {
        targets = new Transform[amountOfTargets];
        Bounds b = new Bounds(transform.position, swimLimits);
        for (int i = 0; i < amountOfTargets; i++)
        {
            Vector3 pos = GetNewPosition();
            GameObject obj = Instantiate(target);
            targets[i] = obj.transform;
            targets[i].position = pos;
        }
    }

    void Update()
    {
        if (Time.time > nextSwap)
        {
            nextSwap = Time.time + targetsSwapTime;
            for (int i = 0; i < amountOfTargets; i++)
            {
                targets[i].position = GetNewPosition();
            }
        }
    }

    Vector3 GetNewPosition()
    {
        return transform.position +
                    new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                    Random.Range(-swimLimits.y, swimLimits.y),
                    Random.Range(-swimLimits.z, swimLimits.z));
    }
}
