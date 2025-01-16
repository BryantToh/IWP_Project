using UnityEngine;

public class BossAttack_TrackingMIssile : MonoBehaviour
{
    public GameObject player;
    public GameObject missileObj;
    public Transform spawnPoint;
    float speed = 5f;

    public void ShootMissile()
    {
        // Instantiate the missile at the spawn point
        GameObject obj = Instantiate(missileObj, spawnPoint.position, Quaternion.Euler(90,0,0));
            
        // Get the Rigidbody component of the missile
        Rigidbody missileRb = obj.GetComponent<Rigidbody>();
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
        // Calculate the direction towards the player
        Vector3 direction = (newPos - spawnPoint.position).normalized;

        // Set the missile's Rigidbody velocity to move towards the player
        missileRb.linearVelocity = direction * speed;

    }
}
