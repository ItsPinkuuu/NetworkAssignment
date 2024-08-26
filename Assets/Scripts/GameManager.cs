using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject _winnerScreenObject;
    [SerializeField] private GameObject _loserScreenObject;
    [SerializeField] private NetworkObject _player1;
    [SerializeField] private NetworkObject _player2;


    
    private void Start()
    {
        _player1 = NetworkManager.Singleton.ConnectedClients[0].PlayerObject;
        _player2 = NetworkManager.Singleton.ConnectedClients[1].PlayerObject;
        _winnerScreenObject = GameObject.Find("WinnerScreen");
        _loserScreenObject = GameObject.Find("Loserscreen");
    }
}
