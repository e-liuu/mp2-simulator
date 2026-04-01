using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuyWateringCan : MonoBehaviour
{
    public float waterCost = 200f;
    public float sunMultiplierIncrease = 1.5f;
    public float spinSpeed = 60f;
    public GameObject can;
    public GameObject window;
    public AudioClip purchaseSound;
    public ParticleSystem purchaseParticles;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    private bool purchased = false;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnBought);
    }

    void Update()
    {
        if (purchased)
        {
            can.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
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

        if (!rm.waterUnlocked)
        {
            Debug.Log("Water must be unlocked first!");
            return;
        }

        if (rm.SpendWater(waterCost))
        {
            rm.sunlightMultiplier *= sunMultiplierIncrease;

            purchased = true;

            // Ease in
            if (can != null) can.AddComponent<ScaleEaseIn>();

            // Sound
            if (purchaseSound != null) AudioSource.PlayClipAtPoint(purchaseSound, transform.position);

            // Particles
            if (purchaseParticles != null) purchaseParticles.Play();

            // Haptic feedback on the purchasing controller
            var interactorMono = args.interactorObject as MonoBehaviour;
            if (interactorMono != null)
            {
                var haptic = interactorMono.GetComponent<UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer>();
                if (haptic != null)
                    haptic.SendHapticImpulse(0.8f, 0.4f);
            }

            // Disable interaction (no more purchases)
            interactable.enabled = false;

            Debug.Log("Watering can purchased! Sun production boosted.");
            window.transform.position = new Vector3(0, -8f, 0);
        }
        else
        {
            Debug.Log("Not enough water! Need: " + waterCost);
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnBought);
    }
}