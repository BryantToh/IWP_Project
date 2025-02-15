using UnityEngine;

public class DestoryParticle : MonoBehaviour
{
    public ParticleSystem particle;
    // Update is called once per frame
    void Update()
    {
        if (particle != null)
        {
            Destroy(gameObject, particle.main.duration);
        }
    }
}
