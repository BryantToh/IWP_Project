using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    public TMP_Text timerObj;
    public OverseerHealth bossObj;
    public PlayerHealth player;
    public UIController uiController;
    public bool bossSpawned = false;
    private float time = 300f;
    private float timeClock;
    public bool gameOver = false;
    public DeathLogic death;
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

            int minutes = Mathf.FloorToInt(timeClock / 60f);
            int seconds = Mathf.FloorToInt(timeClock % 60f);

            timerObj.text = $"Time: {minutes:00}:{seconds:00}";
        }
        else if (timeClock <= 0f)
        {
            timeClock = 0f;
            timerObj.text = "Time: 00:00";
            bossObj.gameObject.SetActive(true);
            bossSpawned = true;
        }

        if (bossObj.currentHealth <= 0f && bossObj.isActiveAndEnabled && !death.activated)
        {
            if (!bossObj.isDead)
                return;

            gameOver = true;
            uiController.ShowGameOver(gameOver);
            uiController.paused += 1;
        }

        if (player.currentHealth <= 0 && !death.activated)
        {
            gameOver = true;
            uiController.ShowGameOver(gameOver);
            uiController.paused += 1;
        }
    }
}
