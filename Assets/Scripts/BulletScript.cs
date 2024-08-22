using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletScript : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _despawnTime;
    public float _damage;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        // GetComponent<Rigidbody2D>().AddForce(transform.forward * _speed);
        
        GetComponent<Rigidbody2D>().velocity = transform.up * _speed;
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(_despawnTime);
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<HealthManager>() && GetComponent<NetworkObject>().OwnerClientId !=
            collider.GetComponent<NetworkObject>().OwnerClientId)
        {
            Destroy(gameObject);
        }
    }
}
