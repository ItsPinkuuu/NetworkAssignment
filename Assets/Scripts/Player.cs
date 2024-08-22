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
    private Vector2 _mousePosNormalized;

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
        if (IsLocalPlayer && Application.isFocused)
        {
            var mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            _mousePosNormalized = (mouseWorld - transform.position).normalized;

            RotatePlayerServerRpc(_mousePosNormalized);

            // if (Input.GetKeyDown((KeyCode.A)))
            // {
            //     MessageRpc("GG");
            // }
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

    // [ServerRpc]
    // private void MessageRpc(string message)
    // {
    //     //chat.singleton.displaychat(message, myId)
    //     
    //     //if(id == myid) put it in local window else put in global window
    // }

    // private void OnFire(Vector2 input)
    // {
    //     
    // }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<BulletScript>() && GetComponent<NetworkObject>().OwnerClientId != collider.GetComponent<NetworkObject>().OwnerClientId)
        {
            Debug.Log("Hit player!");
            GetComponent<HealthManager>().health.Value -= collider.GetComponent<BulletScript>()._damage;
        }
    }
}