using Cinemachine;
using FishNet.Object;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{

    NavMeshAgent navMeshAgent;

    private CinemachineVirtualCamera _camera;

    [SerializeField]
    private InputActionAsset inputActions;

    private InputActionMap playerActionMap;

    private InputAction movement;

    private Animator _animator;

    [SerializeField]
    [Range(0, 0.99f)]
    private float smoothing = 0.25f;

    private Vector3 targetDirection;

    private float lerpTime = 0;

    public float currentMovementSpeed = 0;

    private Vector3 lastDirection;

    private Vector3 movementVector;

    private float targetLerpSpeed = 1;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerActionMap = inputActions.FindActionMap("Player");
        movement = playerActionMap.FindAction("Move");
        movement.started += HandleMovementAction;
        movement.canceled += HandleMovementAction;
        movement.performed += HandleMovementAction;
        movement.Enable();
        _animator = GetComponent<Animator>();
        playerActionMap.Enable();
        inputActions.Enable();
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            _camera = GameObject.Find("Follow Camera").GetComponent<CinemachineVirtualCamera>();
            _camera.Follow = transform;
        }

    }

    private void HandleMovementAction(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;
        Vector2 input = context.ReadValue<Vector2>();
        movementVector = new Vector3(input.x, 0, input.y);
    }

    private void UpdateMovementSpeed()
    {
        // if movementVector is 0 then set currentMovementSpeed to 0, else increment currentMovementSpeed by 1 every second
        if (movementVector == Vector3.zero)
        {
            currentMovementSpeed = 0;
        }
        else
        {
            // max value should be navmeshagent.speed
            currentMovementSpeed = navMeshAgent.speed;

        }
    }

    private void Update()
    {
        if (!base.IsOwner) return;
        movementVector.Normalize();
        UpdateMovementSpeed();
        if (movementVector != lastDirection)
        {
            lerpTime = 0;
        }
        lastDirection = movementVector;
        targetDirection =
            Vector3
                .Lerp(targetDirection,
                movementVector,
                Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));

        navMeshAgent
            .Move(targetDirection * currentMovementSpeed * Time.deltaTime);

        Vector3 lookDirection = movementVector;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation =
                Quaternion
                    .Lerp(transform.rotation,
                    Quaternion.LookRotation(lookDirection),
                    Mathf
                        .Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
        }

        lerpTime += Time.deltaTime;
        // Update animator
        _animator.SetFloat("ForwardMotion", currentMovementSpeed);
    }
}
