using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PullAbilityObj : BaseAbility
{
    public GameObject playerObj;
    public GameObject panel;
    public Camera cam;
    public GameObject pullObj;
    public bool pullOff = false;
    public bool inUse = false;
    public TMP_Text cooldownText;
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
                cooldownText.text = "";
                isOnCooldown = false;
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
        panel.SetActive(true);
    }
}
