using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using XRInputDevice = UnityEngine.XR.InputDevice;
public class SunlightClicker : MonoBehaviour
{
    public float sunlightPerClick = 1f;
    public AudioSource clickSound;
    public ParticleSystem clickParticles;
    public InputActionReference primaryButtonAction;

    private bool wasPressed = false;

    void Update()
    {
        if (primaryButtonAction == null) return;

        bool isPressed = primaryButtonAction.action.IsPressed();

        if (isPressed && !wasPressed)
        {
            ResourceManager.Instance.sunlight += sunlightPerClick;

            if (clickSound != null) clickSound.Play();
            if (clickParticles != null) clickParticles.Play();

            // Haptics
            XRInputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            rightController.SendHapticImpulse(0, 0.5f, 0.1f);
        }

        wasPressed = isPressed;
    }
}