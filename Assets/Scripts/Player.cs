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
    [SerializeField] private GameManager _gameManager;

    private NetworkVariable<Vector2> _moveInput = new();
    
    private Vector2 _mousePosNormalized;
    private Camera _camera;
    private Transform _playerTransform;
    private Rigidbody2D _playerRB;
    [SerializeField] private TextMeshProUGUI _P1MessageTextBox;
    [SerializeField] private TextMeshProUGUI _P2MessageTextBox;
    public TextMeshProUGUI _P1resultText;
    public TextMeshProUGUI _P2resultText;
    [SerializeField] private float _moveSpeed;
    public bool _isDead = false;

    
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        _camera = Camera.main;
        _P1MessageTextBox = GameObject.Find("P1Message").GetComponent<TextMeshProUGUI>();
        _P2MessageTextBox = GameObject.Find("P2Message").GetComponent<TextMeshProUGUI>();
        _P1resultText = GameObject.Find("P1Result").GetComponent<TextMeshProUGUI>();
        _P2resultText = GameObject.Find("P2Result").GetComponent<TextMeshProUGUI>();
        _gameManager = GameObject.Find("__game__").GetComponent<GameManager>();

        if (OwnerClientId == 0)
        {
            _gameManager._player1 = gameObject;
        } else if (OwnerClientId == 1)
        {
            _gameManager._player2 = gameObject;
        }
        
        if (_inputReader != null && IsLocalPlayer)
        {
            if (!_isDead)
            {
                _inputReader.MoveEvent += OnMove;
            }
        }
    }

    void Update()
    {
        if (!_isDead)
        {
            if (IsLocalPlayer && Application.isFocused)
            {
                var mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
                _mousePosNormalized = (mouseWorld - transform.position).normalized;

                RotatePlayerServerRpc(_mousePosNormalized);

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SendPlayerMessageServerRpc("Well Played!");
                } else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SendPlayerMessageServerRpc("You SUCK!");
                } else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    SendPlayerMessageServerRpc("You Shoot Like A Chicken!");
                }
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
        if (IsServer)
        {
            if (collider.GetComponent<BulletScript>() && GetComponent<NetworkObject>().OwnerClientId != collider.GetComponent<NetworkObject>().OwnerClientId)
            {
                GetComponent<HealthManager>().health.Value -= collider.GetComponent<BulletScript>()._damage;
            }
        }
    }

    [ServerRpc]
    private void SendPlayerMessageServerRpc(string message)
    {
        HandlePlayerMessageBoxClientRpc(message);
    }

    [ClientRpc]
    private void HandlePlayerMessageBoxClientRpc(string message)
    {
        UpdateMessageBox(message);
    }

    private void UpdateMessageBox(string message)
    {
        if (OwnerClientId == 0)
        {
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P1MessageTextBox.text = message;
        } else if (OwnerClientId == 1)
        {
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P2MessageTextBox.text = message;
        }
    }
}