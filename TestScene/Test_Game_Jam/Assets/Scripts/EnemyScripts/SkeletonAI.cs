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
    [SerializeField]
    [Tooltip("How far the enemy will stand from its target to make attacks")]
    float range = 1;
    [SerializeField] float speed = 3.5f;
    [SerializeField]
    [Tooltip("Once the skeleton makes an attack this is how long until it can attack again")]
    float attackLockoutTimer = 1;
    [SerializeField] GameObject bone;
    [SerializeField] Transform boneSpawnPoint;
    [SerializeField]
    [Tooltip("This is how long it will take for the projectile to travel to the player at max range, lower this value for higher difficulty")]
    float boneTravelTime = 0.5f;
    [SerializeField] LayerMask sightBlockers;

    bool inRange;
    bool canAttack = true;
    IEnumerator AttackLock;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null) Debug.Log("AI: " + gameObject.name + " does not have a target.");
    }

    private void Update()
    {
        if (target != null)
        {
            if (inRange) inRange = (transform.position - target.position).sqrMagnitude <= range * range;
            Attack();
            Movement();
        }
    }

    private void Movement()
    {
        if (!inRange)
        {
            agent.SetDestination(target.position);
            inRange = agent.remainingDistance <= range;
        }
        
        else
        {
            agent.ResetPath();
            Vector3 lookTarget = target.position;
            lookTarget.y = transform.position.y;
            Vector3 direction = (lookTarget - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
        }
    }

    //logic for when and how the skeleton attacks
    private void Attack()
    {
        if (inRange)
        {
            if (canAttack)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out hit, distance, sightBlockers))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.transform != target)
                    {
                        inRange = false;
                        return;
                    }
                }
                
                ThrowBone();
                AttackLock = AttackLockout(attackLockoutTimer);
                StartCoroutine(AttackLock);
            }
        }
    }

    //spawn a new bone and set its velocity
    private void ThrowBone()
    {
        GameObject newBone = Instantiate(bone, boneSpawnPoint.position, boneSpawnPoint.rotation);
        Rigidbody rb = newBone.GetComponent<Rigidbody>();

        Vector3 boneTarget = target.position;
        boneTarget.y += boneSpawnPoint.localPosition.y;
        Vector3 direction = (boneTarget - boneSpawnPoint.position);
        float distance = direction.magnitude;
        direction.Normalize();

        float time = Mathf.Lerp(0, boneTravelTime, (distance / range));
        time = time * time;
        float verticalVelocity = -Physics.gravity.y * time;
        float height = -Physics.gravity.y * time * time + verticalVelocity * time;
        float horizontalVelocity = (verticalVelocity * distance) / height;

        Vector3 velocity = direction * horizontalVelocity;
        velocity += Vector3.up * verticalVelocity;
        rb.velocity = velocity;
    }

    IEnumerator AttackLockout(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }
}
