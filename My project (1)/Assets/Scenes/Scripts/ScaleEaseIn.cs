using UnityEngine;
using System.Collections;

public class ScaleEaseIn : MonoBehaviour
{
    public float speed = 8f;

    void Start()
    {
        Vector3 targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
        StartCoroutine(Ease(targetScale));
    }

    IEnumerator Ease(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.001f)
        {
            transform.localScale += (targetScale - transform.localScale) * speed * Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
