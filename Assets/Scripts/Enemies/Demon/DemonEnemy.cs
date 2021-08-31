using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using static GlobalEnums;

public class DemonEnemy : MonoBehaviour
{
    public static DemonEnemy instance;

    [SerializeField] private DemonState state;
    public DemonState State
    {
        get { return state; }
        set
        {
            state = value;
            _isSearching = value.Equals(DemonState.Searching);

            IsMovingCheck();
        }
    }

    [Space]

    [HideInInspector] public bool isSummoning = false;
    public float acceptableStoppingDistance = 1.2f;
    public LayerMask ignoreLayers;

    [Space]

    public Transform eyeHeightTransform;
    public Transform handTransform;

    [Space]

    public float searchTime = 3f;
    public float waypointSearchRadius = 10f;
    public float doorOpenTime = 0.75f;

    [Space]

    public AudioSource voiceSource;
    public AudioSource sfxSource;
    public AudioSource droneSource;
    public AudioSource artifactSource;

    public AudioClip[] foundVoiceLines;
    public AudioClip[] searchVoiceLines;
    public AudioClip[] despawnVoiceLines;

    [Space]

    public string[] foundSubtitles;
    public string[] searchSubtitles;
    public string[] despawnSubtitles;

    [Space]

    public AudioClip[] footsteps;

    public AudioClip killClip;
    public AudioClip artifactClip;

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

    private GameSubtitles gameSubtitles;

    private RaycastHit _hit;
    private bool _isChasingPlayer;
    private Vector3 _navTargetPosition;
    private Vector3 _predictedPosition;

    private Vector3 _lookAtPosition;

    private int _prevFoundIndex = -1;
    private int _prevSearchIndex = -1;
    private int _prevDespawnIndex = -1;

    private bool _isSearching = false;
    private bool _isKilling = false;

    private delegate void OnComeplete();

    private void Awake()
    {
        instance = this;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        fps = FindObjectOfType<FirstPersonController>();
        playerCamera = FindObjectOfType<PlayerCamera>();

        gameSubtitles = FindObjectOfType<GameSubtitles>();

        playerMoveSpeed = fps.MoveSpeed;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isKilling) return;

        if (isTiming) timer += Time.deltaTime;

        _lookAtPosition = fps.isCrouching ? fps.transform.position : playerCamera.transform.position;
        if (Physics.Raycast(eyeHeightTransform.position, (_lookAtPosition - eyeHeightTransform.position).normalized, out _hit, Mathf.Infinity, ~ignoreLayers))
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
        Door door = collision.gameObject.GetComponent<Door>();
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
            if (!_isKilling)
            {
                State = DemonState.Killing;

                _isKilling = true;
                agent.isStopped = true;

                anim.Play("Killing");

                voiceSource.Stop();
                sfxSource.PlayOneShot(killClip);

                FindObjectOfType<PlayerBrain>().Die();
            }
        }
    }

    private void IsMovingCheck()
    {
        anim.SetBool("isMoving", (state.Equals(DemonState.Travelling) || state.Equals(DemonState.Chasing)) && !_isKilling);
    }

    private IEnumerator DoorCollisionCoroutine(Door door)
    {
        if (_isKilling) yield return null;

        agent.isStopped = true;
        anim.SetBool("isMoving", false);

        door.Open();

        yield return new WaitForSeconds(doorOpenTime);

        agent.isStopped = false;
        IsMovingCheck();
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
        if (_isKilling) return;

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

            if (gameSubtitles != null) gameSubtitles.ShowSubtitles(foundSubtitles[foundIndex]);
        }

        _isChasingPlayer = true;
    }

    private void OnLostPlayer()
    {
        if (_isKilling) return;

        State = DemonState.Travelling;
        StartTimer();

        playerLostPosition = _navTargetPosition = playerTransform.position;
        lastKnownDirection = CalculatePlayerDirection(playerFoundPosition, playerLostPosition);
    }

    private void OnReachedLastKnownPosition()
    {
        if (_isKilling) return;

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
        if (_isKilling) return;

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
        if (_isKilling) return;

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

            if (gameSubtitles != null) gameSubtitles.ShowSubtitles(searchSubtitles[searchIndex]);
        }

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

            if (gameSubtitles != null) gameSubtitles.ShowSubtitles(despawnSubtitles[despawnIndex]);
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

    public void EyeballGotPosition(Vector3 position, Vector3 direction)
    {
        if (_isKilling) return;

        lastKnownPosition = _navTargetPosition = position;
        lastKnownDirection = direction;
    }

    public void EyeballSummon(bool useVoiceline, bool isCarryingArtifact = false)
    {
        if (_isKilling) return;

        if (isCarryingArtifact && !_isChasingPlayer)
        {
            artifactSource.PlayOneShot(artifactClip);

            if (gameSubtitles != null) gameSubtitles.ShowSubtitles("MY ARTIFACT!");
        }
        else if (useVoiceline)
        {
            int foundIndex = Random.Range(0, foundVoiceLines.Length);
            AudioClip foundClip = foundVoiceLines[foundIndex];
            voiceSource.PlayOneShot(foundClip);

            if (gameSubtitles != null) gameSubtitles.ShowSubtitles(foundSubtitles[foundIndex]);
        }

        agent.SetDestination(lastKnownPosition);
        State = DemonState.Travelling;
        _isChasingPlayer = true;

        StartTimer();

        fps.ToggleChase(true);
        playerCamera.ToggleChase(true);
    }

    public void PlayFootstep()
    {
        AudioClip footstep = footsteps[Random.Range(0, footsteps.Length)];
        sfxSource.PlayOneShot(footstep);
    }

    public void SwitchWorlds()
    {
        if (_isKilling) return;

        State = DemonState.Despawning;

        fps.ToggleChase(false);
        playerCamera.ToggleChase(false);

        gameObject.SetActive(false);
    }
}
