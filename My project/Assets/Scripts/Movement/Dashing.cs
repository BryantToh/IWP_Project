using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement playerMove;
    public OnField playerMaxDash;
    public Image rechargeUI;

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
        rechargeUI.fillAmount = 1f; // Start at full
    }

    // Update is called once per frame
    void Update()
    {
        // Dash input check
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0 && !playerMove.staggered)
        {
            Dash();
        }

        // Cooldown countdown
        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
        }

        // Start recharging if dash is used
        if (dashCount < 3 && dashCDTimer <= 0 && !isAddingDash)
        {
            StartCoroutine(AddDash());
        }

        // Update UI
        UpdateDashUI();
    }

    private void Dash()
    {
        if (dashCDTimer > 0) return; // Can't dash if cooldown is active

        isDashing = true;

        if (dashCount > 0)
        {
            dashCDTimer = dashCD;
            dashCount--;
        }

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

        AudioManager.instance.PlaySFX("dash");
        delayedForceToApply = DashForce;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
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

        float rechargeTime = 4.0f; // Time it takes to recharge a dash
        float timer = 0f;

        while (timer < rechargeTime)
        {
            timer += Time.deltaTime;
            rechargeUI.fillAmount = Mathf.Lerp(rechargeUI.fillAmount, (dashCount + 1) / 3f, timer / rechargeTime);
            yield return null;
        }

        dashCount++;
        isAddingDash = false;
    }

    private void UpdateDashUI()
    {
        rechargeUI.fillAmount = dashCount / 3f; // Scale UI based on available dashes
    }
}
