using System;
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
        
        StartCoroutine(DestroyBullet());
    }

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * _speed;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(10f);
        
        Destroy(gameObject);
    }
    
    
}
