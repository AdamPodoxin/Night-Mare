using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GlobalEnums;

public class DemonEnemy : MonoBehaviour
{
    public static DemonEnemy instance;

    public DemonState state;

    [Space]

    public float acceptableStoppingDistance = 1.2f;
    public LayerMask ignoreLayers;

    [Space]

    public float searchTime = 3f;
    public float waypointRadius = 10f;

    [Space]

    public AudioClip[] foundVoiceLines;
    public AudioClip[] searchVoiceLines;
    public AudioClip[] despawnVoiceLines;

    private NavMeshAgent agent;
    private AudioSource audioSource;
    private Rigidbody rb;
    private Animator anim;

    private Transform playerTransform;
    private float playerMoveSpeed;

    private Vector3 lastKnownPosition;
    private Vector3 lastKnownDirection;

    private Vector3 playerFoundPosition;
    private Vector3 playerLostPosition;
    private Vector3 waypointPosAfterPlayer;

    private float timer = 0f;
    private bool isTiming = true;

    private Transform[] nearbyWaypoints;
    private int waypointIndex = 0;

    private RaycastHit _hit;
    private bool _isChasingPlayer;
    private Vector3 _navTargetPosition;
    private Vector3 _predictedPosition;

    private int _prevFoundIndex = -1;
    private int _prevSearchIndex = -1;
    private int _prevDespawnIndex = -1;

    private delegate void OnComeplete();

