using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public string abilityName;
    public float cooldownTime;
    public float abilityDuration;
    public KeyCode activationKey;

    public abstract void Activate();
}
