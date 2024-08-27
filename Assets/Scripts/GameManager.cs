using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject _resultScreenObject;
    public GameObject _player1;
    public GameObject _player2;


    
    private void Awake()
    {
        _resultScreenObject = GameObject.Find("ResultScreen");
        
        _resultScreenObject.SetActive(false);
    }

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
                _resultScreenObject.SetActive(true);

                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P1resultText.text = "LOSER";
                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P2resultText.text = "WINNER";
            } else if (_player2.GetComponent<Player>()._isDead)
            {
                _resultScreenObject.SetActive(true);

                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P1resultText.text = "WINNER";
                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P2resultText.text = "LOSER";
            }
        }
    }
}
