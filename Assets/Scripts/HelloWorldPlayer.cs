using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

// public class HelloWorldPlayer : NetworkBehaviour
// {
//     public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();
//
//     public override void OnNetworkSpawn()
//     {
//         if (IsOwner)
//         {
//             Move();
//         }
//     }
//
//     public void Move()
//     {
//         SubmitPositionRequestServerRpc();
//     }
//
//     [ServerRpc]
//     void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
//     {
//         var randomPosition = GetRandomPositionOnPlane();
//         transform.position = randomPosition;
//         Position.Value = randomPosition;
//     }
//
//     static Vector2 GetRandomPositionOnPlane()
//     {
//         return new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
//     }
//
//     private void Update()
//     {
//         transform.position = Position.Value;
//     }
// }
