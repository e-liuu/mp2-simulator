using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuySuperPeanut : MonoBehaviour
{
    public float sunCost = 200f;
    public float waterMultiplierIncrease = 2f;
    public float spinSpeed = 60f;
    public GameObject peanut;
    public GameObject window;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    private bool purchased = false;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnBought);
    }

    void Update()
    {
        if (purchased && peanut != null)
        {
            peanut.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }
    }

    void OnBought(SelectEnterEventArgs args)
    {
        if (purchased)
            return;

        ResourceManager rm = ResourceManager.Instance;

        if (rm == null)
        {
            Debug.LogError("ResourceManager not found!");
            return;
        }

        // Require water to be unlocked before super peanut
        if (!rm.waterUnlocked)
        {
            Debug.Log("Water must be unlocked first!");
            return;
        }

        if (rm.SpendSunlight(sunCost))
        {
            rm.waterMultiplier *= waterMultiplierIncrease;

            purchased = true;

            // Disable interaction so it can’t be bought twice
            interactable.enabled = false;

            Debug.Log("Super Peanut purchased! Water production boosted.");
            // if (window != null)
            // {
            //     window.SetActive(false);
            // }
            window.transform.position = new Vector3(0, -8f, 0);
        }
        else
        {
            Debug.Log("Not enough sunlight! Need: " + sunCost);
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnBought);
    }
}