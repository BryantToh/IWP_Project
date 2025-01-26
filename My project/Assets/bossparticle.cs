using UnityEngine;

public class bossparticle : MonoBehaviour
{
    public OverseerEnemy overseerEnemy;
    public void PlayAttackNotice()
    {
        overseerEnemy.PlayAttackNotice();
    }
}
