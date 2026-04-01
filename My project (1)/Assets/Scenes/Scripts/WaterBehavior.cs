using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaterBehavior : MonoBehaviour
{
    public float waterValue = 10f;
    public AudioClip collectSound;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnCollected);
    }

    void OnCollected(SelectEnterEventArgs args)
    {
        ResourceManager rm = ResourceManager.Instance;

        if (rm != null)
        {
            rm.water += waterValue;
        }

        // Haptics
        var interactorMono = args.interactorObject as MonoBehaviour;
        if (interactorMono != null)
        {
            var haptic = interactorMono.GetComponent<UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer>();
            if (haptic != null) haptic.SendHapticImpulse(0.3f, 0.05f);
        }

        // Sound
        if (collectSound != null) AudioSource.PlayClipAtPoint(collectSound, transform.position);

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnCollected);
    }
}