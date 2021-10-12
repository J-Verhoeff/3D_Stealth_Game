using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GuardMovement : MonoBehaviour {
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float closeEnough = 1f;
    [SerializeField] private bool repeat = true;

    private NavMeshAgent agent = null;
    private Animator animator = null;
    private GameManager manager = null;
    private int currentWaypoint = 0;
    private bool patroling = true;
    private bool alert = false;


    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start() {
        if((agent != null) && (waypoints.Length > 0)) {
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    IEnumerator HitAlarm(float delay) {
        animator.SetTrigger("EnterKeyPad");
        yield return new WaitForSeconds(delay);
        manager.turnOnAlarm();
        yield return new WaitForSeconds(2f);
        manager.gameOver();
    }

    private void Update() {
        float distanceToTarget;
        if (alert) {
            distanceToTarget = Vector3.Distance(agent.transform.position, agent.destination);
            if(distanceToTarget < closeEnough) {
                // Debug.Log("At Alarm");
                StartCoroutine("HitAlarm", 7f);
            }
        }

        if(!patroling) {
            return;
        }

        distanceToTarget = Vector3.Distance(agent.transform.position, waypoints[currentWaypoint].position);

        if(distanceToTarget < closeEnough) {
            currentWaypoint++;

            if(currentWaypoint >= waypoints.Length) {
                if(repeat) {
                    currentWaypoint = 0;
                } else {
                    patroling = false;
                    animator.SetFloat("Speed", 0);
                    return;
                }
            }

            agent.SetDestination(waypoints[currentWaypoint].position);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            alert = true;
            patroling = false;
            agent.SetDestination(manager.getClosestAlarm(transform).position);
            agent.speed = 5f;
        }
    }
}
