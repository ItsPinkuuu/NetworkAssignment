using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private NetworkVariable<Vector2> _moveInput = new NetworkVariable<Vector2>();

    [SerializeField] private float _moveSpeed;
    
    
    
    private void Start()
    {
        if (_inputReader != null && IsLocalPlayer)
        {
            _inputReader.MoveEvent += OnMove;
        }
    }

    void Update()
    {
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
}
