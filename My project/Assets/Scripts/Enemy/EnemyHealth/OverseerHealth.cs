using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverseerHealth : Health
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    OverseerEnemy overseer;
    DeathLogic deathLogic;
    SurgeLogic surgeLogic;
    public LayerMask playerLayer;
    public GameObject laserBeam;
    public Transform rangeAttackSpawn;
    public float maxBeamDist;
    public bool laserShot = false;
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        overseer = GetComponent<OverseerEnemy>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
        laserBeam.SetActive(false);
    }
    //public void OnEnemySpawn()
    //{
        
    //}

    //public void OnGet()
    //{
    //    gameObject.SetActive(true);
    //}

    //public void OnRelease()
    //{
    //    gameObject.SetActive(false);
    //}

    //public void OnDestroyInterface()
    //{
    //    Destroy(gameObject);
    //}

    //public void SetPosNRot(Vector3 Pos, Quaternion Rot)
    //{
    //    transform.position = Pos;
    //    transform.rotation = Rot;
    //}

    public void AttackPlayer(Collider other)
    {
        if (!overseer.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            BeamAttack();
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= overseer.attackRange)
        {
            player.TakeDamage(Unit.Damage);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) > overseer.attackRange)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
            else
                Debug.Log("passive already active");
        }
    }
    public void AttackReset(Collider other)
    {
        if (damageSources.Contains(other))
        {
            damageSources.Remove(other);
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (canDie)
        {
            deathLogic.KilledWhenDeathDefiance();
            spawner.mindOnField--;
        }
    }

    private void BeamAttack()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
        LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();
        Vector3 dir = (newPos - rangeAttackSpawn.position).normalized;
        bool collidePlayer = Physics.Raycast(rangeAttackSpawn.position, dir, out RaycastHit hit, maxBeamDist, playerLayer);
        laserBeam.SetActive(true);
        lineRenderer.SetPosition(0, rangeAttackSpawn.position);
        Vector3 targetEndPoint = collidePlayer ? hit.point : rangeAttackSpawn.position + dir * maxBeamDist;
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
