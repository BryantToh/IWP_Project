using UnityEngine;

public interface IPooledEnemy
{
    void OnEnemySpawn();
    void OnGet();
    void OnRelease();
    void OnDestroyInterface();
    void SetPosNRot(Vector3 Pos, Quaternion Rot);
}
