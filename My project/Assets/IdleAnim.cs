using System.Resources;
using UnityEngine;

public class IdleAnim : StateMachineBehaviour
{
    [SerializeField]
    private float timeTillBored;
    [SerializeField]
    private int numOfAnim;
    private bool isIdle;
    private float idleTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isIdle)
        {
            idleTime += Time.deltaTime;
            if (idleTime > timeTillBored)
            {
                isIdle = true;
                int idleAnimation = Random.Range(1, numOfAnim + 1);
                animator.SetFloat("Idle", idleAnimation);
            }
        }
        else if(stateInfo.normalizedTime % 1 > 0.98f)
        {
            ResetIdle(animator);
        }
    }

    private void ResetIdle(Animator animator)
    {
        isIdle = false;
        idleTime = 0;
        animator.SetFloat("Idle", 0);
    }
}
