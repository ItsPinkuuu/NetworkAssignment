using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class HealthManager : NetworkBehaviour
{
    public NetworkVariable<float> health = new();



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        health.Value = 100f;
    }
    
    private void Update()
    {
        if (health.Value <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        health.Value = 100f;
    }
}
