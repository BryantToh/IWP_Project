using UnityEngine;
using TMPro;
public class timer : MonoBehaviour
{
    public TMP_Text timerObj;
    public OverseerHealth bossObj;
    public PlayerHealth player;
    private float time = 30f;
    private float timeClock;
    void Start()
    {
        timeClock = time;
        bossObj.gameObject.SetActive(false);
    }

    void Update()
    {
        //timeClock -= Time.deltaTime;
        //timerObj.text = "Time: " + timeClock.ToString("0:00");
        //if (timeClock <= 0f)
        //{
        //    timeClock = 0f;
        //    timerObj = null;
        //    bossObj.gameObject.SetActive(true);
        //}

        //if (bossObj.currentHealth <= 0f && bossObj.isActiveAndEnabled)
        //{
        //    Debug.Log("you won nigga");
        //    player.currentHealth = 9999999;
        //}
    }
}
