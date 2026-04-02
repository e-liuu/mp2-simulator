using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;

public class BuyFlowerPlane : MonoBehaviour
{
    public GameObject sunflowerPrefab;
    public TextMeshProUGUI costText;

    //tutorial 
    public TutorialPopup tutorialPopup;
    private static bool firstFlowerTutorialShown = false;

    [Header("Row Detection")]
    // public float frontRowy = 0f;
    public float frontRowZ = 0f;
    public float rowSpacing = 2f;

    // private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Start()
    {
        // grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        // grabInteractable.selectEntered.AddListener(OnGrabbed);
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnGrabbed);

        if (tutorialPopup == null)
        {
            tutorialPopup = FindObjectOfType<TutorialPopup>();
        }
        UpdateCostText();
    }

    int GetRowNumber()
    {
        float distance = Mathf.Abs(transform.position.z - frontRowZ);
        int row = Mathf.RoundToInt(distance / rowSpacing) + 1;
        return row;
    }

    float GetCost()
    {
        return 5 - GetRowNumber() * 2 * 10 + 115;
    }

    void UpdateCostText()
    {
        float cost = GetCost();
        costText.text = "Buy Flower - " + cost;
    }

    [Header("Juice")]
    public AudioClip plantSound;
    public ParticleSystem plantParticles;

    void OnGrabbed(SelectEnterEventArgs args)
    {
        ResourceManager rm = ResourceManager.Instance;
        Debug.Log("Tile grabbed!");
        int row = GetRowNumber();
        float cost = (5 - row * 2 * 10 + 115);

        if (rm.SpendSunlight(cost))
        {
            Vector3 newpos = new Vector3(transform.position.x, 0.28f, transform.position.z);
            GameObject sunflower = Instantiate(sunflowerPrefab, newpos, Quaternion.identity);
            sunflower.AddComponent<ScaleEaseIn>();
            StartCoroutine(SinkAndHide());
            
            // tutorial
            if (!firstFlowerTutorialShown && tutorialPopup != null)
            {
                firstFlowerTutorialShown = true;
                tutorialPopup.ShowTutorial(
                    "Tutorial:\nFlowers generate sunlight over time.\nBuy more flowers to grow sunlight faster."
                );
            }

            // Haptics
            var interactorMono = args.interactorObject as MonoBehaviour;
            if (interactorMono != null)
            {
                var haptic = interactorMono.GetComponent<UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer>();
                if (haptic != null) haptic.SendHapticImpulse(0.5f, 0.2f);
            }

            // Sound
            if (plantSound != null) AudioSource.PlayClipAtPoint(plantSound, transform.position);

            // Particles
            if (plantParticles != null) Instantiate(plantParticles, new Vector3(transform.position.x, 0.28f, transform.position.z), Quaternion.identity).Play();
        }
        else
        {
            Debug.Log("Not enough sunlight! Need: " + cost);
        }
    }

    IEnumerator SinkAndHide()
    {
        float speed = 6f;
        Vector3 targetPos = transform.position + Vector3.down * 0.6f;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position += (targetPos - transform.position) * speed * Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        interactable.selectEntered.RemoveListener(OnGrabbed);
    }
}