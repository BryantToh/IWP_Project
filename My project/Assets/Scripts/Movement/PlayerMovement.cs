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

        if (moveDirection.magnitude > 0.1f)
        {
            isMoving = true;
            IdleTimer = 0f;
        }
        else 
            isMoving = false;
    }

    private void CheckIdle()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!isMoving)
        {
            IdleTimer += Time.deltaTime;

            if (IdleTimer >= 4.0f)
            {
                animator.SetInteger("Idle", 1);

                if (stateInfo.IsName("IdleAnim") && stateInfo.normalizedTime > 0.98f)
                    StartCoroutine(ResetIdleAnimation());
            }
            else if (IdleTimer < 4.0f)
            {
                animator.SetInteger("Idle", 0);
            }
        }
    }

    private IEnumerator ResetIdleAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger("Idle", 0);
        IdleTimer = 0f;
    }
}
