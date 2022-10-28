using FishNet.Object;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{

    NavMeshAgent navMeshAgent;

    [SerializeField]
    private InputActionAsset inputActions;

    private InputActionMap playerActionMap;

    private InputAction movement;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    [Range(0, 0.99f)]
    private float smoothing = 0.25f;

    private Vector3 targetDirection;

    private float lerpTime = 0;

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
        playerActionMap.Enable();
        inputActions.Enable();
    }

    private void HandleMovementAction(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;
        Vector2 input = context.ReadValue<Vector2>();
        movementVector = new Vector3(input.x, 0, input.y);
    }

    private void Update()
    {
        if (!base.IsOwner) return;
        movementVector.Normalize();
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
            .Move(targetDirection * navMeshAgent.speed * Time.deltaTime);

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
    }
}
