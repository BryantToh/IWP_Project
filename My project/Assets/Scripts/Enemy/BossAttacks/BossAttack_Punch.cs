using UnityEngine;

public class BossAttack_Punch : MonoBehaviour
{
    public SphereCollider sphereCollider;
    private void Start()
    {
        sphereCollider.enabled = false;
    }
    public void Punching()
    {
        sphereCollider.enabled = true;
    }
    public void NotPunching()
    {
        sphereCollider.enabled = false;
    }
}
