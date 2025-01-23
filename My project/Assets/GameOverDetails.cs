using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOverDetails : MonoBehaviour
{
    public TMP_Text gameOverHeader;
    private void Start()
    {
        gameOverHeader.text = "Game Over";
    }
    public void RtnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
