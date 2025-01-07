using UnityEngine;
using UnityEngine.AI;
public class Pullability : MonoBehaviour
{
    public LayerMask pullableLayer;
    public float stopDistance;
    public float pullStrength;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("asdasd");
        }
        //if (other.gameObject.layer == pullableLayer)
        //{
        //    NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        //    if (agent != null)
        //    {
        //        Vector3 direction = transform.position - other.transform.position;

        //        if (direction.magnitude > stopDistance)
        //        {
        //            Vector3 newPosition = Vector3.MoveTowards(
        //                other.transform.position,
        //                transform.position,
        //                pullStrength * Time.deltaTime
        //            );
        //            agent.Warp(newPosition);
        //        }
        //        else
        //        {
        //            agent.isStopped = true;
        //        }
        //    }
        //}
    }
}
