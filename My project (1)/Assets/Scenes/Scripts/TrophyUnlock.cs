// using UnityEngine;

// public class TrophyUnlock : MonoBehaviour
// {
//     public GameObject trophy;
//     public float sunReq;
//     public float waterReq;

//     private bool unlocked = false;

//     void Update()
//     {
//         if (unlocked || trophy == null)
//             return;

//         ResourceManager rm = ResourceManager.Instance;

//         if (rm == null)
//             return;

//         if (rm.sunlight >= sunReq && rm.water >= waterReq)
//         {
//             trophy.SetActive(true);
//             unlocked = true;

//             Debug.Log("Trophy unlocked!");
//         }
//     }
// }

using UnityEngine;
using TMPro;
using System.Collections;

public class TrophyUnlock : MonoBehaviour
{
    public GameObject trophy;
    public float sunReq;
    public float waterReq;

    public AudioSource audioSource;
    public AudioClip unlockSound;

    public GameObject confettiPrefab;
    public Transform confettiSpawnPoint;

    public GameObject popupTextObject;
    public TMP_Text popupText;
    public string message = "You just won a trophy!";
    public float popupDuration = 2f;

    private bool unlocked = false;

    void Start()
    {
        if (trophy != null)
            trophy.SetActive(false);

        if (popupTextObject != null)
            popupTextObject.SetActive(false);
    }

    void Update()
    {
        if (unlocked || trophy == null)
            return;

        ResourceManager rm = ResourceManager.Instance;
        if (rm == null)
            return;

        if (rm.sunlight >= sunReq && rm.water >= waterReq)
        {
            UnlockTrophy();
        }
    }

    void UnlockTrophy()
    {
        unlocked = true;
        trophy.SetActive(true);

        if (audioSource != null && unlockSound != null)
            audioSource.PlayOneShot(unlockSound);

        if (confettiPrefab != null)
        {
            Vector3 spawnPos = confettiSpawnPoint != null
                ? confettiSpawnPoint.position
                : trophy.transform.position + Vector3.up * 0.5f;

            Quaternion spawnRot = confettiSpawnPoint != null
                ? confettiSpawnPoint.rotation
                : Quaternion.identity;

            Instantiate(confettiPrefab, spawnPos, spawnRot);
        }

        if (popupTextObject != null)
        {
            popupTextObject.SetActive(true);
            if (popupText != null)
                popupText.text = message;

            StartCoroutine(HidePopupAfterDelay());
        }

        Debug.Log("Trophy unlocked!");
    }

    IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);

        if (popupTextObject != null)
            popupTextObject.SetActive(false);
    }
}