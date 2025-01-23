using UnityEngine;
using TMPro;
public class timer : MonoBehaviour
{
    public TMP_Text timerObj;
    public OverseerHealth bossObj;
    public PlayerHealth player;
    public UIController uiController;
    private float time = 5f;
    private float timeClock;
    public bool gameOver = false;
    void Start()
    {
        timeClock = time;
        bossObj.gameObject.SetActive(false);
    }

    void Update()
    {
        timeClock -= Time.deltaTime;
        timerObj.text = "Time: " + timeClock.ToString("0:00");
        if (timeClock <= 0f)
        {
            timeClock = 0f;
            timerObj = null;
            bossObj.gameObject.SetActive(true);
        }

        if (bossObj.currentHealth <= 0f && bossObj.isActiveAndEnabled)
        {
            gameOver = true;
            uiController.ShowGameOver(gameOver);
            uiController.paused += 1;
        }
    }
}
