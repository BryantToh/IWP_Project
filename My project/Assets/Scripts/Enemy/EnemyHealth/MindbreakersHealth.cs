using System.Collections.Generic;
using UnityEngine;

public class MindbreakersHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    MindBreakersEnemy mindBreaker;
    GlitchController glitchCon;
    [Header("Projectile")]
    public GameObject mindProjectile, spawnPoint;
    float projectileSpeed = 5f;
    [Header("Glitch variables")]
    int glitchStack = 0;
    public float glitchResetTime = 0;
    bool resetGlitch = false;
    public void OnEnemySpawn()
    {
        glitchCon = GameObject.Find("GameController").GetComponent<GlitchController>(); 
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        mindBreaker = GetComponent<MindBreakersEnemy>();
    }

    public void OnGet()
    {
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }

    public void OnDestroyInterface()
    {
        Destroy(gameObject);
    }

    public void SetPosNRot(Vector3 Pos, Quaternion Rot)
    {
        transform.position = Pos;
        transform.rotation = Rot;
    }

    public void AttackPlayer(Collider other)
    {
        if (!mindBreaker.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            //damageSources.Add(other);
            ShootMindProj();
            //player.TakeDamage(Unit.Damage);
            //GlitchEffect();
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
            spawner.mindOnField--;
            ObjectPooler.Instance.Release("breaker", this);
        }
    }

    private void GlitchEffect()
    {
        if (glitchStack < 5)
        {
            glitchCon.noiseAmount = Mathf.Lerp(glitchCon.noiseAmount, 10 * glitchStack, Time.deltaTime * 2);
            //glitchCon.noiseAmount += 10f;
            //glitchCon.glitchStrength += 4f;
            //glitchCon.scanLinesStrength -= 0.18f;
            glitchStack++;
        }
    }
    private void ResetGlitch()
    {
        glitchCon.noiseAmount = Mathf.Lerp(glitchCon.noiseAmount, 0, Time.deltaTime);
        glitchCon.glitchStrength = Mathf.Lerp(glitchCon.glitchStrength, 0, Time.deltaTime);
        glitchCon.scanLinesStrength = Mathf.Lerp(glitchCon.scanLinesStrength, 0, Time.deltaTime);
    }

    private void ShootMindProj()
    {
        GameObject obj = Instantiate(mindProjectile, spawnPoint.transform.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (player.transform.position - spawnPoint.transform.position).normalized;
            obj.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
            rb.linearVelocity = dir * projectileSpeed;
        }
    }
}
