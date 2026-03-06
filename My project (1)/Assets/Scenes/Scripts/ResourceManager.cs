using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Sunlight")]
    public float sunlight = 0f;
    public float sunlightBaseRate = 1f;      
    public float sunlightGeneratorRate = 0f; 
    public float sunlightMultiplier = 1f;    

    [Header("Water")]
    public float water = 0f;
    public float waterBaseRate = 0f;
    public float waterGeneratorRate = 0f;
    public float waterMultiplier = 1f;
    public bool waterUnlocked = false;


    public float SunlightRate => (sunlightBaseRate + sunlightGeneratorRate) * sunlightMultiplier;
    public float WaterRate     => (waterBaseRate    + waterGeneratorRate)    * waterMultiplier;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        sunlight += SunlightRate * dt;
        if (waterUnlocked)
            water += WaterRate * dt;
    }

    public bool SpendSunlight(float amount)
    {
        if (sunlight >= amount) { sunlight -= amount; return true; }
        return false;
    }

    public bool SpendWater(float amount)
    {
        if (water >= amount) { water -= amount; return true; }
        return false;
    }
}