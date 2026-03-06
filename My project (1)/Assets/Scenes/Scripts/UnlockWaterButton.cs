using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnlockWaterButton : MonoBehaviour
{
    public float cost = 10f;
    public HUDManager hudManager;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnPressed);
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        ResourceManager rm = ResourceManager.Instance;

        if (rm.waterUnlocked) {
            Debug.Log("water unlocked!");
            return;
        }
        if (rm.SpendSunlight(cost))
        {
            rm.waterUnlocked = true;
            rm.waterBaseRate = 0.5f;
            hudManager.UnlockWaterUI();
            gameObject.SetActive(false);

            GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Not enough sunlight!");
        }
    }

    void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnPressed);
    }
}