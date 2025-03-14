using System.Collections;
using UnityEngine;

public class PhaseTransparent : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private float fadeInDuration = 0.7f;
    [SerializeField] private float fadeOutDuration = 3f;
    private Material[] materials;

    [HideInInspector] public bool faded = false;

    public delegate void FadeCompleteHandler();
    public event FadeCompleteHandler OnFadeComplete;

    private void Start()
    {
        materials = objectRenderer.materials;
    }
    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(1f, fadeInDuration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeTo(0f, fadeOutDuration));
        Debug.Log(materials[0].color.a.ToString());
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        if (materials.Length == 0) yield break;

        float elapsedTime = 0f;
        float[] startAlphas = new float[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            Color startColor = materials[i].GetColor("_BaseColor");
            startAlphas[i] = startColor.a;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlphaFactor = elapsedTime / duration;

            for (int i = 0; i < materials.Length; i++)
            {
                Color startColor = materials[i].GetColor("_BaseColor");
                float newAlpha = Mathf.Lerp(startAlphas[i], targetAlpha, newAlphaFactor);

                Color newColor = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
                materials[i].SetColor("_BaseColor", newColor);
            }

            yield return null;
        }

        for (int i = 0; i < materials.Length; i++)
        {
            Color finalColor = materials[i].GetColor("_BaseColor");
            finalColor.a = targetAlpha;
            materials[i].SetColor("_BaseColor", finalColor);
        }

        faded = targetAlpha <= 0f;
        OnFadeComplete?.Invoke();
    }
}
