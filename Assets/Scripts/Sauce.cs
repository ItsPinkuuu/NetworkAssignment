using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Sauce : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    private Camera _camera;

    //private NetworkVariable<Vector2> _moveInput = new();
    //private NetworkVariable<Vector2> _rotateInput = new(); 

    private Vector2 _moveInput;
    private Vector2 _rotateInput;

    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private float _moveSpeed = 5;


    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        if (_inputReader != null && IsLocalPlayer)
        {
            _inputReader.MoveEvent += OnMove;
            _inputReader.RotateEvent += OnRotate;
        }
    }

    void FixedUpdate()
    {
        /** 
         * Movement should be done via rigidbody 2D but for now it works.
         */
        if (!IsOwner) return;

        Vector3 camera_position = _camera.ScreenToWorldPoint((Vector3) _rotateInput);
        Vector3 dir = camera_position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        transform.position += (Vector3)_moveInput * _moveSpeed * Time.deltaTime;

    }

    private void OnMove(Vector2 input)
    {
        _moveInput = input;
    }

    private void OnRotate(Vector2 input)
    {
        _rotateInput = input;
    }
}