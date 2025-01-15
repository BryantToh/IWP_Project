using UnityEngine;

public class SentinelUpDown : MonoBehaviour
{
    public SentinelHealth senHealth;

    public void AttackPlayerEvent()
    {
        senHealth.AttackPlayerEvent();
    }
}
