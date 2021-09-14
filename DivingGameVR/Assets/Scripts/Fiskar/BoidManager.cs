using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    FishTarget fishTarget;
    public enum GizmoType { Never, SelectedOnly, Always }
    public GizmoType showSpawnRegion;
    public Color gizmoColour;
    //compute shader
    //kan va bättre med 512 eller 256
    const int threadGroupSize = 1024;

    public BoidSettings boidSettings;
    public ComputeShader computeShader;
    Boid[] boids;

    public Boid prefab;
    public float spawnRadius = 10;
    public float spawnCount = 200;

    private void Awake()
    {
        fishTarget = FindObjectOfType<FishTarget>();
        boids = new Boid[(int)spawnCount];

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 poos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate(prefab);
            boid.transform.position = poos;
            boid.transform.forward = Random.insideUnitSphere;

            boids[i] = boid;
        }
    }


    private void Start()
    {
        //boids = FindObjectsOfType<Boid>();
        if (fishTarget != null)
        {
            int i = 0;
            foreach (Boid boid in boids)
            {
                i++;
                boid.SpawnFish(boidSettings, fishTarget.targets[i % fishTarget.amountOfTargets]);
            }
        }

        else
        {
            foreach (Boid boid in boids)
            {
                boid.SpawnFish(boidSettings, null);
            }
        }
    }

    private void Update()
    {
        if (boids != null)
        {
            int numBoids = boids.Length;
            //all fisk data
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Length; i++)
            {
                boidData[i].position = boids[i].position;
                boidData[i].direction = boids[i].forward;
            }

            ComputeBuffer computeBuffer = new ComputeBuffer(numBoids, BoidData.Size);
            computeBuffer.SetData(boidData);

            //sätter boids []
            computeShader.SetBuffer(0, "boids", computeBuffer);

            computeShader.SetInt("numBoids", boids.Length);
            computeShader.SetFloat("ViewRadius", boidSettings.perceptionRadius);
            computeShader.SetFloat("avoidRadius", boidSettings.avoidanceRadius);

            //delar upp
            int threadGroups = Mathf.CeilToInt(numBoids / (float)threadGroupSize);
            computeShader.Dispatch(0, threadGroups, 1, 1);

            computeBuffer.GetData(boidData);

            for (int i = 0; i < boids.Length; i++)
            {
                boids[i].averageFlockCource = boidData[i].flockCource;
                boids[i].centerPosOfFlock = boidData[i].flockCenter;
                boids[i].averageAvoidanceCource = boidData[i].avoidanceDirection;
                boids[i].flockNumber = boidData[i].numberOfFlockMates;

                boids[i].UpdateBoid();
            }

            computeBuffer.Release();

        }
    }

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 flockCource;
        public Vector3 flockCenter;
        public Vector3 avoidanceDirection;
        public int numberOfFlockMates;

        //HUH https://docs.unity3d.com/ScriptReference/ComputeBuffer-ctor.html
        public static int Size
        {
            get
            {
                return sizeof(float) * 3 * 5 + sizeof(int);
            }
        }
    }

    public Boid[] GetBoids()
    {
        return boids;
    }

    public void SetBoids(Boid[] newArray)
    {
        boids = newArray;
    }

    private void OnDrawGizmos()
    {
        if (showSpawnRegion == GizmoType.Always)
        {
            DrawGizmos();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (showSpawnRegion == GizmoType.SelectedOnly)
        {
            DrawGizmos();
        }
    }

    void DrawGizmos()
    {

        Gizmos.color = new Color(gizmoColour.r, gizmoColour.g, gizmoColour.b, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
