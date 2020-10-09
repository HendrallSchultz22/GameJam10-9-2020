using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SkeletonAI : MonoBehaviour
{
    [Tooltip("In most instances this should be set to the player")]
    public Transform target;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null) Debug.Log("AI: " + gameObject.name + " does not have a target.");
    }

    private void Update()
    {
        if (target != null) agent.SetDestination(target.position);
    }
}
