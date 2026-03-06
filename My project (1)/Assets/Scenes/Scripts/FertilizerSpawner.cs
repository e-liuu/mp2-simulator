using UnityEngine;
using System.Collections;

public class FertilizerSpawner : MonoBehaviour
{
    public GameObject fertilizerPrefab;
    // public Transform spawnPosition;
    public float spawnInterval = 60f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (fertilizerPrefab != null)
            {
                Instantiate(fertilizerPrefab, new Vector3(6.21f, -0.14226f, 6.15f), Quaternion.identity);
            }
        }
    }
}