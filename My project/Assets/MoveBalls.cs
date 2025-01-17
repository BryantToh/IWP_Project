using UnityEngine;

public class MoveBalls : MonoBehaviour
{
    public float radius = 2f; // Distance from the center to each sphere
    public float speed = 1f;  // Speed of movement

    private Vector3[] targetPositions; // Target positions for the spheres
    private Vector3[] startPositions;  // Starting positions for the spheres
    private Transform[] spheres;      // References to the child spheres
    private bool moveToTarget = false; // Flag to toggle movement direction

    void Start()
    {
        // Ensure the GameObject has exactly 3 children
        if (transform.childCount != 3)
        {
            Debug.LogError("The GameObject must have exactly 3 child spheres!");
            return;
        }

        // Initialize arrays
        spheres = new Transform[3];
        targetPositions = new Vector3[3];
        startPositions = new Vector3[3];

        // Store initial positions as starting positions
        for (int i = 0; i < 3; i++)
        {
            spheres[i] = transform.GetChild(i);
            startPositions[i] = spheres[i].localPosition;
        }

        // Calculate target positions for the equilateral triangle
        float angleIncrement = 120f * Mathf.Deg2Rad;
        for (int i = 0; i < 3; i++)
        {
            float angle = i * angleIncrement;
            targetPositions[i] = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
        }
    }

    void Update()
    {
        if (moveToTarget)
        {
            MoveToPositions(targetPositions);
        }
        else
        {
            MoveToPositions(startPositions);
        }
    }

    public void MoveToEndpoint()
    {
        moveToTarget = true;
    }

    public void MoveToStartPoint()
    {
        moveToTarget = false;
    }

    private void MoveToPositions(Vector3[] positions)
    {
        for (int i = 0; i < spheres.Length; i++)
        {
            if (spheres[i] != null)
            {
                spheres[i].localPosition = Vector3.MoveTowards(
                    spheres[i].localPosition,
                    positions[i],
                    speed * Time.deltaTime
                );
            }
        }
    }
}
