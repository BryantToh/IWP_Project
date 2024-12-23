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
    public PlayerCam cam;
    Vector3 moveDirection;
    Rigidbody rb;
<<<<<<< Updated upstream
=======
    private Coroutine kickCoroutine;
    private Queue<PrimaryActionCommand> _primaryActionCommandQueue = new Queue<PrimaryActionCommand>();
    public int kickSteps = -1;
    public bool isCharging = false;

    public void ReadPrimaryActionCommand(PrimaryActionCommand command)
    {
        _primaryActionCommandQueue.Enqueue(command);

        if (kickCoroutine == null)
        {
            kickCoroutine = StartCoroutine(PlayKick());
        }
    }
>>>>>>> Stashed changes

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Inputs();
        CheckIdle();
        rotateToTarget();
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

    private void rotateToTarget()
    {
        if (cam.currentTarget != null)
        {
            Vector3 direction = cam.currentTarget.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cam.rotSpeed * Time.deltaTime);
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
<<<<<<< Updated upstream
=======

    private IEnumerator PlayKick()
    {
        var command = _primaryActionCommandQueue.Dequeue();
        isAttacking = true;

        if (command.Action == 1)
        {
            animator.SetTrigger("ChargedKick");

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("Heavy_Attack"))
            {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
            yield return new WaitForSeconds(stateInfo.length - 0.2f);
        }
        else
        {
            kickSteps = (kickSteps + 1) % 5;
            animator.SetInteger("Kick", kickSteps);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("Anim_Kick" + kickSteps))
            {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
            yield return new WaitForSeconds(stateInfo.length - 0.2f);
        }

        if (_primaryActionCommandQueue.Count <= 0 || kickSteps == 4)
        {
            kickSteps = -1;
            animator.SetInteger("Kick", kickSteps);
            kickCoroutine = null;
            isAttacking = false;
            _primaryActionCommandQueue.Clear();
        }
        else
        {
            yield return PlayKick();
        }
    }

>>>>>>> Stashed changes
}
