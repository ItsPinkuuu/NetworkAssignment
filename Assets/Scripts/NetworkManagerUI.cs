using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerUI : MonoBehaviour
{
    private static NetworkManager m_networkManager;

    private void Awake()
    {
        m_networkManager = GetComponent<NetworkManager>();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!m_networkManager.IsClient && !m_networkManager.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }
        
        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) m_networkManager.StartHost();
        if (GUILayout.Button("Client")) m_networkManager.StartClient();
        if (GUILayout.Button("Server")) m_networkManager.StartServer();
    }

    static void StatusLabels()
    {
        var mode = m_networkManager.IsHost ? "Host" : m_networkManager.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " + m_networkManager.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
}
