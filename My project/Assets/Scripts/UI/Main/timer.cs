using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    public TMP_Text timerObj;
    public OverseerHealth bossObj;
    public PlayerHealth player;
    public UIController uiController;
    private float time = 300f; // 5 minutes in seconds
    private float timeClock;
    public bool gameOver = false;

    void Start()
    {
        timeClock = time;
        bossObj.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timeClock > 0)
        {
            timeClock -= Time.deltaTime;

            // Convert timeClock to minutes and seconds
            int minutes = Mathf.FloorToInt(timeClock / 60f);
            int seconds = Mathf.FloorToInt(timeClock % 60f);

            // Update the timer text in MM:SS format
            timerObj.text = $"Time: {minutes:00}:{seconds:00}";
        }
        else if (timeClock <= 0f)
        {
            timeClock = 0f;
            timerObj.text = "Time: 00:00";
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
