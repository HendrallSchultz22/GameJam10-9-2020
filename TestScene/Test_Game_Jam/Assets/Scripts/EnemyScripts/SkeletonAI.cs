using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SkeletonAI : MonoBehaviour
{
    [Tooltip("In most instances this should be set to the player")]
    public Transform target;
    [Tooltip("Adjust this to directly impact how much damage is done by the AI, The exact number will vary")]
    public float baseDamage = 1;
    [Tooltip("How focus will the AI be on attack the player")]
    public float agresstion = 5;

    NavMeshAgent agent;
    [SerializeField]
    [Tooltip("How far the enemy will stand from its target to make attacks")]
    float range = 10;
    [SerializeField]
    [Tooltip("How close The skeleton has to be to start using melee attacks. Note if range is less then this the skeleton will always melee")]
    float meleeRange = 2;
    [SerializeField]
    [Tooltip("Once the skeleton makes an attack this is how long until it can attack again")]
    float attackLockoutTimer = 1;
    [SerializeField] GameObject bone;
    [SerializeField] Transform hand;
    [SerializeField]
    [Tooltip("This is how long it will take for the projectile to travel to the player at max range, lower this value for higher difficulty")]
    float boneTravelTime = 0.5f;
    [SerializeField] LayerMask sightBlockers;
    [SerializeField] float MaxPointOffset = 5;
    [SerializeField]
    [Tooltip("max offset is multiplied by this value for movement")]
    float positionOffserScaler = 5;



    bool inRange;
    bool canAttack = true;
    IEnumerator AttackLock;
    IEnumerator movementOffset;
    Vector3 positionOffset = Vector3.zero;
    float navSeachRadius = 10;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null) Debug.Log("AI: " + gameObject.name + " does not have a target.");
        if (agresstion > EnemyManager.MaxAgression) agresstion = EnemyManager.MaxAgression;
        navSeachRadius = MaxPointOffset * positionOffserScaler * positionOffserScaler;
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
            if (agent.remainingDistance < range || DistanceToPlayer() <= range)
            {
                positionOffset = GetOffset();
            }
            SetDestination(target.position + (positionOffset * positionOffserScaler));
            inRange = DistanceToPlayer() <= range;
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

    private void SetDestination(Vector3 point)
    {
        NavMeshHit nHit;
        NavMesh.SamplePosition(point, out nHit, navSeachRadius, -1);

        NavMeshPath navpath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, nHit.position, -1, navpath);
        if (navpath.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(nHit.position);
        }
    }

    private float DistanceToPlayer()
    {
        Vector3 destination = agent.destination;
        float pathLength = 0;
        NavMeshHit nHit;
        NavMesh.SamplePosition(target.position, out nHit, navSeachRadius, -1);

        NavMeshPath navpath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, nHit.position, -1, navpath);
        if (navpath.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(nHit.position);
            pathLength = agent.remainingDistance;
        }
        agent.SetDestination(destination);
        return pathLength;
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
    public void ThrowBone()
    {
        GameObject newBone = Instantiate(bone, hand.position, hand.rotation);
        Rigidbody rb = newBone.GetComponent<Rigidbody>();

        Vector3 boneTarget = target.position;
        boneTarget.y += hand.localPosition.y;
        boneTarget += GetOffset();
        Vector3 direction = (boneTarget - hand.position);
        float distance = direction.magnitude;
        direction.Normalize();

        float inversAgresstion = (EnemyManager.MaxAgression - agresstion);
        float boneTravelFinal = boneTravelTime * Mathf.Lerp(1, 2, Random.Range(0, inversAgresstion) / EnemyManager.MaxAgression);

        float time = Mathf.Lerp(0, boneTravelFinal, (distance / range));
        time = time * time;
        float verticalVelocity = -Physics.gravity.y * time;
        float height = -Physics.gravity.y * time * time + verticalVelocity * time;
        float horizontalVelocity = (verticalVelocity * distance) / height;

        Vector3 velocity = direction * horizontalVelocity;
        velocity += Vector3.up * verticalVelocity;
        rb.velocity = velocity;
        SpinBone(rb);

        float damage = CalculateDamage();
        newBone.GetComponent<Bone>().damage = damage;
    }

    //Add some spin to the bone
    private void SpinBone(Rigidbody rb)
    {
        float range = 10;
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        rb.angularVelocity = new Vector3(x, y, z);
    }

    private float CalculateDamage()
    {
        return baseDamage * Random.Range(1, agresstion);
    }

    //high agression level will be more likly to return 0
    private Vector3 GetOffset()
    {
        float inversAgresstion = (EnemyManager.MaxAgression - agresstion);
        float x = Mathf.Lerp(0, MaxPointOffset, Random.Range(0, inversAgresstion) / EnemyManager.MaxAgression);
        float y = Mathf.Lerp(0, MaxPointOffset, Random.Range(0, inversAgresstion) / EnemyManager.MaxAgression);
        float z = Mathf.Lerp(0, MaxPointOffset, Random.Range(0, inversAgresstion) / EnemyManager.MaxAgression);
        if (Random.value <= 0.5f) x = -x;
        if (Random.value <= 0.5f) y = -y;
        if (Random.value <= 0.5f) z = -z;
        return new Vector3(x, y, z);
    }

    public void Melee()
    {
        Collider[] cols = Physics.OverlapSphere(hand.position, meleeRange);
        foreach (Collider c in cols)
        {
            if (c.CompareTag("Player"))
            {
                c.SendMessage("DoDamage", CalculateDamage(), SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    IEnumerator AttackLockout(float time)
    {
        canAttack = false;
        float inversAgresstion = (EnemyManager.MaxAgression - agresstion);
        float scaler = Mathf.Lerp(1, 2, Random.Range(0, inversAgresstion) / EnemyManager.MaxAgression);
        yield return new WaitForSeconds(time);
        canAttack = true;
    }
}
