using UnityEngine;

public class WaterParticleEffect : MonoBehaviour
{
    public ParticleSystem waterParticles;

    private ResourceManager rm;
    private float lastWater;

    void Start()
    {
        rm = ResourceManager.Instance;
        lastWater = rm.water;
    }

    void Update()
    {
        int currentWhole = Mathf.FloorToInt(rm.water);
        int lastWhole = Mathf.FloorToInt(lastWater);

        if (currentWhole > lastWhole)
        {
            waterParticles.Emit((currentWhole - lastWhole)/2); // exactly 1 per unit
        }

        lastWater = rm.water;
        }
}