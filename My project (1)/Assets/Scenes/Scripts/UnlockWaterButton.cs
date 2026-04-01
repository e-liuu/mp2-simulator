using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnlockWaterButton : MonoBehaviour
{
    public float cost = 300f;
    public HUDManager hudManager;
    public AudioClip unlockSound;
    public ParticleSystem unlockParticles;

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

            // Haptics
            var interactorMono = args.interactorObject as MonoBehaviour;
            if (interactorMono != null)
            {
                var haptic = interactorMono.GetComponent<UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer>();
                if (haptic != null) haptic.SendHapticImpulse(1.0f, 0.6f);
            }

            // Particles
            if (unlockParticles != null) unlockParticles.Play();

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