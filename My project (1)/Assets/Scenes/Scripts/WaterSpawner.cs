using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject waterPrefab;
    public float spawnInterval = 12f;
    public Vector3 spawnOffset = new Vector3(0, 1.5f, 0);
    public float x;
    public float y;
    public float z;

    void Start()
    {
        InvokeRepeating(nameof(SpawnWater), spawnInterval, spawnInterval);
    }

    void SpawnWater()
    {
        Instantiate(waterPrefab, new Vector3(x, y, z) + spawnOffset, Quaternion.identity);
    }
}