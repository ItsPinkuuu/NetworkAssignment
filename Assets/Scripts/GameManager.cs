using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public GameObject _player1;
    public GameObject _player2;

    private Image _resultBackground;
    private TextMeshProUGUI _P1Text;
    private TextMeshProUGUI _P2Text;


    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        else Destroy(gameObject);
        
        _resultBackground = GameObject.Find("Background").GetComponent<Image>();
        var bgColor = _resultBackground.color;
        bgColor.a = 0f;
        _resultBackground.color = bgColor;
        
        _P1Text = GameObject.Find("P1Text").GetComponent<TextMeshProUGUI>();
        _P1Text.text = "";
        _P2Text = GameObject.Find("P2Text").GetComponent<TextMeshProUGUI>();
        _P2Text.text = "";
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
                HandleResultScreensClientRpc("LOSER", "WINNER", "Player 1", "Player 2");
                
            } else if (_player2.GetComponent<Player>()._isDead)
            {
                HandleResultScreensClientRpc("WINNER", "LOSER", "Player 1", "Player 2");
            }
            
        }
        
    }

    [ClientRpc]
    private void HandleResultScreensClientRpc(string player1Result, string player2Result, string P1Text, string P2Text)
    {
        UpdateResultScreens(player1Result, player2Result, P1Text, P2Text);
    }

    private void UpdateResultScreens(string player1Result, string player2Result, string P1Text, string P2Text)
    {
        var bgColor = _resultBackground.color;
        bgColor.a = 1f;
        _resultBackground.color = bgColor;
        
        _P1Text.text = P1Text;
        _P2Text.text = P2Text;
        
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P1resultText.text = player1Result;
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>()._P2resultText.text = player2Result;
    }
}
