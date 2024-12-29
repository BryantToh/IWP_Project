using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public Material mat;
    public float noiseAmount;
    public float glitchStrength;
    public float scanLinesStrength;

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
    }
}
