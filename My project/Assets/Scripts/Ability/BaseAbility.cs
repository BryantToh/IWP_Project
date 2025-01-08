using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    public string abilityName;
    public float cooldownTime;
    public KeyCode activationKey;
    [HideInInspector]
    public bool isOnCooldown = false;
    [HideInInspector]
    public float cooldownTimer = 0f;

    public abstract void Activate();
}
