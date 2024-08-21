using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    private GameInput _gameInput;
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction FireEvent = delegate { }; 
    public event UnityAction<Vector2> RotateEvent = delegate { };
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireEvent.Invoke();
        }
    }
    
    public void OnRotate(InputAction.CallbackContext context)
    {
        RotateEvent.Invoke(context.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.Player.SetCallbacks(this);
            _gameInput.Player.Enable();
        }
    }
}
