using UnityEngine;

public class OverseerPunnch : MonoBehaviour
{
    public OverseerHealth health;

    public void AttackPlayerEvent()
    {
        health.AttackPlayerEvent();
        AudioManager.instance.PlaySFX("bosspunch");
    }
}
