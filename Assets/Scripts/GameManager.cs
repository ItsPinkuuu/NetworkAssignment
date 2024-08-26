using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winnerScreenObject;
    [SerializeField] private GameObject _loserScreenObject;
    public GameObject _player1;
    public GameObject _player2;


    
    private void Awake()
    {
        if (_player1 == null || _player2 == null)
        {
            return;
        }
    }

    private void Start()
    {
        _winnerScreenObject = GameObject.Find("WinnerScreen");
        _loserScreenObject = GameObject.Find("LoserScreen");
        
        _winnerScreenObject.SetActive(false);
        _loserScreenObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    [ServerRpc]
    public void RequestShowScreenServerRpc()
    {
        ShowWinLoseScreenClientRpc();
    }
    
    [ClientRpc]
    public void ShowWinLoseScreenClientRpc()
    {
        _winnerScreenObject.SetActive(true);
        _loserScreenObject.SetActive(true);
            
        _winnerScreenObject.GetComponent<NetworkObject>().NetworkShow(0);
        _loserScreenObject.GetComponent<NetworkObject>().NetworkShow(1);
    }
}
