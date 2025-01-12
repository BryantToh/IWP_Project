using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Camera Sensitivity")]
    public float sensX;
    public float sensY;

    [Header("Player & Camera Settings")]
    public Transform player;
    public Camera mainCamera;

    [Header("Targeting Settings")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private KeyCode lockTargetKey = KeyCode.Tab;
    [SerializeField] private Vector2 targetLockOffset = Vector2.zero;
    [SerializeField] private float maxDistance;

    public bool isTargeting;
    private float xRotation;
    private float yRotation;
    [HideInInspector]
    public Health currentTargetHealth;
    public Transform currentTarget;
    private float rotSpeed = 8f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!isTargeting)
        {
            HandleManualCameraMovement();
        }
        else
        {
            AdjustCameraToTarget();
        }

        if (Input.GetKeyDown(lockTargetKey))
        {
            ToggleTargetLock();
        }

        if (currentTargetHealth != null && isTargeting)
        {
            if (currentTargetHealth.currentHealth <= 0f)
            {
                SwitchToNextTarget();
            }
        }
    }

    private void HandleManualCameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    private void AdjustCameraToTarget()
    {
        if (!currentTarget) return;

        Vector3 viewPos = mainCamera.WorldToViewportPoint(currentTarget.position);
        viewPos.x += targetLockOffset.x;
        viewPos.y += targetLockOffset.y;

        Vector3 directionToTarget = mainCamera.ViewportToWorldPoint(viewPos) - transform.position;
        directionToTarget.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        xRotation = Mathf.LerpAngle(xRotation, targetRotation.eulerAngles.x, rotSpeed * Time.deltaTime);
        yRotation = Mathf.LerpAngle(yRotation, targetRotation.eulerAngles.y, rotSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.rotation = Quaternion.Slerp(player.rotation, Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f), rotSpeed * Time.deltaTime);
    }
    private void ToggleTargetLock()
    {
        if (isTargeting)
        {
            isTargeting = false;
            currentTarget = null;
        }
        else
        {
            GameObject closest = FindClosestTarget();
            if (closest != null)
            {
                SetNewTarget(closest.transform);
            }
        }
    }

    private void SwitchToNextTarget()
    {
        GameObject nextTarget = FindClosestTarget();

        // Only switch to a new target if it's different and actually closer
        if (nextTarget != null && nextTarget != currentTarget)
        {
            float distanceToNewTarget = Vector3.Distance(transform.position, nextTarget.transform.position);

            // Add a small dead zone to avoid switching to a target that is too close
            if (distanceToNewTarget < maxDistance * 0.8f)
            {
                SetNewTarget(nextTarget.transform);
            }
        }
        else
        {
            isTargeting = false;
            currentTarget = null;
        }
    }


    private void SetNewTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        currentTargetHealth = newTarget.GetComponent<Health>();
        isTargeting = true;
    }

    private GameObject FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float closestDistance = maxDistance;

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;

            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;

            if (distanceToEnemy < closestDistance)
            {
                closest = enemy;
                closestDistance = distanceToEnemy;
            }
        }
        return closest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
