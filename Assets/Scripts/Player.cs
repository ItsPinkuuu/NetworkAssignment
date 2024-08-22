using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private NetworkVariable<Vector2> _moveInput = new();
    
    private Vector2 _mousePosNormalized;
    private Camera _camera;
    private Transform _playerTransform;
    private Rigidbody2D _playerRB;
    [SerializeField] private TextMeshProUGUI _P1MessageTextBox;
    [SerializeField] private TextMeshProUGUI _P2MessageTextBox;
    [SerializeField] private float _moveSpeed;

    
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _camera = Camera.main;
        _P1MessageTextBox = GameObject.Find("P1Message").GetComponent<TextMeshProUGUI>();
        _P2MessageTextBox = GameObject.Find("P2Message").GetComponent<TextMeshProUGUI>();
        
        if (_inputReader != null && IsLocalPlayer)
        {
            _inputReader.MoveEvent += OnMove;
        }
    }

    void FixedUpdate()
    {
        if (IsLocalPlayer && Application.isFocused)
        {
            var mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            _mousePosNormalized = (mouseWorld - transform.position).normalized;

            RotatePlayerServerRpc(_mousePosNormalized);

            if (Input.GetKeyDown(KeyCode.H))
            {
                UpdatePlayerMessageBoxServerRpc("Well Played!");
            } else if (Input.GetKeyDown(KeyCode.J))
            {
                UpdatePlayerMessageBoxServerRpc("You SUCK!");
            } else if (Input.GetKeyDown(KeyCode.K))
            {
                UpdatePlayerMessageBoxServerRpc("You Shoot Like A Chicken!");
            }
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

    [ServerRpc]
    private void RotatePlayerServerRpc(Vector2 input)
    {
        transform.up = input.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<BulletScript>() && GetComponent<NetworkObject>().OwnerClientId != collider.GetComponent<NetworkObject>().OwnerClientId)
        {
            GetComponent<HealthManager>().health.Value -= collider.GetComponent<BulletScript>()._damage;
        }
    }

    [ServerRpc]
    private void UpdatePlayerMessageBoxServerRpc(string message)
    {
        if (OwnerClientId == 0)
        {
            _P1MessageTextBox.text = message;
        } else if (OwnerClientId == 1)
        {
            _P2MessageTextBox.text = message;
        }
    }
}