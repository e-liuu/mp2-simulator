using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialPopup : MonoBehaviour
{
    [Header("References")]
    public GameObject rootObject;
    public RectTransform popupPanel;
    public TMP_Text tutorialText;

    [Header("Timing")]
    public float showTime = 2f;
    public float popInDuration = 0.25f;
    public float settleDuration = 0.15f;
    public float overshootScale = 1.1f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip popupSound;

    private bool isShowing = false;

    void Start()
    {
        if (rootObject != null)
            rootObject.SetActive(false);

        if (popupPanel != null)
            popupPanel.localScale = Vector3.one;
    }

    public void ShowTutorial(string message)
    {
        if (isShowing) return;
        StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        isShowing = true;

        if (tutorialText != null)
            tutorialText.text = message;

        if (rootObject != null)
            rootObject.SetActive(true);

        if (audioSource != null && popupSound != null)
            audioSource.PlayOneShot(popupSound);

        // initial scale = 0
        if (popupPanel != null)
            popupPanel.localScale = Vector3.zero;

        // 0 -> overshootScale
        float timer = 0f;
        while (timer < popInDuration)
        {
            timer += Time.deltaTime;
            float t = timer / popInDuration;

            // ease out
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            if (popupPanel != null)
                popupPanel.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one * overshootScale, eased);

            yield return null;
        }

        // overshootScale -> 1
        timer = 0f;
        while (timer < settleDuration)
        {
            timer += Time.deltaTime;
            float t = timer / settleDuration;

            // ease out
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            if (popupPanel != null)
                popupPanel.localScale = Vector3.LerpUnclamped(Vector3.one * overshootScale, Vector3.one, eased);

            yield return null;
        }

        if (popupPanel != null)
            popupPanel.localScale = Vector3.one;

        yield return new WaitForSeconds(showTime);

        if (rootObject != null)
            rootObject.SetActive(false);

        isShowing = false;
    }
}