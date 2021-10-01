using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        int rand = Random.Range(0, spawnPoints.Length - 1);
        transform.position = spawnPoints[rand].position;
    }
}
