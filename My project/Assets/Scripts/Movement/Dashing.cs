using System.Collections;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement playerMove;
    public OnField playerMaxDash;

    [Header("Dashing")]
    public float dashForce;
    public float dashDuration;
    private Vector3 delayedForceToApply;
    public float dashCount;
    private bool isAddingDash = false;
    public bool isDashing = false;

    [Header("Cooldown")]
    public float dashCD;
    public float dashCDTimer;

    [Header("Settings")]
    public bool useCameraForward;
    public bool allowAllDirections;
    public bool disableGravity;

    Transform forwardT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMove = GetComponent<PlayerMovement>();
        dashCount = playerMaxDash.maxDashCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0)
        {
            Dash();
        }

        if (dashCDTimer > 0)
            dashCDTimer -= Time.deltaTime;

        if (dashCount < 3 && dashCDTimer <= 0 && !isAddingDash)
        {
            StartCoroutine(AddDash());
        }
    }

    private void Dash()
    {
        if (dashCDTimer > 0) return;

        isDashing = true;

        if (dashCount > 0)
            dashCDTimer = dashCD;

        if (useCameraForward)
            forwardT = playerCam;
        else 
            forwardT = player;

        Vector3 Direction = GetDirection(forwardT);
        Vector3 DashForce = Direction * dashForce;

        if (!disableGravity)
        {
            rb.useGravity = true;
        }

        delayedForceToApply = DashForce;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
        dashCount--;
    }

    private void ResetDash()
    {
        if (disableGravity)
            rb.useGravity = false;
        isDashing = false;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
        {
            direction = forwardT.forward;
        }
        return direction.normalized;
    }

    private IEnumerator AddDash()
    {
        isAddingDash = true;
        yield return new WaitForSeconds(4.0f);
        dashCount++;
        isAddingDash = false;
    }
}
