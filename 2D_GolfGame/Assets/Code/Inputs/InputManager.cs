using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance; 

    public bool AttackInput {get; private set; }
    private PlayerInput _playerInput;
    private InputAction _attackAction;

    private void Awake()
    {
        if(instance == null)
        { 
            instance = this;
        }
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions["Attack"];
    }

    private void Update()
    {
        AttackInput = _attackAction.WasPressedThisFrame();
    }
}
