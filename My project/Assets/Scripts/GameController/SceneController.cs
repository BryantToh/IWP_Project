using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public void ChangeToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ChangeToMain()
    {
        SceneManager.LoadScene(1);
    }
    public void ChangeToTutorial()
    {
        SceneManager.LoadScene(2);
    }
    public void CloseApp()
    {
        Application.Quit();
    }
}
