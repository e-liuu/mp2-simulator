using UnityEngine;
using System.Collections;

public class FertilizerSpawner : MonoBehaviour
{
    public GameObject fertilizerPrefab;

    public float minSpawnTime = 30f;
    public float maxSpawnTime = 90f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            if (fertilizerPrefab != null)
            {
                Instantiate(
                    fertilizerPrefab,
                    new Vector3(6.21f, -0.14226f, 6.15f),
                    Quaternion.identity
                );
            }
        }
    }
}