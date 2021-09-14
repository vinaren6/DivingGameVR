using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoidTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillRandomFish();
    }

    public void KillRandomFish()
    {
        var boidM = GetComponent<BoidManager>();
        Boid[] boids = boidM.GetBoids();

        var boidList = new List<Boid>(boids);
        var selectedBoid = (boids[Random.Range(0, boids.Length)]);
        
        boidList.Remove(selectedBoid);
        boidM.SetBoids(boidList.ToArray());

        Destroy(selectedBoid.gameObject);
    }
}
