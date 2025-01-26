using System.Collections;
using UnityEngine;

public class BossAttack_Laser : MonoBehaviour
{
    public GameObject player;
    public GameObject laserBeam;
    public SurgeLogic surgeLogic;
    public float damage;
    public Transform rangeAttackSpawn;
    public float maxBeamDist;
    public LayerMask playerLayer;
    public bool laserShot = false;
    public void BeamAttack()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
        LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();
        Vector3 dir = (newPos - rangeAttackSpawn.position).normalized;
        bool collidePlayer = Physics.Raycast(rangeAttackSpawn.position, dir, out RaycastHit hit, maxBeamDist, playerLayer);
        laserBeam.SetActive(true);
        lineRenderer.SetPosition(0, rangeAttackSpawn.position);
        Vector3 targetEndPoint = collidePlayer ? hit.point : rangeAttackSpawn.position + dir * maxBeamDist;
        if (collidePlayer)
        {
            PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
            Dashing playerDash = player.gameObject.GetComponent<Dashing>();
            if (!playerDash.isDashing)
            {
                playerHealth.TakeDamage(damage);
            }
            else
            {
                if (!surgeLogic.attackDodged)
                    surgeLogic.attackDodged = true;
                else
                    Debug.Log("passive already active");
            }
        }
        StartCoroutine(AnimateLaserBeam(lineRenderer, rangeAttackSpawn.position, targetEndPoint));
    }

    private IEnumerator AnimateLaserBeam(LineRenderer lineRenderer, Vector3 start, Vector3 end)
    {
        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Vector3 currentEndPoint = Vector3.Lerp(start, end, t);
            lineRenderer.SetPosition(1, currentEndPoint);
            yield return null;
        }
        lineRenderer.SetPosition(1, end);
        yield return new WaitForSeconds(0.5f);
        laserBeam.SetActive(false);
        laserShot = true;
    }
}