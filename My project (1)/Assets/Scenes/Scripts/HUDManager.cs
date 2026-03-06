using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Sunlight UI")]
    public TMP_Text sunlightCountText;
    public TMP_Text sunlightRateText;

    [Header("Water UI")]
    public GameObject waterPanel;
    public TMP_Text waterCountText;
    public TMP_Text waterRateText;
    public GameObject lockOverlay;

    private ResourceManager resources;

    void Start()
    {
        resources = ResourceManager.Instance;
        waterPanel.SetActive(true);
        lockOverlay.SetActive(true);
        waterCountText.color = Color.gray;
        waterRateText.color  = Color.gray;
    }

    void Update()
    {
        sunlightCountText.text = $"{resources.sunlight:F0}";
        sunlightRateText.text  = $"+{resources.SunlightRate:F1}/sec";

        waterCountText.text = $"{resources.water:F0}";
        waterRateText.text  = $"+{resources.WaterRate:F1}/sec";
    }

    public void UnlockWaterUI()
    {
        lockOverlay.SetActive(false);
        waterCountText.color = Color.white;
        waterRateText.color  = Color.white;
        StartCoroutine(AnimateUnlock());
    }

    private System.Collections.IEnumerator AnimateUnlock()
    {
        float t = 0f;
        CanvasGroup cg = waterPanel.GetComponent<CanvasGroup>();
        if (cg == null) cg = waterPanel.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            cg.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
    }
}