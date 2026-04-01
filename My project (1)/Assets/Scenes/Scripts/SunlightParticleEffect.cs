using UnityEngine;

public class SunlightParticleEffect : MonoBehaviour
{
    public ParticleSystem sunParticles;

    private ResourceManager rm;
    private float lastSunlight;

    void Start()
    {
        rm = ResourceManager.Instance;
        lastSunlight = rm.sunlight;
    }

    void Update()
    {
        int currentWhole = Mathf.FloorToInt(rm.sunlight);
        int lastWhole = Mathf.FloorToInt(lastSunlight);

        if (currentWhole > lastWhole)
        {
            sunParticles.Emit((currentWhole - lastWhole)/2); // exactly 1 per unit
        }

        lastSunlight = rm.sunlight;
        }
}