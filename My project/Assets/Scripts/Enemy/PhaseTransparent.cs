using System.Collections;
using UnityEngine;

public class PhaseTransparent : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private float fadeInDuration = 0.7f;
    [SerializeField] private float fadeOutDuration = 3f;
    private Material material;

    [HideInInspector]
    public bool faded = false;

    public delegate void FadeCompleteHandler();
    public event FadeCompleteHandler OnFadeComplete;

    private void Start()
    {
        material = objectRenderer.material;
    }

    private void Update()
    {
        //Debug.Log(faded);
    }
    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(1f, fadeInDuration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeTo(0f, fadeOutDuration));
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        if (material == null) yield break;

        Color startColor = material.GetColor("_BaseColor");
        float startAlpha = startColor.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            Color newColor = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            material.SetColor("_BaseColor", newColor);

            yield return null;
        }

        Color finalColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        material.SetColor("_BaseColor", finalColor);
        Debug.Log(targetAlpha);
        if (targetAlpha <= 0f)
            faded = true;
        else
            faded = false;

        OnFadeComplete?.Invoke();
    }
}
