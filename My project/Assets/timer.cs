using UnityEngine;
using TMPro;
public class timer : MonoBehaviour
{
    public TMP_Text timerObj;
    public OverseerHealth bossObj;
    public PlayerHealth player;
    private float time = 30f;
    private float timeClock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeClock = time;
        bossObj.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeClock -= Time.deltaTime;
        timerObj.text = "Time: " + timeClock.ToString("0:00");
        if (timeClock <= 0f)
        {
            bossObj.gameObject.SetActive(true);
        }

        if (bossObj.currentHealth <= 0f && bossObj.isActiveAndEnabled)
        {
            Debug.Log("you won nigga");
            timeClock = 0f;
            player.currentHealth = 9999999;
        }
    }
}
