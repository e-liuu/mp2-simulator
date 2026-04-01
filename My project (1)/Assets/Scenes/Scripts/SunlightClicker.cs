using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class SunlightClicker : MonoBehaviour
{
    public float sunlightPerClick = 1f;
    public AudioSource clickSound;
    public ParticleSystem clickParticles;

    private XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnClick);
    }

    void OnClick(SelectEnterEventArgs args)
    {
        ResourceManager.Instance.sunlight += sunlightPerClick;

        var interactorMono = args.interactorObject as MonoBehaviour;
        if (interactorMono != null)
        {
            var haptic = interactorMono.GetComponent<HapticImpulsePlayer>();
            if (haptic != null)
                haptic.SendHapticImpulse(0.5f, 0.1f);
        }

        if (clickSound != null) clickSound.Play();
        if (clickParticles != null) clickParticles.Play();
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnClick);
    }
}