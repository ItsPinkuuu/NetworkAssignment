using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject _resultScreenPrefab;
    public GameObject _player1;
    public GameObject _player2;

    

    private void Update()
    {
        if (IsServer)
        {
            if (_player1 == null || _player2 == null)
            {
                return;
            }
            
            if (_player1.GetComponent<Player>()._isDead)
            {
                SpawnResultScreensServerRpc();

                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P1resultText.text = "LOSER";
                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P2resultText.text = "WINNER";
            } else if (_player2.GetComponent<Player>()._isDead)
            {
                SpawnResultScreensServerRpc();

                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P1resultText.text = "WINNER";
                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P2resultText.text = "LOSER";
            }
        }
    }

    [ServerRpc]
    private void SpawnResultScreensServerRpc()
    {
        GameObject _resultScreenObject = Instantiate(_resultScreenPrefab, GameObject.Find("WinnerLoserScreens").transform); 
        _resultScreenObject.GetComponent<NetworkObject>().Spawn();
    }
}