    private void Awake()
    {
        instance = this;

        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMoveSpeed = FindObjectOfType<StarterAssets.FirstPersonController>().MoveSpeed;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isTiming) timer += Time.deltaTime;

        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out _hit, Mathf.Infinity, ~ignoreLayers))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                if (!state.Equals(DemonState.Chasing)) OnFoundPlayer();

                lastKnownPosition = playerTransform.position;
                agent.SetDestination(lastKnownPosition);
            }
            else
            {
                if (state.Equals(DemonState.Chasing)) OnLostPlayer();
            }
        }
        else
        {
            if (state.Equals(DemonState.Chasing)) OnLostPlayer();
        }

        if (state.Equals(DemonState.Travelling))
        {
            if (DistanceCheck() && !state.Equals(DemonState.Searching))
            {
                if (_isChasingPlayer)
                {
                    OnReachedLastKnownPosition();
                }
                else
                {
                    OnReachedWaypoint();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Door door = collision.gameObject.GetComponent<Door>();
        if (door != null)
        {
            if (!door.isOpen)
            {
                StartCoroutine(DoorCollisionCoroutine(door));
            }
        }
    }

    private IEnumerator DoorCollisionCoroutine(Door door)
    {
        agent.isStopped = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.AddForce(transform.forward * -130f);

        door.Open();

        yield return new WaitForSeconds(0.75f);

        rb.constraints = RigidbodyConstraints.FreezeAll;
        agent.isStopped = false;
    }

    private bool DistanceCheck()
    {
        Vector2 posV2 = new Vector2(transform.position.x, transform.position.z);
        Vector2 navV2 = new Vector2(_navTargetPosition.x, _navTargetPosition.z);

        return Vector2.SqrMagnitude(navV2 - posV2) <= acceptableStoppingDistance;
    }

    private void StartTimer()
    {
        timer = 0f;
        isTiming = true;
    }

    private void StopTimer()
    {
        isTiming = false;
    }

    private Vector3 CalculatePlayerDirection(Vector3 position1, Vector3 position2)
    {
        return new Vector3(position2.x - position1.x, 0f, position2.z - position1.z).normalized;
    }

    private void OnFoundPlayer()
    {
        state = DemonState.Chasing;
        StopTimer();

        playerFoundPosition = playerTransform.position;

        if (!audioSource.isPlaying && !_isChasingPlayer)
        {
            int foundIndex = Random.Range(0, foundVoiceLines.Length);
            while (foundIndex == _prevFoundIndex) { foundIndex = Random.Range(0, foundVoiceLines.Length); }
            _prevFoundIndex = foundIndex;

            AudioClip foundClip = foundVoiceLines[foundIndex];
            audioSource.PlayOneShot(foundClip);
        }

        _isChasingPlayer = true;
        anim.SetBool("isMoving", true);
    }

    private void OnLostPlayer()
    {
        state = DemonState.Travelling;
        StartTimer();

        playerLostPosition = _navTargetPosition = playerTransform.position;
        lastKnownDirection = CalculatePlayerDirection(playerFoundPosition, playerLostPosition);
    }

    private void OnReachedLastKnownPosition()
    {
        state = DemonState.Searching;
        _isChasingPlayer = false;
        StopTimer();

        _predictedPosition = PredictPosition();
        waypointPosAfterPlayer = NightmareManager.instance.FindNearestWaypoint(_predictedPosition).position;

        OnComeplete onComplete = () =>
        {
            state = DemonState.Travelling;
            _navTargetPosition = waypointPosAfterPlayer;
            agent.SetDestination(_navTargetPosition);

            nearbyWaypoints = NightmareManager.instance.FindWaypointsInRadius(_navTargetPosition, waypointRadius);
            waypointIndex = 1;
        };

        StartCoroutine(SearchCoroutine(onComplete));
    }

    private void OnReachedWaypoint()
    {
        state = DemonState.Searching;

        OnComeplete onComplete = () =>
        {
            if (waypointIndex < nearbyWaypoints.Length)
            {
                Vector3 waypointPos = nearbyWaypoints[waypointIndex].position;

                if (waypointPos == waypointPosAfterPlayer)
                {
                    try
                    {
                        waypointIndex++;
                        waypointPos = nearbyWaypoints[waypointIndex].position;
                    }
                    catch
                    {
                        Despawn();
                    }
                }

                state = DemonState.Travelling;
                _navTargetPosition = waypointPos;
                agent.SetDestination(_navTargetPosition);

                waypointIndex++;
            }
            else
            {
                Despawn();
            }
        };

        StartCoroutine(SearchCoroutine(onComplete));
    }

    private void Despawn()
    {
        StartCoroutine(DespawnCoroutine());
    }

    private IEnumerator SearchCoroutine(OnComeplete onComeplete)
    {
        if (!audioSource.isPlaying)
        {
            int searchIndex = Random.Range(0, searchVoiceLines.Length);
            while (searchIndex == _prevSearchIndex) { searchIndex = Random.Range(0, searchVoiceLines.Length); }
            _prevSearchIndex = searchIndex;

            AudioClip searchClip = searchVoiceLines[searchIndex];
            audioSource.PlayOneShot(searchClip);
        }

        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(searchTime);
        if (!_isChasingPlayer) onComeplete.Invoke();
    }

    private IEnumerator DespawnCoroutine()
    {
        if (!audioSource.isPlaying)
        {
            int despawnIndex = Random.Range(0, despawnVoiceLines.Length);
            while (despawnIndex == _prevDespawnIndex) { despawnIndex = Random.Range(0, despawnVoiceLines.Length); }
            _prevDespawnIndex = despawnIndex;

            AudioClip despawnClip = despawnVoiceLines[despawnIndex];
            audioSource.PlayOneShot(despawnClip);
        }

        yield return new WaitForSeconds(searchTime);
        if (!_isChasingPlayer) gameObject.SetActive(false);
    }

    private Vector3 PredictPosition()
    {
        float predictedDistance = playerMoveSpeed * Mathf.Log(timer, 2);
        return lastKnownPosition + lastKnownDirection * predictedDistance; ;
    }

    public void GotPlayerPosition(Vector3 position, Vector3 direction, bool useVoiceline)
    {
        lastKnownPosition = _navTargetPosition = position;
        lastKnownDirection = direction;

        agent.SetDestination(lastKnownPosition);
        state = DemonState.Travelling;
        _isChasingPlayer = true;
        anim.SetBool("isMoving", true);

        StartTimer();

        JumpscareEnemy.instance.ActivateJumpscare();

        if (useVoiceline)
        {
            AudioClip foundClip = foundVoiceLines[Random.Range(0, foundVoiceLines.Length)];
            audioSource.PlayOneShot(foundClip);
        }
    }
}
