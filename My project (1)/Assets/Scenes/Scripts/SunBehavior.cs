using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SunBehavior : MonoBehaviour
{
    public float sunValue = 10f;

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
            rm.sunlight += sunValue;
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnCollected);
    }
}