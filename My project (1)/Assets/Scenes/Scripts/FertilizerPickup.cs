using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class FertilizerPickup : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 10f;
    public float spinSpeed = 90f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable == null)
            interactable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        interactable.selectEntered.AddListener(OnCollected);
    }

    void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    void OnCollected(SelectEnterEventArgs args)
    {
        ResourceManager rm = ResourceManager.Instance;

        if (rm != null)
        {
            StartCoroutine(TemporaryBoost(rm));
        }

        Destroy(gameObject);
    }

    IEnumerator TemporaryBoost(ResourceManager rm)
    {
        float originalSun = rm.sunlightMultiplier;
        float originalWater = rm.waterMultiplier;

        rm.sunlightMultiplier *= boostMultiplier;
        rm.waterMultiplier *= boostMultiplier;

        Debug.Log("Fertilizer collected! Sun & water boosted.");

        yield return new WaitForSeconds(boostDuration);

        rm.sunlightMultiplier = originalSun;
        rm.waterMultiplier = originalWater;

        Debug.Log("Fertilizer boost ended.");
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnCollected);
    }
}