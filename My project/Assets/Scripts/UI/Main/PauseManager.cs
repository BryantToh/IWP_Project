using Unity.VisualScripting;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void HandleCursor(int pauseState)
    {
        if (pauseState > 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;

        }
        else if (pauseState == 0)
        {
            Cursor.visible = false; ;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
}
