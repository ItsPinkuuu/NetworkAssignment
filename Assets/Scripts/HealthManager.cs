using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class HealthManager : NetworkBehaviour
{
    public NetworkVariable<float> health = new();
    private NetworkObject _playerNetworkObject;



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _playerNetworkObject = GetComponent<NetworkObject>();
        health.Value = 100f;
    }
    
    private void Update()
    {
        if (IsServer)
        {
            if (health.Value <= 0)
            {
                GetComponent<Player>()._isDead = true;
                IsDead();
            }
        }
    }

    private void IsDead()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
