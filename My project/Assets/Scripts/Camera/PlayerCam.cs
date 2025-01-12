using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Camera Sensitivity")]
    public float sensX = 100f;
    public float sensY = 100f;

    [Header("Player & Camera Settings")]
    public Transform player;
    public Camera mainCamera;

    [Header("Targeting Settings")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private KeyCode lockTargetKey = KeyCode.Tab;
    [SerializeField] private Vector2 targetLockOffset = Vector2.zero;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 20f;

    private float xRotation;
    private float yRotation;
    private bool isTargeting;
    [HideInInspector]
    public Transform currentTarget;
    public Health currentTargetHealth;
    private float maxAngle = 360f;
    private float rotSpeed = 5f;

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
            // Check if the current target is dead and switch to the next target
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

        Vector3 directionToTarget = currentTarget.position - transform.position;

        // Smoothly adjust player and camera rotation to face the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        player.rotation = Quaternion.Slerp(player.rotation, Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f), rotSpeed * Time.deltaTime);

        // Camera adjustment for Y rotation
        xRotation = Mathf.Lerp(xRotation, targetRotation.eulerAngles.x, rotSpeed * Time.deltaTime);
        yRotation = targetRotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
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
        if (nextTarget != null)
        {
            SetNewTarget(nextTarget.transform);
        }
        else
        {
            // No valid targets left
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
            if (!enemy.activeInHierarchy) continue; // Ignore inactive enemies

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
