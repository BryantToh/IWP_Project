using UnityEngine;
using UnityEngine.SceneManagement;
public class changescenes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
