using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunPrefab;
    public float spawnInterval = 12f;
    public Vector3 spawnOffset = new Vector3(0, 1.5f, 0);

    void Start()
    {
        InvokeRepeating(nameof(SpawnSun), spawnInterval, spawnInterval);
    }

    void SpawnSun()
    {
        Instantiate(sunPrefab, transform.position + spawnOffset, Quaternion.identity);
    }
}