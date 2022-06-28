using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler StartHitting;
    public event EventHandler SpeedUp;
    [SerializeField] private Transform moveTarget;
    [SerializeField] private float delayToStart = 1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem destroyableParticle;
    private float timeToStartHitting = 20f;
    private float timeToSpeedUp = 60f;
    private bool canHit = false;
    private Collider obstacle;
    private float destroyDelay;
    private float _startDestoryDelay = 3f;
    private float catchingTime = 0.5f;
    private LayerMask destroyableObjects = ~(1 << 11 | 1 << 10 | 1 << 9 | 1 << 8);


    private void Start()
    {
        //agent.SetDestination(moveTarget.position);
        moveTarget.GetComponent<PlayerController>().PlayerDead += PlayerKilled;
        StartHitting += () => canHit = true; // BeAgressive;
        destroyDelay = _startDestoryDelay;
        SpeedUp += () => agent.speed = 4f;
    }

    private void Update()
    {
        if (delayToStart > 0)
        {
            delayToStart -= Time.deltaTime;
            return;
        }
        if (moveTarget != null)
        {
            if (Vector3.Distance(transform.position, moveTarget.position) > 1f)
            {
                animator.SetBool("Moving", true);
                agent.SetDestination(moveTarget.position);
                catchingTime = 0.5f;
            }
            else
            {
                animator.SetBool("Moving", false);
                animator.SetBool("Attac", true);
                catchingTime -= Time.deltaTime;
                if (catchingTime <= 0)
                {
                    //Time.timeScale = 0;
                    moveTarget.GetComponent<PlayerController>().Die();
                }

            }
        }
        if (canHit)
        {
            var colls = Physics.OverlapSphere(transform.position, 1f, destroyableObjects);
            if (colls.Length > 0)
            {
                obstacle = colls[0];
                if (destroyDelay > 0)
                    destroyDelay -= Time.deltaTime;
                else if (obstacle == colls[0] && destroyDelay <= 0)
                {
                    animator.SetBool("Attac", true);
                    var particle = Instantiate(destroyableParticle, obstacle.transform.position, Quaternion.identity);
                    obstacle.gameObject.SetActive(false);
                    destroyDelay = _startDestoryDelay;
                }
                else
                    destroyDelay = _startDestoryDelay;
            }
        }
        else if (!canHit)
        {
            if (timeToStartHitting > 0)
                timeToStartHitting -= Time.deltaTime;
            else
                StartHitting?.Invoke();
        }
        if (timeToSpeedUp > 0)
            timeToSpeedUp -= Time.deltaTime;
        else
            SpeedUp?.Invoke();


    }

    private void PlayerKilled()
    {
        animator.SetBool("Moving", false);
        moveTarget = null;
        agent.enabled = false;
        transform.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezePositionY | transform.GetComponent<Rigidbody>().constraints;
        StartCoroutine(FlyAway());

    }

    // private void BeAgressive()
    // {
    //     canHit = true;
    // }

    private IEnumerator FlyAway()
    {
        while (transform.GetComponentInChildren<Renderer>().isVisible)
        {
            Debug.Log("Entered");
            transform.position = Vector3.MoveTowards(transform.position,
            transform.position + Vector3.up, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        gameObject.SetActive(false);
    }


}
