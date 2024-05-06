using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class MissileExplosion : NetworkBehaviour
{
    public MissileGun parent;

    public GameObject explosionPrefab;

    public float explosionRadius = 5;

    public float explosionForce = 1000;


    public Rigidbody TargetBody;
    private void OnCollisionEnter(Collision collision)
    {
        if(!IsOwner) return;
        InstantiateCollisionEnterServerRpc();
        parent.DestroyMissileServerRpc();   
    }
    //thực thi khi là máy chủ
    [ServerRpc]
    private void InstantiateCollisionEnterServerRpc()
    {
        GameObject hitImpact=Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        NetworkObject networkObject= hitImpact.GetComponent<NetworkObject>();
        networkObject.GetComponent<NetworkObject>().Spawn();

        //Tác dụng 1 lực nổ cho các vật thể ở gần
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionForce);
        foreach(Collider collider in colliders)
        {
            TargetBody = collider.GetComponent<Rigidbody>();
            if (TargetBody != null)
            {
                continue;
            }
            TargetBody.AddExplosionForce(explosionForce,transform.position,explosionRadius);
        }

        Debug.Log("Explosion");
        AutoDetroy autoDetroy = networkObject.GetComponent<AutoDetroy>();
    }
}
