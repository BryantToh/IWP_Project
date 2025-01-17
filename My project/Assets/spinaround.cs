using UnityEngine;

public class spinaround : MonoBehaviour
{
    public float spinSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
