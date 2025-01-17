using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BossAttack_Zoning : MonoBehaviour
{
    public GameObject zoneObj;
    public GameObject player;
    public PlayerMovement playerMove;
    OverseerHealth health;
    MoveBalls moveBalls;
    public float pushBackForce;
    float time = 5f;
    float timer;
    bool inRangeToPush = false;

    private void Start()
    {
        timer = time;
        moveBalls = zoneObj.GetComponent<MoveBalls>();
        health = GetComponent<OverseerHealth>();
    }
    private void Update()
    {
        if (zoneObj.activeInHierarchy)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                moveBalls.MoveToStartPoint();
                zoneObj.SetActive(false);
                timer = time;
            }
        }
    }
    public void SpawnBall()
    {
        zoneObj.SetActive(true);

        if (inRangeToPush)
            PushBack();

        moveBalls.MoveToEndpoint();
    }

    public void PushBack()
    {
        Rigidbody playerrb = player.GetComponent<Rigidbody>();
        Vector3 dir = (player.transform.position - transform.position).normalized;
        playerrb.AddForce(dir * pushBackForce, ForceMode.Impulse);
        StartCoroutine(PlayerStaggered());
        inRangeToPush = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
            inRangeToPush = true;
    }

    IEnumerator PlayerStaggered()
    {
        playerMove.staggered = true;
        yield return new WaitForSeconds(1f);
        playerMove.staggered = false;
    }
}
