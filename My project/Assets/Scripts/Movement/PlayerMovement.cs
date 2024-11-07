using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float dashSpeed;

    [Header("Inputs")]
    float horizontalInput;
    float verticalInput;

    [Header("Animations")]
    public Animator animator;
    public string[] animationArray = new string[3];
    int rndrange;

    public Transform orientation;
    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animationArray[0] = "NinjaIdle";
        animationArray[1] = "NinjaIdleWM";
        animationArray[2] = "NinjaIdleWM2";

        StartCoroutine(IdleAnimationCoroutine());
    }

    private void Update()
    {
        Inputs();
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
    }

    private IEnumerator IdleAnimationCoroutine()
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            foreach (string idleAni in animationArray)
            {
                if (stateInfo.IsName(idleAni))
                {
                    while (stateInfo.normalizedTime < 1f)
                    {
                        yield return null;
                        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    }

                    rndrange = Random.Range(0, 2);
                    animator.SetInteger("Idle", rndrange);

                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return null;
        }
    }
}
