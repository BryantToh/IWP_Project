using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float dashSpeed;
    private bool isMoving = false;

    [Header("Inputs")]
    float horizontalInput;
    float verticalInput;

    [Header("Animations")]
    public Animator animator;
    private float IdleTimer;

    public Transform orientation;
    Vector3 moveDirection;
    Rigidbody rb;
    private Coroutine kickCoroutine;
    private Queue<PrimaryActionCommand> _primaryActionCommandQueue = new Queue<PrimaryActionCommand>();
    public int kickSteps = -1;


    public void ReadPrimaryActionCommand(PrimaryActionCommand command)
    {
        _primaryActionCommandQueue.Enqueue(command);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (kickCoroutine != null)
            return;

        Inputs();
        CheckIdle();


        if (_primaryActionCommandQueue.Count > 0 && kickCoroutine == null && kickSteps < 4)
        {
            kickCoroutine = StartCoroutine(PlayKick());
        }
    }

    private void LateUpdate()
    {
        MovePlayer();
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection * moveSpeed * 5f);

        if (verticalInput != 0 || horizontalInput != 0)
        {
            IdleTimer = 0f;
            isMoving = true;
        }

        if (isMoving)
            MoveAnimation();
    }

    private void CheckIdle()
    {
        if (!isMoving)
        {
            IdleTimer += Time.deltaTime;

            if (IdleTimer >= 4.0f)
            {
                animator.SetInteger("Idle", 1);
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("IdleAnim") && stateInfo.normalizedTime > 0.98f)
                    StartCoroutine(ResetIdleAnimation());
            }
        }
    }

    private void MoveAnimation()
    {
        ResetMovementBools();

        if (verticalInput > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else if (verticalInput < 0)
        {
            animator.SetBool("isMovingBack", true);
        }
        else if (horizontalInput < 0)
        {
            animator.SetBool("isMovingLeft", true);
        }
        else if (horizontalInput > 0)
        {
            animator.SetBool("isMovingRight", true);
        }
        else
        {
            isMoving = false;
        }
    }

    private void ResetMovementBools()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isMovingBack", false);
        animator.SetBool("isMovingLeft", false);
        animator.SetBool("isMovingRight", false);
    }

    private IEnumerator ResetIdleAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger("Idle", 0);
        IdleTimer = 0f;
    }

    private IEnumerator PlayKick()
    {
        _primaryActionCommandQueue.Dequeue();

        kickSteps = (kickSteps + 1) % 4;
        animator.SetInteger("Kick", kickSteps);

        // Update AnimatorStateInfo each time to get the latest animation state
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the animator has entered the new kick animation state
        while (!stateInfo.IsName("Anim_Kick" + kickSteps))
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        // Wait for the animation to finish playing
        yield return new WaitForSeconds(stateInfo.length - 0.2f);

        if (_primaryActionCommandQueue.Count <= 0)
        {
            // Reset kick animation
            kickSteps = -1;
            animator.SetInteger("Kick", kickSteps);
            kickCoroutine = null;
        }
        else
        {
            // Continue to the next kick animation
            kickCoroutine = StartCoroutine(PlayKick());
        }
    }

}
