using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletScript : NetworkBehaviour
{
    [SerializeField] private float _speed;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
    }
}
