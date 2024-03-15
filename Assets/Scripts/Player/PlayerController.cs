using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour, IPlayerActions
{
    private PlayerInput _playerInput;
    private Vector2 _moveInput = new();
    private Vector2 _cursorLocation;

    private Transform _shipTransform;
    private Rigidbody2D _rb;

    private Transform turretPivotTransform;

    public UnityAction<bool> onFireEvent;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float shipRotationSpeed = 100f;
    [SerializeField] private float turretRotationSpeed = 4f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        if (_playerInput == null)
        {
            _playerInput = new();
            _playerInput.Player.SetCallbacks(this);
        }
        _playerInput.Player.Enable();

        _rb = GetComponent<Rigidbody2D>();
        _shipTransform = transform;
        turretPivotTransform = transform.Find("PivotTurret");
        if (turretPivotTransform == null) Debug.LogError("PivotTurret is not found", gameObject);
    }

    public void PlayerRespawn()
    {
        if(!IsOwner) return;
        _playerInput.Player.Enable();
        PlayerRespawnServerRpc();
    }

    [ServerRpc]
    private void PlayerRespawnServerRpc()
    {
        PlayerRespawnClientRpc();
    }

    [ClientRpc]
    private void PlayerRespawnClientRpc()
    {
        transform.position = new Vector2(Random.Range(-4, 4), Random.Range(-2, 2));
    }

    public void PlayerDied()
    {
        if(!IsOwner) return;
        _playerInput.Player.Disable();
        //PlayerDiedServerRpc();
    }

    [ServerRpc]
    
    private void PlayerDiedServerRpc()
    {
        PlayerDiedClientRpc();
    }

    [ClientRpc]
    private void PlayerDiedClientRpc()
    {
        if(!IsOwner) return;
        _playerInput.Player.Disable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFireEvent?.Invoke(true);
            // We Are Firing
        }
        else if (context.canceled)
        {
            onFireEvent?.Invoke(false);
            // We Stoped Firing
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        _rb.velocity = _moveInput.y * movementSpeed * transform.up * Time.fixedDeltaTime;
        _rb.MoveRotation(_rb.rotation + _moveInput.x * -shipRotationSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(_cursorLocation);
        Vector2 targetDirection = new Vector2(screenToWorldPosition.x - turretPivotTransform.position.x, screenToWorldPosition.y - turretPivotTransform.position.y).normalized;
        Vector2 currentDirection = Vector2.Lerp(turretPivotTransform.up, targetDirection, Time.deltaTime * turretRotationSpeed);
        turretPivotTransform.up = currentDirection;
    }

    public void OnAim(InputAction.CallbackContext context) => _cursorLocation = context.ReadValue<Vector2>();
}