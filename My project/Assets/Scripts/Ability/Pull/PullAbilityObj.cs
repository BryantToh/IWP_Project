using UnityEngine;
using UnityEngine.UI;

public class PullAbilityObj : BaseAbility
{
    public GameObject playerObj;
    public Camera cam;
    public Image imageIcon;
    public GameObject pullObj;
    public bool pullOff = false;
    public bool inUse = false;

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                imageIcon.enabled = true;
            }
        }
    }

    public override void Activate()
    {
        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        if (GameObject.FindWithTag("PullObj") != null)
        {
            Debug.Log("A pull object already exists. Cannot create another.");
            return;
        }
        GameObject obj = Instantiate(pullObj, Camera.main.transform.position + Camera.main.transform.forward * 7f, Quaternion.identity);
        imageIcon.enabled = false;
    }
}
