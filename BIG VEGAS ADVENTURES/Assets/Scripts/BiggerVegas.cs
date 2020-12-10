using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerVegas : MonoBehaviour
{
    public float lookRadius = 100f;

    Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        target = PlayerManager.Instance.player.transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            animator.SetFloat("forward", 1.0f);
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetFloat("forward", 0.0f);
        }

        if (distance <= agent.stoppingDistance)
        {
            animator.SetBool("svIsTrue", true);
            FaceTarget();
        }
        else
        {
            animator.SetBool("svIsTrue", false);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
