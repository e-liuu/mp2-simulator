using UnityEngine;

public class SunlightParticleEffect : MonoBehaviour
{
    public ParticleSystem sunParticles;
    public int burstAmount = 3; // particles per sunlight tick

    private ResourceManager rm;
    private float lastSunlight;

    void Start()
    {
        rm = ResourceManager.Instance;
        lastSunlight = rm.sunlight;
    }

    void Update()
    {
        if (rm.sunlight > lastSunlight)
        {
            sunParticles.Emit(burstAmount);
        }

        lastSunlight = rm.sunlight;
    }
}