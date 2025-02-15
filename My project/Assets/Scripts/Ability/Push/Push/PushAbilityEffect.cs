using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PushAbilityEffect : BaseAbility
{
    private float effectRadius = 6f;
    public LayerMask affectedLayer;
    public GameObject panel;
    public TMP_Text cooldownText;
    public ParticleSystem particle;
    [HideInInspector]
    public bool canAOE = false;
    public bool inUse = false;
    private bool hasBeenUsed = false;
    private void Start()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = cooldownTimer.ToString("F1");
            if (cooldownTimer <= 0f)
            {
                panel.SetActive(false);
                isOnCooldown = false;
                cooldownText.text = "";
                hasBeenUsed = false;
                inUse = false;
            }
        }
    }
    public void ApplyPushback(Vector3 sourcePosition)
    {
        if (hasBeenUsed)
            return;

        Collider[] colliders = Physics.OverlapSphere(sourcePosition, effectRadius, affectedLayer);
        foreach (var collider in colliders)
        {
            PushAbilityLogic pushBack = collider.GetComponent<PushAbilityLogic>();
            if (pushBack != null)
            {
                pushBack.ApplyPush(sourcePosition);
            }
        }
        hasBeenUsed = true;
        canAOE = true;
    }
    public override void Activate()
    {
        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }
        ApplyPushback(transform.position);
        Instantiate(particle, transform.position, Quaternion.identity);
        inUse = true;
        panel.SetActive(true);
    }
}
