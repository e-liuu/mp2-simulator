using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnlockWaterButton : MonoBehaviour
{
    public float cost = 300f;
    public HUDManager hudManager;
    public AudioClip unlockSound;

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

            GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
            foreach (GameObject wall in walls)
            {
                // Play spatialized shatter sound at each wall's position
                if (unlockSound != null)
                    AudioSource.PlayClipAtPoint(unlockSound, wall.transform.position);

                wall.SetActive(false);
            }

            gameObject.SetActive(false);
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