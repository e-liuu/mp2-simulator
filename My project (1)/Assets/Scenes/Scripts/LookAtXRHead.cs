using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using XRInputDevice = UnityEngine.XR.InputDevice;

public class LookAtXRHead : MonoBehaviour
{
    public Transform head;
    public float rotationSpeed = 5f;

    [Header("Blink Settings")]
    public float blinkInterval = 3f;
    public float blinkDuration = 0.15f;
    public float minYScale = 0.05f;

    [Header("Expand Settings")]
    public float expandMultiplier = 2.5f; // how tall the eye gets
    public float expandDuration = 1.0f;

    [Header("Input")]
    public InputActionReference primaryButtonAction;

    private Vector3 originalScale;
    private bool isAnimating = false;

    void Start()
    {
        if (head == null)
        {
            head = Camera.main.transform;
        }

        originalScale = transform.localScale;

        StartCoroutine(BlinkRoutine());
    }

    void OnEnable()
    {
        if (primaryButtonAction != null)
        {
            primaryButtonAction.action.performed += OnPrimaryButtonPressed;
            primaryButtonAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (primaryButtonAction != null)
        {
            primaryButtonAction.action.performed -= OnPrimaryButtonPressed;
            primaryButtonAction.action.Disable();
        }
    }

    void LateUpdate()
    {
        if (head == null) return;

        Vector3 direction = head.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void OnPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Expand pressed");
        if (!isAnimating)
        {
            StartCoroutine(ExpandOnce());
        }
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);

            if (!isAnimating)
            {
                yield return StartCoroutine(BlinkOnce());
            }
        }
    }

    IEnumerator BlinkOnce()
    {
        isAnimating = true;

        float closedScale = originalScale.y * minYScale;

        yield return StartCoroutine(ScaleY(originalScale.y, closedScale, blinkDuration));
        yield return StartCoroutine(ScaleY(closedScale, originalScale.y, blinkDuration));

        isAnimating = false;
    }

    IEnumerator ExpandOnce()
    {
        isAnimating = true;

        float expandedScale = originalScale.y * expandMultiplier;

        // Expand up
        yield return StartCoroutine(ScaleY2(originalScale.y, expandedScale, expandDuration));

        // Return to normal
        yield return StartCoroutine(ScaleY2(expandedScale, originalScale.y, expandDuration));

        isAnimating = false;
    }

    IEnumerator ScaleY(float from, float to, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float y = Mathf.Lerp(from, to, t);

            transform.localScale = new Vector3(
                originalScale.x,
                y,
                originalScale.z
            );

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(
            originalScale.x,
            to,
            originalScale.z
        );
    }

    IEnumerator ScaleY2(float from, float to, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float y = Mathf.Lerp(from, to, t);

            transform.localScale = new Vector3(
                originalScale.x,
                y * 4,
                originalScale.z
            );

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(
            originalScale.x,
            to,
            originalScale.z
        );
    }
}
