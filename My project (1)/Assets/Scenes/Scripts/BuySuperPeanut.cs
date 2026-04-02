using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuySuperPeanut : MonoBehaviour
{
    public float sunCost = 200f;
    public float waterMultiplierIncrease = 2f;
    public float spinSpeed = 60f;
    public GameObject peanut;
    public GameObject window;
    public ParticleSystem buyParticles;
    public AudioClip buySound;

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
            peanut.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    void OnBought(SelectEnterEventArgs args)
    {
        if (purchased) return;

        ResourceManager rm = ResourceManager.Instance;

        if (rm == null) { Debug.LogError("ResourceManager not found!"); return; }
        if (!rm.waterUnlocked) { Debug.Log("Water must be unlocked first!"); return; }

        if (rm.SpendSunlight(sunCost))
        {
            rm.waterMultiplier *= waterMultiplierIncrease;
            purchased = true;

            // Haptics
            var interactorMono = args.interactorObject as MonoBehaviour;
            if (interactorMono != null)
            {
                var haptic = interactorMono.GetComponent<UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer>();
                if (haptic != null) haptic.SendHapticImpulse(0.8f, 0.4f);
            }

            // Particles
            if (buyParticles != null) buyParticles.Play();

            // Sound
            if (buySound != null) AudioSource.PlayClipAtPoint(buySound, transform.position);

            interactable.enabled = false;
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