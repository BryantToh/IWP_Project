using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public string abilityName;
    [HideInInspector]
    public float cooldownTimer = 0f;
    public float cooldownTime;
    [HideInInspector]
    public bool isOnCooldown = false;
    public KeyCode activationKey;

    public abstract void Activate();
}
