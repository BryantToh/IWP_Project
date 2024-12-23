using System.Collections;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Inputs();
        CheckIdle();
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
}
