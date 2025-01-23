using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseObj, abilityObj, uiCanvas;
    DetailsHandler details;
    private void Start()
    {
        uiCanvas.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !uiCanvas.activeInHierarchy)
        {
            uiCanvas.SetActive(true);
            abilityObj.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && uiCanvas.activeInHierarchy)
        {
            uiCanvas.SetActive(false);
        }
    }

    public void HidePause()
    {
        pauseObj.SetActive(false);
        abilityObj.SetActive(true);
        details.index = 0;
    }
    public void HideAbility()
    {
        abilityObj.SetActive(false);
        pauseObj.SetActive(true);
    }
}
