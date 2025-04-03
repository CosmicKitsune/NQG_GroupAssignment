using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance; 

    public Vector2 DragPosition {get; private set; }
    public bool AttackInput {get; private set; }
    public bool DragStart {get; private set; }
    public bool DragHolding {get; private set; }
    public bool DragReleased {get; private set; }

    public enum InputType {MouseKeyboard, Gamepad} //to detect which kind of input is being detected
    public InputType CurrentInputType {get; private set; } = InputType.MouseKeyboard; 
    
    private PlayerInput _playerInput;
    private InputAction _attackAction;
    private InputAction _dragAction;
    private InputAction _dragStartAction;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        if(instance == null)
        { 
            instance = this;
        }
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions["Attack"];
        _dragStartAction = _playerInput.actions["StartDrag"]; // Left click or trigger
        _dragAction = _playerInput.actions["Drag"]; //left stick movement
    }

    private void Update()
    {
        AttackInput = _attackAction.WasPressedThisFrame();
        DragStart = _dragStartAction.WasPressedThisFrame();
        DragHolding = _dragStartAction.IsInProgress();
        DragReleased = _dragStartAction.WasReleasedThisFrame();
        if (DragReleased)
        {
            Debug.Log("Left click released!");
        }
    }
    
    private void DetectInputMethod(InputAction.CallbackContext context)
    {
        if (context.control.device is Mouse || context.control.device is Keyboard)
        {
            CurrentInputType = InputType.MouseKeyboard;
        } else if (context.control.device is Gamepad)
        {
            CurrentInputType = InputType.Gamepad;
        }
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        DragPosition = context.ReadValue<Vector2>();
        DetectInputMethod(context);
    }

    //public void OnDragRelease(InputAction.CallbackContext context)
    //{
    //    if (!context.started) return;
        //DragStart = false;
    //    DetectInputMethod(context);
    //}

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue())); //to check if we are hitting something
        if (!rayHit.collider) return;
        
        DetectInputMethod(context);
        Debug.Log(rayHit.collider.name);
    }

    public bool IsUsingGamepad()
    {
        return CurrentInputType == InputType.Gamepad;
    }

    public bool IsUsingMouseKeyboard()
    {
        return CurrentInputType == InputType.MouseKeyboard;
    }
}
