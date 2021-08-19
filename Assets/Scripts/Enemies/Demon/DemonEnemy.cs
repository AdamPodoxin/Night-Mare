using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using static GlobalEnums;

public class DemonEnemy : MonoBehaviour
{
    public static DemonEnemy instance;

    private DemonState state;
    public DemonState State
    {
        get { return state; }
        set
        {
            state = value;
            _isSearching = value.Equals(DemonState.Searching);
        }
    }

    [Space]

    public float acceptableStoppingDistance = 1.2f;
    public LayerMask ignoreLayers;

    public Transform eyeHeightTransform;

    [Space]

    public float searchTime = 3f;
    public float waypointSearchRadius = 10f;
    public float doorOpenTime = 0.75f;

    [Space]

    public AudioSource voiceSource;
    public AudioSource droneSource;
    public AudioSource sfxSource;

    public AudioClip[] foundVoiceLines;
    public AudioClip[] searchVoiceLines;
    public AudioClip[] despawnVoiceLines;

    public AudioClip[] footsteps;

    private NavMeshAgent agent;
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

    private FirstPersonController fps;
    private PlayerCamera playerCamera;

    private RaycastHit _hit;
    private bool _isChasingPlayer;
    private Vector3 _navTargetPosition;
    private Vector3 _predictedPosition;

    private int _prevFoundIndex = -1;
    private int _prevSearchIndex = -1;
    private int _prevDespawnIndex = -1;

    private bool _isSearching = false;

    private delegate void OnComeplete();

    private void Awake()
    {
        instance = this;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        fps = FindObjectOfType<FirstPersonController>();
        playerCamera = FindObjectOfType<PlayerCamera>();

        playerMoveSpeed = fps.MoveSpeed;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isTiming) timer += Time.deltaTime;

        if (Physics.Raycast(eyeHeightTransform.position, playerTransform.position - eyeHeightTransform.position, out _hit, Mathf.Infinity, ~ignoreLayers))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                if (!State.Equals(DemonState.Chasing)) OnFoundPlayer();

                lastKnownPosition = playerTransform.position;
                agent.SetDestination(lastKnownPosition);
            }
            else
            {
                if (State.Equals(DemonState.Chasing)) OnLostPlayer();
            }
        }
        else
        {
            if (State.Equals(DemonState.Chasing)) OnLostPlayer();
        }

        if (State.Equals(DemonState.Travelling))
        {
            if (DistanceCheck() && !State.Equals(DemonState.Searching))
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

        droneSource.volume += Time.deltaTime * (_isSearching ? 1f : -1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Door door = other.GetComponent<Door>();
        if (door != null)
        {
            if (!door.isOpen)
            {
                StartCoroutine(DoorCollisionCoroutine(door));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.isStopped = true;
            anim.Play("Killing");

            PlayerBrain.instance.Die();
        }
    }

    private IEnumerator DoorCollisionCoroutine(Door door)
    {
        agent.isStopped = true;
        anim.SetBool("isMoving", false);

        door.Open();

        yield return new WaitForSeconds(doorOpenTime);

        agent.isStopped = false;
        anim.SetBool("isMoving", true);
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
        State = DemonState.Chasing;
        StopTimer();

        playerFoundPosition = playerTransform.position;

        if (!voiceSource.isPlaying && !_isChasingPlayer)
        {
            int foundIndex = Random.Range(0, foundVoiceLines.Length);
            while (foundIndex == _prevFoundIndex) { foundIndex = Random.Range(0, foundVoiceLines.Length); }
            _prevFoundIndex = foundIndex;

            AudioClip foundClip = foundVoiceLines[foundIndex];
            voiceSource.PlayOneShot(foundClip);
        }

        _isChasingPlayer = true;
        anim.SetBool("isMoving", true);
    }

    private void OnLostPlayer()
    {
        State = DemonState.Travelling;
        StartTimer();

        playerLostPosition = _navTargetPosition = playerTransform.position;
        lastKnownDirection = CalculatePlayerDirection(playerFoundPosition, playerLostPosition);
    }

    private void OnReachedLastKnownPosition()
    {
        State = DemonState.Searching;
        _isChasingPlayer = false;
        StopTimer();

        _predictedPosition = PredictPosition();
        waypointPosAfterPlayer = NightmareManager.instance.FindNearestWaypoint(_predictedPosition).position;

        OnComeplete onComplete = () =>
        {
            State = DemonState.Travelling;
            _navTargetPosition = waypointPosAfterPlayer;
            agent.SetDestination(_navTargetPosition);

            nearbyWaypoints = NightmareManager.instance.FindWaypointsInRadius(_navTargetPosition, waypointSearchRadius);
            waypointIndex = 1;
        };

        StartCoroutine(SearchCoroutine(onComplete));
    }

    private void OnReachedWaypoint()
    {
        State = DemonState.Searching;

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

                State = DemonState.Travelling;
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
        if (!voiceSource.isPlaying)
        {
            int searchIndex = Random.Range(0, searchVoiceLines.Length);
            while (searchIndex == _prevSearchIndex) { searchIndex = Random.Range(0, searchVoiceLines.Length); }
            _prevSearchIndex = searchIndex;

            AudioClip searchClip = searchVoiceLines[searchIndex];
            voiceSource.PlayOneShot(searchClip);
        }

        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(searchTime);
        if (!_isChasingPlayer) onComeplete.Invoke();
    }

    private IEnumerator DespawnCoroutine()
    {
        State = DemonState.Despawning;

        if (!voiceSource.isPlaying)
        {
            int despawnIndex = Random.Range(0, despawnVoiceLines.Length);
            while (despawnIndex == _prevDespawnIndex) { despawnIndex = Random.Range(0, despawnVoiceLines.Length); }
            _prevDespawnIndex = despawnIndex;

            AudioClip despawnClip = despawnVoiceLines[despawnIndex];
            voiceSource.PlayOneShot(despawnClip);
        }

        yield return new WaitForSeconds(searchTime);

        if (!_isChasingPlayer)
        {
            fps.ToggleChase(false);
            playerCamera.ToggleChase(false);

            gameObject.SetActive(false);
        }
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
        State = DemonState.Travelling;
        _isChasingPlayer = true;
        anim.SetBool("isMoving", true);

        StartTimer();

        JumpscareEnemy.instance.ActivateJumpscare();

        fps.ToggleChase(true);
        playerCamera.ToggleChase(true);

        if (useVoiceline)
        {
            AudioClip foundClip = foundVoiceLines[Random.Range(0, foundVoiceLines.Length)];
            voiceSource.PlayOneShot(foundClip);
        }
    }

    public void PlayFootstep()
    {
        AudioClip footstep = footsteps[Random.Range(0, footsteps.Length)];
        sfxSource.PlayOneShot(footstep);
    }
}
