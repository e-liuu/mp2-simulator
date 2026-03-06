using UnityEngine;

public class BuyFlowerGridSpawner : MonoBehaviour
{
    public GameObject buyFlowerPrefab;

    private float[] columns = new float[] { 1.48f, -0.18f, -1.48f };
    private float startZ = 3.14f;
    private float endZ = 7.14f;
    private int rows = 5;

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        float step = (endZ - startZ) / (rows - 1);

        for (int row = 0; row < rows; row++)
        {
            float z = startZ + (step * row);

            for (int col = 0; col < columns.Length; col++)
            {
                // Skip the middle tile in row 5 (row index 4, middle column index 1)
                if (row == 4 && col == 1)
                    continue;

                Vector3 pos = new Vector3(columns[col], 0.76f, z);
                Instantiate(buyFlowerPrefab, pos, Quaternion.Euler(90f, 0f, 0f));
            }
        }
    }
}