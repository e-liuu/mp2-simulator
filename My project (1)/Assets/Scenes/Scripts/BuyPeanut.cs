using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class BuyPeanut : MonoBehaviour
{
    // public GameObject peanutPrefab;
    public GameObject waterSpawner;   // assign in inspector
    public TextMeshProUGUI costText;

    public float sunlightCost = 20f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnGrabbed);

        UpdateCostText();
    }

    void UpdateCostText()
    {
        ResourceManager rm = ResourceManager.Instance;

        // if (rm != null && !rm.waterUnlocked)
        // {
        //     costText.text = "Locked (Water Required)";
        //     interactable.enabled = false;
        //     return;
        // }

        costText.text = "Buy Peanut - " + sunlightCost;
        interactable.enabled = true;
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        ResourceManager rm = ResourceManager.Instance;

        if (rm == null)
        {
            Debug.LogError("ResourceManager not found!");
            return;
        }

        if (!rm.waterUnlocked)
        {
            Debug.Log("Peanut locked: unlock water first.");
            return;
        }

        if (rm.SpendSunlight(sunlightCost))
        {
            Vector3 newPos = new Vector3(
                transform.position.x,
                0.28f,
                transform.position.z
            );

            // Instantiate(peanutPrefab, newPos, Quaternion.identity);

            // activate water spawner
            // if (waterSpawner != null)
            // {
            waterSpawner.SetActive(true);
            // }

            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough sunlight! Need: " + sunlightCost);
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnGrabbed);
    }
}