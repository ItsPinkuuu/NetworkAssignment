using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private NetworkVariable<Vector2> _moveInput = new();
    private NetworkVariable<Vector2> _mousePosNormalized = new();

    private Camera _camera;
    private Transform _playerTransform;
    private Rigidbody2D _playerRB;
    [SerializeField] private float _moveSpeed;


    
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        if (_inputReader != null && IsLocalPlayer)
        {
            _inputReader.MoveEvent += OnMove;
        }
    }

    void FixedUpdate()
    {
        var mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
        _mousePosNormalized.Value = (mouseWorld - transform.position).normalized;
            
        if (IsLocalPlayer)
        {
            transform.up = _mousePosNormalized.Value;
        }
        
        if (IsServer)
        {
            transform.position += (Vector3)_moveInput.Value * (_moveSpeed * Time.deltaTime);
        }
    }

    private void OnMove(Vector2 input)
    {
        MoveServerRpc(input);
    }

    [ServerRpc]
    private void MoveServerRpc(Vector2 data)
    {
        _moveInput.Value = data;
    }

    private void OnFire(Vector2 input)
    {
        
    }
}
