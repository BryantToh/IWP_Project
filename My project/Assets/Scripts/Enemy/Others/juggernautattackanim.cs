using UnityEngine;

public class juggernautattackanim : MonoBehaviour
{
    public JuggernautHealth jugHealth;
    public void AttackPlayerEvent()
    {
        jugHealth.AttackPlayerEvent();
    }
}
