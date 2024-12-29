using System.Collections.Generic;
using UnityEngine;

public class MindbreakersHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    MindBreakersEnemy mindBreaker;
    GlitchController glitchCon;
    int glitchStack = 0;
    public float glitchResetTime = 0;
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
            damageSources.Add(other);
            player.TakeDamage(Unit.Damage);
            GlitchEffect();
            Invoke(nameof(ResetGlitch), glitchResetTime);
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
            ObjectPooler.Instance.Release("mindBreaker", this);
        }
    }

    private void GlitchEffect()
    {
        if (glitchStack < 5)
        {
            glitchCon.noiseAmount += 10f;
            glitchCon.glitchStrength += 4f;
            glitchCon.scanLinesStrength -= 0.18f;
            glitchStack++;
        }

    }
    private void ResetGlitch()
    {
        glitchCon.noiseAmount = 0f;
        glitchCon.glitchStrength = 0f;
        glitchCon.scanLinesStrength = 0f;
    }

    private void Update()
    {
        Debug.Log(glitchStack);
    }
}
