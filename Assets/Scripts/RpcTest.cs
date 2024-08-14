using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// public class RpcTest : NetworkBehaviour
// {
//     public override void OnNetworkSpawn()
//     {
//         if (!IsServer && IsOwner)
//         {
//             OnlyServerRpc(0, NetworkObjectId);
//         }
//     }
//
//     [ClientRpc]
//     void HostAndClientRpc(int value, ulong sourceNetworkObjectId)
//     {
//         Debug.Log($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
//         if (IsOwner)
//         {
//             OnlyServerRpc(value + 1, sourceNetworkObjectId);
//         }
//     }
//
//     [ServerRpc(RequireOwnership = false)]
//     void OnlyServerRpc(int value, ulong sourceNetworkObjectId)
//     {
//         Debug.Log($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
//         HostAndClientRpc(value, sourceNetworkObjectId);
//     }
//     
// }
