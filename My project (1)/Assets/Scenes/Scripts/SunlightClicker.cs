using UnityEngine;
using UnityEngine.InputSystem;

public class SunlightClicker : MonoBehaviour
{
    public float sunlightPerClick = 1f;
    public InputActionReference primaryButtonAction;

    private bool wasPressed = false;

    void Update()
    {
        bool isPressed = primaryButtonAction.action.IsPressed();

        if (isPressed && !wasPressed)
        {
            ResourceManager.Instance.sunlight += sunlightPerClick;
            Debug.Log("Pressed for sun!");
        }

        wasPressed = isPressed;
    }
}