using System.Collections;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public Material mat;
    public float noiseAmount;
    public float glitchStrength;
    public float scanLinesStrength;
    public bool isResetting = false;
    public bool resetChecking = false;

    private void Start()
    {
        noiseAmount = 0f;
        glitchStrength = 0f;
        scanLinesStrength = 1f;
    }

    void Update()
    {
        mat.SetFloat("_NoiseAmount", noiseAmount);
        mat.SetFloat("_GlitchStrength", glitchStrength);
        mat.SetFloat("_ScanLinesStrength", scanLinesStrength);

        ResetGlitch();
        CheckReset();
    }

    public void GlitchEffect()
    {
        noiseAmount = 20f;
        glitchStrength = 40f;
        scanLinesStrength = 0.3f;

        //if (glitchStack != 5)
        //{
        //    // Apply glitch effect
        //    noiseAmount += 10f;
        //    glitchStrength += 4f;
        //    scanLinesStrength -= 0.18f;

        //    // Clamp values to valid ranges
        //    noiseAmount = Mathf.Clamp(noiseAmount, 0, 50);
        //    glitchStrength = Mathf.Clamp(glitchStrength, 0, 50);
        //    scanLinesStrength = Mathf.Clamp(scanLinesStrength, 0, 1);

        //    glitchStack++;

        //    // Interrupt the reset process
        //    isResetting = false;
        //}
    }

    public void ResetGlitch()
    {
        if (!isResetting) return;

        //// Smoothly transition the values
        //noiseAmount = Mathf.Lerp(noiseAmount, 0, Time.deltaTime);
        //glitchStrength = Mathf.Lerp(glitchStrength, 0, Time.deltaTime);
        //scanLinesStrength = Mathf.Lerp(scanLinesStrength, 1, Time.deltaTime);

        //// Clamp values to valid ranges
        //noiseAmount = Mathf.Clamp(noiseAmount, 0, float.MaxValue);
        //glitchStrength = Mathf.Clamp(glitchStrength, 0, float.MaxValue);
        //scanLinesStrength = Mathf.Clamp(scanLinesStrength, 0, 1);

        //// Check if values are approximately at their targets
        //if (noiseAmount <= 0.1f && glitchStrength <= 0.1f && scanLinesStrength >= 0.95f)
        //{
        //    noiseAmount = 0;
        //    glitchStrength = 0;
        //    scanLinesStrength = 1;
        //    isResetting = false;
        //    glitchStack = 0;
        //}

        noiseAmount = 0;
        glitchStrength = 0;
        scanLinesStrength = 1;
        isResetting = false;
    }

    private void CheckReset()
    {
        if (resetChecking)
        {
            StartCoroutine(ResetThingsCoroutine());
        }
    }

    private IEnumerator ResetThingsCoroutine()
    {
        resetChecking = false;
        yield return new WaitForSeconds(0.3f);
        isResetting = true;
    }
}