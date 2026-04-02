using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnlockWaterButton : MonoBehaviour
{
    public float cost = 500f;
    public HUDManager hudManager;
    public ParticleSystem unlockParticles;
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

        if (rm.waterUnlocked) return;

        if (rm.SpendSunlight(cost))
        {
            rm.waterUnlocked = true;
            rm.waterBaseRate = 0.5f;
            hudManager.UnlockWaterUI();

            // Sound
            if (unlockSound != null) AudioSource.PlayClipAtPoint(unlockSound, transform.position);

            // Particles — spawn a detached copy so it survives the disable
            if (unlockParticles != null)
                Instantiate(unlockParticles, transform.position, Quaternion.identity).Play();

            // Disable walls
            GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
            foreach (GameObject wall in walls)
                wall.SetActive(false);

            // Disable interactable instead of whole GameObject
            interactable.enabled = false;
            StartCoroutine(DestroyAfterDelay(2f));
        }
        else
        {
            Debug.Log("Not enough sunlight!");
        }
        IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false); 
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnPressed);
    }
}