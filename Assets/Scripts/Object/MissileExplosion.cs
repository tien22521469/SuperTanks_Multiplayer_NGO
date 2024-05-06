using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class MissileExplosion : NetworkBehaviour
{
    public MissileGun parent;

    public GameObject explosionPrefab;

    public float explosionRadius = 5;

    public float explosionForce = 1000;
   
    

    private void OnCollisionEnter(Collision collision)
    {
        if(!IsOwner) return;
        
        InstantiateCollisionEnterServerRpc();

        parent.DestroyMissileServerRpc();   
    }
    [ServerRpc]
    private void InstantiateCollisionEnterServerRpc()
    {
        GameObject hitImpact=Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        NetworkObject hitImpactNetworkObject = hitImpact.GetComponent<NetworkObject>();
        hitImpactNetworkObject.Spawn(); 
        hitImpactNetworkObject.transform.localEulerAngles =new Vector3(0f,0f,-90f);
    }
}
