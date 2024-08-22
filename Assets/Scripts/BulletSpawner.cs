using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform InitialTransform;
    
    private void Update()
    {
        if (IsLocalPlayer && Application.isFocused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnBulletServerRpc(InitialTransform.position, InitialTransform.rotation);
            }
        }
    }

    [ServerRpc]
    private void SpawnBulletServerRpc(Vector2 position, Quaternion rotation, ServerRpcParams serverRpcParams = default)
    {
        GameObject InstantiatedBullet = Instantiate(bulletPrefab, position, rotation);
        
        InstantiatedBullet.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
}
