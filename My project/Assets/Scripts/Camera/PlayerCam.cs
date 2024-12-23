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
    private Transform currentTarget;
    private float maxAngle = 90f;

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

        if ((currentTarget.position - transform.position).magnitude < minDistance) return;

        float mouseX = (viewPos.x - 0.5f + targetLockOffset.x) * 3f;
        float mouseY = (viewPos.y - 0.5f + targetLockOffset.y) * 3f;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);
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
                currentTarget = closest.transform;
                isTargeting = true;
            }
        }
    }

    private GameObject FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float closestDistance = maxDistance;
        float smallestAngle = maxAngle;

        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;

            if (distanceToEnemy < closestDistance)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(enemy.transform.position);

                if (Vector3.Angle(directionToEnemy.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    closest = enemy;
                    closestDistance = distanceToEnemy;
                    smallestAngle = Vector3.Angle(directionToEnemy.normalized, mainCamera.transform.forward);
                }
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
