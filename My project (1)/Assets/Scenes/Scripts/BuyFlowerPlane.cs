using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class BuyFlowerPlane : MonoBehaviour
{
    public GameObject sunflowerPrefab;
    public TextMeshProUGUI costText;

    [Header("Row Detection")]
    // public float frontRowy = 0f;
    public float frontRowZ = 0f;
    public float rowSpacing = 2f;

    // private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Start()
    {
        // grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        // grabInteractable.selectEntered.AddListener(OnGrabbed);
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnGrabbed);
        UpdateCostText();
    }

    int GetRowNumber()
    {
        float distance = Mathf.Abs(transform.position.z - frontRowZ);
        int row = Mathf.RoundToInt(distance / rowSpacing) + 1;
        return row;
    }

    float GetCost()
    {
        return 5 - GetRowNumber() * 2 * 10 + 115;
    }

    void UpdateCostText()
    {
        float cost = GetCost();
        costText.text = "Buy Flower - " + cost;
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        ResourceManager rm = ResourceManager.Instance;
        Debug.Log("Tile grabbed!");
        int row = GetRowNumber();
        float cost = (5 - row * 2 * 10 + 115);

        if (rm.SpendSunlight(cost))
        {
            Vector3 newpos = new Vector3(transform.position.x, 0.28f, transform.position.z);
            Instantiate(sunflowerPrefab, newpos, Quaternion.identity);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough sunlight! Need: " + cost);
        }
    }

    void OnDestroy()
    {
        // grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        interactable.selectEntered.RemoveListener(OnGrabbed);
    }
}