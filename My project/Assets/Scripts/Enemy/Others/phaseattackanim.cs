using UnityEngine;

public class phaseattackanim : MonoBehaviour
{
    public PhaseHealth phaseHealth;
    public void AttackPlayerEvent()
    {
        phaseHealth.AttackPlayerEvent();
    }
}
